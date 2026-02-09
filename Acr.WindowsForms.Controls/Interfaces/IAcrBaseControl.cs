using Acr.WindowsForms.Controls.Enums;

namespace Acr.WindowsForms.Controls.Interfaces;

public interface IAcrBaseControl
{
    EControlState ControlState { get; set; }

    void ApplyState();
}
