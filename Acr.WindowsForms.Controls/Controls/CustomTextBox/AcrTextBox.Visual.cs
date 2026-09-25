using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private Color _onEnterBackColor = Color.AliceBlue;
    private Color _onLeaveBackColor = Color.White;

    private Label? _titleLabel;

    private TextboxtInputType _inputType = TextboxtInputType.All;
    private EControlState _controlState = EControlState.Normal;

    private bool _labelTitle = false;
    private string _labelTitleText = string.Empty;

    private bool _hovering = false;

    private const int WM_NCPAINT = 0x0085;
    private const int WM_NCCALCSIZE = 0x0083;

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

    private const uint RDW_INVALIDATE = 0x0001;
    private const uint RDW_FRAME = 0x0400;
    private const uint RDW_UPDATENOW = 0x0100;

    [Category("Acr Custom")]
    [Description("Background color when the control is focused.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color OnEnterBackColor
    {
        get => _onEnterBackColor;
        set => _onEnterBackColor = value;
    }

    [Category("Acr Custom")]
    [Description("Background color when the control loses focus.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color OnLeaveBackColor
    {
        get => _onLeaveBackColor;
        set => _onLeaveBackColor = value;
    }

    [Category("Acr Custom")]
    [Description("Define the allowed input type.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public TextboxtInputType InputType
    {
        get => _inputType;
        set => _inputType = value;
    }

    [Category("Acr Custom")]
    [Description("Text for the title label.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string LabelTitleText
    {
        get => _labelTitleText;
        set
        {
            _labelTitleText = value;
            UpdateTitleLabel();
        }
    }

    [Category("Acr Custom")]
    [Description("If true, a title label will be created on top of control.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool LabelTitle
    {
        get => _labelTitle;
        set
        {
            _labelTitle = value;
            UpdateTitleLabel();
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

    partial void UnformatNumeric();
    partial void FormatNumeric();

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        BackColor = _onEnterBackColor;

        if (_selectAllTextOnEnter)
        {
            SelectionStart = 0;
            SelectionLength = Text.Length;
        }

        ClearError();
        RedrawBorder();
        UnformatNumeric();
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        BackColor = _onLeaveBackColor;
        AcrValidationHelper.ValidateRequired(this, _blockLeave);
        if (_validateAsDate) ClearError();
        RedrawBorder();
        FormatNumeric();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _hovering = true;
        RedrawBorder();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hovering = false;
        RedrawBorder();
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        RedrawBorder();
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m.Msg == WM_NCPAINT || m.Msg == WM_NCCALCSIZE)
        {
            PaintCustomBorder();
        }
    }

    private void RedrawBorder()
    {
        if (!IsHandleCreated) return;
        RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME | RDW_UPDATENOW);
    }

    private void PaintCustomBorder()
    {
        if (BorderStyle != BorderStyle.FixedSingle) return;

        var color = !Enabled
            ? AcrColors.BorderDisabled
            : Focused
                ? AcrColors.BorderFocused
                : _hovering
                    ? AcrColors.BorderHover
                    : AcrColors.Border;

        var hdc = GetWindowDC(Handle);
        if (hdc == IntPtr.Zero) return;

        try
        {
            using var g = Graphics.FromHdc(hdc);
            using var pen = new Pen(color, 1);
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }
        finally
        {
            ReleaseDC(Handle, hdc);
        }
    }

    private void UpdateTitleLabel()
    {
        if (!IsHandleCreated || !this.Visible) return;

        if (_labelTitle)
        {
            if (_titleLabel == null) _titleLabel = LabelHelper.CreateLabel(this, _labelTitleText, MessageType.Title, location: "top");
            else
            {
                _titleLabel.Text = _labelTitleText;
                _titleLabel.Visible = true;
            }
        }
        else
        {
            if (_titleLabel != null)
            {
                _titleLabel.Visible = false;
            }
        }

    }

    public void ApplyState()
    {
        switch (_controlState)
        {
            case EControlState.Normal:
                Enabled = true;
                ReadOnly = false;
                break;
            case EControlState.Disabled:
                Enabled = false;
                ReadOnly = false;
                break;
            case EControlState.ReadOnly:
                Enabled = true;
                ReadOnly = true;
                break;
            case EControlState.Edit:
                Enabled = true;
                ReadOnly = false;
                break;
        }
    }


}
