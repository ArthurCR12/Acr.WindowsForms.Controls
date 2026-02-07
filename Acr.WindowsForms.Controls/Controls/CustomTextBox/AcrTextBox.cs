using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Versioning;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

[SupportedOSPlatform("windows")]
public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private Color _onEnterBackColor = Color.AliceBlue;
    private Color _onLeaveBackColor = Color.White;
    private Label? _titleLabel;
    private TextboxtInputType _inputType = TextboxtInputType.All;

    private bool _tabOnEnter = true;
    private bool _validateAsDate = false;
    private bool _selectAllTextOnEnter = false;
    private bool _labelTitle = false;

    private string _warningMessageDate = "Invalid date format.";
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
    [Description("If true, pressing Enter will move focus to the next control.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool TabOnEnter
    {
        get => _tabOnEnter;
        set => _tabOnEnter = value;
    }

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


    [Category("Acr Custom")]
    [Description("If true, all text will be selected when the control receives focus.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool SelectAllTextOnEnter
    {
        get => _selectAllTextOnEnter;
        set => _selectAllTextOnEnter = value;
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
    [Description("Define the allowed input type.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public TextboxtInputType InputType
    {
        get => _inputType;
        set => _inputType = value;
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        ForeColor = Color.FromArgb(50, 50, 50);
        UpdateTitleLabel();
    }

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
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        BackColor = _onLeaveBackColor;
        AcrValidationHelper.ValidateRequired(this, _blockLeave);
        if (_validateAsDate) ClearError();
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
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        if (_validateAsDate)
        {
            LabelHelper.RemoveLabel(this, MessageType.Error);
            string[] parts = Text.Split('/');

            if (parts.Length >= 1 && int.TryParse(parts[0], out int day) && day > 31)
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
            else if (parts.Length >= 2 && int.TryParse(parts[1], out int month) && month > 12)
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
        }
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
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

   
}

