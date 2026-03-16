# Legacy Control Migration: ToolBar

## Scope

This document tracks the legacy `ToolBar` family:

- `ToolBar`
- `ToolBarButton`
- related click-event and enum types

## Current Status

The current branch indicates that the `ToolBar` family has been ported.

Completed items on the branch checklist are:

- `ToolBar.cs`
- `ToolBarAppearance.cs`
- `ToolBarButton.cs`
- `ToolBarButtonClickEventArgs.cs`
- `ToolBarButtonClickEventHandler.cs`
- `ToolBarButtonStyle.cs`
- `ToolBarTextAlign.cs`
- removal of the old split `ToolBarButtonCollection` shim after restoring the real collection implementation inside `ToolBar.cs`

## What Still Matters

Ported code is not the same as fully accepted support. This family still needs:

- focused runtime testing
- painting and image-behavior validation
- click and interaction verification
- designer serialization checks where needed
- cleanup of any obsolete or compatibility-only product messaging that is no longer accurate

## Recommended Next Step

Keep `ToolBar` behind the Menu Stack in validation priority, but treat it as the next candidate for focused verification once Phase 1 is stabilized.