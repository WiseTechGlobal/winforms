// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable RS0016, CA1822

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms;

#nullable disable

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
[DefaultEvent(nameof(PanelClick))]
[DefaultProperty(nameof(Text))]
[Designer($"System.Windows.Forms.Design.StatusBarDesigner, {Assemblies.SystemDesign}")]
public partial class StatusBar : Control
{
    private const int SimpleIndex = 0xFF;
    private const string BadStatusBarPanelMessage = "Value must be a StatusBarPanel.";

    private static readonly object s_panelClickEvent = new();
    private static readonly object s_drawItemEvent = new();

    private static VisualStyleRenderer s_renderer;

    private readonly List<StatusBarPanel> _panels = [];
    private Point _lastClick;
    private StatusBarPanelCollection _panelsCollection;
    private ControlToolTip _tooltips;
    private int _panelsRealized;
    private int _sizeGripWidth;
    private bool _layoutDirty;
    private bool _showPanels;
    private bool _sizeGrip = true;
    private string _simpleText;

    public StatusBar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable, false);

        Dock = DockStyle.Bottom;
        TabStop = false;
    }

    private static VisualStyleRenderer VisualStyleRenderer
    {
        get
        {
            if (!VisualStyleRenderer.IsSupported)
            {
                s_renderer = null;

                return null;
            }

            s_renderer ??= new VisualStyleRenderer(VisualStyleElement.ToolBar.Button.Normal);

            return s_renderer;
        }
    }

    private int SizeGripWidth
    {
        get
        {
            if (_sizeGripWidth != 0)
            {
                return _sizeGripWidth;
            }

            if (Application.RenderWithVisualStyles && VisualStyleRenderer is not null)
            {
                using Graphics graphics = Graphics.FromHwndInternal(Handle);
                VisualStyleRenderer renderer = VisualStyleRenderer;

                renderer.SetParameters(VisualStyleElement.Status.GripperPane.Normal);
                Size paneSize = renderer.GetPartSize(graphics, ThemeSizeType.True);

                renderer.SetParameters(VisualStyleElement.Status.Gripper.Normal);
                Size gripSize = renderer.GetPartSize(graphics, ThemeSizeType.True);

                _sizeGripWidth = Math.Max(paneSize.Width + gripSize.Width, 16);
            }
            else
            {
                _sizeGripWidth = 16;
            }

            return _sizeGripWidth;
        }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Color BackColor
    {
        get => SystemColors.Control;
        set { }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler BackColorChanged
    {
        add => base.BackColorChanged += value;
        remove => base.BackColorChanged -= value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Image BackgroundImage
    {
        get => base.BackgroundImage;
        set => base.BackgroundImage = value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler BackgroundImageChanged
    {
        add => base.BackgroundImageChanged += value;
        remove => base.BackgroundImageChanged -= value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override ImageLayout BackgroundImageLayout
    {
        get => base.BackgroundImageLayout;
        set => base.BackgroundImageLayout = value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler BackgroundImageLayoutChanged
    {
        add => base.BackgroundImageLayoutChanged += value;
        remove => base.BackgroundImageLayoutChanged -= value;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ClassName = LegacyStatusBarInterop.StatusClassName;
            cp.Style |= LegacyStatusBarInterop.CcsNoParentAlign | LegacyStatusBarInterop.CcsNoResize;

            if (_sizeGrip)
            {
                cp.Style |= LegacyStatusBarInterop.SbarsSizeGrip;
            }
            else
            {
                cp.Style &= ~LegacyStatusBarInterop.SbarsSizeGrip;
            }

            return cp;
        }
    }

    protected override ImeMode DefaultImeMode => ImeMode.Disable;

    protected override Size DefaultSize => new(100, 22);

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override bool DoubleBuffered
    {
        get => base.DoubleBuffered;
        set => base.DoubleBuffered = value;
    }

    [Localizable(true)]
    [DefaultValue(DockStyle.Bottom)]
    public override DockStyle Dock
    {
        get => base.Dock;
        set => base.Dock = value;
    }

    [Localizable(true)]
    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            SetPanelContentsWidths(newPanels: false);
        }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Color ForeColor
    {
        get => base.ForeColor;
        set => base.ForeColor = value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler ForeColorChanged
    {
        add => base.ForeColorChanged += value;
        remove => base.ForeColorChanged -= value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new ImeMode ImeMode
    {
        get => base.ImeMode;
        set => base.ImeMode = value;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler ImeModeChanged
    {
        add => base.ImeModeChanged += value;
        remove => base.ImeModeChanged -= value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Localizable(true)]
    [SRCategory(nameof(SR.CatAppearance))]
    [MergableProperty(false)]
    public StatusBarPanelCollection Panels => _panelsCollection ??= new StatusBarPanelCollection(this);

    [Localizable(true)]
    public override string Text
    {
        get => _simpleText ?? string.Empty;
        set
        {
            value ??= string.Empty;

            if (string.Equals(Text, value, StringComparison.Ordinal))
            {
                return;
            }

            _simpleText = value.Length == 0 ? null : value;
            SetSimpleText(_simpleText);

            OnTextChanged(EventArgs.Empty);
        }
    }

    [SRCategory(nameof(SR.CatBehavior))]
    [DefaultValue(false)]
    public bool ShowPanels
    {
        get => _showPanels;
        set
        {
            if (_showPanels == value)
            {
                return;
            }

            _showPanels = value;
            _layoutDirty = true;

            if (!IsHandleCreated)
            {
                return;
            }

            LegacyStatusBarInterop.SendMessageIntPtr(
                Handle,
                LegacyStatusBarInterop.SbSimple,
                (IntPtr)(value ? 0 : 1),
                IntPtr.Zero);

            if (value)
            {
                PerformLayout();
                RealizePanels();
            }
            else if (_tooltips is not null)
            {
                foreach (StatusBarPanel panel in _panels)
                {
                    _tooltips.SetTool(panel, null);
                }
            }

            SetSimpleText(_simpleText);
        }
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(true)]
    public bool SizingGrip
    {
        get => _sizeGrip;
        set
        {
            if (_sizeGrip == value)
            {
                return;
            }

            _sizeGrip = value;
            _sizeGripWidth = 0;
            RecreateHandle();
        }
    }

    [DefaultValue(false)]
    public new bool TabStop
    {
        get => base.TabStop;
        set => base.TabStop = value;
    }

    [SRCategory(nameof(SR.CatBehavior))]
    public event StatusBarDrawItemEventHandler DrawItem
    {
        add => Events.AddHandler(s_drawItemEvent, value);
        remove => Events.RemoveHandler(s_drawItemEvent, value);
    }

    [SRCategory(nameof(SR.CatMouse))]
    public event StatusBarPanelClickEventHandler PanelClick
    {
        add => Events.AddHandler(s_panelClickEvent, value);
        remove => Events.RemoveHandler(s_panelClickEvent, value);
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event PaintEventHandler Paint
    {
        add => base.Paint += value;
        remove => base.Paint -= value;
    }

    internal bool ArePanelsRealized() => _showPanels && IsHandleCreated;

    internal void DirtyLayout() => _layoutDirty = true;

    internal void ForcePanelUpdate()
    {
        if (!ArePanelsRealized())
        {
            return;
        }

        _layoutDirty = true;
        SetPanelContentsWidths(newPanels: true);
        PerformLayout();
        RealizePanels();
    }

    internal void RealizePanels()
    {
        int oldCount = _panelsRealized;
        _panelsRealized = 0;

        if (_panels.Count == 0)
        {
            LegacyStatusBarInterop.SendMessageString(Handle, LegacyStatusBarInterop.SbSetText, IntPtr.Zero, string.Empty);
        }

        int index = 0;
        for (; index < _panels.Count; index++)
        {
            try
            {
                _panels[index].Realize();
                _panelsRealized++;
            }
            catch
            {
            }
        }

        for (; index < oldCount; index++)
        {
            LegacyStatusBarInterop.SendMessageString(Handle, LegacyStatusBarInterop.SbSetText, IntPtr.Zero, null);
        }
    }

    internal void RemoveAllPanelsWithoutUpdate()
    {
        foreach (StatusBarPanel panel in _panels)
        {
            panel.ParentInternal = null;
        }

        _panels.Clear();

        if (_showPanels)
        {
            ApplyPanelWidths();
            ForcePanelUpdate();
        }
    }

    internal void SetPanelContentsWidths(bool newPanels)
    {
        bool changed = false;

        foreach (StatusBarPanel panel in _panels)
        {
            if (panel.AutoSize != StatusBarPanelAutoSize.Contents)
            {
                continue;
            }

            int newWidth = panel.GetContentsWidth(newPanels);
            if (panel.Width == newWidth)
            {
                continue;
            }

            panel.Width = newWidth;
            changed = true;
        }

        if (!changed)
        {
            return;
        }

        DirtyLayout();
        PerformLayout();
    }

    internal void UpdateTooltip(StatusBarPanel panel)
    {
        if (_tooltips is null)
        {
            if (IsHandleCreated && !DesignMode)
            {
                _tooltips = new ControlToolTip(this);
            }
            else
            {
                return;
            }
        }

        if (panel.Parent == this && panel.ToolTipText.Length > 0)
        {
            int border = SystemInformation.Border3DSize.Width;
            ControlToolTip.Tool tool = _tooltips.GetTool(panel) ?? new ControlToolTip.Tool();
            tool.Text = panel.ToolTipText;
            tool.Bounds = new Rectangle(panel.Right - panel.Width + border, 0, panel.Width - border, Height);
            _tooltips.SetTool(panel, tool);

            return;
        }

        _tooltips.SetTool(panel, null);
    }

    internal void UpdatePanelIndex()
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            _panels[i].Index = i;
        }
    }

    protected override unsafe void CreateHandle()
    {
        if (!RecreatingHandle)
        {
            using ThemingScope scope = new(Application.UseVisualStyles);
            PInvoke.InitCommonControlsEx(new INITCOMMONCONTROLSEX
            {
                dwSize = (uint)sizeof(INITCOMMONCONTROLSEX),
                dwICC = INITCOMMONCONTROLSEX_ICC.ICC_BAR_CLASSES
            });
        }

        base.CreateHandle();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _panelsCollection is not null)
        {
            StatusBarPanel[] panelCopy = [.. _panels];
            _panelsCollection.Clear();

            foreach (StatusBarPanel panel in panelCopy)
            {
                panel.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    protected virtual void OnDrawItem(StatusBarDrawItemEventArgs sbdievent)
        => (Events[s_drawItemEvent] as StatusBarDrawItemEventHandler)?.Invoke(this, sbdievent);

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        if (!DesignMode)
        {
            _tooltips = new ControlToolTip(this);
        }

        if (!_showPanels)
        {
            LegacyStatusBarInterop.SendMessageIntPtr(
                Handle,
                LegacyStatusBarInterop.SbSimple,
                (IntPtr)1,
                IntPtr.Zero);
            SetSimpleText(_simpleText);

            return;
        }

        ForcePanelUpdate();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        base.OnHandleDestroyed(e);

        _tooltips?.Dispose();
        _tooltips = null;
    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
        if (_showPanels)
        {
            LayoutPanels();

            if (IsHandleCreated && _panelsRealized != _panels.Count)
            {
                RealizePanels();
            }
        }

        base.OnLayout(levent);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _lastClick = e.Location;

        base.OnMouseDown(e);
    }

    protected virtual void OnPanelClick(StatusBarPanelClickEventArgs e)
        => (Events[s_panelClickEvent] as StatusBarPanelClickEventHandler)?.Invoke(this, e);

    protected override void OnResize(EventArgs e)
    {
        Invalidate();

        base.OnResize(e);
    }

    public override string ToString()
    {
        string text = base.ToString();
        text += ", Panels.Count: " + Panels.Count.ToString(CultureInfo.CurrentCulture);

        if (Panels.Count > 0)
        {
            text += ", Panels[0]: " + Panels[0];
        }

        return text;
    }

    protected override unsafe void WndProc(ref Message m)
    {
        switch (m.MsgInternal)
        {
            case PInvokeCore.WM_NCHITTEST:
                WmNCHitTest(ref m);
                break;
            case MessageId.WM_REFLECT_DRAWITEM:
                WmDrawItem(ref m);
                break;
            case PInvokeCore.WM_NOTIFY:
            case MessageId.WM_REFLECT_NOTIFY:
                NMHDR* note = (NMHDR*)(nint)m.LParamInternal;
                if (note->code is PInvoke.NM_CLICK or PInvoke.NM_RCLICK or PInvoke.NM_DBLCLK or PInvoke.NM_RDBLCLK)
                {
                    WmNotifyNmClick(note->code);
                }
                else
                {
                    base.WndProc(ref m);
                }

                break;
            default:
                base.WndProc(ref m);
                break;
        }
    }

    private void ApplyPanelWidths()
    {
        if (!IsHandleCreated)
        {
            return;
        }

        if (_panels.Count == 0)
        {
            int[] offsets = [Size.Width - (_sizeGrip ? SizeGripWidth : 0)];
            LegacyStatusBarInterop.SendMessageParts(Handle, LegacyStatusBarInterop.SbSetParts, (IntPtr)1, offsets);
            LegacyStatusBarInterop.SendMessageIntPtr(Handle, LegacyStatusBarInterop.SbSetIcon, IntPtr.Zero, IntPtr.Zero);

            return;
        }

        int[] offsets2 = new int[_panels.Count];
        int currentOffset = 0;
        for (int i = 0; i < _panels.Count; i++)
        {
            StatusBarPanel panel = _panels[i];
            currentOffset += panel.Width;
            offsets2[i] = currentOffset;
            panel.Right = currentOffset;
        }

        LegacyStatusBarInterop.SendMessageParts(Handle, LegacyStatusBarInterop.SbSetParts, (IntPtr)offsets2.Length, offsets2);

        foreach (StatusBarPanel panel in _panels)
        {
            UpdateTooltip(panel);
        }

        _layoutDirty = false;
    }

    private void LayoutPanels()
    {
        int fixedWidth = 0;
        List<StatusBarPanel> springPanels = [];
        bool changed = false;

        foreach (StatusBarPanel panel in _panels)
        {
            if (panel.AutoSize == StatusBarPanelAutoSize.Spring)
            {
                springPanels.Add(panel);
            }
            else
            {
                fixedWidth += panel.Width;
            }
        }

        if (springPanels.Count > 0)
        {
            int springPanelsLeft = springPanels.Count;
            int leftoverWidth = Bounds.Width - fixedWidth;
            if (_sizeGrip)
            {
                leftoverWidth -= SizeGripWidth;
            }

            int lastLeftoverWidth = int.MinValue;

            while (springPanelsLeft > 0)
            {
                int widthOfSpringPanel = leftoverWidth / springPanelsLeft;
                if (leftoverWidth == lastLeftoverWidth)
                {
                    break;
                }

                lastLeftoverWidth = leftoverWidth;

                for (int i = 0; i < springPanels.Count; i++)
                {
                    StatusBarPanel panel = springPanels[i];
                    if (panel is null)
                    {
                        continue;
                    }

                    if (widthOfSpringPanel < panel.MinWidth)
                    {
                        if (panel.Width != panel.MinWidth)
                        {
                            changed = true;
                        }

                        panel.Width = panel.MinWidth;
                        springPanels[i] = null;
                        springPanelsLeft--;
                        leftoverWidth -= panel.MinWidth;

                        continue;
                    }

                    if (panel.Width != widthOfSpringPanel)
                    {
                        changed = true;
                    }

                    panel.Width = widthOfSpringPanel;
                }
            }
        }

        if (changed || _layoutDirty)
        {
            ApplyPanelWidths();
        }
    }

    private void SetSimpleText(string simpleText)
    {
        if (_showPanels || !IsHandleCreated)
        {
            return;
        }

        int wParam = SimpleIndex | LegacyStatusBarInterop.SbtNoBorders;
        if (RightToLeft == RightToLeft.Yes)
        {
            wParam |= LegacyStatusBarInterop.SbtRtlReading;
        }

        LegacyStatusBarInterop.SendMessageString(Handle, LegacyStatusBarInterop.SbSetText, (IntPtr)wParam, simpleText);
    }

    private void WmDrawItem(ref Message m)
    {
        unsafe
        {
            DRAWITEMSTRUCT* dis = (DRAWITEMSTRUCT*)(nint)m.LParamInternal;
            if (dis->itemID >= (uint)_panels.Count)
            {
                Debug.Fail("OwnerDraw item out of range");

                return;
            }

            StatusBarPanel panel = _panels[(int)dis->itemID];

            using Graphics graphics = Graphics.FromHdcInternal((IntPtr)dis->hDC);
            Rectangle bounds = Rectangle.FromLTRB(dis->rcItem.left, dis->rcItem.top, dis->rcItem.right, dis->rcItem.bottom);
            OnDrawItem(new StatusBarDrawItemEventArgs(graphics, Font, bounds, (int)dis->itemID, DrawItemState.None, panel, ForeColor, BackColor));
            m.ResultInternal = (LRESULT)1;
        }
    }

    private void WmNCHitTest(ref Message m)
    {
        int x = PARAM.SignedLOWORD(m.LParamInternal);
        Rectangle bounds = Bounds;
        bool callSuper = true;

        if (x > bounds.X + bounds.Width - SizeGripWidth)
        {
            Control parent = ParentInternal;
            if (parent is Form form)
            {
                FormBorderStyle borderStyle = form.FormBorderStyle;
                if (borderStyle is not FormBorderStyle.Sizable and not FormBorderStyle.SizableToolWindow)
                {
                    callSuper = false;
                }

                if (!form.TopLevel || Dock != DockStyle.Bottom)
                {
                    callSuper = false;
                }

                if (callSuper)
                {
                    foreach (Control child in parent.Controls)
                    {
                        if (child != this && child.Dock == DockStyle.Bottom && child.Top > Top)
                        {
                            callSuper = false;

                            break;
                        }
                    }
                }
            }
            else
            {
                callSuper = false;
            }
        }

        if (callSuper)
        {
            base.WndProc(ref m);

            return;
        }

        m.ResultInternal = (LRESULT)(nint)PInvoke.HTCLIENT;
    }

    private void WmNotifyNmClick(uint code)
    {
        if (!_showPanels)
        {
            return;
        }

        int currentOffset = 0;
        int index = -1;
        for (int i = 0; i < _panels.Count; i++)
        {
            StatusBarPanel panel = _panels[i];
            currentOffset += panel.Width;
            if (_lastClick.X < currentOffset)
            {
                index = i;

                break;
            }
        }

        if (index == -1)
        {
            return;
        }

        MouseButtons button = code switch
        {
            PInvoke.NM_RCLICK or PInvoke.NM_RDBLCLK => MouseButtons.Right,
            _ => MouseButtons.Left
        };
        int clicks = code switch
        {
            PInvoke.NM_DBLCLK or PInvoke.NM_RDBLCLK => 2,
            _ => 1
        };

        OnPanelClick(new StatusBarPanelClickEventArgs(_panels[index], button, clicks, _lastClick.X, _lastClick.Y));
    }

    [ListBindable(false)]
    public class StatusBarPanelCollection : IList
    {
        private readonly StatusBar _owner;
        private int _lastAccessedIndex = -1;

        public StatusBarPanelCollection(StatusBar owner)
        {
            ArgumentNullException.ThrowIfNull(owner);

            _owner = owner;
        }

        public virtual StatusBarPanel this[int index]
        {
            get => _owner._panels[index];
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                _owner._layoutDirty = true;

                if (value.Parent is not null)
                {
                    throw new ArgumentException(SR.ObjectHasParent, nameof(value));
                }

                if (index < 0 || index >= _owner._panels.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, string.Format(SR.InvalidArgument, nameof(index), index));
                }

                StatusBarPanel oldPanel = _owner._panels[index];
                oldPanel.ParentInternal = null;
                value.ParentInternal = _owner;
                if (value.AutoSize == StatusBarPanelAutoSize.Contents)
                {
                    value.Width = value.GetContentsWidth(newPanel: true);
                }

                _owner._panels[index] = value;
                value.Index = index;

                if (_owner.ArePanelsRealized())
                {
                    _owner.PerformLayout();
                    value.Realize();
                }
            }
        }

        object IList.this[int index]
        {
            get => this[index];
            set
            {
                if (value is not StatusBarPanel panel)
                {
                    throw new ArgumentException(BadStatusBarPanelMessage, nameof(value));
                }

                this[index] = panel;
            }
        }

        public virtual StatusBarPanel this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                int index = IndexOfKey(key);

                return IsValidIndex(index) ? this[index] : null;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Count => _owner._panels.Count;

        public bool IsReadOnly => false;

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        bool IList.IsFixedSize => false;

        public virtual StatusBarPanel Add(string text)
        {
            StatusBarPanel panel = new()
            {
                Text = text
            };

            Add(panel);

            return panel;
        }

        public virtual int Add(StatusBarPanel value)
        {
            int index = _owner._panels.Count;
            Insert(index, value);

            return index;
        }

        int IList.Add(object value)
        {
            if (value is not StatusBarPanel panel)
            {
                throw new ArgumentException(BadStatusBarPanelMessage, nameof(value));
            }

            return Add(panel);
        }

        public virtual void AddRange(StatusBarPanel[] panels)
        {
            ArgumentNullException.ThrowIfNull(panels);

            foreach (StatusBarPanel panel in panels)
            {
                Add(panel);
            }
        }

        public virtual void Clear()
        {
            _owner.RemoveAllPanelsWithoutUpdate();
            _owner.PerformLayout();
        }

        public bool Contains(StatusBarPanel panel) => IndexOf(panel) != -1;

        bool IList.Contains(object panel) => panel is StatusBarPanel statusBarPanel && Contains(statusBarPanel);

        public virtual bool ContainsKey(string key) => IsValidIndex(IndexOfKey(key));

        void ICollection.CopyTo(Array dest, int index) => ((ICollection)_owner._panels).CopyTo(dest, index);

        public IEnumerator GetEnumerator() => _owner._panels.GetEnumerator();

        public int IndexOf(StatusBarPanel panel)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == panel)
                {
                    return i;
                }
            }

            return -1;
        }

        int IList.IndexOf(object panel) => panel is StatusBarPanel statusBarPanel ? IndexOf(statusBarPanel) : -1;

        public virtual int IndexOfKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }

            if (IsValidIndex(_lastAccessedIndex)
                && WindowsFormsUtils.SafeCompareStrings(this[_lastAccessedIndex].Name, key, ignoreCase: true))
            {
                return _lastAccessedIndex;
            }

            for (int i = 0; i < Count; i++)
            {
                if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, ignoreCase: true))
                {
                    _lastAccessedIndex = i;

                    return i;
                }
            }

            _lastAccessedIndex = -1;

            return -1;
        }

        public virtual void Insert(int index, StatusBarPanel value)
        {
            ArgumentNullException.ThrowIfNull(value);

            _owner._layoutDirty = true;
            if (value.Parent != _owner && value.Parent is not null)
            {
                throw new ArgumentException(SR.ObjectHasParent, nameof(value));
            }

            if (index < 0 || index > _owner._panels.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, string.Format(SR.InvalidArgument, nameof(index), index));
            }

            value.ParentInternal = _owner;
            if (value.AutoSize == StatusBarPanelAutoSize.Contents)
            {
                value.Width = value.GetContentsWidth(newPanel: true);
            }

            _owner._panels.Insert(index, value);
            _owner.UpdatePanelIndex();
            _owner.ForcePanelUpdate();
        }

        void IList.Insert(int index, object value)
        {
            if (value is not StatusBarPanel panel)
            {
                throw new ArgumentException(BadStatusBarPanelMessage, nameof(value));
            }

            Insert(index, panel);
        }

        public virtual void Remove(StatusBarPanel value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Parent != _owner)
            {
                return;
            }

            RemoveAt(value.Index);
        }

        void IList.Remove(object value)
        {
            if (value is StatusBarPanel panel)
            {
                Remove(panel);
            }
        }

        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, string.Format(SR.InvalidArgument, nameof(index), index));
            }

            StatusBarPanel panel = _owner._panels[index];
            _owner._panels.RemoveAt(index);
            panel.ParentInternal = null;
            _owner.UpdateTooltip(panel);
            _owner.UpdatePanelIndex();
            _owner.ForcePanelUpdate();
        }

        public virtual void RemoveByKey(string key)
        {
            int index = IndexOfKey(key);
            if (IsValidIndex(index))
            {
                RemoveAt(index);
            }
        }

        private bool IsValidIndex(int index) => index >= 0 && index < Count;
    }

    private sealed class ControlToolTip : IHandle<HWND>, IDisposable
    {
        public sealed class Tool
        {
            public Rectangle Bounds = Rectangle.Empty;
            public string Text;
            internal nint _id = -1;
        }

        private readonly Dictionary<object, Tool> _tools = [];
        private readonly Control _parent;
        private readonly ToolTipNativeWindow _window;
        private int _nextId;

        public ControlToolTip(Control parent)
        {
            _parent = parent;
            _window = new ToolTipNativeWindow(this);
        }

        public HWND Handle
        {
            get
            {
                if (_window.Handle == IntPtr.Zero)
                {
                    CreateHandle();
                }

                return (HWND)_window.Handle;
            }
        }

        private bool IsHandleCreated => _window.Handle != IntPtr.Zero;

        private CreateParams CreateParams => new()
        {
            Parent = IntPtr.Zero,
            ClassName = PInvoke.TOOLTIPS_CLASS,
            Style = (int)PInvoke.TTS_ALWAYSTIP,
            ExStyle = 0,
            Caption = null
        };

        public void Dispose()
        {
            if (IsHandleCreated)
            {
                _window.DestroyHandle();
                _tools.Clear();
            }

            GC.SuppressFinalize(this);
        }

        public Tool GetTool(object key) => _tools.TryGetValue(key, out Tool tool) ? tool : null;

        public void SetTool(object key, Tool tool)
        {
            bool remove = _tools.TryGetValue(key, out Tool existingTool);
            bool add = tool is not null;
            bool update = add && remove && tool._id == existingTool._id;

            if (update)
            {
                UpdateTool(tool);
            }
            else
            {
                if (remove)
                {
                    RemoveTool(existingTool);
                }

                if (add)
                {
                    AddTool(tool);
                }
            }

            if (tool is not null)
            {
                _tools[key] = tool;
            }
            else
            {
                _tools.Remove(key);
            }
        }

        private void AddTool(Tool tool)
        {
            if (tool is null || string.IsNullOrEmpty(tool.Text))
            {
                return;
            }

            ToolInfoWrapper<Control> info = GetToolInfo(tool);
            if (info.SendMessage(this, PInvoke.TTM_ADDTOOLW) == IntPtr.Zero)
            {
                throw new InvalidOperationException(SR.ToolTipAddFailed);
            }
        }

        private void AssignId(Tool tool) => tool._id = _nextId++;

        private unsafe void CreateHandle()
        {
            if (IsHandleCreated)
            {
                return;
            }

            PInvoke.InitCommonControlsEx(new INITCOMMONCONTROLSEX
            {
                dwSize = (uint)sizeof(INITCOMMONCONTROLSEX),
                dwICC = INITCOMMONCONTROLSEX_ICC.ICC_TAB_CLASSES
            });
            _window.CreateHandle(CreateParams);
            PInvoke.SetWindowPos(
                (HWND)_window.Handle,
                HWND.HWND_TOPMOST,
                0,
                0,
                0,
                0,
                SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
            PInvokeCore.SendMessage(this, PInvoke.TTM_SETMAXTIPWIDTH, 0, SystemInformation.MaxWindowTrackSize.Width);
        }

        private ToolInfoWrapper<Control> GetMinToolInfo(Tool tool)
        {
            if (tool._id < 0)
            {
                AssignId(tool);
            }

            return new ToolInfoWrapper<Control>(_parent, id: tool._id);
        }

        private ToolInfoWrapper<Control> GetToolInfo(Tool tool)
        {
            ToolInfoWrapper<Control> toolInfo = GetMinToolInfo(tool);
            toolInfo.Info.uFlags |= TOOLTIP_FLAGS.TTF_TRANSPARENT | TOOLTIP_FLAGS.TTF_SUBCLASS;
            if (_parent.RightToLeft == RightToLeft.Yes && !_parent.IsMirrored)
            {
                toolInfo.Info.uFlags |= TOOLTIP_FLAGS.TTF_RTLREADING;
            }

            toolInfo.Text = tool.Text;
            toolInfo.Info.rect = tool.Bounds;

            return toolInfo;
        }

        private void RemoveTool(Tool tool)
        {
            if (tool is null || string.IsNullOrEmpty(tool.Text) || tool._id < 0)
            {
                return;
            }

            GetMinToolInfo(tool).SendMessage(this, PInvoke.TTM_DELTOOLW);
        }

        private void UpdateTool(Tool tool)
        {
            if (tool is null || string.IsNullOrEmpty(tool.Text) || tool._id < 0)
            {
                return;
            }

            GetToolInfo(tool).SendMessage(this, PInvoke.TTM_SETTOOLINFOW);
        }

        private void WndProc(ref Message msg)
        {
            if (msg.MsgInternal == PInvokeCore.WM_SETFOCUS)
            {
                return;
            }

            _window.DefWndProc(ref msg);
        }

        private sealed class ToolTipNativeWindow : NativeWindow
        {
            private readonly ControlToolTip _control;

            internal ToolTipNativeWindow(ControlToolTip control)
            {
                _control = control;
            }

            protected override void WndProc(ref Message m) => _control.WndProc(ref m);
        }
    }
}

internal static class LegacyStatusBarInterop
{
    internal const string StatusClassName = "msctls_statusbar32";

    internal const int CcsNoResize = 0x0004;
    internal const int CcsNoParentAlign = 0x0008;
    internal const int SbarsSizeGrip = 0x0100;

    internal const int SbtNoBorders = 0x0100;
    internal const int SbtPopout = 0x0200;
    internal const int SbtRtlReading = 0x0400;
    internal const int SbtOwnerDraw = 0x1000;

    internal const int SbSetText = (int)PInvokeCore.WM_USER + 1;
    internal const int SbSetParts = (int)PInvokeCore.WM_USER + 4;
    internal const int SbGetRect = (int)PInvokeCore.WM_USER + 10;
    internal const int SbSimple = (int)PInvokeCore.WM_USER + 9;
    internal const int SbSetIcon = (int)PInvokeCore.WM_USER + 15;

    [DllImport(Libraries.User32, EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
    internal static extern nint SendMessageString(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    [DllImport(Libraries.User32, EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageIntPtr(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport(Libraries.User32, EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageParts(IntPtr hWnd, int msg, IntPtr wParam, [In] int[] lParam);

    [DllImport(Libraries.User32, EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageRect(IntPtr hWnd, int msg, IntPtr wParam, ref RECT lParam);
}
