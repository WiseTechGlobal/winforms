# Legacy Control Migration: ToolBar

## Scope

This document tracks the legacy `ToolBar` family and its supporting types:

- `ToolBar`
- `ToolBarButton`
- related click-event and enum types

## Current Status

The `ToolBar` family **has been ported**. All types have been replaced with full working implementations ported from the upstream runtime. The old stub layer — every file that previously threw `PlatformNotSupportedException` — has been replaced.

### Files ported / changed

| File | Change |
|---|---|
| `ToolBar.cs` | Replaced shim with full implementation including embedded `ToolBarButtonCollection` |
| `ToolBarButton.cs` | Full button component implementation (was throw-stubs) |
| `ToolBarAppearance.cs` | Enum — updated to file-scoped namespace with XML doc comments |
| `ToolBarButtonStyle.cs` | Enum — updated to file-scoped namespace with XML doc comments |
| `ToolBarTextAlign.cs` | Enum — updated to file-scoped namespace with XML doc comments |
| `ToolBarButtonClickEventArgs.cs` | Full event args implementation |
| `ToolBarButtonClickEventHandler.cs` | Delegate declaration |

### New files added

| File | Purpose |
|---|---|
| `LegacyToolBarInteropCompat.cs` | Compatibility shim providing `LegacyToolBarSR` (error message constants), `ToolBarNativeMethods` (TB\* Win32 constants, HICF flags enum, TBBUTTON / TBBUTTONINFO / NMTOOLBAR / NMTBHOTITEM / TOOLTIPTEXT structs, and low-level bit helpers) |

### Files deleted

| File | Reason |
|---|---|
| `ToolBar.ToolBarButtonCollection.cs` | Old throw-stub split file; real collection restored as a nested class inside `ToolBar.cs` |

### Resource changes

All `ToolBar`-family string resources (23 entries, e.g. `ToolBarAppearanceDescr`,
`ToolBarButtonClickDescr`, `ToolBarButtonsDescr`, etc.) have been added to `SR.resx` and all
13 language XLF files. The XLF files carry `state="translated"` with translations sourced
from the upstream `dotnet/winforms` repository.

---

## Migration Approach

### Step 1 — Copy the baseline from upstream

Take each file verbatim from the upstream `dotnet/winforms` source before the control was
removed from the main runtime. The starting point retains:

- the old-style block namespace (`namespace System.Windows.Forms { … }`)
- `#nullable disable`
- references to `NativeMethods`, `SafeNativeMethods`, `ClientUtils`, etc.

### Step 2 — Create `LegacyToolBarInteropCompat.cs`

The upstream source references internal helpers and Win32 constant definitions that no
longer exist in the current codebase. A thin compatibility shim is created in-folder:

| Shim type | Purpose |
|---|---|
| `LegacyToolBarSR` | Static string constants for the two hard-coded error messages (`ToolBarBadToolBarButton`, `ToolBarButtonInvalidDropDownMenuType`) |
| `ToolBarNativeMethods` | All `TB*` / `CCS_*` / `TBSTYLE_*` / `TBSTATE_*` / `TBIF_*` / `TBN_*` Win32 constants; `HICF` flags enum; `TBBUTTON`, `TBBUTTONINFO`, `NMTOOLBAR`, `NMTBHOTITEM`, `TOOLTIPTEXT` structs; `Util` helper with `MAKELONG` / `MAKELPARAM` / `HIWORD` / `LOWORD` |

The implementation files use `SourceGenerated.EnumValidator.Validate()` instead of the
upstream `ClientUtils.IsEnumValid()`, and `PInvokeCore.SendMessage` / `PInvoke.*` instead
of `NativeMethods.*`.

### Step 3 — Add `SRDescription` attributes and string resources

All public properties and events on `ToolBar` and `ToolBarButton` are decorated with
`[SRDescription(nameof(SR.ToolBar*Descr))]` attributes, matching the upstream metadata.
The 23 required SR keys were added to `SR.resx` and propagated to all 13 XLF files.

**`ToolBar.cs` — 14 attributes added:**
`Appearance`, `AutoSize`, `BorderStyle`, `Buttons`, `ButtonSize`, `Divider`,
`DropDownArrows`, `ImageList`, `ImageSize`, `ShowToolTips`, `TextAlign`, `Wrappable`,
`ButtonClick` (event), `ButtonDropDown` (event)

**`ToolBarButton.cs` — 10 attributes added:**
`DropDownMenu`, `Enabled`, `ImageIndex`, `ImageKey`, `PartialPush`, `Pushed`,
`Style`, `Text`, `ToolTipText`, `Visible`

---

## Remaining Work

- Focused runtime testing
- Painting and image-behavior validation
- Click and interaction verification
- Designer serialization checks
- Formal accessibility object review against current `AccessibleObject` patterns