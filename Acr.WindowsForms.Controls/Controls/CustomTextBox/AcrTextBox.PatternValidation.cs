using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private TextValidationType _validationType = TextValidationType.None;
    private string _customValidationPattern = string.Empty;
    private string _warningMessagePattern = "Invalid value.";

    [Category("Acr Custom")]
    [Description("Built-in pattern validation applied when the control loses focus.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public TextValidationType ValidationType
    {
        get => _validationType;
        set => _validationType = value;
    }

    [Category("Acr Custom")]
    [Description("Regex pattern used when ValidationType is set to Custom.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CustomValidationPattern
    {
        get => _customValidationPattern;
        set => _customValidationPattern = value;
    }

    [Category("Acr Custom")]
    [Description("Message shown when the pattern validation fails.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string WarningMessagePattern
    {
        get => _warningMessagePattern;
        set => _warningMessagePattern = value;
    }

    partial void ValidatePattern(CancelEventArgs e)
    {
        if (_validationType == TextValidationType.None) return;
        if (string.IsNullOrWhiteSpace(Text)) return;

        bool isValid = AcrValidationHelper.IsValidByType(Text, _validationType, _customValidationPattern);

        if (isValid)
        {
            LabelHelper.RemoveLabel(this, MessageType.Error);
        }
        else
        {
            if (_blockLeave) e.Cancel = true;
            LabelHelper.CreateLabel(this, _warningMessagePattern, MessageType.Error);
        }
    }
}
