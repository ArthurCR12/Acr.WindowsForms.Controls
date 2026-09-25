using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomToolTip;

/// <summary>
/// ToolTip moderno: cantos arredondados, espaçamento interno e título opcional em negrito.
/// Por padrão usa um fundo escuro (contraste com o tema); use <see cref="Inverted"/> = false
/// para seguir a superfície do tema.
/// </summary>
[ToolboxBitmap(typeof(ToolTip))]
public class AcrToolTip : ToolTip
{
    private const int PaddingX = 10;
    private const int PaddingY = 6;

    public AcrToolTip()
    {
        OwnerDraw = true;
        InitialDelay = 400;
        ReshowDelay = 100;
        AutoPopDelay = 8000;
        Popup += OnPopup;
        Draw += OnDraw;
    }

    public AcrToolTip(IContainer container) : this()
    {
        container.Add(this);
    }

    [Category("Acr Custom")]
    [Description("If true (default), draws a dark tooltip on light themes and a light one on dark themes.")]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Inverted { get; set; } = true;

    [Category("Acr Custom")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Font TextFont { get; set; } = AcrFonts.Get(8.5F);

    private Color Back => Inverted ? AcrColors.Text : AcrColors.Surface;
    private Color Fore => Inverted ? AcrColors.Surface : AcrColors.Text;

    private void OnPopup(object? sender, PopupEventArgs e)
    {
        string text = GetToolTip(e.AssociatedControl);
        var size = TextRenderer.MeasureText(text, TextFont, new Size(320, int.MaxValue), TextFormatFlags.WordBreak);

        if (!string.IsNullOrEmpty(ToolTipTitle))
        {
            var titleSize = TextRenderer.MeasureText(ToolTipTitle, AcrFonts.WithStyle(TextFont, FontStyle.Bold));
            size = new Size(Math.Max(size.Width, titleSize.Width), size.Height + titleSize.Height + 2);
        }

        e.ToolTipSize = new Size(size.Width + PaddingX * 2, size.Height + PaddingY * 2);
    }

    private void OnDraw(object? sender, DrawToolTipEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // O ToolTip é retangular: pinta o fundo e desenha o corpo arredondado por cima.
        using (var clear = new SolidBrush(Back))
            g.FillRectangle(clear, e.Bounds);
        using (var path = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1), 4))
        using (var pen = new Pen(Inverted ? Back : AcrColors.BorderSubtle))
            g.DrawPath(pen, path);

        var rect = new Rectangle(PaddingX, PaddingY, e.Bounds.Width - PaddingX * 2, e.Bounds.Height - PaddingY * 2);

        if (!string.IsNullOrEmpty(ToolTipTitle))
        {
            var bold = AcrFonts.WithStyle(TextFont, FontStyle.Bold);
            int titleHeight = TextRenderer.MeasureText(ToolTipTitle, bold).Height;
            TextRenderer.DrawText(g, ToolTipTitle, bold, new Rectangle(rect.X, rect.Y, rect.Width, titleHeight), Fore, TextFormatFlags.Left);
            rect = new Rectangle(rect.X, rect.Y + titleHeight + 2, rect.Width, rect.Height - titleHeight - 2);
        }

        TextRenderer.DrawText(g, e.ToolTipText, TextFont, rect, Fore, TextFormatFlags.Left | TextFormatFlags.WordBreak);
    }
}
