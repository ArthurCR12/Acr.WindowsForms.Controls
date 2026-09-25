using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomSidebar;

public class AcrSidebarItem
{
    public string Text { get; set; }
    public string Glyph { get; set; }
    public object? Tag { get; set; }

    public AcrSidebarItem(string text, string glyph = "•", object? tag = null)
    {
        Text = text;
        Glyph = glyph;
        Tag = tag;
    }
}

public class AcrSidebar : Control
{
    private const int ItemHeight = 42;
    private const int ToggleHeight = 44;
    private const int GlyphWidth = 40;
    private const int ExpandedWidth = 220;
    private const int CollapsedWidth = 60;

    public List<AcrSidebarItem> Items { get; } = new();

    private int _selectedIndex = -1;
    private int _hoveredIndex = -1;
    private bool _collapsed = false;
    private bool _hoveringToggle = false;
    private string _headerText = "Menu";
    private readonly List<Rectangle> _itemRects = new();

    public event EventHandler<int>? ItemClick;
    public event EventHandler? CollapsedChanged;

    public AcrSidebar()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 9.5F);
        Dock = DockStyle.Left;
        BackColor = Color.FromArgb(250, 250, 251);
        Cursor = Cursors.Hand;

        Width = ExpandedWidth;
    }

    public void SetItems(params AcrSidebarItem[] items)
    {
        Items.Clear();
        Items.AddRange(items);
        if (_selectedIndex >= Items.Count) _selectedIndex = Items.Count > 0 ? 0 : -1;
        Invalidate();
    }

    [Category("Acr Custom")]
    [Description("Text shown in the header area above the menu items.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string HeaderText
    {
        get => _headerText;
        set
        {
            _headerText = value ?? string.Empty;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Index of the currently selected menu item.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            int clamped = Items.Count == 0 ? -1 : Math.Clamp(value, 0, Items.Count - 1);
            if (clamped == _selectedIndex) return;
            _selectedIndex = clamped;
            Invalidate();
            ItemClick?.Invoke(this, _selectedIndex);
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the sidebar shows only icons (no text).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Collapsed
    {
        get => _collapsed;
        set
        {
            if (_collapsed == value) return;
            _collapsed = value;
            Width = _collapsed ? CollapsedWidth : ExpandedWidth;
            Invalidate();
            CollapsedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public AcrSidebarItem? SelectedItem => _selectedIndex >= 0 && _selectedIndex < Items.Count ? Items[_selectedIndex] : null;

    private Rectangle ToggleRect => new(0, 0, Width, ToggleHeight);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        bool hoveringToggle = ToggleRect.Contains(e.Location);
        if (hoveringToggle != _hoveringToggle)
        {
            _hoveringToggle = hoveringToggle;
            Invalidate();
        }

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
        _hoveringToggle = false;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        if (ToggleRect.Contains(e.Location))
        {
            Collapsed = !Collapsed;
            return;
        }

        int index = IndexAt(e.Location);
        if (index >= 0) SelectedIndex = index;
    }

    private int IndexAt(Point p)
    {
        for (int i = 0; i < _itemRects.Count; i++)
        {
            if (_itemRects[i].Contains(p)) return i;
        }
        return -1;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        pevent.Graphics.Clear(BackColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        _itemRects.Clear();

        using (var borderPen = new Pen(Color.FromArgb(230, 230, 230), 1))
            e.Graphics.DrawLine(borderPen, Width - 1, 0, Width - 1, Height);

        DrawToggle(e.Graphics);

        if (!_collapsed)
        {
            var headerRect = new Rectangle(16, ToggleHeight + 8, Width - 32, 24);
            using var headerFont = new Font(Font, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, _headerText, headerFont, headerRect, AcrColors.Neutral, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        int y = ToggleHeight + (_collapsed ? 8 : 40);

        for (int i = 0; i < Items.Count; i++)
        {
            var itemRect = new Rectangle(8, y, Width - 16, ItemHeight);
            _itemRects.Add(itemRect);

            bool isSelected = i == _selectedIndex;
            bool isHovered = i == _hoveredIndex;

            if (isSelected || isHovered)
            {
                var fill = isSelected ? Color.FromArgb(28, AcrColors.Primary.R, AcrColors.Primary.G, AcrColors.Primary.B) : Color.FromArgb(235, 235, 236);
                using var itemPath = AcrGraphics.CreateRoundedRectPath(itemRect, 8);
                using var itemBrush = new SolidBrush(fill);
                e.Graphics.FillPath(itemBrush, itemPath);
            }

            if (isSelected)
            {
                using var accentBrush = new SolidBrush(AcrColors.Primary);
                e.Graphics.FillRectangle(accentBrush, 0, y + 6, 3, ItemHeight - 12);
            }

            var glyphRect = new Rectangle(itemRect.X, itemRect.Y, GlyphWidth, ItemHeight);
            var itemColor = isSelected ? AcrColors.Primary : AcrColors.Text;
            TextRenderer.DrawText(e.Graphics, Items[i].Glyph, Font, glyphRect, itemColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            if (!_collapsed)
            {
                var textRect = new Rectangle(itemRect.X + GlyphWidth, itemRect.Y, itemRect.Width - GlyphWidth - 8, ItemHeight);
                TextRenderer.DrawText(e.Graphics, Items[i].Text, Font, textRect, itemColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            y += ItemHeight + 2;
        }
    }

    private void DrawToggle(Graphics g)
    {
        var rect = ToggleRect;

        if (_hoveringToggle)
        {
            using var hoverBrush = new SolidBrush(Color.FromArgb(235, 235, 236));
            g.FillRectangle(hoverBrush, rect);
        }

        int cx = _collapsed ? Width / 2 : 26;
        int cy = ToggleHeight / 2;
        using var pen = new Pen(AcrColors.IconGlyph, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round };

        for (int i = -1; i <= 1; i++)
            g.DrawLine(pen, cx - 8, cy + i * 6, cx + 8, cy + i * 6);
    }
}
