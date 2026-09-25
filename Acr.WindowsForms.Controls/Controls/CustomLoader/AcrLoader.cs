using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomLoader;

public class AcrLoader : Control
{
    private readonly System.Windows.Forms.Timer _timer;
    private int _angle = 0;
    private int _lineWeight = 4;
    private Color _spinnerColor = AcrColors.Primary;
    private int _speed = 40;

    public AcrLoader()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Size = new Size(32, 32);
        BackColor = Color.Transparent;

        _timer = new System.Windows.Forms.Timer { Interval = _speed };
        _timer.Tick += (_, _) =>
        {
            _angle = (_angle + 12) % 360;
            Invalidate();
        };
    }

    [Category("Acr Custom")]
    [Description("Color of the spinning arc.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color SpinnerColor
    {
        get => _spinnerColor;
        set
        {
            _spinnerColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Thickness, in pixels, of the spinning arc.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int LineWeight
    {
        get => _lineWeight;
        set
        {
            _lineWeight = Math.Max(1, value);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Interval, in milliseconds, between animation frames. Lower is faster.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Speed
    {
        get => _speed;
        set
        {
            _speed = Math.Max(1, value);
            _timer.Interval = _speed;
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the spinner animates. Set to false to pause it hidden.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool IsSpinning
    {
        get => _timer.Enabled;
        set => _timer.Enabled = value;
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        _timer.Enabled = Visible && !DesignMode;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        _timer.Enabled = Visible && !DesignMode;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // intentionally left blank to support a transparent background
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var parentBack = Parent?.BackColor ?? Color.White;
        e.Graphics.Clear(parentBack.A == 0 ? Color.White : parentBack);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int diameter = Math.Min(Width, Height) - _lineWeight;
        if (diameter <= 0) return;

        var rect = new Rectangle((Width - diameter) / 2, (Height - diameter) / 2, diameter, diameter);

        using var trackPen = new Pen(Color.FromArgb(40, _spinnerColor), _lineWeight);
        e.Graphics.DrawEllipse(trackPen, rect);

        using var arcPen = new Pen(_spinnerColor, _lineWeight) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        e.Graphics.DrawArc(arcPen, rect, _angle, 90);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
