# Legacy Control Migration: StatusBar

## Scope

This document tracks the legacy `StatusBar` family:

- `StatusBar`
- `StatusBarPanel`
- related draw, click, and enum types

## Current Status

The current branch indicates that the `StatusBar` family has been ported from the old runtime implementation into the repository.

The branch checklist marks the following as completed:

- `StatusBar.cs`
- `StatusBarDrawItemEventArgs.cs`
- `StatusBarDrawItemEventHandler.cs`
- `StatusBarPanel.cs`
- `StatusBarPanelAutoSize.cs`
- `StatusBarPanelBorderStyle.cs`
- `StatusBarPanelClickEventArgs.cs`
- `StatusBarPanelClickEventHandler.cs`
- `StatusBarPanelStyle.cs`
- replacement of the old split collection shim if the real collection implementation lives inside `StatusBar.cs`

## What Still Matters

Even with the files ported, this family still needs the same follow-up work as the Menu Stack:

- focused runtime validation
- rendering and layout verification
- event-behavior verification
- designer and serialization checks where applicable
- product-semantic cleanup if the public surface is no longer just compatibility-only

## Recommended Next Step

Treat the `StatusBar` family as implemented but not yet fully validated. The next useful work here is focused testing rather than more architectural planning.