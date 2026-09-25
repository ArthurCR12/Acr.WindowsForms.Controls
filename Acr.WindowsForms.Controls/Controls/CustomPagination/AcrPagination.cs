using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomPagination;

public class AcrPagination : Control
{
    private const int ButtonSize = 28;
    private const int Gap = 6;
    private const int PrevIndex = -1;
    private const int NextIndex = -2;
    private const int EllipsisIndex = -3;

    private int _pageCount = 1;
    private int _currentPage = 1;
    private int _hoveredButton = int.MinValue;

    private readonly List<(Rectangle Rect, int Page)> _buttons = new();

    public event EventHandler<int>? PageChanged;

    public AcrPagination()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 9F);
        Size = new Size(320, ButtonSize);
        Cursor = Cursors.Hand;
    }

    [Category("Acr Custom")]
    [Description("Total number of pages.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int PageCount
    {
        get => _pageCount;
        set
        {
            _pageCount = Math.Max(1, value);
            if (_currentPage > _pageCount) _currentPage = _pageCount;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("The current, 1-based, selected page.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CurrentPage
    {
        get => _currentPage;
        set => SetCurrentPage(value, raiseEvent: false);
    }

    private void SetCurrentPage(int page, bool raiseEvent)
    {
        page = Math.Clamp(page, 1, _pageCount);
        if (page == _currentPage) return;

        _currentPage = page;
        Invalidate();

        if (raiseEvent) PageChanged?.Invoke(this, _currentPage);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? BackColor;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        _buttons.Clear();

        int x = 0;
        int y = (Height - ButtonSize) / 2;

        x = DrawButton(e.Graphics, x, y, PrevIndex, "‹", _currentPage > 1);

        foreach (var page in BuildPageSequence())
        {
            if (page == EllipsisIndex)
            {
                TextRenderer.DrawText(e.Graphics, "…", Font, new Rectangle(x, y, ButtonSize, ButtonSize), AcrColors.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                _buttons.Add((new Rectangle(x, y, ButtonSize, ButtonSize), EllipsisIndex));
                x += ButtonSize + Gap;
            }
            else
            {
                x = DrawButton(e.Graphics, x, y, page, page.ToString(), true);
            }
        }

        DrawButton(e.Graphics, x, y, NextIndex, "›", _currentPage < _pageCount);
    }

    private int DrawButton(Graphics g, int x, int y, int page, string text, bool enabled)
    {
        var rect = new Rectangle(x, y, ButtonSize, ButtonSize);

        bool isCurrent = page == _currentPage;
        bool isHovered = enabled && _hoveredButton == page;

        var backColor = isCurrent
            ? AcrColors.Primary
            : isHovered
                ? Color.FromArgb(240, 240, 240)
                : Color.White;

        var borderColor = isCurrent ? AcrColors.Primary : AcrColors.Border;
        var textColor = !enabled
            ? AcrColors.DisabledFore
            : isCurrent
                ? Color.White
                : AcrColors.Text;

        using (var path = AcrGraphics.CreateRoundedRectPath(rect, 6))
        {
            using var backBrush = new SolidBrush(backColor);
            g.FillPath(backBrush, path);

            using var borderPen = new Pen(borderColor, 1);
            g.DrawPath(borderPen, path);
        }

        TextRenderer.DrawText(g, text, Font, rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        if (enabled) _buttons.Add((rect, page));

        return x + ButtonSize + Gap;
    }

    private IEnumerable<int> BuildPageSequence()
    {
        const int window = 1;

        if (_pageCount <= 7)
        {
            for (int i = 1; i <= _pageCount; i++) yield return i;
            yield break;
        }

        yield return 1;

        int start = Math.Max(2, _currentPage - window);
        int end = Math.Min(_pageCount - 1, _currentPage + window);

        if (start > 2) yield return EllipsisIndex;

        for (int i = start; i <= end; i++) yield return i;

        if (end < _pageCount - 1) yield return EllipsisIndex;

        yield return _pageCount;
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        foreach (var (rect, page) in _buttons)
        {
            if (!rect.Contains(e.Location)) continue;

            switch (page)
            {
                case PrevIndex:
                    SetCurrentPage(_currentPage - 1, raiseEvent: true);
                    break;
                case NextIndex:
                    SetCurrentPage(_currentPage + 1, raiseEvent: true);
                    break;
                case EllipsisIndex:
                    break;
                default:
                    SetCurrentPage(page, raiseEvent: true);
                    break;
            }
            return;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        int hovered = int.MinValue;
        foreach (var (rect, page) in _buttons)
        {
            if (rect.Contains(e.Location) && page != EllipsisIndex)
            {
                hovered = page;
                break;
            }
        }

        if (hovered != _hoveredButton)
        {
            _hoveredButton = hovered;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveredButton = int.MinValue;
        Invalidate();
    }
}
