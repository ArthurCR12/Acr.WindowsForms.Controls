using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomChip;

public class AcrChip : Control
{
    private const int CloseSize = 14;
    private const int HorizontalPadding = 10;
    private const int VerticalPadding = 4;
    private const int CloseGap = 6;

    private AcrBadgeVariant _variant = AcrBadgeVariant.Primary;
    private bool _removable = true;
    private bool _selected = false;
    private bool _hoveringClose = false;
    private bool _hoveringChip = false;

    public event EventHandler? Removed;
    public event EventHandler? ChipClick;

    public AcrChip()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Cursor = Cursors.Hand;
        Font = AcrFonts.Get(8.5F);
        Text = "Chip";
        UpdateSize();
    }

    [Category("Acr Custom")]
    [Description("Color variant applied to the chip.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public AcrBadgeVariant Variant
    {
        get => _variant;
        set
        {
            _variant = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, shows a small close (×) button that raises Removed when clicked.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Removable
    {
        get => _removable;
        set
        {
            _removable = value;
            UpdateSize();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the chip is drawn as selected (filled with its accent color).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            Invalidate();
        }
    }

    private Color AccentColor => _variant switch
    {
        AcrBadgeVariant.Success => AcrColors.Success,
        AcrBadgeVariant.Warning => AcrColors.Warning,
        AcrBadgeVariant.Error => AcrColors.Error,
        AcrBadgeVariant.Info => AcrColors.Info,
        AcrBadgeVariant.Neutral => AcrColors.Neutral,
        _ => AcrColors.Primary,
    };

    private Rectangle CloseRect
    {
        get
        {
            int y = (Height - CloseSize) / 2;
            return new Rectangle(Width - HorizontalPadding - CloseSize, y, CloseSize, CloseSize);
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        UpdateSize();
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        UpdateSize();
    }

    private void UpdateSize()
    {
        var textSize = TextRenderer.MeasureText(string.IsNullOrEmpty(Text) ? " " : Text, Font);
        int extra = _removable ? CloseGap + CloseSize : 0;
        Size = new Size(textSize.Width + HorizontalPadding * 2 + extra, textSize.Height + VerticalPadding * 2);
        Invalidate();
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var accent = AccentColor;
        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        int radius = Height / 2;

        using var path = AcrGraphics.CreateRoundedRectPath(bounds, radius);

        var backFill = _selected
            ? accent
            : Color.FromArgb(_hoveringChip ? 40 : 24, accent.R, accent.G, accent.B);

        using (var backBrush = new SolidBrush(backFill))
            e.Graphics.FillPath(backBrush, path);

        using (var borderPen = new Pen(accent, 1))
            e.Graphics.DrawPath(borderPen, path);

        var textColor = _selected ? Color.White : accent;
        var textRect = new Rectangle(HorizontalPadding, 0, _removable ? CloseRect.Left - HorizontalPadding : Width - HorizontalPadding * 2, Height);
        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        if (_removable)
        {
            var closeRect = CloseRect;
            var closeColor = _selected ? Color.White : (_hoveringClose ? AcrColors.IconGlyphHover : accent);
            using var closePen = new Pen(closeColor, 1.5f);
            int inset = 4;
            e.Graphics.DrawLine(closePen, closeRect.Left + inset, closeRect.Top + inset, closeRect.Right - inset, closeRect.Bottom - inset);
            e.Graphics.DrawLine(closePen, closeRect.Right - inset, closeRect.Top + inset, closeRect.Left + inset, closeRect.Bottom - inset);
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _hoveringChip = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveringChip = false;
        _hoveringClose = false;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (!_removable) return;

        bool hovering = CloseRect.Contains(e.Location);
        if (hovering != _hoveringClose)
        {
            _hoveringClose = hovering;
            Invalidate();
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        if (_removable && CloseRect.Contains(e.Location))
        {
            Removed?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChipClick?.Invoke(this, EventArgs.Empty);
    }
}
