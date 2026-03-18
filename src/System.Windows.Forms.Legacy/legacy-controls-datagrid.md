# Legacy Control Migration: DataGrid

## Scope

This document tracks the legacy `DataGrid` family and its supporting types.

## Current Status

The `DataGrid` family **has been ported** (uncommitted, in-progress). All major types have been replaced with full working implementations ported from the `release/3.0` upstream runtime. The old stub layer — every file that previously threw `PlatformNotSupportedException` — has been replaced.

### Files ported / changed

| File | Change |
|---|---|
| `DataGrid.cs` | Replaced shim with full ~10 000-line implementation |
| `DataGridBoolColumn.cs` | Full column implementation (was throw-stubs) |
| `DataGridColumnStyle.cs` | Full abstract base (was throw-stubs) |
| `DataGridTableStyle.cs` | Full table style (was throw-stubs) |
| `DataGridTextBox.cs` | Full embedded text-box control |
| `DataGridTextBoxColumn.cs` | Full text-box column |
| `DataGridCell.cs` | Full struct implementation |
| `DataGridLineStyle.cs` | Enum — updated namespace style |
| `DataGridParentRowsLabel.cs` | Enum — updated namespace style |
| `DataGridPreferredColumnWidthTypeConverter.cs` | Full converter |
| `GridColumnStylesCollection.cs` | Full collection |
| `GridTableStylesCollection.cs` | Full collection |
| `GridTablesFactory.cs` | Full factory |
| `IDataGridEditingService.cs` | Full interface (was throw-stub) |

### New files added

| File | Purpose |
|---|---|
| `DataGrid.Compatibility.cs` | Adapter layer bridging `release/3.0` idioms to current platform APIs (see below) |
| `DataGridAddNewRow.cs` | Internal add-new-row row type |
| `DataGridCaption.cs` | Internal caption bar rendering |
| `DataGridParentRows.cs` | Parent-row navigation panel |
| `DataGridRelationshipRow.cs` | Relationship-navigation row |
| `DataGridRow.cs` | Base row class |
| `DataGridState.cs` | Internal navigation-state snapshot |
| `DataGridToolTip.cs` | Tooltip helper |

### Files deleted (content inlined or replaced)

| File | Reason |
|---|---|
| `DataGrid.HitTestInfo.cs` | Merged into `DataGrid.cs` as a nested class |
| `DataGrid.HitTestType.cs` | Merged into `DataGrid.cs` as a nested enum |
| `DataGridColumnStyle.CompModSwitches.cs` | Moved to `DataGrid.Compatibility.cs` as `CompModSwitches` |
| `DataGridColumnStyle.DataGridColumnHeaderAccessibleObject.cs` | Inline in `DataGridColumnStyle.cs` |

### Resource changes

All `DataGrid`-family string resources (~90 entries, e.g. `DataGridAllowSortingDescr`,
`DataGridColumnCollectionMissing`, etc.) have been added to `SR.resx` and all
13 language XLF files.

### Demo changes (`System.Windows.Forms.Legacy.Demo`)

`DataGridForm.cs` / `DataGridForm.Designer.cs` have been extended with:
- Buttons 7–11 exercising color schemes, border styles, cell navigation, row selection, and parent-row label styles.
- Checkboxes for `ColumnHeadersVisible`, `AllowSorting`, and grid-lines visibility to make the demo a more complete smoke-test harness.

---

## Migration Approach

### Step 1 — Copy the baseline from `release/3.0`

Take each file verbatim from the upstream `dotnet/winforms release/3.0` branch.
The starting point is the last version of each file before the control was
removed from the main runtime. The files retain:

- the old-style block namespace (`namespace System.Windows.Forms { … }`)
- `#nullable disable`
- `using` aliases for `SafeNativeMethods`, `UnsafeNativeMethods`, `NativeMethods`
- references to `CompModSwitches`, `DpiHelper`, `WindowMessages`, `ClientUtils`

### Step 2 — Create `DataGrid.Compatibility.cs`

The `release/3.0` source references several internal helpers that no longer
exist in the current codebase or have been renamed/restructured. Rather than
editing every use site immediately, a thin compatibility shim file is created:

| Shim type | Maps to current API |
|---|---|
| `DataGridClientUtils.IsEnumValid` | Implements the old `ClientUtils.IsEnumValid` contract |
| `WindowMessages` (constants) | Re-exposes WM_* / TTM_* from `PInvokeCore` / `PInvoke` |
| `DpiHelper.GetBitmapFromIcon` | Delegates to `ScaleHelper.GetIconResourceAsDefaultSizeBitmap` |
| `DataGridNativeMethods` | Exposes `INITCOMMONCONTROLSEX`, `TOOLTIPS_CLASS`, `TTS_ALWAYSTIP`, `WHEEL_DELTA` |
| `DataGridSafeNativeMethods` | Wraps `PInvoke.InitCommonControlsEx` and `PInvoke.ScrollWindow` |
| `DataGridUnsafeNativeMethods` | Wraps `HRGN.GetRegionRects` |
| `CompModSwitches` | Provides `TraceSwitch` instances for all DataGrid diagnostic categories |
| `TriangleDirection` / `Triangle` | Internal triangle-rendering helper used by caption |

The `using` aliases at the top of each ported file resolve against these shims:
```csharp
using SafeNativeMethods = System.Windows.Forms.DataGridSafeNativeMethods;
using UnsafeNativeMethods = System.Windows.Forms.DataGridUnsafeNativeMethods;
using NativeMethods = System.Windows.Forms.DataGridNativeMethods;
```

### Step 3 — Add string resources

All string literals that the implementation retrieves via `SR.GetString(…)` must
exist in `SR.resx`. Add the missing DataGrid-family entries there and re-run the
localization tooling to propagate them to the XLF files.

### Step 4 — Wire up the demo

Update `DataGridForm.cs` and its designer file to exercise the newly live
surface so that every common API path can be verified interactively before tests
are written.

---

## Why This Family Is Different

`DataGrid` is the highest-risk family in the legacy migration for several reasons:

- very large implementation footprint (~10 000 lines in `DataGrid.cs` alone)
- custom layout and painting behavior
- data-binding and `CurrencyManager` interactions
- editing-service integration (`IDataGridEditingService`)
- accessibility object complexity
- broader designer and serializer expectations

This is why the overall migration plan treats `DataGrid` as a separate project
rather than a quick extension of the Menu Stack, `StatusBar`, or `ToolBar` work.

---

## Remaining Work

- Formal accessibility object pass (the `release/3.0` accessibility objects need review against current `AccessibleObject` patterns)
- Focused unit tests for: data binding, editing, painting, navigation, and hit-testing
- Consider whether to modernise the ported code to C# 13 / NRT patterns or keep it in the `release/3.0` style for easier upstream comparison