// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using static System.Windows.Forms.ToolStrip;
using static Interop;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Represents a menu structure for a form.
    /// </summary>
    [ToolboxItemFilter("System.Windows.Forms.MainMenu")]
    public class MainMenu : Menu
    {
        internal Form? _form;
        internal Form ownerForm;  // this is the form that created this menu, and is the only form allowed to dispose it.
        private RightToLeft _rightToLeft = RightToLeft.Inherit;
        private EventHandler _onCollapse;


#if DEBUG
        internal static readonly TraceSwitch s_selectionDebug = new("SelectionDebug", "Debug ToolStrip Selection code");
        internal static readonly TraceSwitch s_dropTargetDebug = new("DropTargetDebug", "Debug ToolStrip Drop code");
        internal static readonly TraceSwitch s_layoutDebugSwitch = new("Layout debug", "Debug ToolStrip layout code");
        internal static readonly TraceSwitch s_mouseActivateDebug = new("ToolStripMouseActivate", "Debug ToolStrip WM_MOUSEACTIVATE code");
        internal static readonly TraceSwitch s_mergeDebug = new("ToolStripMergeDebug", "Debug toolstrip merging");
        internal static readonly TraceSwitch s_snapFocusDebug = new("SnapFocus", "Debug snapping/restoration of focus");
        internal static readonly TraceSwitch s_flickerDebug = new("FlickerDebug", "Debug excessive calls to Invalidate()");
        internal static readonly TraceSwitch s_itemReorderDebug = new("ItemReorderDebug", "Debug excessive calls to Invalidate()");
        internal static readonly TraceSwitch s_mdiMergeDebug = new("MDIMergeDebug", "Debug toolstrip MDI merging");
        internal static readonly TraceSwitch s_menuAutoExpandDebug = new("MenuAutoExpand", "Debug menu auto expand");
        internal static readonly TraceSwitch s_controlTabDebug = new("ControlTab", "Debug ToolStrip Control+Tab selection");
#else
    internal static readonly TraceSwitch? s_selectionDebug;
    internal static readonly TraceSwitch? s_dropTargetDebug;
    internal static readonly TraceSwitch? s_layoutDebugSwitch;
    internal static readonly TraceSwitch? s_mouseActivateDebug;
    internal static readonly TraceSwitch? s_mergeDebug;
    internal static readonly TraceSwitch? s_snapFocusDebug;
    internal static readonly TraceSwitch? s_flickerDebug;
    internal static readonly TraceSwitch? s_itemReorderDebug;
    internal static readonly TraceSwitch? s_mdiMergeDebug;
    internal static readonly TraceSwitch? s_menuAutoExpandDebug;
    internal static readonly TraceSwitch? s_controlTabDebug;
#endif

        private Point _mouseEnterWhenShown = s_invalidMouseEnter;
        internal static Point s_invalidMouseEnter = new(int.MaxValue, int.MaxValue);
        private HWND _hwndThatLostFocus;

        /// <summary> handy check for painting and sizing </summary>
        [Browsable(false)]
        public bool IsDropDown
        {
            get { return (this is ToolStripDropDown); }
        }

        // remembers the current mouse location so we can determine
        // later if we need to shift selection.
        internal void SnapMouseLocation()
        {
            _mouseEnterWhenShown = WindowsFormsUtils.LastCursorPoint;
        }

        /// <summary> SnapFocus
        ///  When get focus to the toolstrip (and we're not participating in the tab order)
        ///  it's probably cause someone hit the ALT key. We need to remember who that was
        ///  so when we're done here we can RestoreFocus back to it.
        ///
        ///  We're called from WM_SETFOCUS, and otherHwnd is the HWND losing focus.
        ///
        ///  Required checks
        ///  - make sure it's not a dropdown
        ///  - make sure it's not a child control of this control.
        ///  - make sure the control is on this window
        /// </summary>
        private void SnapFocus(HWND otherHwnd)
        {
#if DEBUG
            Debug.WriteLineIf(s_snapFocusDebug.TraceVerbose, $"{!Environment.StackTrace.Contains("FocusInternal")}", "who is setting focus to us?");
#endif
            // we need to know who sent us focus so we know who to send it back to later.

            if (!TabStop && !IsDropDown)
            {
                bool snapFocus = false;
                if (Focused && (otherHwnd != Handle))
                {
                    // the case here is a label before a combo box calling FocusInternal in ProcessMnemonic.
                    // we'll filter out children later.
                    snapFocus = true;
                }
                else if (!ContainsFocus && !Focused)
                {
                    snapFocus = true;
                }

                if (snapFocus)
                {
                    // remember the current mouse position so that we can check later if it actually moved
                    // otherwise we'd unexpectedly change selection to whatever the cursor was over at this moment.
                    SnapMouseLocation();

                    // make sure the otherHandle is not a child of thisHandle
                    if ((Handle != otherHwnd) && !PInvoke.IsChild(this, otherHwnd))
                    {
                        // make sure the root window of the otherHwnd is the same as
                        // the root window of thisHwnd.
                        HWND thisHwndRoot = PInvoke.GetAncestor(this, GET_ANCESTOR_FLAGS.GA_ROOT);
                        HWND otherHwndRoot = PInvoke.GetAncestor(otherHwnd, GET_ANCESTOR_FLAGS.GA_ROOT);

                        if (thisHwndRoot == otherHwndRoot && !thisHwndRoot.IsNull)
                        {
                            s_snapFocusDebug.TraceVerbose(
                                $"[ToolStrip SnapFocus]: Caching for return focus:{WindowsFormsUtils.GetControlInformation(otherHwnd)}");

                            // We know we're in the same window heirarchy.
                            _hwndThatLostFocus = otherHwnd;
                        }
                    }
                }
            }
        }

        private ToolStripItem? _lastMouseActiveItem;
        private ToolStripItem? _lastMouseDownedItem;

        internal bool IsInDesignMode
        {
            get
            {
                return DesignMode;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.MsgInternal == PInvoke.WM_SETFOCUS)
            {
                SnapFocus((HWND)(nint)m.WParamInternal);
            }

            if (m.MsgInternal == PInvoke.WM_MOUSEACTIVATE)
            {
                // We want to prevent taking focus if someone clicks on the toolstrip dropdown itself. The mouse message
                // will still go through, but focus won't be taken. If someone clicks on a child control (combobox,
                // textbox, etc) focus will be taken - but we'll handle that in WM_NCACTIVATE handler.
                Point pt = PointToClient(WindowsFormsUtils.LastCursorPoint);
                HWND hwndClicked = PInvoke.ChildWindowFromPointEx(
                    this,
                    pt,
                    CWP_FLAGS.CWP_SKIPINVISIBLE | CWP_FLAGS.CWP_SKIPDISABLED | CWP_FLAGS.CWP_SKIPTRANSPARENT);

                // If we click on the toolstrip itself, eat the activation.
                // If we click on a child control, allow the toolstrip to activate.
                if (hwndClicked == HWND)
                {
                    _lastMouseDownedItem = null;
                    m.ResultInternal = (LRESULT)(nint)PInvoke.MA_NOACTIVATE;

                    if (!IsDropDown && !IsInDesignMode)
                    {
                        // If our root HWND is not the active hwnd,eat the mouse message and bring the form to the front.
                        HWND rootHwnd = PInvoke.GetAncestor(this, GET_ANCESTOR_FLAGS.GA_ROOT);
                        if (!rootHwnd.IsNull)
                        {
                            // snap the active window and compare to our root window.
                            HWND hwndActive = PInvoke.GetActiveWindow();
                            if (hwndActive != rootHwnd)
                            {
                                // Activate the window, and discard the mouse message.
                                // this appears to be the same behavior as office.
                                m.ResultInternal = (LRESULT)(nint)PInvoke.MA_ACTIVATEANDEAT;
                            }
                        }
                    }

                    return;
                }
                else
                {
                    // We're setting focus to a child control - remember who gave it to us so we can restore it on ESC.
                    SnapFocus(PInvoke.GetFocus());
                    if (!IsDropDown && !TabStop)
                    {
                        s_snapFocusDebug.TraceVerbose("Installing restoreFocusFilter");
                        // PERF
                        //Application.ThreadContext.FromCurrent().AddMessageFilter(RestoreFocusFilter);
                    }
                }
            }

            base.WndProc(ref m);

            if (m.Msg == (int)PInvoke.WM_NCDESTROY)
            {
                // Destroy the owner window, if we created one.  We
                // cannot do this in OnHandleDestroyed, because at
                // that point our handle is not actually destroyed so
                // destroying our parent actually causes a recursive
                // WM_DESTROY.
                _dropDownOwnerWindow?.DestroyHandle();
            }
        }

        private NativeWindow? _dropDownOwnerWindow;

        private RestoreFocusMessageFilter? _restoreFocusFilter;

        internal RestoreFocusMessageFilter RestoreFocusFilter
        {
            get
            {
                //_restoreFocusFilter ??= new RestoreFocusMessageFilter(this);

                return _restoreFocusFilter;
            }
        }

        /// <summary>
        ///  Creates a new MainMenu control.
        /// </summary>
        public MainMenu() : base(null)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref='MainMenu'/> class with the specified container.
        /// </summary>
        public MainMenu(IContainer container) : this()
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        /// <summary>
        ///  Creates a new MainMenu control with the given items to start with.
        /// </summary>
        public MainMenu(MenuItem[] items) : base(items)
        {
        }

        [SRDescription(nameof(SR.MainMenuCollapseDescr))]
        public event EventHandler Collapse
        {
            add => _onCollapse += value;
            remove => _onCollapse -= value;
        }

        /// <summary>
        ///  This is used for international applications where the language is written from RightToLeft.
        ///  When this property is true, text alignment and reading order will be from right to left.
        /// </summary>
        [Localizable(true)]
        [AmbientValue(RightToLeft.Inherit)]
        [SRDescription(nameof(SR.MenuRightToLeftDescr))]
        public virtual RightToLeft RightToLeft
        {
            get
            {
                if (_rightToLeft == RightToLeft.Inherit)
                {
                    if (_form != null)
                    {
                        return _form.RightToLeft;
                    }

                    return RightToLeft.Inherit;
                }

                return _rightToLeft;
            }
            set
            {
                if (!ClientUtils.IsEnumValid(value, (int)value, (int)RightToLeft.No, (int)RightToLeft.Inherit))
                {
                    throw new InvalidEnumArgumentException(nameof(RightToLeft), (int)value, typeof(RightToLeft));
                }

                if (_rightToLeft != value)
                {
                    _rightToLeft = value;
                    UpdateRtl((value == RightToLeft.Yes));
                }
            }
        }

        internal override bool RenderIsRightToLeft => (RightToLeft == RightToLeft.Yes && (_form == null || !_form.IsMirrored));

        /// <summary>
        ///  Creates a new MainMenu object which is a dupliate of this one.
        /// </summary>
        public virtual MainMenu CloneMenu()
        {
            var newMenu = new MainMenu();
            newMenu.CloneMenu(this);
            return newMenu;
        }

        protected override IntPtr CreateMenuHandle() => User32.CreateMenu();

        /// <summary>
        ///  Clears out this MainMenu object and discards all of it's resources.
        ///  If the menu is parented in a form, it is disconnected from that as
        ///  well.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_form != null && (ownerForm == null || _form == ownerForm))
                {
                    _form.Menu = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///  Indicates which form in which we are currently residing [if any]
        /// </summary>
        public Form GetForm() => _form;

        internal override void ItemsChanged(int change)
        {
            base.ItemsChanged(change);
            _form?.MenuChanged(change, this);
        }

        internal virtual void ItemsChanged(int change, Menu menu) => _form?.MenuChanged(change, menu);

        /// <summary>
        ///  Fires the collapse event
        /// </summary>
        protected internal virtual void OnCollapse(EventArgs e) => _onCollapse?.Invoke(this, e);

        /// <summary>
        ///  Returns true if the RightToLeft should be persisted in code gen.
        /// </summary>
        internal virtual bool ShouldSerializeRightToLeft() => _rightToLeft == RightToLeft.Inherit;

        /// <summary>
        ///  Returns a string representation for this control.
        /// </summary>
        public override string ToString() => base.ToString();
    }
}
