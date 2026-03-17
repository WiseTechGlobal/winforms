# WTG.Windows.Forms

WiseTech Global's fork of **System.Windows.Forms** that restores controls deprecated in .NET Core 3.1 and .NET 5, including `DataGrid`, `Menu`, `ToolBar`, and `StatusBar`.

## Overview

When Microsoft moved Windows Forms to .NET Core, several legacy controls were removed. This package brings those controls back, allowing WiseTech Global applications to migrate to modern .NET runtimes without requiring an immediate rewrite of UI code that relies on these controls.

## Getting Started

Add the package to your project:

```xml
<PackageReference Include="WTG.Windows.Forms" Version="*" />
```

Then use the restored controls as you would in .NET Framework — no API changes are required.

## Restored Controls

| Control | Description |
|---|---|
| `DataGrid` | Displays ADO.NET data in a scrollable grid |
| `MainMenu` / `ContextMenu` | Classic menu controls |
| `MenuItem` | Individual items within a `MainMenu` or `ContextMenu` |
| `ToolBar` / `ToolBarButton` | Legacy toolbar control |
| `StatusBar` / `StatusBarPanel` | Legacy status bar control |

## Source

This package is based on the [dotnet/winforms](https://github.com/dotnet/winforms) repository, which is itself a fork of the Windows Forms implementation from .NET Framework 4.8.

## License

Licensed under the [MIT License](https://github.com/dotnet/winforms/blob/main/LICENSE.TXT).
