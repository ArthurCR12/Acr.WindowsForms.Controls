using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomRadioButton;

public class AcrRadioButton : RadioButton, IAcrBaseControl
{
    private const int CircleSize = 16;
    private const int TextGap = 8;

    private EControlState _controlState = EControlState.Normal;
    private Color _accentColor = AcrColors.Primary;
    private bool _hovering = false;

    public AcrRadioButton()
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
    [Description("Color used for the circle border and fill when selected.")]
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
        int height = Math.Max(CircleSize, textSize.Height) + 8;
        int width = CircleSize + TextGap + textSize.Width + 4;
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

        var circleRect = new Rectangle(0, (Height - CircleSize) / 2, CircleSize, CircleSize);

        var borderColor = !Enabled
            ? AcrColors.BorderDisabled
            : Checked
                ? _accentColor
                : _hovering
                    ? AcrColors.BorderHover
                    : AcrColors.Border;

        var fillColor = !Enabled ? AcrColors.DisabledBack : AcrColors.Surface;

        using (var backBrush = new SolidBrush(fillColor))
            e.Graphics.FillEllipse(backBrush, circleRect);

        using (var borderPen = new Pen(borderColor, 1.5f))
            e.Graphics.DrawEllipse(borderPen, circleRect);

        if (Checked)
        {
            var dotColor = !Enabled ? AcrColors.DisabledFore : _accentColor;
            int inset = 4;
            var dotRect = Rectangle.Inflate(circleRect, -inset, -inset);
            using var dotBrush = new SolidBrush(dotColor);
            e.Graphics.FillEllipse(dotBrush, dotRect);
        }

        var textColor = Enabled ? AcrColors.Text : AcrColors.DisabledFore;
        var textRect = new Rectangle(circleRect.Right + TextGap, 0, Math.Max(0, Width - circleRect.Right - TextGap), Height);
        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
