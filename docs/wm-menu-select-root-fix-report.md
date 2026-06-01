# WM_MENUSELECT root-fix report / WM_MENUSELECT 根因修复报告

Date: 2026-06-01T18:36:25.924+08:00

## English

### Why the root fix is valid
- In the legacy CargoWise WinForms repo, `Control.WmMenuSelect` handled `WM_MENUSELECT` and called `MenuItem.PerformSelect()` for both command and popup menu items.
- In the new WinForms repo, that behavior is restored through `Control.WmMenuSelect`, `Form.WmMenuSelect`, and `Menu.ProcessMenuSelect`.
- So the change is framework parity restoration, not an app-level workaround.
- CargoWise PR #53445 is the downstream workaround; this repo change restores the missing framework behavior.

### Test placement
- The tests now live in the new WinForms unit test tree:
  - `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/MenuSelectTests.cs`
  - `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/ContextMenuSubMenuPopupTests.cs`
- These tests cover:
  - command-item `WM_MENUSELECT` on `Control`
  - popup-item `WM_MENUSELECT` on `Form`
  - direct submenu `WM_INITMENUPOPUP`
  - nested submenu `WM_INITMENUPOPUP`

### Similar migration omissions to review
- The two regression tests originally lived under `src/System.Windows.Forms.Legacy/System.Windows.Forms.Legacy.Tests/`; that was the migration drift and has now been corrected.
- If we want fuller parity, review the remaining legacy-menu tests under `src/System.Windows.Forms.Legacy/System.Windows.Forms.Legacy.Tests/` (for example `ContextMenuTests.cs`, `MenuTests.cs`, `MenuItemTests.cs`, `MenuItemCollectionTests.cs`, and the `MainMenu*` tests) to decide whether they should also be mirrored into the new unit tree.

### Validation
- `dotnet build src\test\unit\System.Windows.Forms\System.Windows.Forms.Tests.csproj --no-restore` succeeded.
- `dotnet test` in this environment aborts because the local `testhost` package is unavailable, so build success is the reliable validation signal here.

## 中文

### 为什么这个根因修复是成立的
- 在旧的 CargoWise WinForms 仓库里，`Control.WmMenuSelect` 直接处理 `WM_MENUSELECT`，并且会对 command / popup 菜单项调用 `MenuItem.PerformSelect()`。
- 在新的 WinForms 仓库里，这个行为通过 `Control.WmMenuSelect`、`Form.WmMenuSelect` 和 `Menu.ProcessMenuSelect` 补回来了。
- 所以这次改动是框架行为补齐，不是应用层 workaround。
- CargoWise PR #53445 只是下游 workaround；当前这个仓库改动才是框架层的缺失修复。

### 测试放置位置
- 测试已经移动到新的 WinForms 单测目录：
  - `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/MenuSelectTests.cs`
  - `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/ContextMenuSubMenuPopupTests.cs`
- 这两组测试覆盖：
  - `Control` 上 command-item 的 `WM_MENUSELECT`
  - `Form` 上 popup-item 的 `WM_MENUSELECT`
  - 直接子菜单的 `WM_INITMENUPOPUP`
  - 嵌套子菜单的 `WM_INITMENUPOPUP`

### 其他类似遗漏的检查项
- 这两份回归测试原本放在 `src/System.Windows.Forms.Legacy/System.Windows.Forms.Legacy.Tests/` 下；这就是这次迁移漂移，已经修正。
- 如果要继续补齐更完整的 parity，建议继续审查 `src/System.Windows.Forms.Legacy/System.Windows.Forms.Legacy.Tests/` 下剩余的菜单类测试（例如 `ContextMenuTests.cs`、`MenuTests.cs`、`MenuItemTests.cs`、`MenuItemCollectionTests.cs` 以及各个 `MainMenu*` 测试），再决定是否需要同步到新的单测树。

### 验证
- `dotnet build src\test\unit\System.Windows.Forms\System.Windows.Forms.Tests.csproj --no-restore` 已成功。
- 这个环境下 `dotnet test` 会因为本地缺少 `testhost` 包而中止，所以当前最可靠的本地验证是编译成功。
