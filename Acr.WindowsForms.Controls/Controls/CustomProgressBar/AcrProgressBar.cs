using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomProgressBar;

public class AcrProgressBar : Control
{
    private int _minimum = 0;
    private int _maximum = 100;
    private int _value = 0;
    private Color _fillColor = AcrColors.Primary;
    private bool _showPercentageText = true;

    public AcrProgressBar()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 8F);
        Size = new Size(200, 18);
        ForeColor = Color.White;
    }

    [Category("Acr Custom")]
    [Description("Minimum value of the progress range.")]
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
    [Description("Maximum value of the progress range.")]
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
    [Description("Current progress value.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Value
    {
        get => _value;
        set
        {
            _value = Math.Clamp(value, _minimum, _maximum);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Color used to fill the completed portion of the bar.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, draws the completion percentage as text over the bar.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowPercentageText
    {
        get => _showPercentageText;
        set
        {
            _showPercentageText = value;
            Invalidate();
        }
    }

    public double Percentage => _maximum == _minimum ? 0 : (double)(_value - _minimum) / (_maximum - _minimum);

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? BackColor;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int radius = Math.Min(Height / 2, 8);
        var trackRect = new Rectangle(0, 0, Math.Max(0, Width - 1), Math.Max(0, Height - 1));

        using (var trackPath = AcrGraphics.CreateRoundedRectPath(trackRect, radius))
        {
            using var trackBrush = new SolidBrush(Color.FromArgb(235, 235, 235));
            e.Graphics.FillPath(trackBrush, trackPath);
        }

        int fillWidth = (int)(trackRect.Width * Percentage);
        if (fillWidth > 0)
        {
            var fillRect = new Rectangle(0, 0, fillWidth, trackRect.Height);
            using var fillPath = AcrGraphics.CreateRoundedRectPath(fillRect, radius);
            using var fillBrush = new SolidBrush(_fillColor);
            e.Graphics.FillPath(fillBrush, fillPath);
        }

        using (var borderPen = new Pen(Color.FromArgb(220, 220, 220), 1))
            e.Graphics.DrawPath(borderPen, AcrGraphics.CreateRoundedRectPath(trackRect, radius));

        if (_showPercentageText)
        {
            var text = $"{Percentage:P0}";
            var textColor = fillWidth > Width / 2 ? ForeColor : AcrColors.Text;
            TextRenderer.DrawText(e.Graphics, text, Font, ClientRectangle, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
