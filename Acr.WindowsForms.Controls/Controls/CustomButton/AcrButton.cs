using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomButton;

public class AcrButton : Button, IAcrBaseControl
{
    private EControlState _controlState = EControlState.Normal;
    private int _cornerRadius = 6;
    private Color _accentColor = AcrColors.Primary;
    private AcrButtonVariant _variant = AcrButtonVariant.Primary;
    private bool _hovering;
    private bool _pressed;
    private bool _isLoading;
    private string _loadingText = "Carregando...";
    private float _spinnerAngle;
    private readonly System.Windows.Forms.Timer _spinnerTimer;

    public AcrButton()
    {
        SetStyle(
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true);

        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;

        BackColor = AcrColors.Primary;
        ForeColor = Color.White;
        Cursor = Cursors.Hand;
        Font = new Font("Segoe UI", 9F);
        Padding = new Padding(6, 2, 6, 2);

        _spinnerTimer = new System.Windows.Forms.Timer { Interval = 30 };
        _spinnerTimer.Tick += (_, _) => { _spinnerAngle = (_spinnerAngle + 20) % 360; Invalidate(); };
    }

    [Category("Acr Custom")]
    [Description("Radius, in pixels, used to round the button corners.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CornerRadius
    {
        get => _cornerRadius;
        set
        {
            _cornerRadius = Math.Max(0, value);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Base color used for the button background and hover/pressed shades.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => _accentColor;
        set
        {
            _accentColor = value;
            BackColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Visual style of the button: Primary, Secondary, Outline, Ghost, Danger or Success.")]
    [Browsable(true)]
    [DefaultValue(AcrButtonVariant.Primary)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public AcrButtonVariant Variant
    {
        get => _variant;
        set
        {
            _variant = value;
            switch (value)
            {
                case AcrButtonVariant.Danger: _accentColor = AcrColors.Error; break;
                case AcrButtonVariant.Success: _accentColor = AcrColors.Success; break;
                case AcrButtonVariant.Primary:
                    if (_accentColor == AcrColors.Error || _accentColor == AcrColors.Success) _accentColor = AcrColors.Primary;
                    break;
            }
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, shows a spinner and blocks clicks until set back to false.")]
    [Browsable(true)]
    [DefaultValue(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading == value) return;
            _isLoading = value;
            if (value) _spinnerTimer.Start(); else _spinnerTimer.Stop();
            Cursor = value ? Cursors.WaitCursor : (Enabled ? Cursors.Hand : Cursors.Default);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Text shown while IsLoading is true. Leave empty to keep the original text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string LoadingText
    {
        get => _loadingText;
        set { _loadingText = value ?? string.Empty; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public EControlState ControlState
    {
        get => _controlState;
        set
        {
            if (_controlState == value) return;
            _controlState = value;
            ApplyState();
        }
    }

    public void ApplyState()
    {
        switch (_controlState)
        {
            case EControlState.Normal:
                Enabled = true; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                Enabled = false; break;
            case EControlState.Edit:
                Enabled = true; break;
        }
    }

    protected override void OnClick(EventArgs e)
    {
        if (_isLoading) return;
        base.OnClick(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovering = true; Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovering = false; _pressed = false; Invalidate(); }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);
        if (mevent.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        _pressed = false;
        Invalidate();
    }

    protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
    protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

    private (Color Back, Color Fore, Color Border) ResolveColors()
    {
        if (!Enabled)
            return (_variant == AcrButtonVariant.Ghost ? Color.Transparent : AcrColors.DisabledBack, AcrColors.DisabledFore, AcrColors.BorderDisabled);

        Color accent = _accentColor;
        Color Shade(Color c) => _pressed ? ControlPaint.Dark(c, 0.1f) : _hovering ? ControlPaint.Light(c, 0.15f) : c;
        Color Tint(float alpha) => Color.FromArgb((int)(255 * alpha), accent);

        return _variant switch
        {
            AcrButtonVariant.Secondary => (
                _pressed ? Color.FromArgb(222, 222, 222) : _hovering ? Color.FromArgb(232, 232, 232) : Color.FromArgb(242, 242, 242),
                AcrColors.Text,
                Color.FromArgb(225, 225, 225)),
            AcrButtonVariant.Outline => (
                _pressed ? Tint(0.18f) : _hovering ? Tint(0.08f) : Color.Transparent,
                accent,
                accent),
            AcrButtonVariant.Ghost => (
                _pressed ? Tint(0.18f) : _hovering ? Tint(0.08f) : Color.Transparent,
                accent,
                Color.Transparent),
            _ => (Shade(accent), ForeColor, Color.Transparent),
        };
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        var g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var parentBack = Parent?.BackColor ?? SystemColors.Control;
        g.Clear(parentBack.A == 0 ? Color.White : parentBack);

        var (back, fore, border) = ResolveColors();
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (var path = AcrGraphics.CreateRoundedRectPath(rect, Math.Min(_cornerRadius, Height / 2)))
        {
            if (back.A > 0)
            {
                using var brush = new SolidBrush(back);
                g.FillPath(brush, path);
            }
            if (border.A > 0)
            {
                using var pen = new Pen(border, 1);
                g.DrawPath(pen, path);
            }
        }

        if (Focused && ShowFocusCues && Enabled)
        {
            var focusRect = Rectangle.Inflate(rect, -3, -3);
            using var focusPath = AcrGraphics.CreateRoundedRectPath(focusRect, Math.Max(0, Math.Min(_cornerRadius - 2, focusRect.Height / 2)));
            using var focusPen = new Pen(Color.FromArgb(120, fore), 1) { DashStyle = DashStyle.Dot };
            g.DrawPath(focusPen, focusPath);
        }

        string text = _isLoading && !string.IsNullOrEmpty(_loadingText) ? _loadingText : Text;
        var contentRect = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical);

        if (_isLoading)
        {
            const int spinner = 14;
            var textSize = TextRenderer.MeasureText(text, Font);
            int total = spinner + (string.IsNullOrEmpty(text) ? 0 : 6 + textSize.Width);
            int sx = contentRect.X + (contentRect.Width - total) / 2;
            int sy = contentRect.Y + (contentRect.Height - spinner) / 2;
            using (var pen = new Pen(fore, 2) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                g.DrawArc(pen, sx, sy, spinner, spinner, _spinnerAngle, 270);
            contentRect = new Rectangle(sx + spinner + 6, contentRect.Y, contentRect.Right - sx - spinner - 6, contentRect.Height);
            TextRenderer.DrawText(g, text, Font, contentRect, fore, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            return;
        }

        if (Image != null)
        {
            var textSize = string.IsNullOrEmpty(text) ? Size.Empty : TextRenderer.MeasureText(text, Font);
            int gap = string.IsNullOrEmpty(text) ? 0 : 6;
            int total = Image.Width + gap + textSize.Width;
            int ix = contentRect.X + (contentRect.Width - total) / 2;
            int iy = contentRect.Y + (contentRect.Height - Image.Height) / 2;
            if (Enabled) g.DrawImage(Image, ix, iy, Image.Width, Image.Height);
            else ControlPaint.DrawImageDisabled(g, Image, ix, iy, back);
            contentRect = new Rectangle(ix + Image.Width + gap, contentRect.Y, textSize.Width + 2, contentRect.Height);
            TextRenderer.DrawText(g, text, Font, contentRect, fore, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            return;
        }

        TextRenderer.DrawText(g, text, Font, contentRect, fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.WordBreak);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _spinnerTimer.Dispose();
        base.Dispose(disposing);
    }
}
