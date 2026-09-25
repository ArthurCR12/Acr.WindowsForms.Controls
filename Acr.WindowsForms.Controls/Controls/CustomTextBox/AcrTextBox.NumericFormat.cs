using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Globalization;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

public partial class AcrTextBox : TextBox, IAcrValidatableControl
{
    private int _decimalPlaces = 2;
    private bool _useThousandsSeparator = true;

    [Category("Acr Custom")]
    [Description("Number of decimal places applied when InputType is Decimal (formatted when the control loses focus).")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int DecimalPlaces
    {
        get => _decimalPlaces;
        set => _decimalPlaces = value < 0 ? 0 : value;
    }

    [Category("Acr Custom")]
    [Description("If true and InputType is Decimal, formats the value with thousands separators when the control loses focus.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool UseThousandsSeparator
    {
        get => _useThousandsSeparator;
        set => _useThousandsSeparator = value;
    }

    partial void UnformatNumeric()
    {
        if (_inputType != TextboxtInputType.Decimal) return;
        if (string.IsNullOrWhiteSpace(Text)) return;

        if (decimal.TryParse(Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
            Text = value.ToString("F" + _decimalPlaces, CultureInfo.CurrentCulture);
    }

    partial void FormatNumeric()
    {
        if (_inputType != TextboxtInputType.Decimal) return;
        if (string.IsNullOrWhiteSpace(Text)) return;

        if (!decimal.TryParse(Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
            return;

        string format = (_useThousandsSeparator ? "N" : "F") + _decimalPlaces;
        Text = value.ToString(format, CultureInfo.CurrentCulture);
    }
}
