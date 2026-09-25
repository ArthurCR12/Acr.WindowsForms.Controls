using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private bool _validateAsDate = false;
    private string _warningMessageDate = "Invalid date format.";

    [Category("Acr Custom")]
    [Description("If true, the text will be validated as a date.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ValidateAsDate
    {
        get => _validateAsDate;
        set => _validateAsDate = value;
    }

    [Category("Acr Custom")]
    [Description("Custom warning message for invalid date format.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string WarningMessageDate
    {
        get => _warningMessageDate;
        set => _warningMessageDate = value;
    }

    partial void ValidatePattern(CancelEventArgs e);

    protected override void OnValidating(CancelEventArgs e)
    {
        base.OnValidating(e);

        // Validação de campo obrigatório
        AcrValidationHelper.ValidateRequired(this, _blockLeave, e);

        // Validação de data (se ativada)
        if (_validateAsDate && !e.Cancel)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            try
            {
                Text = Convert.ToDateTime(Text).ToShortDateString();
            }
            catch
            {
                e.Cancel = true;
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
            }
        }

        // Validação de padrão (Email, CPF, CNPJ, Telefone, Custom)
        if (!e.Cancel)
            ValidatePattern(e);
    }
    private void DateValidator(bool valid, KeyPressEventArgs e)
    {
        if (!valid) return;

        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
        {
            e.Handled = true;
            return;
        }

        if (e.KeyChar == (char)Keys.Back) return;

        if (Text.Length == 10)
        {
            e.Handled = true;
            return;
        }

        if (Text.Length == 2 || Text.Length == 5)
        {
            Text += "/";
            SelectionStart = Text.Length;
        }

        if (e.KeyChar == '/')
        {
            string[] parts = Text.Split('/');
            int l = Text.Length;

            if (Text == "0") e.Handled = true;
            else if (l == 1) Text = "0" + Text;
            else if (l == 4) Text = $"{parts[0]}/0{parts[1]}";
            else
            {
                e.Handled = true;
                return;
            }

            SelectionStart = Text.Length;
        }
    }
}
