using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

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
        UnformatNumeric();
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        BackColor = _onLeaveBackColor;
        AcrValidationHelper.ValidateRequired(this, _blockLeave);
        if (_validateAsDate) ClearError();
        FormatNumeric();
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
                ReadOnly = false; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                ReadOnly = true; break;
            case EControlState.Edit:
                ReadOnly = false;
                Enabled = true;
                break;
        }
    }

    
}
