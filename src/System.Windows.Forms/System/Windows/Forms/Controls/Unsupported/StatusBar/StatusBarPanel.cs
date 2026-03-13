// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable RS0016, IDE0031

using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms;

#nullable disable

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultProperty(nameof(Text))]
public class StatusBarPanel : Component, ISupportInitialize
{
    private const int DefaultWidth = 100;
    private const int DefaultMinWidth = 10;
    private const int PanelTextInset = 3;
    private const int PanelGap = 2;

    private HorizontalAlignment _alignment = HorizontalAlignment.Left;
    private Icon _icon;
    private StatusBar _parent;
    private StatusBarPanelAutoSize _autoSize = StatusBarPanelAutoSize.None;
    private StatusBarPanelBorderStyle _borderStyle = StatusBarPanelBorderStyle.Sunken;
    private StatusBarPanelStyle _style = StatusBarPanelStyle.Text;
    private bool _initializing;
    private int _index;
    private int _minWidth = DefaultMinWidth;
    private int _right;
    private int _width = DefaultWidth;
    private object _userData;
    private string _name = string.Empty;
    private string _text = string.Empty;
    private string _toolTipText = string.Empty;

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(HorizontalAlignment.Left)]
    [Localizable(true)]
    public HorizontalAlignment Alignment
    {
        get => _alignment;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(HorizontalAlignment));
            }

            if (_alignment == value)
            {
                return;
            }

            _alignment = value;
            Realize();
        }
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(StatusBarPanelAutoSize.None)]
    [RefreshProperties(RefreshProperties.All)]
    public StatusBarPanelAutoSize AutoSize
    {
        get => _autoSize;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(StatusBarPanelAutoSize));
            }

            if (_autoSize == value)
            {
                return;
            }

            _autoSize = value;
            UpdateSize();
        }
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(StatusBarPanelBorderStyle.Sunken)]
    [DispId(-504)]
    public StatusBarPanelBorderStyle BorderStyle
    {
        get => _borderStyle;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(StatusBarPanelBorderStyle));
            }

            if (_borderStyle == value)
            {
                return;
            }

            _borderStyle = value;
            Realize();

            if (Created)
            {
                _parent.Invalidate();
            }
        }
    }

    internal bool Created => _parent is not null && _parent.ArePanelsRealized();

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(null)]
    [Localizable(true)]
    public Icon Icon
    {
        get => _icon;
        set
        {
            _icon = value is not null
                && (value.Height > SystemInformation.SmallIconSize.Height || value.Width > SystemInformation.SmallIconSize.Width)
                    ? new Icon(value, SystemInformation.SmallIconSize)
                    : value;

            if (Created)
            {
                LegacyStatusBarInterop.SendMessageIntPtr(
                    _parent.Handle,
                    LegacyStatusBarInterop.SbSetIcon,
                    (IntPtr)GetIndex(),
                    _icon is null ? IntPtr.Zero : _icon.Handle);
            }

            UpdateSize();

            if (Created)
            {
                _parent.Invalidate();
            }
        }
    }

    internal int Index
    {
        get => _index;
        set => _index = value;
    }

    [SRCategory(nameof(SR.CatBehavior))]
    [DefaultValue(DefaultMinWidth)]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.All)]
    public int MinWidth
    {
        get => _minWidth;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            if (_minWidth == value)
            {
                return;
            }

            _minWidth = value;
            UpdateSize();
            if (_minWidth > Width)
            {
                Width = value;
            }
        }
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [Localizable(true)]
    public string Name
    {
        get => WindowsFormsUtils.GetComponentName(this, _name);
        set
        {
            _name = value ?? string.Empty;
            if (Site is not null)
            {
                Site.Name = _name;
            }
        }
    }

    [Browsable(false)]
    public StatusBar Parent => _parent;

    internal StatusBar ParentInternal
    {
        set => _parent = value;
    }

    internal int Right
    {
        get => _right;
        set => _right = value;
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(StatusBarPanelStyle.Text)]
    public StatusBarPanelStyle Style
    {
        get => _style;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(StatusBarPanelStyle));
            }

            if (_style == value)
            {
                return;
            }

            _style = value;
            Realize();

            if (Created)
            {
                _parent.Invalidate();
            }
        }
    }

    [SRCategory(nameof(SR.CatData))]
    [Localizable(false)]
    [Bindable(true)]
    [SRDescription(nameof(SR.ControlTagDescr))]
    [DefaultValue(null)]
    [TypeConverter(typeof(StringConverter))]
    public object Tag
    {
        get => _userData;
        set => _userData = value;
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [Localizable(true)]
    [DefaultValue("")]
    public string Text
    {
        get => _text ?? string.Empty;
        set
        {
            value ??= string.Empty;

            if (string.Equals(Text, value, StringComparison.Ordinal))
            {
                return;
            }

            _text = value.Length == 0 ? null : value;
            Realize();
            UpdateSize();
        }
    }

    [SRCategory(nameof(SR.CatAppearance))]
    [Localizable(true)]
    [DefaultValue("")]
    public string ToolTipText
    {
        get => _toolTipText ?? string.Empty;
        set
        {
            value ??= string.Empty;

            if (string.Equals(ToolTipText, value, StringComparison.Ordinal))
            {
                return;
            }

            _toolTipText = value.Length == 0 ? null : value;

            if (Created)
            {
                _parent.UpdateTooltip(this);
            }
        }
    }

    [Localizable(true)]
    [SRCategory(nameof(SR.CatAppearance))]
    [DefaultValue(DefaultWidth)]
    public int Width
    {
        get => _width;
        set
        {
            if (!_initializing && value < _minWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(Width), SR.WidthGreaterThanMinWidth);
            }

            _width = value;
            UpdateSize();
        }
    }

    public void BeginInit() => _initializing = true;

    protected override void Dispose(bool disposing)
    {
        if (disposing && _parent is not null)
        {
            int index = GetIndex();
            if (index != -1)
            {
                _parent.Panels.RemoveAt(index);
            }
        }

        base.Dispose(disposing);
    }

    public void EndInit()
    {
        _initializing = false;

        if (Width < MinWidth)
        {
            Width = MinWidth;
        }
    }

    internal int GetContentsWidth(bool newPanel)
    {
        string text = newPanel ? _text ?? string.Empty : Text;

        using Graphics graphics = _parent.CreateGraphicsInternal();
        Size size = Size.Ceiling(graphics.MeasureString(text, _parent.Font));
        if (_icon is not null)
        {
            size.Width += _icon.Size.Width + 5;
        }

        int width = size.Width + SystemInformation.BorderSize.Width * 2 + PanelTextInset * 2 + PanelGap;

        return Math.Max(width, _minWidth);
    }

    internal void Realize()
    {
        if (!Created)
        {
            return;
        }

        string text = _text ?? string.Empty;
        HorizontalAlignment alignment = _alignment;
        if (_parent.RightToLeft == RightToLeft.Yes)
        {
            alignment = alignment switch
            {
                HorizontalAlignment.Left => HorizontalAlignment.Right,
                HorizontalAlignment.Right => HorizontalAlignment.Left,
                _ => alignment
            };
        }

        string sendText = alignment switch
        {
            HorizontalAlignment.Center => "\t" + text,
            HorizontalAlignment.Right => "\t\t" + text,
            _ => text
        };

        int border = _borderStyle switch
        {
            StatusBarPanelBorderStyle.None => LegacyStatusBarInterop.SbtNoBorders,
            StatusBarPanelBorderStyle.Raised => LegacyStatusBarInterop.SbtPopout,
            _ => 0
        };

        if (_style == StatusBarPanelStyle.OwnerDraw)
        {
            border |= LegacyStatusBarInterop.SbtOwnerDraw;
        }

        int wParam = GetIndex() | border;
        if (_parent.RightToLeft == RightToLeft.Yes)
        {
            wParam |= LegacyStatusBarInterop.SbtRtlReading;
        }

        int result = (int)LegacyStatusBarInterop.SendMessageString(
            _parent.Handle,
            LegacyStatusBarInterop.SbSetText,
            (IntPtr)wParam,
            sendText);

        if (result == 0)
        {
            throw new InvalidOperationException(SR.UnableToSetPanelText);
        }

        if (_icon is not null && _style != StatusBarPanelStyle.OwnerDraw)
        {
            LegacyStatusBarInterop.SendMessageIntPtr(
                _parent.Handle,
                LegacyStatusBarInterop.SbSetIcon,
                (IntPtr)GetIndex(),
                _icon.Handle);
        }
        else
        {
            LegacyStatusBarInterop.SendMessageIntPtr(
                _parent.Handle,
                LegacyStatusBarInterop.SbSetIcon,
                (IntPtr)GetIndex(),
                IntPtr.Zero);
        }

        if (_style != StatusBarPanelStyle.OwnerDraw)
        {
            return;
        }

        RECT rect = default;
        result = (int)LegacyStatusBarInterop.SendMessageRect(
            _parent.Handle,
            LegacyStatusBarInterop.SbGetRect,
            (IntPtr)GetIndex(),
            ref rect);

        if (result != 0)
        {
            _parent.Invalidate(Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom));
        }
    }

    public override string ToString() => "StatusBarPanel: {" + Text + '}';

    private void ApplyContentSizing()
    {
        if (_autoSize != StatusBarPanelAutoSize.Contents || _parent is null)
        {
            return;
        }

        int newWidth = GetContentsWidth(newPanel: false);
        if (newWidth == Width)
        {
            return;
        }

        Width = newWidth;
        if (Created)
        {
            _parent.DirtyLayout();
            _parent.PerformLayout();
        }
    }

    private int GetIndex() => _index;

    private void UpdateSize()
    {
        if (_autoSize == StatusBarPanelAutoSize.Contents)
        {
            ApplyContentSizing();

            return;
        }

        if (Created)
        {
            _parent.DirtyLayout();
            _parent.PerformLayout();
        }
    }
}
