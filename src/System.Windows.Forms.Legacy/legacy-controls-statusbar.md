# Legacy Control Migration: StatusBar

## Status

Migration complete. All nine StatusBar family files have been ported from the upstream source into `src/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/StatusBar`, force-synced against upstream declarations and documentation, and verified build-clean.

## Files ported / changed

| File | Change |
|---|---|
| `StatusBar.cs` | Working control implementation (layout, simple-text mode, panel realization, owner-draw dispatch, tooltip handling, `StatusBarPanelCollection`); upstream class summary and constructor doc synced |
| `StatusBarPanel.cs` | Working panel implementation (sizing, content measurement, icon handling, owner-draw realization, parent linkage); upstream class summary and explicit default constructor synced |
| `StatusBarDrawItemEventArgs.cs` | Working constructors and `Panel` property; upstream XML docs and license header synced |
| `StatusBarDrawItemEventHandler.cs` | Delegate surface; upstream summary and license header synced |
| `StatusBarPanelAutoSize.cs` | Real enum; full upstream `Spring` summary (cross-referencing `None` and `Contents`) synced |
| `StatusBarPanelBorderStyle.cs` | Real enum; license header synced |
| `StatusBarPanelClickEventArgs.cs` | Working constructor and `StatusBarPanel` property; upstream class and member summaries synced |
| `StatusBarPanelClickEventHandler.cs` | Delegate surface; upstream summary and license header synced |
| `StatusBarPanelStyle.cs` | Real enum; license header synced |

## Files deleted

| File | Reason |
|---|---|
| `StatusBar.StatusBarPanelCollection.cs` | `StatusBarPanelCollection` implementation now lives inside `StatusBar.cs` |

## Resource and metadata parity

Upstream `SRDescription` attributes restored for `StatusBar.cs` and `StatusBarPanel.cs` (`Panels`, `ShowPanels`, `SizingGrip`, `DrawItem`, `PanelClick`, and all panel properties). Matching keys added to `src/System.Windows.Forms/Resources/SR.resx`.

## XLF translation parity

15 StatusBar-specific trans-unit entries added to all 13 locale XLF files under `src/System.Windows.Forms/Resources/xlf`, sourced verbatim from the upstream sibling repository:

`StatusBarDrawItem` · `StatusBarOnPanelClickDescr` · `StatusBarPanelAlignmentDescr` · `StatusBarPanelAutoSizeDescr` · `StatusBarPanelBorderStyleDescr` · `StatusBarPanelIconDescr` · `StatusBarPanelMinWidthDescr` · `StatusBarPanelNameDescr` · `StatusBarPanelsDescr` · `StatusBarPanelStyleDescr` · `StatusBarPanelTextDescr` · `StatusBarPanelToolTipTextDescr` · `StatusBarPanelWidthDescr` · `StatusBarShowPanelsDescr` · `StatusBarSizingGripDescr`

Locales: `cs` · `de` · `es` · `fr` · `it` · `ja` · `ko` · `pl` · `pt-BR` · `ru` · `tr` · `zh-Hans` · `zh-Hant`

## Tests (`System.Windows.Forms.Legacy.Tests`)

`StatusBarTests.cs` covers: default property values, `StatusBarPanelCollection` operations (add, insert, remove, clear, contains, key lookup), panel-mode toggle, `ToString()`, panel property semantics (alignment, autosize, border style, owner-draw, width, min width, tooltip text), and `PanelClick`/`DrawItem` event dispatch.

## Demo (`System.Windows.Forms.Legacy.Demo`)

`StatusBarForm.cs` / `StatusBarForm.Designer.cs` exercise panel mode vs. simple-text mode, dynamic panel insertion/removal, sizing-grip toggle, border-style cycling, owner-draw rendering, per-panel tooltips, panel-click logging, and a live clock panel. Main demo launcher updated to open `StatusBar` directly.

## Remaining Work

- Run StatusBar tests in the build environment and confirm they stay green.
- Manually verify owner-draw panels, grip behavior, and border-style transitions.
- Review designer and serialization behavior for `StatusBar` and `StatusBarPanel`.
- Verify XLF entries round-trip through the localization build pipeline.