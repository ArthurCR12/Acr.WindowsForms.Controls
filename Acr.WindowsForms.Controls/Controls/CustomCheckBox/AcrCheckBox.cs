using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomCheckBox;

[ToolboxBitmap(typeof(CheckBox))]
[DefaultEvent("CheckedChanged")]
public class AcrCheckBox : CheckBox, IAcrBaseControl
{
    private const int BoxSizeLogical = 16;
    private int BoxSize => LogicalToDeviceUnits(BoxSizeLogical);
    private const int TextGapLogical = 8;
    private int TextGap => LogicalToDeviceUnits(TextGapLogical);

    private EControlState _controlState = EControlState.Normal;
    private Color _accentColor = AcrColors.Primary;
    private bool _hovering = false;

    public AcrCheckBox()
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
    }

    [Category("Acr Custom")]
    [Description("Color used for the box border and fill when checked.")]
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

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _hovering = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hovering = false;
        Invalidate();
    }

    protected override void OnCheckedChanged(EventArgs e)
    {
        base.OnCheckedChanged(e);
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
        int height = Math.Max(BoxSize, textSize.Height) + 8;
        int width = BoxSize + TextGap + textSize.Width + 4;
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

        var boxRect = new Rectangle(0, (Height - BoxSize) / 2, BoxSize, BoxSize);

        var borderColor = !Enabled
            ? AcrColors.BorderDisabled
            : Checked
                ? _accentColor
                : _hovering
                    ? AcrColors.BorderHover
                    : AcrColors.Border;

        var fillColor = !Enabled
            ? AcrColors.DisabledBack
            : Checked
                ? _accentColor
                : AcrColors.Surface;

        using (var path = AcrGraphics.CreateRoundedRectPath(boxRect, 4))
        {
            using var backBrush = new SolidBrush(fillColor);
            e.Graphics.FillPath(backBrush, path);

            using var borderPen = new Pen(borderColor, 1.5f);
            e.Graphics.DrawPath(borderPen, path);
        }

        if (Checked)
        {
            var checkColor = Enabled ? Color.White : AcrColors.DisabledFore;
            using var checkPen = new Pen(checkColor, 2f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            var points = new[]
            {
                new PointF(boxRect.Left + 3.5f, boxRect.Top + 8.5f),
                new PointF(boxRect.Left + 6.5f, boxRect.Top + 11.5f),
                new PointF(boxRect.Right - 3f, boxRect.Top + 4f)
            };
            e.Graphics.DrawLines(checkPen, points);
        }

        var textColor = Enabled ? AcrColors.Text : AcrColors.DisabledFore;
        var textRect = new Rectangle(boxRect.Right + TextGap, 0, Math.Max(0, Width - boxRect.Right - TextGap), Height);
        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
