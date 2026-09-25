using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private const int IconSize = 16;
    private const int IconGap = 2;

    private bool _showClearButton = false;
    private bool _isPasswordField = false;

    private Label? _clearButtonIcon;
    private Label? _passwordToggleIcon;

    [Category("Acr Custom")]
    [Description("If true, shows a small button inside the control to quickly clear its text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowClearButton
    {
        get => _showClearButton;
        set
        {
            _showClearButton = value;
            UpdateIcons();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, masks the text as a password and shows a toggle button to reveal/hide it.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool IsPasswordField
    {
        get => _isPasswordField;
        set
        {
            _isPasswordField = value;
            UseSystemPasswordChar = value;
            UpdateIcons();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateIcons();
    }

    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        RepositionIcons();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        RepositionIcons();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        RepositionIcons();
    }

    partial void UpdateClearButtonVisibility()
    {
        if (_clearButtonIcon != null)
            _clearButtonIcon.Visible = _showClearButton && Enabled && !ReadOnly && Text.Length > 0;
    }

    private void UpdateIcons()
    {
        if (!IsHandleCreated || Parent == null) return;

        if (_showClearButton && _clearButtonIcon == null)
        {
            _clearButtonIcon = CreateIconLabel("✕");
            _clearButtonIcon.Click += (_, _) =>
            {
                Clear();
                Focus();
            };
        }
        else if (!_showClearButton && _clearButtonIcon != null)
        {
            Parent.Controls.Remove(_clearButtonIcon);
            _clearButtonIcon.Dispose();
            _clearButtonIcon = null;
        }

        if (_isPasswordField && _passwordToggleIcon == null)
        {
            _passwordToggleIcon = CreateIconLabel(PasswordToggleGlyph);
            _passwordToggleIcon.Click += (_, _) =>
            {
                UseSystemPasswordChar = !UseSystemPasswordChar;
                _passwordToggleIcon!.Text = PasswordToggleGlyph;
            };
        }
        else if (!_isPasswordField && _passwordToggleIcon != null)
        {
            Parent.Controls.Remove(_passwordToggleIcon);
            _passwordToggleIcon.Dispose();
            _passwordToggleIcon = null;
        }

        UpdateClearButtonVisibility();
        RepositionIcons();
        ApplyRightMargin();
    }

    private string PasswordToggleGlyph => UseSystemPasswordChar ? "●" : "○";

    private Label CreateIconLabel(string glyph)
    {
        var icon = new Label
        {
            Name = $"icon_{Name}_{glyph.GetHashCode()}",
            Text = glyph,
            AutoSize = false,
            Size = new Size(IconSize, IconSize),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            BackColor = Color.Transparent,
            ForeColor = AcrColors.IconGlyph,
            Font = new Font("Segoe UI Symbol", 8f)
        };
        icon.MouseEnter += (_, _) => icon.ForeColor = AcrColors.IconGlyphHover;
        icon.MouseLeave += (_, _) => icon.ForeColor = AcrColors.IconGlyph;
        icon.CreateControl();
        Parent!.Controls.Add(icon);
        icon.BringToFront();
        return icon;
    }

    private void RepositionIcons()
    {
        if (Parent == null) return;

        int rightOffset = Right - IconSize - IconGap;
        int centerY = Top + (Height - IconSize) / 2;

        if (_passwordToggleIcon != null)
        {
            _passwordToggleIcon.Location = new Point(rightOffset, centerY);
            rightOffset -= IconSize + IconGap;
        }

        if (_clearButtonIcon != null)
        {
            _clearButtonIcon.Location = new Point(rightOffset, centerY);
            rightOffset -= IconSize + IconGap;
        }
    }

    private void ApplyRightMargin()
    {
        if (!IsHandleCreated) return;

        int iconCount = (_clearButtonIcon != null ? 1 : 0) + (_passwordToggleIcon != null ? 1 : 0);
        int margin = iconCount * (IconSize + IconGap);
        NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, (IntPtr)NativeMethods.EC_RIGHTMARGIN, (IntPtr)(margin << 16));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _clearButtonIcon?.Dispose();
            _passwordToggleIcon?.Dispose();
        }
        base.Dispose(disposing);
    }
}

file static class NativeMethods
{
    public const int EM_SETMARGINS = 0xD3;
    public const int EC_RIGHTMARGIN = 0x2;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}
