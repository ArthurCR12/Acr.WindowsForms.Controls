using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomAvatar;

[ToolboxBitmap(typeof(PictureBox))]
public class AcrAvatar : Control
{
    private string _initials = "AB";
    private Image? _image;
    private Color _backColorAvatar = AcrColors.Primary;
    private AcrAvatarStatus _status = AcrAvatarStatus.None;

    public AcrAvatar()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Size = new Size(40, 40);
        Font = AcrFonts.Get(12F, FontStyle.Bold);
    }

    [Category("Acr Custom")]
    [Description("Initials shown when no Image is set (up to 2 characters are used).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Initials
    {
        get => _initials;
        set
        {
            _initials = (value ?? string.Empty).Trim();
            if (_initials.Length > 2) _initials = _initials[..2];
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Optional photo. When set, it is drawn instead of the initials.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Image? Image
    {
        get => _image;
        set
        {
            _image = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Background color used behind the initials.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AvatarColor
    {
        get => _backColorAvatar;
        set
        {
            _backColorAvatar = value;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Small status dot drawn at the bottom-right corner.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public AcrAvatarStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            Invalidate();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int diameter = Math.Min(Width, Height);
        var circleRect = new Rectangle((Width - diameter) / 2, (Height - diameter) / 2, diameter, diameter);

        using (var clipPath = new GraphicsPath())
        {
            clipPath.AddEllipse(circleRect);

            if (_image != null)
            {
                using var region = new Region(clipPath);
                var oldClip = e.Graphics.Clip;
                e.Graphics.SetClip(region, CombineMode.Replace);
                e.Graphics.DrawImage(_image, circleRect);
                e.Graphics.Clip = oldClip;
            }
            else
            {
                using var backBrush = new SolidBrush(_backColorAvatar);
                e.Graphics.FillEllipse(backBrush, circleRect);

                var text = string.IsNullOrEmpty(_initials) ? "?" : _initials.ToUpperInvariant();
                TextRenderer.DrawText(e.Graphics, text, Font, circleRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        if (_status != AcrAvatarStatus.None)
        {
            int dotSize = Math.Max(8, diameter / 4);
            var dotRect = new Rectangle(circleRect.Right - dotSize, circleRect.Bottom - dotSize, dotSize, dotSize);

            var dotColor = _status switch
            {
                AcrAvatarStatus.Online => AcrColors.Success,
                AcrAvatarStatus.Away => AcrColors.Warning,
                AcrAvatarStatus.Busy => AcrColors.Error,
                _ => AcrColors.Neutral,
            };

            using var ringBrush = new SolidBrush(Parent?.BackColor ?? AcrColors.Surface);
            var ringRect = Rectangle.Inflate(dotRect, 2, 2);
            e.Graphics.FillEllipse(ringBrush, ringRect);

            using var dotBrush = new SolidBrush(dotColor);
            e.Graphics.FillEllipse(dotBrush, dotRect);
        }
    }
}

public enum AcrAvatarStatus
{
    None,
    Online,
    Away,
    Busy,
    Offline
}
