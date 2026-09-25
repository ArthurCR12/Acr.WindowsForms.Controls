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
    private int _contentTopPadding = 16;
    private Color _titleColor = AcrColors.Text;
    private Color _accentColor = Color.Empty;
    private int _borderWidth = 1;
    private bool _hoverEffect;
    private bool _hovering;

    /// <summary>Disparado ao clicar no card ou em qualquer controle dentro dele.</summary>
    public event EventHandler? CardClick;

    public AcrCard()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Padding = new Padding(16);
        Font = new Font("Segoe UI", 9F);
        DoubleBuffered = true;
    }

    [Category("Acr Custom")]
    [Description("Color of the title text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color TitleColor
    {
        get => _titleColor;
        set { _titleColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Optional accent color drawn as a stripe on the left side of the card. Empty = no stripe.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => _accentColor;
        set { _accentColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Width, in pixels, of the card outline. 0 hides the border.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderWidth
    {
        get => _borderWidth;
        set { _borderWidth = Math.Max(0, value); Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("If true, the border is highlighted and the cursor becomes a hand when hovering (clickable card).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool HoverEffect
    {
        get => _hoverEffect;
        set
        {
            _hoverEffect = value;
            Cursor = value ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        e.Control!.MouseEnter += Child_MouseChanged;
        e.Control.MouseLeave += Child_MouseChanged;
        e.Control.Click += Child_Click;
    }

    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        e.Control!.MouseEnter -= Child_MouseChanged;
        e.Control.MouseLeave -= Child_MouseChanged;
        e.Control.Click -= Child_Click;
    }

    private void Child_MouseChanged(object? sender, EventArgs e) => UpdateHover();
    private void Child_Click(object? sender, EventArgs e) => CardClick?.Invoke(this, e);

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); UpdateHover(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); UpdateHover(); }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        CardClick?.Invoke(this, e);
    }

    private void UpdateHover()
    {
        bool hovering = _hoverEffect && IsHandleCreated && ClientRectangle.Contains(PointToClient(Cursor.Position));
        if (hovering == _hovering) return;
        _hovering = hovering;
        Invalidate();
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
            UpdateRegion();
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
        if (_titleHeight == 0) _contentTopPadding = Padding.Top;

        if (string.IsNullOrEmpty(_title))
        {
            _titleHeight = 0;
            Padding = new Padding(Padding.Left, _contentTopPadding, Padding.Right, Padding.Bottom);
            return;
        }

        using var titleFont = new Font(Font, FontStyle.Bold);
        _titleHeight = TextRenderer.MeasureText(_title, titleFont).Height + 16;
        Padding = new Padding(Padding.Left, _titleHeight, Padding.Right, Padding.Bottom);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var parentBack = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(parentBack.A == 0 ? Color.White : parentBack);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = AcrGraphics.CreateRoundedRectPath(bounds, _cornerRadius);

        using (var backBrush = new SolidBrush(BackColor))
            e.Graphics.FillPath(backBrush, path);

        if (_accentColor != Color.Empty)
        {
            var state = e.Graphics.Save();
            e.Graphics.SetClip(path);
            using var accentBrush = new SolidBrush(_accentColor);
            e.Graphics.FillRectangle(accentBrush, 0, 0, 4, Height);
            e.Graphics.Restore(state);
        }

        var borderColor = _hovering ? AcrColors.Primary : _borderColor;
        if (_borderWidth > 0)
        {
            using var borderPen = new Pen(borderColor, _borderWidth) { Alignment = PenAlignment.Inset };
            e.Graphics.DrawPath(borderPen, path);
        }

        if (!string.IsNullOrEmpty(_title))
        {
            using var titleFont = new Font(Font, FontStyle.Bold);
            var titleRect = new Rectangle(16, 0, Width - 32, _titleHeight);
            TextRenderer.DrawText(e.Graphics, _title, titleFont, titleRect, _titleColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

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

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        if (!string.IsNullOrEmpty(_title)) UpdateTitleLayout();
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), _cornerRadius);
        var old = Region;
        Region = new Region(path);
        old?.Dispose();
    }
}
