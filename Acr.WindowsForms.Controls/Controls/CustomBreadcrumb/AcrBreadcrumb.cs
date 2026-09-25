using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomBreadcrumb;

[ToolboxBitmap(typeof(ToolStrip))]
[DefaultEvent(nameof(AcrBreadcrumb.ItemClicked))]
public class AcrBreadcrumb : Control
{
    private const int GapLogical = 6;
    private int Gap => LogicalToDeviceUnits(GapLogical);
    private const int SeparatorWidthLogical = 14;
    private int SeparatorWidth => LogicalToDeviceUnits(SeparatorWidthLogical);

    public List<string> Items { get; } = new();

    private int _hoveredIndex = -1;
    private readonly List<Rectangle> _itemRects = new();

    public event EventHandler<AcrBreadcrumbItemEventArgs>? ItemClicked;

    public AcrBreadcrumb()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = AcrFonts.Get(9F);
        Size = new Size(400, 24);
        Cursor = Cursors.Default;
    }

    public void SetPath(params string[] items)
    {
        Items.Clear();
        Items.AddRange(items);
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
        _itemRects.Clear();

        int x = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            bool isLast = i == Items.Count - 1;
            var textSize = TextRenderer.MeasureText(Items[i], Font);
            var rect = new Rectangle(x, 0, textSize.Width, Height);
            _itemRects.Add(rect);

            var color = isLast
                ? AcrColors.Text
                : (i == _hoveredIndex ? AcrColors.Primary : AcrColors.Neutral);

            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
            if (!isLast) flags |= TextFormatFlags.Default;

            TextRenderer.DrawText(e.Graphics, Items[i], Font, rect, color, isLast ? (TextFormatFlags.VerticalCenter | TextFormatFlags.Left) : flags);

            x += textSize.Width;

            if (!isLast)
            {
                x += Gap;
                using var pen = new Pen(AcrColors.Neutral, 1.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                int cy = Height / 2;
                e.Graphics.DrawLine(pen, x + 3, cy - 4, x + SeparatorWidth - 5, cy);
                e.Graphics.DrawLine(pen, x + SeparatorWidth - 5, cy, x + 3, cy + 4);
                x += SeparatorWidth + Gap;
            }
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        int index = IndexAt(e.Location);
        bool clickable = index >= 0 && index < Items.Count - 1;
        Cursor = clickable ? Cursors.Hand : Cursors.Default;

        if (index != _hoveredIndex)
        {
            _hoveredIndex = index;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        Cursor = Cursors.Default;
        if (_hoveredIndex != -1)
        {
            _hoveredIndex = -1;
            Invalidate();
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        int index = IndexAt(e.Location);
        if (index >= 0 && index < Items.Count - 1)
            ItemClicked?.Invoke(this, new AcrBreadcrumbItemEventArgs(index, Items[index]));
    }

    private int IndexAt(Point p)
    {
        for (int i = 0; i < _itemRects.Count; i++)
        {
            if (_itemRects[i].Contains(p)) return i;
        }
        return -1;
    }
}

public class AcrBreadcrumbItemEventArgs : EventArgs
{
    public int Index { get; }
    public string Text { get; }

    public AcrBreadcrumbItemEventArgs(int index, string text)
    {
        Index = index;
        Text = text;
    }
}
