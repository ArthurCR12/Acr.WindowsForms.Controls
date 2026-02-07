using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Class;

public abstract class AcrBaseControl : Control
{
    private bool _requiredField = false;

    private string _warningMessageRequiredField = "This field is required.";


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

    
    protected virtual bool ValidateRequired()
    {
        if (!RequiredField) return true;

        bool isEmpty = IsControlEmpty();

        if (isEmpty)
        {
            LabelHelper.CreateLabel(this, WarningMessageRequiredField, MessageType.Error);
            return false;
        }

        LabelHelper.RemoveLabel(this, MessageType.Error);
        return true;
    }

    // Cada filho pode implementar comforme o necessário. Como ele é vazio ou não
    protected abstract bool IsControlEmpty();

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        LabelHelper.RemoveLabel(this, MessageType.Error);
    }
    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        ValidateRequired();
    }

}
