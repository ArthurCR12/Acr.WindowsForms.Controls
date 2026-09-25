using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Globalization;

namespace Acr.WindowsForms.Controls.Controls.CustomNumericUpDown;

/// <summary>
/// Campo numérico com botões "−" e "+" nas laterais, no mesmo visual do AcrTextBox (Outlined).
/// Aceita roda do mouse e as setas ↑/↓ do teclado.
/// </summary>
[ToolboxBitmap(typeof(NumericUpDown))]
[DefaultEvent(nameof(AcrNumericUpDown.ValueChanged))]
public class AcrNumericUpDown : Control, IAcrThemeable
{
    private const int RingWidthLogical = 3;
    private const int ButtonWidthLogical = 30;

    private readonly TextBox _textBox;
    private decimal _value;
    private decimal _minimum;
    private decimal _maximum = 100;
    private decimal _increment = 1;
    private int _decimalPlaces;
    private int _borderRadius = 8;
    private int _hoveredButton; // -1 = menos, 1 = mais, 0 = nenhum
    private bool _hovering;

    public event EventHandler? ValueChanged;

    public AcrNumericUpDown()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.Selectable,
            true);

        Font = AcrFonts.Get(9F);
        ForeColor = AcrColors.Text;
        BackColor = AcrColors.Surface;

        _textBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            TextAlign = HorizontalAlignment.Center,
            Font = Font,
            ForeColor = ForeColor,
            BackColor = BackColor,
        };
        _textBox.GotFocus += (_, _) => Invalidate();
        _textBox.LostFocus += (_, _) => { CommitText(); Invalidate(); };
        _textBox.KeyDown += TextBox_KeyDown;
        _textBox.KeyPress += TextBox_KeyPress;
        _textBox.MouseEnter += (_, _) => SetHover(true);
        _textBox.MouseLeave += (_, _) => SetHover(ClientRectangle.Contains(PointToClient(Cursor.Position)));
        Controls.Add(_textBox);

        Size = new Size(140, 34);
        UpdateText();
    }

    [Category("Acr Custom")]
    [Description("Current value.")]
    [DefaultValue(typeof(decimal), "0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public decimal Value
    {
        get => _value;
        set
        {
            var clamped = Math.Round(Math.Clamp(value, _minimum, _maximum), _decimalPlaces);
            if (clamped == _value) { UpdateText(); return; }
            _value = clamped;
            UpdateText();
            Invalidate();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [Category("Acr Custom")]
    [DefaultValue(typeof(decimal), "0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public decimal Minimum
    {
        get => _minimum;
        set { _minimum = value; if (_maximum < value) _maximum = value; Value = _value; }
    }

    [Category("Acr Custom")]
    [DefaultValue(typeof(decimal), "100")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public decimal Maximum
    {
        get => _maximum;
        set { _maximum = value; if (_minimum > value) _minimum = value; Value = _value; }
    }

    [Category("Acr Custom")]
    [Description("Amount added/removed by the buttons, arrow keys and mouse wheel.")]
    [DefaultValue(typeof(decimal), "1")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public decimal Increment
    {
        get => _increment;
        set => _increment = Math.Abs(value);
    }

    [Category("Acr Custom")]
    [DefaultValue(0)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int DecimalPlaces
    {
        get => _decimalPlaces;
        set { _decimalPlaces = Math.Clamp(value, 0, 10); Value = _value; UpdateText(); }
    }

    [Category("Acr Custom")]
    [DefaultValue(8)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = Math.Max(0, value); Invalidate(); }
    }

    public void UpButton() => Value = _value + _increment;
    public void DownButton() => Value = _value - _increment;

    private int RingWidth => LogicalToDeviceUnits(RingWidthLogical);
    private int ButtonWidth => LogicalToDeviceUnits(ButtonWidthLogical);
    private Rectangle MinusRect => new(RingWidth, RingWidth, ButtonWidth, Height - RingWidth * 2);
    private Rectangle PlusRect => new(Width - RingWidth - ButtonWidth, RingWidth, ButtonWidth, Height - RingWidth * 2);
    private bool IsFocusedInside => ContainsFocus;

    private void UpdateText()
    {
        _textBox.Text = _value.ToString("N" + _decimalPlaces, CultureInfo.CurrentCulture);
    }

    private void CommitText()
    {
        if (decimal.TryParse(_textBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
            Value = parsed;
        else
            UpdateText();
    }

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Up: CommitText(); UpButton(); e.Handled = e.SuppressKeyPress = true; break;
            case Keys.Down: CommitText(); DownButton(); e.Handled = e.SuppressKeyPress = true; break;
            case Keys.Enter: CommitText(); _textBox.SelectAll(); e.Handled = e.SuppressKeyPress = true; break;
        }
    }

    private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
    {
        var nf = CultureInfo.CurrentCulture.NumberFormat;
        bool ok = char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)
                  || (e.KeyChar.ToString() == nf.NegativeSign && _minimum < 0)
                  || (_decimalPlaces > 0 && e.KeyChar.ToString() == nf.NumberDecimalSeparator)
                  || e.KeyChar.ToString() == nf.NumberGroupSeparator;
        if (!ok) e.Handled = true;
    }

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        _textBox.Focus();
        _textBox.SelectAll();
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (!Enabled) return;
        CommitText();
        if (e.Delta > 0) UpButton(); else DownButton();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int hovered = MinusRect.Contains(e.Location) ? -1 : PlusRect.Contains(e.Location) ? 1 : 0;
        Cursor = hovered != 0 ? Cursors.Hand : Cursors.Default;
        if (hovered != _hoveredButton) { _hoveredButton = hovered; Invalidate(); }
        SetHover(true);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoveredButton = 0;
        SetHover(ClientRectangle.Contains(PointToClient(Cursor.Position)));
        Invalidate();
    }

    private void SetHover(bool hovering)
    {
        if (_hovering == hovering) return;
        _hovering = hovering;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (!Enabled || e.Button != MouseButtons.Left) return;
        CommitText();
        if (MinusRect.Contains(e.Location)) DownButton();
        else if (PlusRect.Contains(e.Location)) UpButton();
        _textBox.Focus();
    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        int textHeight = _textBox.PreferredHeight;
        int left = RingWidth + ButtonWidth + 2;
        _textBox.SetBounds(left, (Height - textHeight) / 2, Math.Max(10, Width - left * 2), textHeight);
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        _textBox.Font = Font;
        PerformLayout();
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        _textBox.Enabled = Enabled;
        _textBox.BackColor = Enabled ? BackColor : AcrColors.DisabledBack;
        _textBox.ForeColor = Enabled ? ForeColor : AcrColors.DisabledFore;
        Invalidate();
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        _textBox.BackColor = BackColor;
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        _textBox.ForeColor = ForeColor;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(AcrColors.ParentBack(this));

        bool focused = IsFocusedInside && Enabled;
        var border = !Enabled ? AcrColors.BorderDisabled
            : focused ? AcrColors.Primary
            : _hovering ? AcrColors.BorderHover
            : AcrColors.Border;
        var fill = Enabled ? BackColor : AcrColors.DisabledBack;

        AcrGraphics.DrawFieldFrame(g, ClientRectangle, fill, border, focused, AcrColors.Primary, LogicalToDeviceUnits(_borderRadius), RingWidth);

        DrawButton(g, MinusRect, "−", _hoveredButton == -1, Enabled && _value > _minimum);
        DrawButton(g, PlusRect, "+", _hoveredButton == 1, Enabled && _value < _maximum);
    }

    private void DrawButton(Graphics g, Rectangle rect, string glyph, bool hovered, bool enabled)
    {
        var inner = Rectangle.Inflate(rect, -3, -3);
        if (hovered && enabled)
        {
            using var brush = new SolidBrush(AcrColors.SurfaceHover);
            using var path = AcrGraphics.CreateRoundedRectPath(inner, Math.Max(2, LogicalToDeviceUnits(_borderRadius) - 3));
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillPath(brush, path);
        }

        var color = !enabled ? AcrColors.TextDisabled : hovered ? AcrColors.Primary : AcrColors.IconGlyph;
        TextRenderer.DrawText(g, glyph, AcrFonts.WithStyle(Font, FontStyle.Bold), rect, color,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        BackColor = AcrTheme.Swap(BackColor, o.Surface, n.Surface);
        ForeColor = AcrTheme.Swap(ForeColor, o.Text, n.Text);
        Invalidate();
    }
}
