using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomStepper;

public class AcrStepper : Control
{
    private const int CircleSize = 28;
    private const int LabelGap = 6;

    public List<string> Steps { get; } = new();

    private int _currentStep = 0;

    public event EventHandler<int>? StepChanged;

    public AcrStepper()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = AcrFonts.Get(8.5F);
        Size = new Size(500, 60);
    }

    public void SetSteps(params string[] steps)
    {
        Steps.Clear();
        Steps.AddRange(steps);
        _currentStep = Math.Clamp(_currentStep, 0, Math.Max(0, Steps.Count - 1));
        Invalidate();
    }

    [Category("Acr Custom")]
    [Description("Zero-based index of the current active step.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CurrentStep
    {
        get => _currentStep;
        set
        {
            int clamped = Math.Clamp(value, 0, Math.Max(0, Steps.Count - 1));
            if (clamped == _currentStep) return;
            _currentStep = clamped;
            Invalidate();
            StepChanged?.Invoke(this, _currentStep);
        }
    }

    public void Next()
    {
        if (_currentStep < Steps.Count - 1) CurrentStep = _currentStep + 1;
    }

    public void Previous()
    {
        if (_currentStep > 0) CurrentStep = _currentStep - 1;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        if (Steps.Count == 0) return;

        int segmentWidth = Width / Steps.Count;
        int cy = CircleSize / 2;

        for (int i = 0; i < Steps.Count; i++)
        {
            int cx = segmentWidth * i + segmentWidth / 2;
            bool isDone = i < _currentStep;
            bool isCurrent = i == _currentStep;

            if (i < Steps.Count - 1)
            {
                int nextCx = segmentWidth * (i + 1) + segmentWidth / 2;
                var lineColor = isDone ? AcrColors.Primary : AcrColors.Track;
                using var linePen = new Pen(lineColor, 2);
                e.Graphics.DrawLine(linePen, cx + CircleSize / 2, cy, nextCx - CircleSize / 2, cy);
            }

            var circleRect = new Rectangle(cx - CircleSize / 2, cy - CircleSize / 2, CircleSize, CircleSize);

            var fillColor = isDone || isCurrent ? AcrColors.Primary : AcrColors.Surface;
            var borderColor = isDone || isCurrent ? AcrColors.Primary : AcrColors.Border;

            using (var fillBrush = new SolidBrush(fillColor))
                e.Graphics.FillEllipse(fillBrush, circleRect);

            using (var borderPen = new Pen(borderColor, 2))
                e.Graphics.DrawEllipse(borderPen, circleRect);

            if (isDone)
            {
                using var checkPen = new Pen(Color.White, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
                var points = new[]
                {
                    new PointF(circleRect.Left + 7, circleRect.Top + 14),
                    new PointF(circleRect.Left + 12, circleRect.Top + 19),
                    new PointF(circleRect.Right - 7, circleRect.Top + 9),
                };
                e.Graphics.DrawLines(checkPen, points);
            }
            else
            {
                var numberText = (i + 1).ToString();
                var numberColor = isCurrent ? Color.White : AcrColors.Neutral;
                TextRenderer.DrawText(e.Graphics, numberText, Font, circleRect, numberColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            var labelRect = new Rectangle(cx - segmentWidth / 2, CircleSize + LabelGap, segmentWidth, Height - CircleSize - LabelGap);
            var labelColor = isCurrent ? AcrColors.Text : AcrColors.Neutral;
            var labelFont = isCurrent ? AcrFonts.WithStyle(Font, FontStyle.Bold) : Font;
            TextRenderer.DrawText(e.Graphics, Steps[i], labelFont, labelRect, labelColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.WordBreak);
        }
    }
}
