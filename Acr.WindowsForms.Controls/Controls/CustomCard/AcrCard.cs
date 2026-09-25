using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomCard;

public class AcrCard : Panel
{
    private int _cornerRadius = 10;
    private Color _borderColor = Color.FromArgb(225, 225, 225);
    private string _title = string.Empty;
    private int _titleHeight = 0;

    public AcrCard()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Padding = new Padding(16);
        Font = new Font("Segoe UI", 9F);
    }

    [Category("Acr Custom")]
    [Description("Radius, in pixels, used to round the card corners.")]
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
    [Description("Color used for the card outline.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Optional title text drawn at the top of the card, above its content.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => _title;
        set
        {
            _title = value ?? string.Empty;
            UpdateTitleLayout();
            Invalidate();
        }
    }

    private void UpdateTitleLayout()
    {
        if (string.IsNullOrEmpty(_title))
        {
            _titleHeight = 0;
            Padding = new Padding(Padding.Left, 16, Padding.Right, Padding.Bottom);
            return;
        }

        using var titleFont = new Font(Font, FontStyle.Bold);
        _titleHeight = TextRenderer.MeasureText(_title, titleFont).Height + 16;
        Padding = new Padding(Padding.Left, _titleHeight, Padding.Right, Padding.Bottom);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = AcrGraphics.CreateRoundedRectPath(bounds, _cornerRadius);

        using (var backBrush = new SolidBrush(BackColor))
            e.Graphics.FillPath(backBrush, path);

        using (var borderPen = new Pen(_borderColor, 1))
            e.Graphics.DrawPath(borderPen, path);

        if (!string.IsNullOrEmpty(_title))
        {
            using var titleFont = new Font(Font, FontStyle.Bold);
            var titleRect = new Rectangle(16, 0, Width - 32, _titleHeight);
            TextRenderer.DrawText(e.Graphics, _title, titleFont, titleRect, AcrColors.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            using var separatorPen = new Pen(Color.FromArgb(235, 235, 235), 1);
            e.Graphics.DrawLine(separatorPen, 16, _titleHeight - 1, Width - 16, _titleHeight - 1);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateRegion();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateRegion();
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        Region = new Region(AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), _cornerRadius));
    }
}
