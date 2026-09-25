using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private string _inputMask = string.Empty;
    private int _lastMaskTextLength = 0;

    /// <summary>
    /// Input mask pattern: '0' = digit, 'L' = letter, 'A' = letter or digit.
    /// Any other character is a literal, auto-inserted as the user types
    /// (e.g. "000.000.000-00", "00.000.000/0000-00", "(00) 00000-0000").
    /// </summary>
    [Category("Acr Custom")]
    [Description("Input mask: '0' digit, 'L' letter, 'A' letter or digit, any other char is a literal (e.g. \"000.000.000-00\").")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string InputMask
    {
        get => _inputMask;
        set
        {
            _inputMask = value ?? string.Empty;
            MaxLength = _inputMask.Length;
        }
    }

    private static bool IsMaskEditablePosition(char maskChar) => maskChar is '0' or 'L' or 'A';

    private static bool IsCharValidForMaskPosition(char maskChar, char input) => maskChar switch
    {
        '0' => char.IsDigit(input),
        'L' => char.IsLetter(input),
        'A' => char.IsLetterOrDigit(input),
        _ => true
    };

    partial void ValidateMask(KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar)) return;

        if (SelectionStart >= _inputMask.Length)
        {
            e.Handled = true;
            return;
        }

        char maskChar = _inputMask[SelectionStart];
        if (!IsCharValidForMaskPosition(maskChar, e.KeyChar))
            e.Handled = true;
    }

    partial void AutoInsertMaskLiterals()
    {
        // Only auto-fill literals when the text just grew (typing forward).
        // Otherwise a Backspace over a literal would be re-inserted immediately,
        // trapping the user.
        bool grew = Text.Length > _lastMaskTextLength;
        _lastMaskTextLength = Text.Length;

        if (string.IsNullOrEmpty(_inputMask) || !grew) return;
        if (Text.Length == 0 || Text.Length >= _inputMask.Length) return;

        while (Text.Length < _inputMask.Length && !IsMaskEditablePosition(_inputMask[Text.Length]))
        {
            Text += _inputMask[Text.Length];
            _lastMaskTextLength = Text.Length;
        }

        SelectionStart = Text.Length;
    }
}
