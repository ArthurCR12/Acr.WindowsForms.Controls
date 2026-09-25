using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomTabControl;

/// <summary>
/// TabControl moderno: abas planas com sublinhado na aba ativa, hover e cores do tema.
/// Use como um TabControl comum (TabPages no Designer).
/// </summary>
[ToolboxBitmap(typeof(TabControl))]
public class AcrTabControl : TabControl, IAcrThemeable
{
    private const int HeaderHeightLogical = 38;
    private const int TabPaddingLogical = 18;
    private const int IndicatorHeightLogical = 3;

    private int _hoveredIndex = -1;
    private Color _accentColor = AcrColors.Primary;

    public AcrTabControl()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        Font = AcrFonts.Get(9.5F);
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(0, HeaderHeightLogical);
    }

    [Category("Acr Custom")]
    [Description("Color of the active tab text and underline.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => _accentColor;
        set { _accentColor = value; Invalidate(); }
    }

    private int HeaderHeight => LogicalToDeviceUnits(HeaderHeightLogical);

    /// <summary>Área das páginas: logo abaixo do cabeçalho, sem as bordas 3D do TabControl nativo.</summary>
    public override Rectangle DisplayRectangle =>
        new(0, HeaderHeight + 1, Width, Math.Max(0, Height - HeaderHeight - 1));

    private Rectangle TabRect(int index)
    {
        int x = 0;
        int pad = LogicalToDeviceUnits(TabPaddingLogical);
        for (int i = 0; i < TabCount; i++)
        {
            int w = TextRenderer.MeasureText(TabPages[i].Text, AcrFonts.WithStyle(Font, FontStyle.Bold)).Width + pad * 2;
            if (i == index) return new Rectangle(x, 0, w, HeaderHeight);
            x += w;
        }
        return Rectangle.Empty;
    }

    private int IndexAt(Point p)
    {
        for (int i = 0; i < TabCount; i++)
            if (TabRect(i).Contains(p)) return i;
        return -1;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        // Seleção pela nossa geometria (as abas nativas têm outro tamanho).
        int index = IndexAt(e.Location);
        if (index >= 0 && e.Button == MouseButtons.Left) SelectedIndex = index;
        else base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int index = IndexAt(e.Location);
        if (index == _hoveredIndex) return;
        _hoveredIndex = index;
        Cursor = index >= 0 ? Cursors.Hand : Cursors.Default;
        Invalidate(new Rectangle(0, 0, Width, HeaderHeight + 1));
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveredIndex = -1;
        Invalidate(new Rectangle(0, 0, Width, HeaderHeight + 1));
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        base.OnSelectedIndexChanged(e);
        Invalidate();
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TabPage page)
        {
            page.BackColor = AcrTheme.Swap(page.BackColor, AcrColors.Surface, AcrColors.Surface, SystemColors.Control, Color.Transparent);
            page.TextChanged += (_, _) => Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(AcrColors.ParentBack(this));

        using (var lineBrush = new SolidBrush(AcrColors.Separator))
            g.FillRectangle(lineBrush, 0, HeaderHeight, Width, 1);

        int indicator = LogicalToDeviceUnits(IndicatorHeightLogical);
        for (int i = 0; i < TabCount; i++)
        {
            var rect = TabRect(i);
            bool selected = i == SelectedIndex;
            bool hovered = i == _hoveredIndex && !selected;

            if (hovered)
            {
                using var hover = new SolidBrush(AcrColors.SurfaceHover);
                g.FillRectangle(hover, rect.X, rect.Y + 4, rect.Width, rect.Height - 8);
            }

            var font = selected ? AcrFonts.WithStyle(Font, FontStyle.Bold) : Font;
            var color = !Enabled ? AcrColors.TextDisabled : selected ? _accentColor : hovered ? AcrColors.Text : AcrColors.TextMuted;
            TextRenderer.DrawText(g, TabPages[i].Text, font, rect, color,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            if (selected)
            {
                using var accent = new SolidBrush(_accentColor);
                g.FillRectangle(accent, rect.X + 6, HeaderHeight - indicator + 1, rect.Width - 12, indicator);
            }
        }
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        _accentColor = AcrTheme.Swap(_accentColor, o.Primary, n.Primary);
        foreach (TabPage page in TabPages)
            page.BackColor = AcrTheme.Swap(page.BackColor, o.Surface, n.Surface, o.Background);
        Invalidate();
    }
}
