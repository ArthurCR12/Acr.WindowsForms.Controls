using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomComboBox;

[ToolboxBitmap(typeof(ComboBox))]
[DefaultEvent("SelectedIndexChanged")]
public class AcrComboBox : ComboBox, IAcrValidatableControl, IAcrBaseControl, IAcrThemeable
{
    private bool _requiredField = false;
    private bool _blockLeave = false;
    private string _warningMessageRequiredField = "This field is required!";

    public AcrComboBox()
    {
        FlatStyle = FlatStyle.Flat;
        BackColor = AcrColors.Surface;
        ForeColor = AcrColors.Text;
        Font = AcrFonts.Get(9F);

        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 22;
        DrawItem += OnDrawItem;
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        BackColor = Enabled ? AcrColors.Surface : AcrColors.DisabledBack;
        ForeColor = Enabled ? AcrColors.Text : AcrColors.DisabledFore;
    }

    private void OnDrawItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();

        if (e.Index < 0 || e.Index >= Items.Count) return;

        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

        using var backBrush = new SolidBrush(selected ? AcrColors.GridSelectionBack : AcrColors.Surface);
        e.Graphics.FillRectangle(backBrush, e.Bounds);

        var text = GetItemText(Items[e.Index]);
        var textRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, text, e.Font ?? Font, textRect, selected ? AcrColors.GridSelectionFore : AcrColors.Text,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        e.DrawFocusRectangle();
    }

    public bool IsControlEmpty => SelectedIndex == -1;


    [Category("Acr Custom")]
    [Description("If true, the field is marked as required.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool RequiredField
    {
        get => _requiredField;
        set => _requiredField = value;
    }

    [Category("Acr Custom")]
    [Description("Message for a Required Field")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string WarningMessageRequiredField
    {
        get => _warningMessageRequiredField;
        set => _warningMessageRequiredField = value;
    }

    [Category("Acr Custom")]
    [Description("If true, the control will block leaving when validation fails.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool BlockLeave
    {
        get => _blockLeave;
        set => _blockLeave = value;
    }

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
                Enabled = true; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                Enabled = false; break;
            case EControlState.Edit:
                Enabled = true; break;
        }
    }

    public void ClearError()
    {
        LabelHelper.RemoveLabel(this, MessageType.Error);
    }

    public void ShowRequiredFieldError()
    {
        LabelHelper.CreateLabel(this, WarningMessageRequiredField, MessageType.Error);
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        BackColor = Enabled ? AcrTheme.Swap(BackColor, o.Surface, n.Surface) : n.DisabledBack;
        ForeColor = Enabled ? AcrTheme.Swap(ForeColor, o.Text, n.Text) : n.DisabledFore;
        Invalidate();
    }
}
