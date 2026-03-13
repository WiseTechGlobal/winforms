# Supporting Legacy WinForms Controls

## Purpose

This document explains what would be required to support the old WinForms controls that were removed after .NET Core 3.0 and later reintroduced in this repository as binary-compatibility shims.

The short version is:

- The .NET `release/3.0` branch contained real implementations of these controls.
- This repository currently contains mostly hollow compatibility types for the same public surface.
- If the requirement is actual runtime support, the current `Unsupported` area is the wrong abstraction layer to build on incrementally.
- The lowest-risk path is to restore the 3.0 implementations and then adapt them to the current codebase, rather than trying to evolve the shims into working controls member-by-member.

## Historical Baseline

In the upstream `c:\git\github\dotnet\winforms` checkout on `release/3.0`, the legacy controls were still present as normal runtime implementations under:

`src/System.Windows.Forms/src/System/Windows/Forms/`

Representative examples from that branch include:

- `ContextMenu.cs`
- `Menu.cs`
- `MenuItem.cs`
- `MainMenu.cs`
- `DataGrid.cs`
- `DataGridTableStyle.cs`
- `DataGridTextBoxColumn.cs`
- `StatusBar.cs`
- `StatusBarPanel.cs`
- `ToolBar.cs`
- `ToolBarButton.cs`

Those files were not stubs. They contained state, rendering behavior, event wiring, message processing, layout logic, interop, and designer-facing metadata.

## Current State In This Repository

The current repository contains the old surface area under:

`src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/`

The families currently present are:

- `ContextMenu/`
- `DataGrid/`
- `MainMenu/`
- `StatusBar/`
- `ToolBar/`

The implementation model is documented in `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/readme.md`.

That readme is explicit about the intent:

- the types exist for binary compatibility
- they are not intended to be executed
- constructors throw `PlatformNotSupportedException`
- non-void members typically `throw null`
- void members do nothing
- inherited members are only reintroduced where metadata requires it

This means the current code is designed to let old assemblies load, not to let old controls work.

## Evidence That The Current Surface Is Hollow

Representative examples in this repository:

- `ContextMenu` throws from both public constructors and exposes no real event or display behavior.
- `DataGrid` throws from its constructor and exposes a large metadata-shaped surface with inert getters, setters, and events.
- `MainMenu`, `StatusBar`, and `ToolBar` follow the same pattern.
- `Obsoletions.cs` marks these APIs as binary compatibility only and points developers to modern replacements.

There are also important host integration gaps:

- `Control.ContextMenu` is currently just a compatibility property/event shell.
- `Form.Menu` and `Form.MergedMenu` are currently compatibility properties, not real menu plumbing.

That difference matters more than the type definitions themselves. Even if the legacy controls were fully implemented, they would still not behave correctly until the owning `Control` and `Form` integration was restored.

## Gap Versus The 3.0 Implementation

The divergence from the 3.0 implementation is large, not cosmetic.

Two representative comparisons:

- `ContextMenu.cs`: the current shim is roughly a few dozen lines of metadata and throwing members, while the 3.0 implementation contained full popup, collapse, RTL, command-key, and rendering behavior.
- `DataGrid.cs`: a direct no-index diff against the upstream 3.0 file shows that the current version removes roughly ten thousand lines of implementation logic.

This is the central architectural fact for any recovery plan:

> We are not missing a few handlers or a few constructors. We are missing the actual control implementations.

## Why The Existing Unsupported Folder Should Not Be Evolved In Place

The local `Unsupported/readme.md` says the folder should not be modified. Even without that warning, the current design makes it a poor place to grow real support:

- the code structure is intentionally metadata-first, not behavior-first
- the nullability mode is intentionally disabled for compatibility
- members are shaped to preserve reflection and JIT behavior, not runtime semantics
- the folder encodes an explicit product position: load old binaries, do not support the controls

If the business requirement has changed from compatibility to support, that change should be reflected in the architecture instead of slowly weakening the shim model.

## Recommended Strategy

If actual support is required, use this strategy.

### 1. Decide The Support Contract First

Be explicit about which of these is required:

- binary compatibility only
- runtime support for legacy applications
- designer support
- accessibility parity with current WinForms expectations
- long-term supported public API, or temporary migration bridge

Without this decision, the implementation will drift between two incompatible goals.

### 2. Treat `release/3.0` As The Behavioral Source Of Truth

Use the upstream `release/3.0` implementation as the starting point for behavior, not the current `Unsupported` folder.

That means importing or porting the 3.0 versions of:

- `ContextMenu`, `Menu`, `MenuItem`, `MainMenu`
- `DataGrid` and its dependent types
- `StatusBar` and panel/event types
- `ToolBar` and button/event types

The current shim files are still useful as a quick inventory of required public API and obsoletion metadata, but they are not a viable base for runtime work.

### 3. Restore Host Integration At The Same Time

Do not port the controls in isolation. The owning framework types must be wired back up in the same effort.

At minimum this includes:

- `Control.ContextMenu`
- `Control.ContextMenuChanged`
- `Control.OnContextMenuChanged`
- `Form.Menu`
- `Form.MergedMenu`
- `Form.MenuStart`
- `Form.MenuComplete`
- menu handle update logic and message processing paths that interact with legacy menus

The upstream 3.0 branch already shows these members participating in runtime behavior. In this repository they are currently compatibility shells.

### 4. Port By Control Family, Not By Individual File

The safest sequence is:

1. Menu stack: `Menu`, `MenuItem`, `ContextMenu`, `MainMenu`, plus `Control` and `Form` integration.
2. `StatusBar` family.
3. `ToolBar` family.
4. `DataGrid` family.

This order is deliberate:

- the menu stack has the smallest conceptual surface and exposes whether host integration is correct
- `StatusBar` and `ToolBar` are self-contained compared to `DataGrid`
- `DataGrid` is the highest-risk port because of its size, layout logic, binding behavior, and accessibility implications

### 5. Preserve Public Shape, Reevaluate Product Semantics

Once a family becomes truly supported, revisit whether these should still:

- throw `PlatformNotSupportedException`
- remain hidden with `EditorBrowsable(Never)`
- remain marked with the current `WFDEV006` guidance
- remain described as binary compatibility only in docs and package metadata

If the runtime behavior is restored but the warnings still tell users not to use the types, the product story becomes internally inconsistent.

## Expected Porting Work

Porting the 3.0 code is not a straight file copy. Expect adaptation work in these areas.

### API And Type System Drift

- current repository coding style is significantly newer than the 3.0 code style
- nullable reference types have evolved, but the legacy surface intentionally disables nullability in some areas
- helper APIs, interop wrappers, and internal utility names may have changed

### Rendering And Platform Drift

- current WinForms code has moved on in DPI handling, theming, and message processing
- any imported rendering path must be checked against the current visual style and DPI model
- old controls may assume older system metrics or menu handle behavior

### Accessibility

The original reason for removing these controls was not arbitrary. They lagged in accessibility and modern platform expectations.

If support is required, decide whether the target is:

- strict behavioral restoration of 3.0 behavior, or
- restoration plus modern accessibility fixes

That is a product decision, not just an implementation detail.

### Designer And Serialization

Many of these types carry designer attributes and serializer-facing metadata. Runtime support without designer validation is incomplete for many WinForms applications.

At minimum, validate:

- designer load behavior
- property grid visibility and serialization
- event hookup serialization
- resource serialization
- form/menu merge scenarios

### DataGrid Complexity

`DataGrid` is the hardest family by far.

Reasons:

- very large implementation footprint
- custom layout and painting
- binding manager interactions
- editing service interactions
- accessibility subtypes
- table and column style object graph

If schedule is tight, `DataGrid` should be scoped separately from the menu, `StatusBar`, and `ToolBar` work.

## Suggested Execution Plan

## Execution Checklist

### Phase 1: Menu Stack

- [x] Restore `release/3.0` `Menu.cs` into `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/Menu.cs`.
- [x] Restore `release/3.0` `MenuItem.cs` into `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/MenuItem.cs`.
- [x] Restore `release/3.0` `ContextMenu.cs` into `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/ContextMenu.cs`.
- [x] Restore `release/3.0` `MainMenu.cs` into `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/MainMenu/MainMenu.cs`.
- [x] Remove the old split `Menu.MenuItemCollection.cs` shim because the real implementation now lives in `Menu.cs`.
- [x] Rewire `Control.ContextMenu`, `Control.ContextMenuChanged`, `Control.OnContextMenuChanged`, `Control.WmContextMenu`, and `Control.ProcessCmdKey`.
- [x] Rewire `Form.Menu`, `Form.MergedMenu`, `Form.ProcessCmdKey`, `Form.UpdateMenuHandles`, `WM_INITMENUPOPUP`, `WM_MENUCHAR`, and `WM_UNINITMENUPOPUP`.
- [ ] Run focused menu-stack behavior tests.
- [ ] Reconcile product semantics for the restored menu stack: obsolete warnings, IntelliSense visibility, and package messaging.

### Phase 2: StatusBar

- [x] Port `release/3.0` `StatusBar.cs`.
- [x] Port `release/3.0` `StatusBarDrawItemEventArgs.cs`.
- [x] Port `release/3.0` `StatusBarDrawItemEventHandler.cs`.
- [x] Port `release/3.0` `StatusBarPanel.cs`.
- [x] Port `release/3.0` `StatusBarPanelAutoSize.cs`.
- [x] Port `release/3.0` `StatusBarPanelBorderStyle.cs`.
- [x] Port `release/3.0` `StatusBarPanelClickEventArgs.cs`.
- [x] Port `release/3.0` `StatusBarPanelClickEventHandler.cs`.
- [x] Port `release/3.0` `StatusBarPanelStyle.cs`.
- [x] Collapse or replace the current split `StatusBar.StatusBarPanelCollection.cs` if the real collection implementation is restored inside `StatusBar.cs`.

### Phase 3: ToolBar

- [ ] Port `release/3.0` `ToolBar.cs`.
- [ ] Port `release/3.0` `ToolBarAppearance.cs`.
- [ ] Port `release/3.0` `ToolBarButton.cs`.
- [ ] Port `release/3.0` `ToolBarButtonClickEventArgs.cs`.
- [ ] Port `release/3.0` `ToolBarButtonClickEventHandler.cs`.
- [ ] Port `release/3.0` `ToolBarButtonStyle.cs`.
- [ ] Port `release/3.0` `ToolBarTextAlign.cs`.
- [ ] Collapse or replace the current split `ToolBar.ToolBarButtonCollection.cs` if the real collection implementation is restored inside `ToolBar.cs`.

### Phase 4: DataGrid

- [ ] Port `release/3.0` `DataGrid.cs`.
- [ ] Port `release/3.0` `DataGridAddNewRow.cs`.
- [ ] Port `release/3.0` `DataGridBoolColumn.cs`.
- [ ] Port `release/3.0` `DataGridCaption.cs`.
- [ ] Port `release/3.0` `DataGridCell.cs`.
- [ ] Port `release/3.0` `DataGridColumnCollection.cs`.
- [ ] Port `release/3.0` `DataGridColumnStyle.cs` and reconcile the current split `DataGridColumnStyle.CompModSwitches.cs` and `DataGridColumnStyle.DataGridColumnHeaderAccessibleObject.cs` layout.
- [ ] Port `release/3.0` `DataGridDefaultColumnWidthTypeConverter.cs` and reconcile the current `DataGridPreferredColumnWidthTypeConverter.cs` naming difference.
- [ ] Port `release/3.0` `DataGridLineStyle.cs`.
- [ ] Port `release/3.0` `DataGridParentRows.cs`.
- [ ] Port `release/3.0` `DataGridParentRowsLabel.cs`.
- [ ] Port `release/3.0` `DataGridRelationshipRow.cs`.
- [ ] Port `release/3.0` `DataGridRow.cs`.
- [ ] Port `release/3.0` `DataGridState.cs`.
- [ ] Port `release/3.0` `DataGridTableCollection.cs`.
- [ ] Port `release/3.0` `DataGridTableStyle.cs`.
- [ ] Port `release/3.0` `DataGridTextBox.cs`.
- [ ] Port `release/3.0` `DataGridTextBoxColumn.cs`.
- [ ] Port `release/3.0` `DataGridToolTip.cs`.
- [ ] Port `release/3.0` `GridTablesFactory.cs`.
- [ ] Port `release/3.0` `IDataGridEditingService.cs`.
- [ ] Add the currently missing supporting files under `Controls/Unsupported/DataGrid/` rather than continuing to expand the shim surface ad hoc.

## File-By-File Port Map

### Menu Stack

- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/Menu.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/Menu.cs`
	Status: started
	Note: the real `MenuItemCollection` now lives inside `Menu.cs`; the old split shim file should stay deleted.
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/MenuItem.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/MenuItem.cs`
	Status: started
	Note: this also carries `MenuItemData` and `MdiListUserData` helper types.
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ContextMenu.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/ContextMenu.cs`
	Status: started
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/MainMenu.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/MainMenu/MainMenu.cs`
	Status: started
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/MenuMerge.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/MenuMerge.cs`
	Status: pending review
	Note: the local enum file already matches the required public shape closely and may not need replacement.
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/Control.cs` classic menu members -> `src/System.Windows.Forms/System/Windows/Forms/Control.cs`
	Status: started
	Scope: `ContextMenu`, `ContextMenuChanged`, `OnContextMenuChanged`, `ProcessCmdKey`, `WmContextMenu`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/Form.cs` classic menu members -> `src/System.Windows.Forms/System/Windows/Forms/Form.cs`
	Status: started
	Scope: `Menu`, `MergedMenu`, cached current menu state, `UpdateMenuHandles`, `ProcessCmdKey`, `WM_INITMENUPOPUP`, `WM_MENUCHAR`, `WM_UNINITMENUPOPUP`

### StatusBar

- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBar.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBar.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarDrawItemEventArgs.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarDrawItemEventArgs.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarDrawItemEventHandler.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarDrawItemEventHandler.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanel.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanel.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanelAutoSize.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanelAutoSize.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanelBorderStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanelBorderStyle.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanelClickEventArgs.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanelClickEventArgs.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanelClickEventHandler.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanelClickEventHandler.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/StatusBarPanelStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBarPanelStyle.cs`
- `release/3.0` embedded collection implementation in `StatusBar.cs` -> current split `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar/StatusBar.StatusBarPanelCollection.cs`
	Note: likely collapse this split once the real `StatusBar.cs` is ported.

### ToolBar

- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBar.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBar.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarAppearance.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarAppearance.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarButton.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarButton.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarButtonClickEventArgs.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarButtonClickEventArgs.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarButtonClickEventHandler.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarButtonClickEventHandler.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarButtonStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarButtonStyle.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/ToolBarTextAlign.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBarTextAlign.cs`
- `release/3.0` embedded collection implementation in `ToolBar.cs` -> current split `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ToolBar/ToolBar.ToolBarButtonCollection.cs`
	Note: likely collapse this split once the real `ToolBar.cs` is ported.

### DataGrid

- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGrid.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGrid.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridAddNewRow.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridBoolColumn.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridBoolColumn.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridCaption.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridCell.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridCell.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridColumnCollection.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridColumnStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridColumnStyle.cs`
- `release/3.0` supporting pieces inside `DataGridColumnStyle.cs` -> current split `DataGridColumnStyle.CompModSwitches.cs` and `DataGridColumnStyle.DataGridColumnHeaderAccessibleObject.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridDefaultColumnWidthTypeConverter.cs` -> reconcile with `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridPreferredColumnWidthTypeConverter.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridLineStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridLineStyle.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridParentRows.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridParentRowsLabel.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridParentRowsLabel.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridRelationshipRow.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridRow.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridState.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridTableCollection.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridTableStyle.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridTableStyle.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridTextBox.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridTextBox.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridTextBoxColumn.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/DataGridTextBoxColumn.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/DataGridToolTip.cs` -> add new file under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/GridTablesFactory.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/GridTablesFactory.cs`
- `release/3.0` `src/System.Windows.Forms/src/System/Windows/Forms/IDataGridEditingService.cs` -> `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/DataGrid/IDataGridEditingService.cs`

### Phase 1: Source Import And Inventory

- map every current shim type to its `release/3.0` implementation file
- identify supporting internal helpers required by each family
- decide whether to port by cherry-pick, manual copy, or staged diff application

### Phase 2: Menu Stack Recovery

- restore `Menu`, `MenuItem`, `ContextMenu`, and `MainMenu`
- restore `Control` and `Form` integration
- add smoke tests for popup display, merge behavior, and menu events

### Phase 3: StatusBar And ToolBar Recovery

- restore the runtime implementations
- validate layout, painting, events, image handling, and designer serialization

### Phase 4: DataGrid Recovery

- port the full family together
- add binding, editing, painting, and accessibility tests before considering the work stable

### Phase 5: Product Cleanup

- revisit `WFDEV006` messaging
- update package description and docs to match the actual support level
- decide whether any APIs should remain hidden from IntelliSense

## Test Strategy

Minimum test coverage should include:

- construction and disposal of each restored control family
- Win32 handle creation and recreation
- keyboard and mouse interaction
- menu popup and collapse behavior
- form menu merge behavior
- status bar panel rendering and click events
- toolbar button rendering and click events
- `DataGrid` data binding, edit commit, and navigation
- designer serialization round-trip for representative scenarios
- accessibility smoke tests for each family

## Practical Recommendation

If the requirement is truly to support these old controls, do not continue from the current hollow implementation model.

Instead:

1. Keep the current shim code only as a temporary compatibility layer and API inventory.
2. Use upstream `release/3.0` as the behavioral baseline.
3. Restore one control family at a time, together with the owning `Control` and `Form` integration.
4. Treat `DataGrid` as a separate project with its own risk and test budget.

That is the only approach that aligns the codebase with the actual requirement. Anything smaller will likely produce a half-working surface that compiles, loads, and still fails in real applications.