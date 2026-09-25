using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomBadge;

[ToolboxBitmap(typeof(Label))]
public class AcrBadge : Control
{
    private AcrBadgeVariant _variant = AcrBadgeVariant.Primary;
    private int _horizontalPadding = 10;
    private int _verticalPadding = 4;

    public AcrBadge()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        BackColor = Color.Transparent;
        Font = AcrFonts.Get(8F, FontStyle.Bold);
        Text = "Badge";
        UpdateSize();
    }

    [Category("Acr Custom")]
    [Description("Color variant applied to the badge background and text.")]
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
    [Description("Horizontal padding, in pixels, applied on each side of the badge text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int HorizontalPadding
    {
        get => _horizontalPadding;
        set
        {
            _horizontalPadding = Math.Max(0, value);
            UpdateSize();
        }
    }

    [Category("Acr Custom")]
    [Description("Vertical padding, in pixels, applied above and below the badge text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int VerticalPadding
    {
        get => _verticalPadding;
        set
        {
            _verticalPadding = Math.Max(0, value);
            UpdateSize();
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
        Size = new Size(textSize.Width + _horizontalPadding * 2, textSize.Height + _verticalPadding * 2);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var accent = AccentColor;
        var backColor = Color.FromArgb(28, accent.R, accent.G, accent.B);
        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        int radius = Height / 2;

        using var path = AcrGraphics.CreateRoundedRectPath(bounds, radius);

        using (var backBrush = new SolidBrush(backColor))
            e.Graphics.FillPath(backBrush, path);

        using (var borderPen = new Pen(accent, 1))
            e.Graphics.DrawPath(borderPen, path);

        TextRenderer.DrawText(
            e.Graphics,
            Text,
            Font,
            ClientRectangle,
            accent,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    protected override Size DefaultSize => new(60, 22);
}
