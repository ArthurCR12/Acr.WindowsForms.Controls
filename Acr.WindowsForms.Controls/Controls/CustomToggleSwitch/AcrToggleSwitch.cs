using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomToggleSwitch;

[ToolboxBitmap(typeof(CheckBox))]
[DefaultEvent("CheckedChanged")]
public class AcrToggleSwitch : CheckBox, IAcrBaseControl
{
    private const int TrackWidthLogical = 40;
    private int TrackWidth => LogicalToDeviceUnits(TrackWidthLogical);
    private const int TrackHeightLogical = 20;
    private int TrackHeight => LogicalToDeviceUnits(TrackHeightLogical);
    private const int ThumbInsetLogical = 2;
    private int ThumbInset => LogicalToDeviceUnits(ThumbInsetLogical);
    private const int TextGapLogical = 8;
    private int TextGap => LogicalToDeviceUnits(TextGapLogical);

    private readonly System.Windows.Forms.Timer _animationTimer;
    private double _thumbPosition = 0; // 0 = off, 1 = on
    private double _thumbTarget = 0;

    private EControlState _controlState = EControlState.Normal;
    private Color _onColor = AcrColors.Primary;

    public AcrToggleSwitch()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Cursor = Cursors.Hand;
        Font = AcrFonts.Get(9F);
        AutoSize = true;

        _animationTimer = new System.Windows.Forms.Timer { Interval = 12 };
        _animationTimer.Tick += (_, _) => StepAnimation();
    }

    [Category("Acr Custom")]
    [Description("Color used for the track and thumb when the switch is on.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color OnColor
    {
        get => _onColor;
        set
        {
            _onColor = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public EControlState ControlState
    {
        get => _controlState;
        set
        {
            if (_controlState == value) return;
            _controlState = value;
            ApplyState();
        }
    }

    public void ApplyState()
    {
        switch (_controlState)
        {
            case EControlState.Normal:
                Enabled = true; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                Enabled = false; break;
            case EControlState.Edit:
                Enabled = true; break;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        _thumbPosition = Checked ? 1 : 0;
        _thumbTarget = _thumbPosition;
    }

    protected override void OnCheckedChanged(EventArgs e)
    {
        base.OnCheckedChanged(e);
        _thumbTarget = Checked ? 1 : 0;
        if (!DesignMode) _animationTimer.Start();
        else
        {
            _thumbPosition = _thumbTarget;
            Invalidate();
        }
    }

    private void StepAnimation()
    {
        const double step = 0.25;

        if (_thumbPosition < _thumbTarget)
            _thumbPosition = Math.Min(_thumbTarget, _thumbPosition + step);
        else if (_thumbPosition > _thumbTarget)
            _thumbPosition = Math.Max(_thumbTarget, _thumbPosition - step);

        if (_thumbPosition == _thumbTarget) _animationTimer.Stop();

        Invalidate();
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
        Invalidate();
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var textSize = TextRenderer.MeasureText(string.IsNullOrEmpty(Text) ? " " : Text, Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine);
        int height = Math.Max(TrackHeight, textSize.Height) + 8;
        int width = TrackWidth + (string.IsNullOrEmpty(Text) ? 0 : TextGap + textSize.Width) + 4;
        return new Size(width, height);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? BackColor;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var trackRect = new Rectangle(0, (Height - TrackHeight) / 2, TrackWidth, TrackHeight);

        var trackOffColor = !Enabled ? AcrColors.DisabledBack : AcrColors.Track;
        var trackOnColor = !Enabled ? AcrColors.DisabledFore : _onColor;
        var trackColor = Blend(trackOffColor, trackOnColor, _thumbPosition);

        using (var path = AcrGraphics.CreateRoundedRectPath(trackRect, TrackHeight / 2))
        {
            using var trackBrush = new SolidBrush(trackColor);
            e.Graphics.FillPath(trackBrush, path);
        }

        int thumbDiameter = TrackHeight - ThumbInset * 2;
        int minX = trackRect.Left + ThumbInset;
        int maxX = trackRect.Right - ThumbInset - thumbDiameter;
        int thumbX = minX + (int)Math.Round((maxX - minX) * _thumbPosition);
        var thumbRect = new Rectangle(thumbX, trackRect.Top + ThumbInset, thumbDiameter, thumbDiameter);

        using (var thumbBrush = new SolidBrush(Color.White))
            e.Graphics.FillEllipse(thumbBrush, thumbRect);

        if (!string.IsNullOrEmpty(Text))
        {
            var textColor = Enabled ? AcrColors.Text : AcrColors.DisabledFore;
            var textRect = new Rectangle(trackRect.Right + TextGap, 0, Math.Max(0, Width - trackRect.Right - TextGap), Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }

    private static Color Blend(Color from, Color to, double fraction)
    {
        fraction = Math.Clamp(fraction, 0, 1);
        int r = (int)(from.R + (to.R - from.R) * fraction);
        int g = (int)(from.G + (to.G - from.G) * fraction);
        int b = (int)(from.B + (to.B - from.B) * fraction);
        return Color.FromArgb(r, g, b);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _animationTimer.Dispose();
        base.Dispose(disposing);
    }
}
