# WM_MENUSELECT Root-Fix Report / WM_MENUSELECT 根因修复报告

Date: 2026-06-01T18:36:25.924+08:00

## English

### Why the root fix is justified
- In the old CargoWise legacy repo, `Control.WmMenuSelect` handled `WM_MENUSELECT` inline and `Control.WmExitMenuLoop` raised `ContextMenu.Collapse`.
- In the new winforms fork, `WM_MENUSELECT` was missing from the legacy menu flow and `WM_EXITMENULOOP` still fell through to the default path, so both `MenuItem.Select` and `ContextMenu.Collapse` parity were at risk.
- The current winforms fix restores framework parity in three places: `Control.WndProc`, `Form.WndProc`, and `Menu.ProcessMenuSelect`, plus the restored `Control.WmExitMenuLoop` path.

### Why the test belongs in the new winforms repo
- The regression tests live under `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/`.
- That is the correct home because the bug is on the legacy menu surface (`ContextMenu`, `MainMenu`, and `MenuItem.Select`).
- The tests currently prove three important paths: command-item selection from `Control`, popup-item selection from `Form`, and context-menu collapse on `WM_EXITMENULOOP`.

### Similar omissions still worth tracking
- The only clear code-path omission found in this comparison was `Control.WmExitMenuLoop`; that is now fixed.
- Test coverage could still be hardened around the `MF_POPUP` nested-submenu branch and the `MF_SYSMENU` negative path in `Menu.ProcessMenuSelect`.

### Draft PR wording
**Title**
Restore `WM_MENUSELECT` dispatch for legacy menus

**Body**
- Restore framework-level `WM_MENUSELECT` routing so legacy `MenuItem.Select` fires again for both command and popup items.
- Restore `WM_EXITMENULOOP` routing so legacy `ContextMenu.Collapse` still fires.
- Keep the new winforms behavior aligned with the old legacy repo instead of forcing app code to rely on workarounds.
- Add regression coverage in the new `System.Windows.Forms` unit test tree for the control and form entry points.

## 中文

### 为什么这个根因修复是成立的
- 在旧的 CargoWise legacy 仓库里，`Control.WmMenuSelect` 直接处理 `WM_MENUSELECT`，而 `Control.WmExitMenuLoop` 会触发 `ContextMenu.Collapse`。
- 在新的 winforms fork 里，`WM_MENUSELECT` 的 legacy 菜单分发路径缺失，`WM_EXITMENULOOP` 也仍然会落到默认分支，所以 `MenuItem.Select` 和 `ContextMenu.Collapse` 都有回归风险。
- 现在的 winforms 修复把框架行为补回到三处：`Control.WndProc`、`Form.WndProc`、`Menu.ProcessMenuSelect`，并恢复了 `Control.WmExitMenuLoop` 路径。

### 为什么测试应该放在新的 winforms 仓库里
- 回归测试位于 `src/test/unit/System.Windows.Forms/System/Windows/Forms/Controls/Unsupported/ContextMenu/`。
- 这个位置是正确的，因为问题出在 legacy 菜单表面（`ContextMenu`、`MainMenu`、`MenuItem.Select`）。
- 现有测试已经覆盖三个关键路径：`Control` 的 command-item 选择、`Form` 的 popup-item 选择，以及 `WM_EXITMENULOOP` 的 collapse 事件。

### 仍然建议继续补齐的类似遗漏
- 本次对比里，唯一明确的代码路径遗漏就是 `Control.WmExitMenuLoop`；这项已经修复。
- 测试覆盖还可以继续加固 `Menu.ProcessMenuSelect` 的 `MF_POPUP` 嵌套子菜单分支和 `MF_SYSMENU` 负向路径。

### PR 文案草稿
**标题**
恢复 legacy 菜单的 `WM_MENUSELECT` 分发

**正文**
- 恢复框架级 `WM_MENUSELECT` 路由，让 legacy `MenuItem.Select` 在 command 和 popup 菜单项上重新触发。
- 恢复 `WM_EXITMENULOOP` 路由，让 legacy `ContextMenu.Collapse` 继续触发。
- 让新的 winforms 行为继续和旧的 legacy 仓库保持一致，而不是依赖下游应用把问题当成 workaround。
- 在新的 `System.Windows.Forms` 单元测试树中补上 control 和 form 两个入口的回归测试。
