using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Helpers;

public static class AcrValidationHelper
{
    public static void ValidateRequired(IAcrValidatableControl control, bool blockLeave, CancelEventArgs? e = null)
    {
        if (!control.RequiredField)
            return;

        if (control.IsControlEmpty)
        {
            control.ShowRequiredFieldError();
            if (blockLeave && e != null)
                e.Cancel = true;
        }
        else control.ClearError();
    }
}
