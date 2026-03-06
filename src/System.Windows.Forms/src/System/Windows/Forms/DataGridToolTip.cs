// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Drawing;
using System.Runtime.InteropServices;
using static Interop;

namespace System.Windows.Forms
{
    // this class is basically a NativeWindow that does toolTipping
    // should be one for the entire grid
    internal class DataGridToolTip : MarshalByRefObject
    {
        // the toolTip control
        private NativeWindow tipWindow;

        // the dataGrid which contains this toolTip
        private readonly DataGrid dataGrid;

        // CONSTRUCTOR
        public DataGridToolTip(DataGrid dataGrid)
        {
            Debug.Assert(dataGrid != null, "can't attach a tool tip to a null grid");
            this.dataGrid = dataGrid;
        }

        // will ensure that the toolTip window was created
        public void CreateToolTipHandle()
        {
            if (tipWindow == null || tipWindow.Handle == IntPtr.Zero)
            {
                var icc = new ComCtl32.INITCOMMONCONTROLSEX
                {
                    dwICC = ComCtl32.ICC.TAB_CLASSES
                };
                ComCtl32.InitCommonControlsEx(ref icc);

                var cparams = new CreateParams
                {
                    Parent = dataGrid.Handle,
                    ClassName = ComCtl32.WindowClasses.TOOLTIPS_CLASS,
                    Style = (int)ComCtl32.TTS.ALWAYSTIP,
                };
                tipWindow = new NativeWindow();
                tipWindow.CreateHandle(cparams);

                User32.SendMessageW((IHandle<HWND>)tipWindow, (User32.WM)ComCtl32.TTM.SETMAXTIPWIDTH, IntPtr.Zero, (IntPtr)SystemInformation.MaxWindowTrackSize.Width);
                User32.SetWindowPos(
                    new HandleRef(tipWindow, tipWindow.Handle),
                    User32.HWND_NOTOPMOST,
                    flags: User32.SWP.NOSIZE | User32.SWP.NOMOVE | User32.SWP.NOACTIVATE);
                User32.SendMessageW((IHandle<HWND>)tipWindow, (User32.WM)ComCtl32.TTM.SETDELAYTIME, (IntPtr)ComCtl32.TTDT.INITIAL, (IntPtr)0);
            }
        }

        public void AddToolTip(string toolTipString, IntPtr toolTipId, Rectangle iconBounds)
        {
            Debug.Assert(tipWindow != null && tipWindow.Handle != IntPtr.Zero, "the tipWindow was not initialized, bailing out");
            if (iconBounds.IsEmpty)
                throw new ArgumentNullException(nameof(iconBounds), SR.DataGridToolTipEmptyIcon);

            if (toolTipString == null)
                throw new ArgumentNullException(nameof(toolTipString));

            var info = new ComCtl32.ToolInfoWrapper<DataGrid>(dataGrid, toolTipId, TOOLTIP_FLAGS.TTF_SUBCLASS, toolTipString, iconBounds);
            info.SendMessage(tipWindow, PInvoke.TTM_ADDTOOLW);
        }

        public void RemoveToolTip(IntPtr toolTipId)
        {
            var info = new ComCtl32.ToolInfoWrapper<DataGrid>(dataGrid, toolTipId);
            info.SendMessage(tipWindow, PInvoke.TTM_ADDTOOLW);
        }

        // will destroy the tipWindow
        public void Destroy()
        {
            Debug.Assert(tipWindow != null, "how can one destroy a null window");
            tipWindow.DestroyHandle();
            tipWindow = null;
        }
    }
}
