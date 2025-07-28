using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace Acr.WindowsForms.Controls.Controls;

[SupportedOSPlatform("windows")]
public class Acr_TextBox : TextBox
{
    private Color _onEnterBackColor = Color.AliceBlue;
    private Color _onLeaveBackColor = Color.White;
    
    private bool _tabOnEnter = true;
    private bool _validateAsDate = false;
    private bool _selectAllTextOnEnter = false;

    private string _warningMessageDate = "Invalid date format.";


    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        ForeColor = Color.FromArgb(50, 50, 50);
    }


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



    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        BackColor = _onEnterBackColor;

        if (_selectAllTextOnEnter)
        {
            SelectionStart = 0;
            SelectionLength = Text.Length;
        }

    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        BackColor = _onLeaveBackColor;
        if (_validateAsDate) LabelHelper.RemoveLabel(this);        
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
        if (_validateAsDate)
        {
            if (Text == string.Empty) return;

            try
            {
                Text = Convert.ToDateTime(Text).ToShortDateString();                
            }
            catch (Exception)
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
            LabelHelper.RemoveLabel(this);
            string[] parts = Text.Split('/');

            if (parts.Length >= 1 && int.TryParse(parts[0], out int day) && day > 31)
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
            else if (parts.Length >= 2 && int.TryParse(parts[1], out int month) && month > 12)
                LabelHelper.CreateLabel(this, _warningMessageDate, MessageType.Error);
        }
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        if (_validateAsDate && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
            e.Handled = true;

        if (_validateAsDate)
        {
            LabelHelper.RemoveLabel(this);
            int l = Text.Length;

            if (e.KeyChar == (char)Keys.Back) return;

            string[] parts = Text.Split('/');

            if (Text.Length == 10)
            {
                if (e.KeyChar == (char)Keys.Back) return;
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
                if (Text == "0")
                {
                    e.Handled = true;
                    return;
                }
                else if (l == 1)
                    Text = "0" + Text;
                else if (l == 4)
                    Text = $"{parts[0]}/0{parts[1]}";                
                else
                {
                    e.Handled = true;
                    return;
                }

                SelectionStart = Text.Length;
            }            
        }

    }

}
