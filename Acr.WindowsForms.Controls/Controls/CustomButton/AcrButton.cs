using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomButton;

public class AcrButton : Button, IAcrBaseControl
{
    private EControlState _controlState = EControlState.Normal;
    private int _cornerRadius = 6;
    private Color _accentColor = AcrColors.Primary;

    public AcrButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);

        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = AcrColors.PrimaryHover;
        FlatAppearance.MouseDownBackColor = AcrColors.PrimaryPressed;

        BackColor = AcrColors.Primary;
        ForeColor = Color.White;
        Cursor = Cursors.Hand;
        Font = new Font("Segoe UI", 9F);
        Padding = new Padding(6, 2, 6, 2);
    }

    [Category("Acr Custom")]
    [Description("Radius, in pixels, used to round the button corners.")]
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
    [Description("Base color used for the button background and hover/pressed shades.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => _accentColor;
        set
        {
            _accentColor = value;
            BackColor = value;
            FlatAppearance.MouseOverBackColor = ControlPaint.Light(value, 0.15f);
            FlatAppearance.MouseDownBackColor = ControlPaint.Dark(value, 0.1f);
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

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;

        using var path = CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), _cornerRadius);
        Region = new Region(path);
    }

    private static GraphicsPath CreateRoundedRectPath(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();

        if (radius <= 0)
        {
            path.AddRectangle(bounds);
            return path;
        }

        int diameter = radius * 2;
        var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

        path.AddArc(arc, 180, 90);

        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);

        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);

        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }
}
