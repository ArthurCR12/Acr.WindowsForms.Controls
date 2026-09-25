using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;

namespace Acr.WindowsForms.Controls.Helpers;

public static class AcrStateHelper
{
    /// <summary>
    /// Recursively applies a single EControlState to every Acr* control (IAcrBaseControl)
    /// inside a container (Form, Panel, GroupBox, etc). Use this to lock/unlock an entire
    /// screen from one variable, e.g. AcrStateHelper.ApplyState(this, isEditing
    ///     ? EControlState.Edit
    ///     : EControlState.ReadOnly);
    /// </summary>
    public static void ApplyState(Control container, EControlState state)
    {
        foreach (Control child in container.Controls)
        {
            if (child is IAcrBaseControl acrControl)
                acrControl.ControlState = state;

            if (child.HasChildren)
                ApplyState(child, state);
        }
    }
}
