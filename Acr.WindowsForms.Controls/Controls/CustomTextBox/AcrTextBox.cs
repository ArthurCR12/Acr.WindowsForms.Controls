using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Versioning;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

[SupportedOSPlatform("windows")]
public partial class AcrTextBox : TextBox, IAcrValidatableControl, IAcrBaseControl 
{
    private bool _tabOnEnter = true;    
    private bool _selectAllTextOnEnter = false;

    [Category("Acr Custom")]
    [Description("If true, pressing Enter will move focus to the next control.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool TabOnEnter
    {
        get => _tabOnEnter;
        set => _tabOnEnter = value;
    }

    [Category("Acr Custom")]
    [Description("If true, all text will be selected when the control receives focus.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool SelectAllTextOnEnter
    {
        get => _selectAllTextOnEnter;
        set => _selectAllTextOnEnter = value;
    }    

    public AcrTextBox()
    {
        // Definidos no construtor (e não em OnCreateControl) para não sobrescrever
        // a fonte/cor configuradas pelo usuário no Designer.
        BorderStyle = BorderStyle.FixedSingle;
        // A altura é controlada pelo estilo (ver AcrTextBox.Style.cs), não pelo AutoSize nativo.
        AutoSize = false;
        ForeColor = AcrColors.Text;
        Font = new Font("Segoe UI", 9F);
        ApplyModernHeight();
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        UpdateTitleLabel();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (_tabOnEnter && e.KeyCode == Keys.Enter)
        {
            SendKeys.Send("{TAB}");
            e.SuppressKeyPress = true;
        }
    }

    partial void ValidateMask(KeyPressEventArgs e);
    partial void AutoInsertMaskLiterals();
    partial void UpdateClearButtonVisibility();

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        if (_validateAsDate)
        {
            LabelHelper.RemoveLabel(this, MessageType.Error);
            HasError = false;
            string[] parts = Text.Split('/');

            bool invalidDay = parts.Length >= 1 && int.TryParse(parts[0], out int day) && day > 31;
            bool invalidMonth = parts.Length >= 2 && int.TryParse(parts[1], out int month) && month > 12;
            if (invalidDay || invalidMonth)
            {
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
                HasError = true;
            }
        }

        AutoInsertMaskLiterals();
        UpdateClearButtonVisibility();
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (!string.IsNullOrEmpty(_inputMask))
        {
            ValidateMask(e);
            if (e.Handled) return;
            base.OnKeyPress(e);
            return;
        }

        if (!IsKeyValidForInputType(e.KeyChar))
        {
            e.Handled = true;
            return;
        }

        DateValidator(_validateAsDate, e);
        base.OnKeyPress(e);
    }

    private bool IsKeyValidForInputType(char c)
    {
        if (_inputType == TextboxtInputType.All) return true;
        if (_validateAsDate) return true;

        bool isControl = char.IsControl(c);
        bool isDigit = char.IsDigit(c);
        bool isLetter = char.IsLetter(c);
        bool isWhiteSpace = char.IsWhiteSpace(c);

        char decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        switch (_inputType)
        {
            case TextboxtInputType.Integer:
                return isControl || isDigit;

            case TextboxtInputType.Decimal:
                if (isControl || isDigit) return true;
                if (c == decimalSeparator && !Text.Contains(decimalSeparator)) return true;

                return false;

            case TextboxtInputType.Letter:
                return isControl || isLetter || isWhiteSpace;

            case TextboxtInputType.Alphanumeric:
                return isControl || isLetter || isDigit || isWhiteSpace;

            default:
                return true;
        }


    }

    

   

   
}

