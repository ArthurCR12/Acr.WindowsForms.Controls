using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomSegmentedControl;

public class AcrSegmentedControl : Control
{
    private const int Padding = 3;

    public List<string> Items { get; } = new();

    private int _selectedIndex = 0;
    private int _hoveredIndex = -1;
    private Color _accentColor = AcrColors.Primary;

    public event EventHandler<int>? SelectedIndexChanged;

    public AcrSegmentedControl()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Cursor = Cursors.Hand;
        Font = new Font("Segoe UI", 9F);
        Size = new Size(300, 32);
    }

    public void SetItems(params string[] items)
    {
        Items.Clear();
        Items.AddRange(items);
        _selectedIndex = Math.Clamp(_selectedIndex, 0, Math.Max(0, Items.Count - 1));
        Invalidate();
    }

    [Category("Acr Custom")]
    [Description("Index of the currently selected segment.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (Items.Count == 0) return;
            int clamped = Math.Clamp(value, 0, Items.Count - 1);
            if (clamped == _selectedIndex) return;
            _selectedIndex = clamped;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, _selectedIndex);
        }
    }

    [Category("Acr Custom")]
    [Description("Color used for the selected segment's background.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => _accentColor;
        set
        {
            _accentColor = value;
            Invalidate();
        }
    }

    public string? SelectedItem => _selectedIndex >= 0 && _selectedIndex < Items.Count ? Items[_selectedIndex] : null;

    private int SegmentWidth => Items.Count == 0 ? 0 : (Width - Padding * 2) / Items.Count;

    private int IndexAt(Point p)
    {
        if (Items.Count == 0 || SegmentWidth <= 0) return -1;
        int index = (p.X - Padding) / SegmentWidth;
        return index >= 0 && index < Items.Count ? index : -1;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int index = IndexAt(e.Location);
        if (index != _hoveredIndex)
        {
            _hoveredIndex = index;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveredIndex = -1;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        int index = IndexAt(e.Location);
        if (index >= 0) SelectedIndex = index;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var outerRect = new Rectangle(0, 0, Width - 1, Height - 1);
        int radius = Math.Min(Height / 2, 8);

        using (var outerPath = AcrGraphics.CreateRoundedRectPath(outerRect, radius))
        {
            using var backBrush = new SolidBrush(Color.FromArgb(240, 241, 243));
            e.Graphics.FillPath(backBrush, outerPath);

            using var borderPen = new Pen(Color.FromArgb(225, 225, 225), 1);
            e.Graphics.DrawPath(borderPen, outerPath);
        }

        if (Items.Count == 0) return;

        int segmentWidth = SegmentWidth;

        var selectedRect = new Rectangle(Padding + _selectedIndex * segmentWidth, Padding, segmentWidth, Height - Padding * 2);
        using (var selectedPath = AcrGraphics.CreateRoundedRectPath(selectedRect, Math.Max(1, radius - 2)))
        using (var selectedBrush = new SolidBrush(Color.White))
        {
            e.Graphics.FillPath(selectedBrush, selectedPath);
            using var selectedBorder = new Pen(_accentColor, 1);
            e.Graphics.DrawPath(selectedBorder, selectedPath);
        }

        for (int i = 0; i < Items.Count; i++)
        {
            var rect = new Rectangle(Padding + i * segmentWidth, 0, segmentWidth, Height);

            if (i == _hoveredIndex && i != _selectedIndex)
            {
                using var hoverBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0));
                using var hoverPath = AcrGraphics.CreateRoundedRectPath(new Rectangle(rect.X, Padding, rect.Width, Height - Padding * 2), Math.Max(1, radius - 2));
                e.Graphics.FillPath(hoverBrush, hoverPath);
            }

            var textColor = i == _selectedIndex ? _accentColor : AcrColors.Neutral;
            TextRenderer.DrawText(e.Graphics, Items[i], Font, rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
