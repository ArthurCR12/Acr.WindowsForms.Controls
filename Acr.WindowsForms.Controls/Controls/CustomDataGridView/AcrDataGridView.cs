using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomDataGridView;

public class AcrDataGridView : DataGridView, IAcrBaseControl
{
    private EControlState _controlState = EControlState.Normal;
    [Category("Acr Custom")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public EControlState ControlState
    {
        get => _controlState;
        set
        {
            if (_controlState == value) return;
            _controlState = value;
            ApplyState();
        }
    }

    public void ApplyState()
    {
        switch (_controlState)
        {
            case EControlState.Normal:
                ReadOnly = false;
                Enabled = true; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                ReadOnly = true; break;
            case EControlState.Edit:
                ReadOnly = false;
                Enabled = true; break;
        }
    }
}
