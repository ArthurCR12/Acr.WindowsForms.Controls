using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomComboBox;

public class AcrComboBox : ComboBox, IAcrValidatableControl, IAcrBaseControl
{
    private bool _requiredField = false;
    private bool _blockLeave = false;
    private string _warningMessageRequiredField = "This field is required!";
    

    public bool IsControlEmpty => SelectedIndex == -1;


    [Category("Acr Custom")]
    [Description("If true, the field is marked as required.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool RequiredField
    {
        get => _requiredField;
        set => _requiredField = value;
    }

    [Category("Acr Custom")]
    [Description("Message for a Required Field")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string WarningMessageRequiredField
    {
        get => _warningMessageRequiredField;
        set => _warningMessageRequiredField = value;
    }

    [Category("Acr Custom")]
    [Description("If true, the control will block leaving when validation fails.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool BlockLeave
    {
        get => _blockLeave;
        set => _blockLeave = value;
    }

    private EControlState _controlState = EControlState.Normal;
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

    public void ClearError()
    {
        LabelHelper.RemoveLabel(this, MessageType.Error);
    }

    public void ShowRequiredFieldError()
    {
        LabelHelper.CreateLabel(this, WarningMessageRequiredField, MessageType.Error);
    }
}
