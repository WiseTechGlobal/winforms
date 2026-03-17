# Legacy Control Migration: Menu Stack

## Scope

This document describes the current Menu Stack migration work for the legacy WinForms menu surface:

- `Menu`
- `MenuItem`
- `ContextMenu`
- `MainMenu`
- `Control.ContextMenu`
- `Form.Menu`
- `Form.MergedMenu`

It is based on the actual changes currently on this branch, not just on the original migration checklist.

## Current Status

The Menu Stack is the active migration area.

The branch has already restored the core runtime surface and the main host integration points. What remains is focused validation, plus product cleanup around obsolete warnings, IntelliSense visibility, and package messaging.

## What Changed On This Branch

### 1. The hollow menu shims were replaced with runtime implementations

The following legacy menu types are no longer just binary-compatibility shells:

- `Menu`
- `MenuItem`
- `ContextMenu`
- `MainMenu`

The restored code now brings back behavior such as:

- real menu item collections
- menu merging logic
- shortcut-key routing
- popup and collapse events
- owner-draw handling
- MDI window list behavior
- right-to-left menu behavior
- Win32 menu handle creation and update paths

This is the important architectural shift: the branch is no longer trying to simulate support with metadata-only stubs. It is restoring the runtime model.

### 2. `Menu.cs` now carries the real menu tree infrastructure

The restored `Menu` implementation now provides the shared runtime base for the whole stack.

Key behavior brought back includes:

- menu handle creation and destruction
- population of child menu items into native handles
- item change tracking
- menu merging
- shortcut processing
- `WM_MENUCHAR` handling for owner-draw menus
- the embedded `MenuItemCollection` implementation

One practical cleanup tied to this change is that the old split shim file `Menu.MenuItemCollection.cs` was removed because the real collection implementation now lives inside `Menu.cs`.

### 3. `MenuItem.cs` now restores the bulk of menu-item behavior

`MenuItem` is where most of the menu-state and menu-event machinery lives, and the current branch restores that implementation rather than preserving the old throw-only shell.

The restored work includes:

- checked, radio-check, break, and default-item state
- caption, shortcut, and visibility handling
- popup, click, select, measure, and draw events
- owner-draw plumbing
- MDI list population
- menu cloning and merging support
- native menu item creation and update logic
- per-item linkage through `MenuItemData`
- compatibility support for menu-item lookup from native item data

This is one of the clearest signals that the migration is runtime-first now: the branch restored the actual stateful implementation model instead of keeping a reflection-only public shape.

### 4. `ContextMenu.cs` now behaves like a working context menu again

`ContextMenu` was previously a hollow shell. On this branch it now supports:

- real constructors
- `Popup` and `Collapse` events
- `SourceControl`
- `RightToLeft`
- `Show(Control, Point)`
- `Show(Control, Point, LeftRightAlignment)`
- command-key participation through `ProcessCmdKey`

That means a control-associated legacy context menu can now participate in real interaction again instead of existing only for binary compatibility.

### 5. `MainMenu.cs` now restores top-level form menu behavior

`MainMenu` has also been turned back into a runtime type.

The restored work includes:

- top-level menu handle creation
- form ownership and lifetime tracking
- collapse event support
- `RightToLeft`
- cloning support
- propagation of item changes back into the owning form

This is important because legacy menu support depends on the form being able to own and refresh a real native main menu.

### 6. `MenuMerge.cs` was reopened as real public surface

`MenuMerge` no longer carries the old obsolete-and-hidden compatibility-only presentation.

That is consistent with the branch direction: once the runtime implementation is being restored, the public surface can no longer be treated purely as a dead compatibility shape.

## Host Integration Restored In Core Framework Types

### 1. `Control` now supports real legacy `ContextMenu` behavior again

The branch rewired `Control` so legacy context menus participate in runtime behavior rather than acting as inert compatibility properties.

The restored behavior includes:

- a stored `ContextMenu` property backed by the control property store
- a real `ContextMenuChanged` event
- `OnContextMenuChanged`
- disposal detachment for the assigned context menu
- command-key routing into the legacy context menu
- `WmContextMenu` dispatch that shows the legacy menu at the expected point
- `HasMenu` integration so menu-aware size calculations use the correct native window metrics

That last point matters because once a form or control really has a menu again, the non-client sizing code needs to account for it.

### 2. `Form` now owns, merges, and refreshes legacy menus again

The `Form` changes are the other major integration point.

The current branch restores:

- a real `Menu` property backed by the property store
- a computed `MergedMenu` path for MDI scenarios
- tracking for the current active menu handle
- `HasMenu` participation in size calculation
- update of native menu handles through `UpdateMenuHandles`
- command-key routing into the current legacy menu
- handling for `WM_INITMENUPOPUP`
- handling for `WM_MENUCHAR`
- handling for `WM_UNINITMENUPOPUP`
- menu invalidation and cleanup when forms are destroyed or menus are replaced
- menu-change propagation for MDI parent and child interactions

This is the host-level plumbing that makes the restored menu types usable in a real form lifecycle rather than only constructible.

### 3. `TreeNode.ContextMenu` support was added back

The branch also restores legacy `ContextMenu` support on `TreeNode`.

That includes:

- a real `TreeNode.ContextMenu` property
- cloning of the assigned legacy context menu when a tree node is cloned

This is a smaller change than the `Control` and `Form` integration work, but it is still important because it proves the migration is covering old usage patterns beyond top-level forms.

## Compatibility Interop Added For The Restored Menu Stack

The branch adds `LegacyMenuInteropCompat.cs` to carry Win32 compatibility wrappers and menu-related structures needed by the restored implementation.

That file provides the interop support used by the menu runtime for:

- native menu creation and destruction
- menu item info reads and writes
- popup tracking
- menu bar redraw
- owner-draw support structures

This is effectively the compatibility bridge that lets the restored legacy menu code participate in the current codebase's interop layer without requiring a full historical file-for-file import.

## Validation Assets Added On This Branch

### 1. Focused legacy test project

The branch adds `System.Windows.Forms.Legacy.Tests` and wires it into `WinformsLegacy.sln`.

The current test coverage includes:

- `MainMenuTests`
- `MenuItemTests`
- `MenuSizeCalculationTests`

Those tests specifically exercise areas that match the restored host integration:

- assigning menus to forms
- dynamic population during popup
- owner-draw event plumbing
- size calculations for forms with and without menus
- behavior when menus are added or removed before handle creation

This is the first focused verification layer for the restored stack.

### 2. Demo project for manual verification

The branch also adds `System.Windows.Forms.Legacy.Demo` and a launcher form dedicated to legacy-control exploration.

The Menu Stack demo exercises:

- `MainMenu`
- `MenuItem`
- `ContextMenu`
- dynamic popup rebuilding
- owner-draw menu items
- `TreeNode.ContextMenu`
- event logging for manual inspection

This is useful because a lot of menu behavior is still easiest to validate interactively before the automation coverage is complete.

## Net Effect Of The Current Menu Stack Migration

The current branch has already accomplished the hard part of Phase 1:

- it restored the real runtime types
- it rewired the critical `Control` and `Form` integration points
- it added a dedicated demo and focused tests around the restored surface

In other words, the Menu Stack is no longer just a design proposal on this branch. It is an implemented runtime migration that still needs validation and product cleanup.

## Remaining Work

The main remaining tasks are:

1. Run focused Menu Stack behavior tests and verify they pass in the intended build environment.
2. Review remaining API semantics such as obsolete warnings and `EditorBrowsable` behavior now that the types are no longer hollow shims.
3. Reconcile package and documentation messaging so the restored runtime surface is not still described purely as binary compatibility.
4. Backfill the migrated menu SR XLF entries by copying existing translations from `src/System.Windows.Forms/src/Resources` when equivalent resource text already exists, and leave newly added menu-only entries in source-language form until upstream translations are available.
5. Continue checking edge cases around MDI merge behavior, native handle lifecycle, and owner-draw interaction.

## Practical Summary

If someone asks what the current legacy-control migration is doing, the clearest answer is:

The branch is actively restoring the classic WinForms Menu Stack as working runtime behavior. It ports back the real `Menu` / `MenuItem` / `ContextMenu` / `MainMenu` implementation model, reconnects that model to `Control` and `Form`, and adds both automated tests and a manual demo to verify the recovered behavior.