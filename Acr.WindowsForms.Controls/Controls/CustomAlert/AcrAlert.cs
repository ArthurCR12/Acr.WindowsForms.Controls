using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomAlert;

public class AcrAlert : Control
{
    private const int AccentBarWidth = 4;
    private const int CornerRadius = 6;
    private const int CloseButtonSize = 18;

    private AcrBadgeVariant _variant = AcrBadgeVariant.Info;
    private bool _showCloseButton = true;
    private bool _hoveringClose = false;

    public event EventHandler? Closed;

    public AcrAlert()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 9F);
        Size = new Size(400, 48);
        Text = "Mensagem de alerta";
    }

    [Category("Acr Custom")]
    [Description("Color variant applied to the alert's accent bar, border and text.")]
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
    [Description("If true, shows a small close (×) button on the right.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowCloseButton
    {
        get => _showCloseButton;
        set
        {
            _showCloseButton = value;
            Invalidate();
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    private Color AccentColor => _variant switch
    {
        AcrBadgeVariant.Success => AcrColors.Success,
        AcrBadgeVariant.Warning => AcrColors.Warning,
        AcrBadgeVariant.Error => AcrColors.Error,
        AcrBadgeVariant.Neutral => AcrColors.Neutral,
        AcrBadgeVariant.Primary => AcrColors.Primary,
        _ => AcrColors.Info,
    };

    private Rectangle CloseButtonRect => new(Width - CloseButtonSize - 10, (Height - CloseButtonSize) / 2, CloseButtonSize, CloseButtonSize);

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var accent = AccentColor;
        var backTint = Color.FromArgb(22, accent.R, accent.G, accent.B);
        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

        using (var path = AcrGraphics.CreateRoundedRectPath(bounds, CornerRadius))
        {
            using var backBrush = new SolidBrush(backTint);
            e.Graphics.FillPath(backBrush, path);

            using var borderPen = new Pen(accent, 1);
            e.Graphics.DrawPath(borderPen, path);
        }

        using (var accentBrush = new SolidBrush(accent))
        {
            var barRect = new Rectangle(0, 0, AccentBarWidth, Height);
            using var barPath = AcrGraphics.CreateRoundedRectPath(barRect, 2);
            e.Graphics.FillPath(accentBrush, barPath);
        }

        int rightPadding = _showCloseButton ? Width - CloseButtonRect.Left + 4 : 12;
        var textRect = new Rectangle(AccentBarWidth + 14, 0, Math.Max(0, Width - AccentBarWidth - 14 - rightPadding), Height);
        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, AcrColors.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.WordBreak);

        if (_showCloseButton)
        {
            var closeRect = CloseButtonRect;
            var closeColor = _hoveringClose ? AcrColors.IconGlyphHover : AcrColors.IconGlyph;
            using var closePen = new Pen(closeColor, 1.5f);
            int inset = 5;
            e.Graphics.DrawLine(closePen, closeRect.Left + inset, closeRect.Top + inset, closeRect.Right - inset, closeRect.Bottom - inset);
            e.Graphics.DrawLine(closePen, closeRect.Right - inset, closeRect.Top + inset, closeRect.Left + inset, closeRect.Bottom - inset);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (!_showCloseButton) return;

        bool hovering = CloseButtonRect.Contains(e.Location);
        if (hovering != _hoveringClose)
        {
            _hoveringClose = hovering;
            Cursor = hovering ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveringClose = false;
        Cursor = Cursors.Default;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (_showCloseButton && CloseButtonRect.Contains(e.Location))
        {
            Visible = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
