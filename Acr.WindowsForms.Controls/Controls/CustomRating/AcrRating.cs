using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomRating;

public class AcrRating : Control, IAcrThemeable
{
    private const int Gap = 4;

    private int _starCount = 5;
    private int _starSize = 24;
    private int _value = 0;
    private int _hoverValue = -1;
    private bool _readOnly = false;
    private Color _filledColor = Color.FromArgb(255, 180, 0);
    private Color _emptyColor = AcrColors.Track;

    public event EventHandler<int>? ValueChanged;

    public AcrRating()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Cursor = Cursors.Hand;
        UpdateSize();
    }

    [Category("Acr Custom")]
    [Description("Number of stars shown.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int StarCount
    {
        get => _starCount;
        set
        {
            _starCount = Math.Max(1, value);
            Value = Math.Min(_value, _starCount);
            UpdateSize();
        }
    }

    [Category("Acr Custom")]
    [Description("Size, in pixels, of each star.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int StarSize
    {
        get => _starSize;
        set
        {
            _starSize = Math.Max(8, value);
            UpdateSize();
        }
    }

    [Category("Acr Custom")]
    [Description("Current rating value (0 to StarCount).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Value
    {
        get => _value;
        set
        {
            int clamped = Math.Clamp(value, 0, _starCount);
            if (clamped == _value) return;
            _value = clamped;
            Invalidate();
            ValueChanged?.Invoke(this, _value);
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the rating cannot be changed by the user.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ReadOnly
    {
        get => _readOnly;
        set
        {
            _readOnly = value;
            Cursor = value ? Cursors.Default : Cursors.Hand;
        }
    }

    [Category("Acr Custom")]
    [Description("Color used for filled stars.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FilledColor
    {
        get => _filledColor;
        set
        {
            _filledColor = value;
            Invalidate();
        }
    }

    private void UpdateSize()
    {
        Size = new Size(_starCount * _starSize + (_starCount - 1) * Gap, _starSize);
        Invalidate();
    }

    private int StarIndexAt(Point p)
    {
        if (p.X < 0) return -1;
        int index = p.X / (_starSize + Gap);
        return index >= 0 && index < _starCount ? index : -1;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_readOnly) return;

        int index = StarIndexAt(e.Location);
        int newHover = index >= 0 ? index + 1 : -1;
        if (newHover != _hoverValue)
        {
            _hoverValue = newHover;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hoverValue != -1)
        {
            _hoverValue = -1;
            Invalidate();
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (_readOnly) return;

        int index = StarIndexAt(e.Location);
        if (index >= 0) Value = index + 1;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int displayValue = _hoverValue >= 0 ? _hoverValue : _value;

        for (int i = 0; i < _starCount; i++)
        {
            var rect = new Rectangle(i * (_starSize + Gap), 0, _starSize, _starSize);
            bool filled = i < displayValue;
            using var brush = new SolidBrush(filled ? _filledColor : _emptyColor);
            e.Graphics.FillPath(brush, CreateStarPath(rect));
        }
    }

    private static GraphicsPath CreateStarPath(Rectangle bounds)
    {
        var path = new GraphicsPath();
        var center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
        float outerRadius = bounds.Width / 2f;
        float innerRadius = outerRadius * 0.42f;

        var points = new PointF[10];
        for (int i = 0; i < 10; i++)
        {
            double angle = Math.PI / 2 * 3 + i * Math.PI / 5;
            float radius = i % 2 == 0 ? outerRadius : innerRadius;
            points[i] = new PointF(
                center.X + (float)(radius * Math.Cos(angle)),
                center.Y + (float)(radius * Math.Sin(angle)));
        }

        path.AddPolygon(points);
        return path;
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        _emptyColor = AcrTheme.Swap(_emptyColor, o.Track, n.Track);
        Invalidate();
    }
}
