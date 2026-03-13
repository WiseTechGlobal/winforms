// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable IDE0040, IDE1006, SA1005, SA1206, SA1400

#nullable disable

using System.Drawing;
using System.Runtime.InteropServices;
using DSR = System.Windows.Forms.DataGridStrings;

namespace System.Windows.Forms;
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
            ArgumentNullException.ThrowIfNull(dataGrid);
            Debug.Assert(dataGrid is not null, "can't attach a tool tip to a null grid");
            this.dataGrid = dataGrid;
        }

        // will ensure that the toolTip window was created
        public void CreateToolTipHandle()
        {
            if (tipWindow is null || tipWindow.Handle == IntPtr.Zero)
            {
                PInvoke.InitCommonControlsEx(new INITCOMMONCONTROLSEX
                {
                    dwSize = (uint)Marshal.SizeOf<INITCOMMONCONTROLSEX>(),
                    dwICC = INITCOMMONCONTROLSEX_ICC.ICC_TAB_CLASSES
                });

                var cparams = new CreateParams
                {
                    Parent = dataGrid.Handle,
                    ClassName = PInvoke.TOOLTIPS_CLASS,
                    Style = (int)PInvoke.TTS_ALWAYSTIP,
                };

                tipWindow = new NativeWindow();
                tipWindow.CreateHandle(cparams);

                PInvokeCore.SendMessage(tipWindow, PInvoke.TTM_SETMAXTIPWIDTH, 0, SystemInformation.MaxWindowTrackSize.Width);
                PInvoke.SetWindowPos(
                    tipWindow,
                    HWND.HWND_NOTOPMOST,
                    0, 0, 0, 0,
                    SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
                PInvokeCore.SendMessage(tipWindow, PInvoke.TTM_SETDELAYTIME, (nuint)PInvoke.TTDT_INITIAL, 0);
            }
        }

        public void AddToolTip(string toolTipString, IntPtr toolTipId, Rectangle iconBounds)
        {
            Debug.Assert(tipWindow is not null && tipWindow.Handle != IntPtr.Zero, "the tipWindow was not initialized, bailing out");
            if (iconBounds.IsEmpty)
            {
                throw new ArgumentNullException(nameof(iconBounds), DSR.DataGridToolTipEmptyIcon);
            }

            ArgumentNullException.ThrowIfNull(toolTipString);

            var info = new ToolInfoWrapper<DataGrid>(dataGrid, toolTipId, TOOLTIP_FLAGS.TTF_SUBCLASS, toolTipString, iconBounds);
            info.SendMessage(tipWindow, PInvoke.TTM_ADDTOOLW);
        }

        public void RemoveToolTip(IntPtr toolTipId)
        {
            var info = new ToolInfoWrapper<DataGrid>(dataGrid, toolTipId);
            info.SendMessage(tipWindow, PInvoke.TTM_DELTOOLW);
        }

        // will destroy the tipWindow
        public void Destroy()
        {
            Debug.Assert(tipWindow is not null, "how can one destroy a null window");
            tipWindow.DestroyHandle();
            tipWindow = null;
        }
    }
