# DataGrid Port Plan

## Scope

Source of truth for legacy implementations:

`C:\git\github\WiseTechGlobal\CargoWise.Controls.WinForms.Legacy\src\System.Windows.Forms\src\System\Windows\Forms\`

Target directory:

`src\System.Windows.Forms\System\Windows\Forms\Controls\Unsupported\DataGrid\`

This plan exists to port the legacy `DataGrid` family with the smallest practical amount of translation from the legacy source.

## Working Rules

Apply these rules to every file unless there is a strong reason not to:

1. Start from the legacy implementation, not the current shim.
2. Convert to file-scoped `namespace System.Windows.Forms;` when the file can be converted cleanly.
3. Keep `#nullable disable` unless the file is actually nullable-clean.
4. Keep the .NET Foundation license header.
5. Keep `[Obsolete(Obsoletions.DataGridMessage, ...)]`, `[EditorBrowsable(EditorBrowsableState.Never)]`, and `[Browsable(false)]` on public unsupported types.
6. Do not do broad C# 13 modernization in the first pass. Get the file compiling first.
7. Keep split partial files only where they preserve the legacy structure cleanly.

## Unsupported Compat Rule

When current-repo helpers, interop wrappers, or utility APIs differ from the legacy source, prefer a small local `*Compat.cs` adapter in `Controls/Unsupported/DataGrid/`.

That compat layer should adapt the current repo to the legacy code shape, not rewrite the legacy code to fit modern helpers.

Use a local compat file when:

- the mismatch is mechanical rather than architectural
- the need is specific to the Unsupported `DataGrid` port
- a narrow wrapper preserves the legacy flow with minimal translation
- changing shared infrastructure would broaden impact for little value

Typical cases:

- interop signatures that differ only by wrapper shape
- helper methods that moved or changed overload form
- small aliases needed to preserve near-verbatim legacy code

Avoid:

- changing shared interop just to make `DataGrid` compile
- rewriting large legacy call sites to match modern helper conventions
- spreading Unsupported-only compatibility concerns into shared layers

Example: if legacy code expects `User32.ScrollWindow(..., ref RECT, ref RECT)` and the current repo exposes a different wrapper shape, add a narrow local compat adapter instead of changing shared interop.

## Current Inventory

Already correct enough to leave alone until the main family compiles:

- `DataGridLineStyle.cs`
- `DataGridParentRowsLabel.cs`

Not in scope:

- `DataGridView.cs` and all `DataGridView.*.cs` files
- all `DataGridView*` column, cell, band, and related types

## Port Order

### Step 1: Small replacements

Replace the current shims with the legacy implementations for:

- `DataGridCell.cs`
- `IDataGridEditingService.cs`
- `GridTablesFactory.cs`

Acceptance: each file compiles in isolation.

### Step 2: Missing support files

Add:

- `DataGridAddNewRow.cs`
- `DataGridToolTip.cs`
- `DataGridState.cs`
- `DataGridCaption.cs`

`DataGridState.cs` remains `internal`.

Acceptance: all files compile without expanding the intended public API.

### Step 3: Type converter and editor control

Port:

- `DataGridPreferredColumnWidthTypeConverter.cs`
- `DataGridTextBox.cs`

Acceptance: `DataGridTextBox` is ready for `DataGridTextBoxColumn`.

### Step 4: Column styles

Replace and reconcile:

- `DataGridColumnStyle.cs`
- `DataGridColumnStyle.CompModSwitches.cs`
- `DataGridColumnStyle.DataGridColumnHeaderAccessibleObject.cs`
- `GridColumnStylesCollection.cs`

Acceptance: `DataGridBoolColumn` and `DataGridTextBoxColumn` can consume `DataGridColumnStyle` without compile errors.

### Step 5: Column implementations

Port:

- `DataGridBoolColumn.cs`
- `DataGridTextBoxColumn.cs`

Acceptance: both compile after Step 4.

### Step 6: Table styles

Port and reconcile:

- `DataGridTableStyle.cs`
- `GridTableStylesCollection.cs`

Acceptance: `DataGrid.cs` can consume the table-style graph.

### Step 7: Internal row and rendering types

Add:

- `DataGridParentRows.cs`
- `DataGridRelationshipRow.cs`
- `DataGridRow.cs`

These remain internal implementation types.

Acceptance: row and layout types compile without changing public API.

### Step 8: Main control

Port `DataGrid.cs`.

Also reconcile `HitTestInfo` and `HitTestType`, which currently exist as split shim files.

Expect first-pass errors around:

- missing `using` directives
- interop wrapper differences
- resource or `SRDescription` name drift
- moved accessibility APIs
- removed feature switches such as older accessibility-improvement toggles

Acceptance: the project builds cleanly and focused tests pass.

## Files To Restore Or Add

Main public or surface-adjacent files:

- `DataGrid.cs`
- `DataGridBoolColumn.cs`
- `DataGridCell.cs`
- `DataGridColumnStyle.cs`
- `DataGridPreferredColumnWidthTypeConverter.cs`
- `DataGridTableStyle.cs`
- `DataGridTextBox.cs`
- `DataGridTextBoxColumn.cs`
- `GridTablesFactory.cs`
- `IDataGridEditingService.cs`

Missing support files:

- `DataGridAddNewRow.cs`
- `DataGridCaption.cs`
- `DataGridColumnCollection.cs`
- `DataGridParentRows.cs`
- `DataGridRelationshipRow.cs`
- `DataGridRow.cs`
- `DataGridState.cs`
- `DataGridTableCollection.cs`
- `DataGridToolTip.cs`

Split files to reconcile as part of the port:

- `DataGridColumnStyle.CompModSwitches.cs`
- `DataGridColumnStyle.DataGridColumnHeaderAccessibleObject.cs`
- `GridColumnStylesCollection.cs`
- `GridTableStylesCollection.cs`
- `DataGrid.HitTestInfo.cs`
- `DataGrid.HitTestType.cs`

## Deferred Follow-Up

Do not block the initial port on broader cleanup. Leave these for a later pass unless they directly prevent the build:

- resource and `SRDescription` metadata differences versus the legacy source
- deeper legacy-alignment issues that need broader review
