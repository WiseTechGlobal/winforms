using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.WindowsAndMessaging;
using DrawingPoint = System.Drawing.Point;
using DrawingSize = System.Drawing.Size;

namespace System.Windows.Forms.Legacy.Tests
{
    public class ToolBarToolTipTests
    {
        // Minimal interop needed for this test (use internal wrappers/constants where available)

        // Hot item flags
        [Flags]
        private enum HICF : uint
        {
            ENTERING = 0x0010,
        }

        private const uint TbnHotItemChange = unchecked((uint)-713);

        [StructLayout(LayoutKind.Sequential)]
        private struct NMTBHOTITEM
        {
            public NMHDR hdr;
            public int idOld;
            public int idNew;
            public HICF dwFlags;
        }

        // High DPI thread awareness helpers (User32)
        [DllImport("user32.dll", EntryPoint = "SetThreadDpiAwarenessContext", ExactSpelling = true)]
        private static extern IntPtr SetThreadDpiAwarenessContext(IntPtr dpiContext);

        [DllImport("user32.dll", EntryPoint = "GetThreadDpiAwarenessContext", ExactSpelling = true)]
        private static extern IntPtr GetThreadDpiAwarenessContext();

        // Known DPI_AWARENESS_CONTEXT values (casted from macros):
        // https://learn.microsoft.com/windows/win32/hidpi/dpi-awareness-context
        private static readonly IntPtr s_dpiAwarenessContextPerMonitorAwareV2 = new(-4);

        private sealed class DpiAwarenessScope : IDisposable
        {
            private readonly IntPtr _old;
            private readonly bool _enabled;

            public DpiAwarenessScope(IntPtr newContext)
            {
                try
                {
                    _old = GetThreadDpiAwarenessContext();
                    _enabled = SetThreadDpiAwarenessContext(newContext) != IntPtr.Zero;
                }
                catch (EntryPointNotFoundException)
                {
                    _enabled = false; // OS doesn't support per-thread DPI; proceed without changing
                }
            }

            public void Dispose()
            {
                if (_enabled)
                {
                    try
                    {
                        SetThreadDpiAwarenessContext(_old);
                    }
                    catch (EntryPointNotFoundException)
                    {
                        /* ignore */
                    }
                }
            }
        }

        [StaFact]
        public void ToolBar_ToolTip_Show_DoesNotMove_ToolBar()
        {
            using var form = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new DrawingPoint(100, 100),
                Size = new DrawingSize(400, 200)
            };

            var toolBar = new ToolBar
            {
                ShowToolTips = true,
                Dock = DockStyle.None,
                Location = new DrawingPoint(0, 0)
            };

            toolBar.Buttons.Add(new ToolBarButton("Btn") { ToolTipText = "Tip" });
            form.Controls.Add(toolBar);

            form.Show();
            Application.DoEvents();

            // Precondition: toolbar starts at 0,0
            Assert.Equal(new DrawingPoint(0, 0), toolBar.Location);

            // Get the native tooltip HWND created by the toolbar
            HWND tooltipHwnd = (HWND)PInvokeCore.SendMessage(toolBar, PInvoke.TB_GETTOOLTIPS);
            Assert.True(tooltipHwnd != HWND.Null, "Expected native tooltip window");

            // Force the tooltip window top-left at (0,0) so TTN_SHOW logic will try to reposition it
            PInvoke.SetWindowPos(tooltipHwnd, HWND.Null, 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);

            // 1) Simulate hot item change so internal hotItem != -1
            var hot = new NMTBHOTITEM
            {
                hdr = new NMHDR { hwndFrom = (HWND)toolBar.Handle, idFrom = 0, code = TbnHotItemChange },
                idOld = -1,
                idNew = 0,
                dwFlags = HICF.ENTERING
            };
            PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref hot);

            Application.DoEvents();

            // 2) Simulate TTN_SHOW from the tooltip window
            var nmhdr = new NMHDR { hwndFrom = tooltipHwnd, idFrom = 0, code = PInvoke.TTN_SHOW };
            PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref nmhdr);

            Application.DoEvents();

            // Assertion: Showing the tooltip must NOT move the toolbar.
            // This would fail under the original bug where SetWindowPos targeted the toolbar HWND.
            Assert.Equal(new DrawingPoint(0, 0), toolBar.Location);

            form.Close();
        }

        [StaFact]
        public void ToolBar_ToolTip_Show_Returns_1_When_Tooltip_Is_At_0_0()
        {
            using var form = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new DrawingPoint(200, 200),
                Size = new DrawingSize(400, 200)
            };

            var toolBar = new ToolBar
            {
                ShowToolTips = true,
                Dock = DockStyle.None,
                Location = new DrawingPoint(10, 10)
            };

            toolBar.Buttons.Add(new ToolBarButton("Btn") { ToolTipText = "Tip" });
            form.Controls.Add(toolBar);

            form.Show();
            Application.DoEvents();

            // Ensure toolbar is not at 0,0 so wrong GetWindowPlacement handle avoids the reposition branch
            Assert.Equal(new DrawingPoint(10, 10), toolBar.Location);

            // Acquire native tooltip HWND
            HWND tooltipHwnd = (HWND)PInvokeCore.SendMessage(toolBar, PInvoke.TB_GETTOOLTIPS);
            Assert.NotEqual(HWND.Null, tooltipHwnd);

            // Force the tooltip at (0,0) to trigger the reposition path when correct handle is used
            PInvoke.SetWindowPos(tooltipHwnd, HWND.Null, 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);

            // Simulate hot item change so hotItem != -1
            var hot = new NMTBHOTITEM
            {
                hdr = new NMHDR { hwndFrom = (HWND)toolBar.Handle, idFrom = 0, code = TbnHotItemChange },
                idOld = -1,
                idNew = 0,
                dwFlags = HICF.ENTERING
            };
            PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref hot);

            Application.DoEvents();

            // Simulate TTN_SHOW and capture the return value from WndProc
            var nmhdr = new NMHDR { hwndFrom = tooltipHwnd, idFrom = 0, code = PInvoke.TTN_SHOW };
            nint ret = PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref nmhdr);

            // Expect: when the correct window (tooltip) is queried for placement, TTN_SHOW repositions and returns 1.
            // With the buggy code that queries the toolbar's placement, the condition won't trigger and ret will be 0.
            Assert.Equal((nint)1, ret);

            form.Close();
        }

        [StaFact]
        public void ToolBar_ToolTip_TTN_SHOW_PerMonitorV2_DoesNotMove_And_Returns_1()
        {
            // Enter Per-Monitor V2 DPI context for the thread (best effort; no-op on older OS)
            using var scope = new DpiAwarenessScope(s_dpiAwarenessContextPerMonitorAwareV2);

            using var form = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new DrawingPoint(300, 300),
                Size = new DrawingSize(500, 300)
            };

            var toolBar = new ToolBar
            {
                ShowToolTips = true,
                Dock = DockStyle.None,
                Location = new DrawingPoint(12, 12)
            };

            toolBar.Buttons.Add(new ToolBarButton("Btn") { ToolTipText = "Tip" });
            form.Controls.Add(toolBar);

            form.Show();
            Application.DoEvents();

            var originalLocation = toolBar.Location;

            // Acquire native tooltip HWND
            HWND tooltipHwnd = (HWND)PInvokeCore.SendMessage(toolBar, PInvoke.TB_GETTOOLTIPS);
            Assert.NotEqual(HWND.Null, tooltipHwnd);

            // Force tooltip to (0,0) so TTN_SHOW reposition path is taken
            PInvoke.SetWindowPos(tooltipHwnd, HWND.Null, 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);

            // Simulate hot item change so hotItem != -1
            var hot = new NMTBHOTITEM
            {
                hdr = new NMHDR { hwndFrom = (HWND)toolBar.Handle, idFrom = 0, code = TbnHotItemChange },
                idOld = -1,
                idNew = 0,
                dwFlags = HICF.ENTERING
            };
            PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref hot);

            Application.DoEvents();

            // Simulate TTN_SHOW and capture return value
            var nmhdr = new NMHDR { hwndFrom = tooltipHwnd, idFrom = 0, code = PInvoke.TTN_SHOW };
            nint ret = PInvokeCore.SendMessage(toolBar, MessageId.WM_REFLECT_NOTIFY, 0, ref nmhdr);

            Application.DoEvents();

            // Assertions: TTN_SHOW is handled (ret==1) and the toolbar itself does not move
            Assert.Equal((nint)1, ret);
            Assert.Equal(originalLocation, toolBar.Location);

            form.Close();
        }
    }
}
