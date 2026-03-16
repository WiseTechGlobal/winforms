# Legacy WinForms Control Migration Overview

## Purpose

This document is the entry point for legacy WinForms control migration work in this repository.

It replaces the previous all-in-one write-up with a smaller overview plus control-family-specific migration documents. The goal is to keep the shared architectural guidance in one place while letting each family track its own implementation status and recovery details.

## Architectural Baseline

The main architectural facts have not changed:

- The old controls under `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/` started as binary-compatibility shims.
- Those shims were designed to let old assemblies load, not to provide working runtime behavior.
- The upstream `.NET release/3.0` branch is still the right behavioral source of truth for restoring real support.
- Restoring a legacy control family requires host integration work in core framework types such as `Control` and `Form`, not just reviving the leaf types.

That means actual support should continue to be treated as a port-and-adapt effort, not as incremental extension of metadata-first shims.

## Document Map

- [Menu Stack Migration](legacy-controls-menu-stack.md)
- [StatusBar Migration](legacy-controls-statusbar.md)
- [ToolBar Migration](legacy-controls-toolbar.md)
- [DataGrid Migration](legacy-controls-datagrid.md)

## Current Status

| Control family | Current state | Notes |
| --- | --- | --- |
| Menu Stack | In progress, active focus | Runtime implementations and host integration have been restored on this branch. Remaining work is validation and product cleanup. |
| StatusBar | Ported | Runtime files have been restored, but focused validation and product-semantic cleanup are still pending. |
| ToolBar | Ported | Runtime files have been restored, with follow-up validation and packaging cleanup still pending. |
| DataGrid | Not yet ported | This remains the highest-risk family and should continue as a separate effort. |

## What The Current Branch Is Doing

The current branch is centered on the Menu Stack migration.

That work has already moved beyond a planning-only state:

- `Menu`, `MenuItem`, `ContextMenu`, and `MainMenu` have been restored as runtime implementations rather than throwing shims.
- `Control.ContextMenu` and `Form.Menu` / `Form.MergedMenu` have been rewired into real runtime behavior.
- Menu-specific Win32 message handling and handle lifecycle now participate in form behavior again.
- A dedicated demo project and a focused legacy test project were added to exercise the restored surface.

The detailed breakdown of those changes now lives in [Menu Stack Migration](legacy-controls-menu-stack.md).

## Shared Migration Principles

These principles still apply across all legacy control families:

1. Use upstream `release/3.0` as the behavioral baseline.
2. Port by control family, not by isolated member or file.
3. Restore owning-framework integration in the same phase as the control family.
4. Validate runtime behavior before changing product messaging.
5. Revisit obsolete attributes, IntelliSense visibility, and package description only after runtime support is credible.

## Recommended Sequence

The recommended implementation order remains:

1. Menu Stack
2. StatusBar
3. ToolBar
4. DataGrid

This order keeps early work focused on the smallest host-integration surface first, while deferring the much larger `DataGrid` recovery until the migration approach is proven.

## Cross-Cutting Risks

The main risks remain the same across all families:

- divergence between old `release/3.0` code and the current repository's internal APIs
- DPI, theming, and message-processing drift
- accessibility expectations that are newer than the original implementations
- designer and serialization behavior that may lag runtime support
- product inconsistency if restored runtime behavior is still documented as unsupported

## Test Expectations

Each family should eventually have focused coverage for:

- construction and disposal
- handle creation and recreation
- keyboard and mouse interaction
- message-loop integration
- designer and serialization smoke coverage where applicable
- accessibility smoke checks where applicable

For the Menu Stack specifically, focused tests and a demo are already present on the branch, but the work should still be treated as incomplete until those assets are run and the remaining product-semantic cleanup is settled.