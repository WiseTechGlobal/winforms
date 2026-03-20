// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using SafeNativeMethods = System.Windows.Forms.DataGridSafeNativeMethods;
using NativeMethods = System.Windows.Forms.DataGridNativeMethods;
using static Interop;

namespace System.Windows.Forms
{
    // this class is basically a NativeWindow that does toolTipping
    // should be one for the entire grid
    internal class DataGridToolTip : MarshalByRefObject
    {
        // the toolTip control
        private NativeWindow tipWindow = null;

        // the dataGrid which contains this toolTip
        private readonly DataGrid dataGrid = null;

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
                DataGridNativeMethods.INITCOMMONCONTROLSEX icc = new DataGridNativeMethods.INITCOMMONCONTROLSEX
                {
                    dwICC = DataGridNativeMethods.ICC_TAB_CLASSES
                };
                icc.dwSize = (uint)Marshal.SizeOf(icc);
                SafeNativeMethods.InitCommonControlsEx(icc);
                CreateParams cparams = new CreateParams
                {
                    Parent = dataGrid.Handle,
                    ClassName = DataGridNativeMethods.TOOLTIPS_CLASS,
                    Style = DataGridNativeMethods.TTS_ALWAYSTIP
                };
                tipWindow = new NativeWindow();
                tipWindow.CreateHandle(cparams);

                PInvokeCore.SendMessage(tipWindow, PInvoke.TTM_SETMAXTIPWIDTH, (WPARAM)0, (LPARAM)SystemInformation.MaxWindowTrackSize.Width);
                PInvoke.SetWindowPos(tipWindow, HWND.HWND_NOTOPMOST, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
                PInvokeCore.SendMessage(tipWindow, PInvoke.TTM_SETDELAYTIME, (WPARAM)PInvoke.TTDT_INITIAL);
            }
        }

        public void AddToolTip(string toolTipString, IntPtr toolTipId, Rectangle iconBounds)
        {
            Debug.Assert(tipWindow != null && tipWindow.Handle != IntPtr.Zero, "the tipWindow was not initialized, bailing out");
            if (iconBounds.IsEmpty)
                throw new ArgumentNullException(nameof(iconBounds), SR.DataGridToolTipEmptyIcon);

            if (toolTipString == null)
                throw new ArgumentNullException(nameof(toolTipString));

            RECT iconRect = new(iconBounds.Left, iconBounds.Top, iconBounds.Right, iconBounds.Bottom);
            ToolInfoWrapper<DataGrid> info = new(dataGrid, (nint)toolTipId, TOOLTIP_FLAGS.TTF_SUBCLASS, toolTipString, iconRect);
            info.SendMessage(tipWindow, PInvoke.TTM_ADDTOOLW);
        }

        public void RemoveToolTip(IntPtr toolTipId)
        {
            ToolInfoWrapper<DataGrid> info = new(dataGrid, (nint)toolTipId);
            info.SendMessage(tipWindow, PInvoke.TTM_DELTOOLW);
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
