using System.ComponentModel;
using System.Runtime.Versioning;

namespace Acr.WindowsForms.Controls;

[SupportedOSPlatform("windows")]
public class Acr_TextBox : TextBox
{
    private Color _onEnterBackColor = Color.AliceBlue;
    private Color _onLeaveBackColor = Color.White;
    private bool _tabOnEnter = true;


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




    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        BackColor = _onEnterBackColor;
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        BackColor = _onLeaveBackColor;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (_tabOnEnter && e.KeyCode == Keys.Enter)
        {
            SendKeys.Send("{TAB}");
            e.SuppressKeyPress = true;
        }
    }}
