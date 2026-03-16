# Legacy Control Migration: DataGrid

## Scope

This document tracks the legacy `DataGrid` family and its supporting types.

## Current Status

`DataGrid` remains the largest unfinished migration area.

The current branch checklist still shows the family as not yet ported, including major types such as:

- `DataGrid`
- `DataGridTableStyle`
- `DataGridColumnStyle`
- `DataGridTextBoxColumn`
- `DataGridBoolColumn`
- `GridTablesFactory`
- `IDataGridEditingService`
- several supporting row, caption, parent-row, tooltip, and collection types

## Why This Family Is Different

`DataGrid` is the highest-risk family for several reasons:

- very large implementation footprint
- custom layout and painting behavior
- data-binding interactions
- editing-service integration
- accessibility object complexity
- broader designer and serializer expectations

This is why the overall migration plan continues to treat `DataGrid` as a separate project rather than as a quick extension of the Menu Stack, `StatusBar`, or `ToolBar` work.

## Recommended Next Step

Do not start `DataGrid` member-by-member from the existing shim layer. Continue using the same migration rule:

1. take the upstream `release/3.0` runtime implementation as the baseline
2. port the family together
3. add focused tests for binding, editing, painting, and accessibility before considering the work stable