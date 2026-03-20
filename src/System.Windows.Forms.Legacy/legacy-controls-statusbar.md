# Legacy Control Migration: StatusBar

## Scope

This document tracks the legacy `StatusBar` family and its supporting types.

## Current Status

The `StatusBar` family has been ported at the runtime-behavior level and force-synced against the upstream source.

The old compatibility-only layer has been replaced with working implementations based on the upstream `StatusBar.cs` and `StatusBarPanel.cs` files from `C:\git\github\winforms\src\System.Windows.Forms\src\System\Windows\Forms`, adapted into this fork's `Controls/Unsupported/StatusBar` layout.

The local port restores the main control, panel, events, collection, interop, demo, and tests. The missing upstream `SRDescription` metadata for `StatusBar.cs` and `StatusBarPanel.cs` has now also been restored in local code, with the corresponding StatusBar-specific keys added to `SR.resx`. All 13 locale XLF files have been updated with the full set of StatusBar translation entries.

### Files ported / changed

| File | Change |
|---|---|
| `StatusBar.cs` | Replaced the throw-stub with the working control implementation, including layout, simple-text mode, panel realization, owner-draw dispatch, tooltip handling, internal interop helpers, and the nested `StatusBarPanelCollection`; upstream class `<summary>` and constructor doc comment added |
| `StatusBarDrawItemEventArgs.cs` | Restored working event-args constructors and `Panel` property; upstream XML docs and `// See the LICENSE file` header line synced |
| `StatusBarDrawItemEventHandler.cs` | Restored usable draw-item delegate surface; upstream delegate `<summary>` and full license header synced |
| `StatusBarPanel.cs` | Replaced the throw-stub with the working panel implementation, including sizing, content measurement, tooltip text, icon handling, owner-draw realization, and parent linkage; upstream class `<summary>`, explicit default constructor, and constructor doc comment added |
| `StatusBarPanelAutoSize.cs` | Reopened as a real enum with runtime semantics; full upstream `<summary>` for the `Spring` member (cross-referencing `None` and `Contents`) synced |
| `StatusBarPanelBorderStyle.cs` | Reopened as a real enum with runtime semantics; `// See the LICENSE file` header line synced |
| `StatusBarPanelClickEventArgs.cs` | Restored working click-event constructor and `StatusBarPanel` property; upstream class and member `<summary>` docs and full license header synced |
| `StatusBarPanelClickEventHandler.cs` | Restored usable panel-click delegate surface; upstream delegate `<summary>` and full license header synced |
| `StatusBarPanelStyle.cs` | Reopened as a real enum with runtime semantics; `// See the LICENSE file` header line synced |

### Files deleted (content inlined or replaced)

| File | Reason |
|---|---|
| `StatusBar.StatusBarPanelCollection.cs` | Removed because the real `StatusBarPanelCollection` implementation now lives inside `StatusBar.cs` |

### Resource and metadata parity

The upstream `SRDescription` parity for `StatusBar.cs` and `StatusBarPanel.cs` has been restored in the local port.

That includes the StatusBar-specific description attributes for:

- `Panels`
- `ShowPanels`
- `SizingGrip`
- `DrawItem`
- `PanelClick`
- panel alignment, autosize, border style, icon, min width, name, style, text, tooltip text, and width

The matching StatusBar-specific resource keys have also been added to `src/System.Windows.Forms/Resources/SR.resx`.

### XLF translation parity

All 13 locale XLF files under `src/System.Windows.Forms/Resources/xlf` have been updated with the 15 StatusBar-specific translation entries sourced verbatim from the upstream sibling repository:

| Key |
|---|
| `StatusBarDrawItem` |
| `StatusBarOnPanelClickDescr` |
| `StatusBarPanelAlignmentDescr` |
| `StatusBarPanelAutoSizeDescr` |
| `StatusBarPanelBorderStyleDescr` |
| `StatusBarPanelIconDescr` |
| `StatusBarPanelMinWidthDescr` |
| `StatusBarPanelNameDescr` |
| `StatusBarPanelsDescr` |
| `StatusBarPanelStyleDescr` |
| `StatusBarPanelTextDescr` |
| `StatusBarPanelToolTipTextDescr` |
| `StatusBarPanelWidthDescr` |
| `StatusBarShowPanelsDescr` |
| `StatusBarSizingGripDescr` |

Locales updated: `cs`, `de`, `es`, `fr`, `it`, `ja`, `ko`, `pl`, `pt-BR`, `ru`, `tr`, `zh-Hans`, `zh-Hant`.

### Validation changes (`System.Windows.Forms.Legacy.Tests`)

`StatusBarTests.cs` has been added to cover the restored surface.

The current tests exercise:

- default `StatusBar` property values
- `StatusBarPanelCollection` add, insert, remove, clear, contains, and key lookup behavior
- preservation of panels when toggling between panel mode and simple-text mode
- `ToString()` behavior for `StatusBar` and `StatusBarPanel`
- panel property semantics for alignment, autosizing, border style, owner-draw mode, width and minimum width, and tooltip text
- `PanelClick` and `DrawItem` event dispatch through protected `On...` methods

### Demo changes (`System.Windows.Forms.Legacy.Demo`)

`StatusBarForm.cs` and `StatusBarForm.Designer.cs` have been added as a dedicated manual-validation surface.

The demo exercises:

- panel mode versus simple-text mode
- dynamic panel insertion and removal
- sizing-grip toggling
- border-style cycling
- owner-draw rendering
- per-panel tooltips
- panel-click logging
- a live clock panel to keep panel realization and updates active

The main demo launcher was also updated so `StatusBar` can be opened directly beside Menu Stack and DataGrid.

---

## Migration Approach

### Step 1 - Restore the runtime family from the upstream StatusBar implementation

The branch restores the upstream StatusBar runtime family from `C:\git\github\winforms\src\System.Windows.Forms\src\System\Windows\Forms`:

- `StatusBar`
- `StatusBarPanel`
- draw-item and panel-click event types
- `StatusBarPanelAutoSize`
- `StatusBarPanelBorderStyle`
- `StatusBarPanelStyle`

The main difference is that this is an adapted port, not a literal copy. In this fork the files live under `Controls/Unsupported/StatusBar`, but the runtime behavior and the core StatusBar-specific `SRDescription` metadata now line up with the upstream source set.

### Step 2 - Replace the hollow collection shim with the real collection implementation

The old split file `StatusBar.StatusBarPanelCollection.cs` was a compatibility-only shell.

That file has been removed, and the working `StatusBarPanelCollection` now lives inside `StatusBar.cs`, matching the restored runtime model instead of the old throw-only shape.

### Step 3 - Restore StatusBar-specific descriptions and resource keys

The local implementation now carries the upstream StatusBar-specific `SRDescription` metadata for `StatusBar.cs` and `StatusBarPanel.cs`.

The corresponding resource keys have been added to `src/System.Windows.Forms/Resources/SR.resx`, which restores the design-time description surface that was present in the upstream source.

### Step 4 - Add focused verification assets

The branch adds both:

- a focused `StatusBarTests` file in `System.Windows.Forms.Legacy.Tests`
- a dedicated `StatusBarForm` demo in `System.Windows.Forms.Legacy.Demo`

That means this migration is already beyond simple file porting. It includes an initial automated and manual verification layer.

### Step 5 - Force-sync declarations and documentation with upstream

All nine StatusBar family files were re-aligned against the upstream sibling source:

- The seven smaller files (`StatusBarDrawItemEventArgs.cs`, `StatusBarDrawItemEventHandler.cs`, `StatusBarPanelAutoSize.cs`, `StatusBarPanelBorderStyle.cs`, `StatusBarPanelClickEventArgs.cs`, `StatusBarPanelClickEventHandler.cs`, `StatusBarPanelStyle.cs`) were overwritten with upstream-matching declarations, XML summaries, and the full `// See the LICENSE file in the project root for more information.` header line.
- `StatusBar.cs` and `StatusBarPanel.cs` were updated to add the upstream class-level `<summary>` comments and explicit default constructors with doc comments, while preserving the fork-specific interop layer (`LegacyStatusBarInterop`, `Assemblies.SystemDesign`) that keeps them compiling in this repository.

### Step 6 - Sync XLF locale translations

The 15 StatusBar trans-unit blocks were inserted into all 13 locale XLF files under `src/System.Windows.Forms/Resources/xlf`, sourced verbatim from the upstream sibling repository. The entries were placed immediately after the existing `DataGridVertScrollBarDescr` entry in each file, matching the `SR.resx` key ordering.

---

## Why This Family Is Simpler Than DataGrid

`StatusBar` is still a meaningful legacy-control recovery, but it is much smaller and more self-contained than `DataGrid`.

The main implementation concerns here are:

- panel collection behavior
- layout and spring or contents sizing
- simple-text versus multi-panel mode
- owner-draw dispatch
- native status-bar interop and sizing-grip behavior
- tooltip and click-event routing

That smaller scope is why the runtime port is much easier than `DataGrid`. The main remaining work is verification and any remaining designer or package-surface cleanup, not the core runtime or basic metadata restoration.

---

## Remaining Work

- Run the focused StatusBar tests in the intended build environment and confirm they stay green.
- Continue manual verification of painting and layout details, especially owner-draw panels, grip behavior, and border-style transitions.
- Review designer and serialization behavior for `StatusBar` and `StatusBarPanel` now that the runtime implementation is active again.
- Check whether any remaining obsolete or package-surface messaging still describes `StatusBar` as compatibility-only.
- Verify XLF entries round-trip correctly through the localization build pipeline.

## Practical Summary

The StatusBar migration is no longer just a checklist item. This branch restores the working `StatusBar` and `StatusBarPanel` runtime family, removes the old collection shim, restores the core upstream StatusBar-specific `SRDescription` metadata and base resource keys, adds focused tests and a dedicated demo, force-syncs all nine StatusBar family file declarations and XML documentation against the upstream sibling source, and populates the full set of StatusBar translation entries across all 13 locale XLF files.