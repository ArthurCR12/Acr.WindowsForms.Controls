using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomAccordion;

public class AcrAccordion : Control
{
    private const int HeaderHeight = 36;
    private const int CornerRadius = 6;

    private readonly Panel _contentPanel;
    private bool _expanded = true;
    private int _expandedContentHeight = 160;
    private string _headerText = "Título da seção";
    private bool _hoveringHeader = false;

    public event EventHandler? ExpandedChanged;

    public AcrAccordion()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        Cursor = Cursors.Hand;

        _contentPanel = new Panel
        {
            Location = new Point(1, HeaderHeight),
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            Padding = new Padding(12, 8, 12, 12),
        };
        Controls.Add(_contentPanel);

        Size = new Size(360, HeaderHeight + _expandedContentHeight);
        UpdateContentPanelSize();
    }

    public Panel ContentPanel => _contentPanel;

    [Category("Acr Custom")]
    [Description("Header text shown at the top of the accordion.")]
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
    [Description("Height, in pixels, of the content area when expanded.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int ExpandedContentHeight
    {
        get => _expandedContentHeight;
        set
        {
            _expandedContentHeight = Math.Max(0, value);
            if (_expanded) ApplyHeight();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the content area is visible.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Expanded
    {
        get => _expanded;
        set
        {
            if (_expanded == value) return;
            _expanded = value;
            _contentPanel.Visible = _expanded;
            ApplyHeight();
            Invalidate();
            ExpandedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ApplyHeight()
    {
        Height = HeaderHeight + (_expanded ? _expandedContentHeight : 0);
        UpdateContentPanelSize();
    }

    private void UpdateContentPanelSize()
    {
        _contentPanel.Size = new Size(Math.Max(0, Width - 2), Math.Max(0, Height - HeaderHeight - 1));
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateContentPanelSize();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        bool hovering = e.Y <= HeaderHeight;
        if (hovering != _hoveringHeader)
        {
            _hoveringHeader = hovering;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveringHeader = false;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (e.Y <= HeaderHeight) Expanded = !Expanded;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var outerPath = AcrGraphics.CreateRoundedRectPath(bounds, CornerRadius))
        {
            using var borderPen = new Pen(Color.FromArgb(225, 225, 225), 1);
            e.Graphics.DrawPath(borderPen, outerPath);
        }

        var headerRect = new Rectangle(1, 1, Width - 2, HeaderHeight - 1);
        using (var headerBrush = new SolidBrush(_hoveringHeader ? Color.FromArgb(248, 248, 248) : Color.White))
            e.Graphics.FillRectangle(headerBrush, headerRect);

        var textRect = new Rectangle(14, 0, Width - 50, HeaderHeight);
        TextRenderer.DrawText(e.Graphics, _headerText, Font, textRect, AcrColors.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        DrawChevron(e.Graphics);

        if (_expanded)
        {
            using var separatorPen = new Pen(Color.FromArgb(230, 230, 230), 1);
            e.Graphics.DrawLine(separatorPen, 1, HeaderHeight, Width - 2, HeaderHeight);
        }
    }

    private void DrawChevron(Graphics g)
    {
        int cx = Width - 24;
        int cy = HeaderHeight / 2;
        int size = 5;

        using var pen = new Pen(AcrColors.IconGlyph, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round };

        if (_expanded)
        {
            // Downward chevron (∨) — content is open, click to collapse.
            g.DrawLine(pen, cx - size, cy - size / 2, cx, cy + size / 2);
            g.DrawLine(pen, cx, cy + size / 2, cx + size, cy - size / 2);
        }
        else
        {
            // Rightward chevron (>) — content is closed, click to expand.
            g.DrawLine(pen, cx - size / 2, cy - size, cx + size / 2, cy);
            g.DrawLine(pen, cx + size / 2, cy, cx - size / 2, cy + size);
        }
    }
}
