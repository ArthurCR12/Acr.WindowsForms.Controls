using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomSkeleton;

public class AcrSkeleton : Control, IAcrThemeable
{
    private readonly System.Windows.Forms.Timer _timer;
    private float _shimmerOffset = -0.5f;
    private int _cornerRadius = 6;
    private Color _baseColor = AcrColors.SurfaceSunken;
    private Color _shimmerColor = AcrColors.SurfaceHover;

    public AcrSkeleton()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Size = new Size(200, 16);

        _timer = new System.Windows.Forms.Timer { Interval = 30 };
        _timer.Tick += (_, _) =>
        {
            _shimmerOffset += 0.04f;
            if (_shimmerOffset > 1.5f) _shimmerOffset = -0.5f;
            Invalidate();
        };
    }

    [Category("Acr Custom")]
    [Description("Radius, in pixels, used to round the placeholder block corners.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CornerRadius
    {
        get => _cornerRadius;
        set
        {
            _cornerRadius = Math.Max(0, value);
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Base color of the placeholder block.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BaseColor
    {
        get => _baseColor;
        set
        {
            _baseColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Highlight color used for the sweeping shimmer effect.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color ShimmerColor
    {
        get => _shimmerColor;
        set
        {
            _shimmerColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, the shimmer sweep animation is running.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool IsAnimating
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
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Math.Max(1, Width), Math.Max(1, Height));
        using var path = AcrGraphics.CreateRoundedRectPath(bounds, _cornerRadius);

        using (var baseBrush = new SolidBrush(_baseColor))
            e.Graphics.FillPath(baseBrush, path);

        if (Width <= 1) return;

        float sweepWidth = Width * 0.6f;
        float sweepX = Width * _shimmerOffset;

        using var region = new Region(path);
        var oldClip = e.Graphics.Clip;
        e.Graphics.SetClip(region, CombineMode.Replace);

        using var gradient = new LinearGradientBrush(
            new PointF(sweepX, 0),
            new PointF(sweepX + sweepWidth, 0),
            Color.FromArgb(0, _shimmerColor),
            Color.FromArgb(0, _shimmerColor))
        {
            InterpolationColors = new ColorBlend
            {
                Colors = new[] { Color.FromArgb(0, _shimmerColor), Color.FromArgb(160, _shimmerColor), Color.FromArgb(0, _shimmerColor) },
                Positions = new[] { 0f, 0.5f, 1f }
            }
        };

        e.Graphics.FillRectangle(gradient, sweepX, 0, sweepWidth, Height);
        e.Graphics.Clip = oldClip;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        _baseColor = AcrTheme.Swap(_baseColor, o.SurfaceSunken, n.SurfaceSunken);
        _shimmerColor = AcrTheme.Swap(_shimmerColor, o.SurfaceHover, n.SurfaceHover);
        Invalidate();
    }
}
