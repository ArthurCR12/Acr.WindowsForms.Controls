using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomAccordion;

[ToolboxBitmap(typeof(Panel))]
[DefaultEvent(nameof(AcrAccordion.ExpandedChanged))]
public class AcrAccordion : Control, IAcrThemeable
{
    private const int HeaderHeightLogical = 36;
    private int HeaderHeight => LogicalToDeviceUnits(HeaderHeightLogical);
    private const int CornerRadiusLogical = 6;
    private int CornerRadius => LogicalToDeviceUnits(CornerRadiusLogical);

    private readonly Panel _contentPanel;
    private bool _expanded = true;
    private int _expandedContentHeight = 160;
    private string _headerText = "Título da seção";
    private bool _hoveringHeader = false;
    private bool _animate = true;
    private bool _collapseSiblings;
    private Color _headerBackColor = AcrColors.Surface;
    private Color _headerHoverColor = AcrColors.SurfaceHover;
    private Color _headerForeColor = AcrColors.Text;
    private Color _expandedAccentColor = AcrColors.Primary;
    private readonly System.Windows.Forms.Timer _animationTimer = new() { Interval = 15 };
    private int _targetHeight;

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

        Font = AcrFonts.Get(9.5F, FontStyle.Bold);
        Cursor = Cursors.Hand;
        TabStop = true;
        SetStyle(ControlStyles.Selectable, true);
        _animationTimer.Tick += AnimationTimer_Tick;

        _contentPanel = new Panel
        {
            Location = new Point(1, HeaderHeight),
            BorderStyle = BorderStyle.None,
            BackColor = AcrColors.Surface,
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
    [Description("If true, expanding/collapsing is animated.")]
    [Browsable(true)]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Animate
    {
        get => _animate;
        set => _animate = value;
    }

    [Category("Acr Custom")]
    [Description("If true, expanding this accordion collapses the sibling accordions (same parent) that also have this option enabled.")]
    [Browsable(true)]
    [DefaultValue(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool CollapseSiblings
    {
        get => _collapseSiblings;
        set => _collapseSiblings = value;
    }

    [Category("Acr Custom")]
    [Description("Background color of the header.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HeaderBackColor
    {
        get => _headerBackColor;
        set { _headerBackColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Background color of the header when hovered.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HeaderHoverColor
    {
        get => _headerHoverColor;
        set { _headerHoverColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Text color of the header.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HeaderForeColor
    {
        get => _headerForeColor;
        set { _headerForeColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Color of the small bar drawn at the left of the header while expanded. Empty = no bar.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color ExpandedAccentColor
    {
        get => _expandedAccentColor;
        set { _expandedAccentColor = value; Invalidate(); }
    }

    public void Expand() => Expanded = true;
    public void Collapse() => Expanded = false;
    public void Toggle() => Expanded = !Expanded;

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
            if (_expanded && _collapseSiblings && Parent != null)
            {
                foreach (var sibling in Parent.Controls.OfType<AcrAccordion>())
                    if (sibling != this && sibling._collapseSiblings) sibling.Expanded = false;
            }
            if (_animate && IsHandleCreated && Visible && !DesignMode)
            {
                _contentPanel.Visible = true;
                _targetHeight = HeaderHeight + (_expanded ? _expandedContentHeight : 0);
                _animationTimer.Start();
            }
            else
            {
                _contentPanel.Visible = _expanded;
                ApplyHeight();
            }
            Invalidate();
            ExpandedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        int diff = _targetHeight - Height;
        int step = Math.Sign(diff) * Math.Max(4, Math.Abs(diff) / 4);
        if (Math.Abs(diff) <= Math.Abs(step))
        {
            _animationTimer.Stop();
            _contentPanel.Visible = _expanded;
            ApplyHeight();
            return;
        }
        Height += step;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _animationTimer.Dispose();
        base.Dispose(disposing);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is Keys.Enter or Keys.Space)
        {
            Toggle();
            e.Handled = true;
        }
    }

    protected override bool IsInputKey(Keys keyData) =>
        keyData is Keys.Enter or Keys.Space || base.IsInputKey(keyData);

    protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
    protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

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
        if (e.Y <= HeaderHeight)
        {
            Focus();
            Toggle();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var outerPath = AcrGraphics.CreateRoundedRectPath(bounds, CornerRadius))
        {
            using var borderPen = new Pen(AcrColors.BorderSubtle, 1);
            e.Graphics.DrawPath(borderPen, outerPath);
        }

        var headerRect = new Rectangle(1, 1, Width - 2, HeaderHeight - 1);
        using (var headerBrush = new SolidBrush(_hoveringHeader ? _headerHoverColor : _headerBackColor))
            e.Graphics.FillRectangle(headerBrush, headerRect);

        if (_expanded && _expandedAccentColor != Color.Empty)
        {
            using var accentBrush = new SolidBrush(_expandedAccentColor);
            e.Graphics.FillRectangle(accentBrush, 1, 8, 3, HeaderHeight - 16);
        }

        var textRect = new Rectangle(14, 0, Width - 50, HeaderHeight);
        TextRenderer.DrawText(e.Graphics, _headerText, Font, textRect, Enabled ? _headerForeColor : AcrColors.TextDisabled, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        DrawChevron(e.Graphics);

        if (Focused && ShowFocusCues)
        {
            var focusRect = new Rectangle(4, 4, Width - 9, HeaderHeight - 8);
            ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
        }

        if (_expanded)
        {
            using var separatorPen = new Pen(AcrColors.Separator, 1);
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

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        _headerBackColor = AcrTheme.Swap(_headerBackColor, o.Surface, n.Surface);
        _headerHoverColor = AcrTheme.Swap(_headerHoverColor, o.SurfaceHover, n.SurfaceHover);
        _headerForeColor = AcrTheme.Swap(_headerForeColor, o.Text, n.Text);
        _expandedAccentColor = AcrTheme.Swap(_expandedAccentColor, o.Primary, n.Primary);
        _contentPanel.BackColor = AcrTheme.Swap(_contentPanel.BackColor, o.Surface, n.Surface);
        foreach (Control child in _contentPanel.Controls)
        {
            if (child is IAcrThemeable themeable) { themeable.ApplyTheme(o, n); continue; }
            child.ForeColor = AcrTheme.Swap(child.ForeColor, o.Text, n.Text, SystemColors.ControlText);
        }
        Invalidate(true);
    }
}
