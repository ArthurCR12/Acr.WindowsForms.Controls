using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomSlider;

public class AcrSlider : Control
{
    private const int TrackHeight = 4;
    private const int ThumbDiameter = 16;

    private int _minimum = 0;
    private int _maximum = 100;
    private int _value = 50;
    private bool _dragging = false;
    private bool _hoveringThumb = false;
    private Color _accentColor = AcrColors.Primary;
    private bool _showValueLabel = true;

    public event EventHandler<int>? ValueChanged;

    public AcrSlider()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 8F);
        Size = new Size(220, 32);
    }

    [Category("Acr Custom")]
    [Description("Minimum value of the slider range.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Minimum
    {
        get => _minimum;
        set
        {
            _minimum = value;
            if (_maximum < _minimum) _maximum = _minimum;
            Value = Math.Clamp(_value, _minimum, _maximum);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Maximum value of the slider range.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Maximum
    {
        get => _maximum;
        set
        {
            _maximum = value;
            if (_minimum > _maximum) _minimum = _maximum;
            Value = Math.Clamp(_value, _minimum, _maximum);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Current slider value.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Value
    {
        get => _value;
        set
        {
            int clamped = Math.Clamp(value, _minimum, _maximum);
            if (clamped == _value) return;
            _value = clamped;
            Invalidate();
            ValueChanged?.Invoke(this, _value);
        }
    }

    [Category("Acr Custom")]
    [Description("Color used for the filled portion of the track and the thumb.")]
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

    [Category("Acr Custom")]
    [Description("If true, draws the current value above the thumb.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowValueLabel
    {
        get => _showValueLabel;
        set
        {
            _showValueLabel = value;
            Invalidate();
        }
    }

    private int TrackY => _showValueLabel ? (Height / 2) + 6 : Height / 2;

    private double Fraction => _maximum == _minimum ? 0 : (double)(_value - _minimum) / (_maximum - _minimum);

    private int ThumbX => ThumbDiameter / 2 + (int)((Width - ThumbDiameter) * Fraction);

    private Rectangle ThumbRect => new(ThumbX - ThumbDiameter / 2, TrackY - ThumbDiameter / 2, ThumbDiameter, ThumbDiameter);

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (!Enabled) return;

        _dragging = true;
        UpdateValueFromMouse(e.X);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        bool hovering = ThumbRect.Contains(e.Location);
        if (hovering != _hoveringThumb)
        {
            _hoveringThumb = hovering;
            Invalidate();
        }

        if (_dragging) UpdateValueFromMouse(e.X);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _dragging = false;
    }

    private void UpdateValueFromMouse(int x)
    {
        int span = Width - ThumbDiameter;
        if (span <= 0) return;

        double fraction = Math.Clamp((x - ThumbDiameter / 2.0) / span, 0, 1);
        Value = _minimum + (int)Math.Round(fraction * (_maximum - _minimum));
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int trackY = TrackY;
        var trackRect = new Rectangle(ThumbDiameter / 2, trackY - TrackHeight / 2, Math.Max(0, Width - ThumbDiameter), TrackHeight);

        using (var trackPath = AcrGraphics.CreateRoundedRectPath(trackRect, TrackHeight / 2))
        using (var trackBrush = new SolidBrush(Color.FromArgb(225, 225, 225)))
            e.Graphics.FillPath(trackBrush, trackPath);

        var fillRect = new Rectangle(trackRect.X, trackRect.Y, ThumbX - trackRect.X, TrackHeight);
        if (fillRect.Width > 0)
        {
            using var fillPath = AcrGraphics.CreateRoundedRectPath(fillRect, TrackHeight / 2);
            using var fillBrush = new SolidBrush(Enabled ? _accentColor : AcrColors.DisabledFore);
            e.Graphics.FillPath(fillBrush, fillPath);
        }

        var thumbRect = ThumbRect;
        var thumbColor = Enabled ? _accentColor : AcrColors.DisabledFore;

        using (var haloBrush = new SolidBrush(Color.FromArgb(_hoveringThumb || _dragging ? 40 : 0, thumbColor)))
            e.Graphics.FillEllipse(haloBrush, Rectangle.Inflate(thumbRect, 6, 6));

        using (var thumbBrush = new SolidBrush(Color.White))
            e.Graphics.FillEllipse(thumbBrush, thumbRect);

        using (var thumbPen = new Pen(thumbColor, 2))
            e.Graphics.DrawEllipse(thumbPen, thumbRect);

        if (_showValueLabel)
        {
            var text = _value.ToString();
            var textSize = TextRenderer.MeasureText(text, Font);
            var textRect = new Rectangle(ThumbX - textSize.Width / 2, 0, textSize.Width, textSize.Height);
            TextRenderer.DrawText(e.Graphics, text, Font, textRect, AcrColors.Text);
        }
    }
}
