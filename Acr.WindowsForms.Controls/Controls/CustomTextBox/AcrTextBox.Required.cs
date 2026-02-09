using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private bool _requiredField = false;
    private bool _blockLeave = false;
    private string _warningMessageRequiredField = "This field is required!";
    public bool IsControlEmpty => string.IsNullOrWhiteSpace(Text);

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
    public void ShowRequiredFieldError()
    {
        LabelHelper.CreateLabel(this, WarningMessageRequiredField, MessageType.Error);
    }

    public void ClearError()
    {
        LabelHelper.RemoveLabel(this, MessageType.Error);
    }
}
