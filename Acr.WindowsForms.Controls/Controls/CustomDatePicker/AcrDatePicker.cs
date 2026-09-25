using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Acr.WindowsForms.Controls.Controls.CustomDatePicker;

/// <summary>
/// Campo de data com calendário em popup, no visual Outlined do AcrTextBox.
/// Aceita valor vazio (<see cref="Value"/> = null). Teclado: Enter/Espaço/Alt+↓ abre, Delete limpa.
/// </summary>
[ToolboxBitmap(typeof(DateTimePicker))]
[DefaultEvent(nameof(AcrDatePicker.ValueChanged))]
public class AcrDatePicker : Control, IAcrThemeable
{
    private const int RingWidthLogical = 3;
    private const int IconAreaLogical = 30;

    private DateTime? _value;
    private DateTime _minDate = DateTime.MinValue;
    private DateTime _maxDate = DateTime.MaxValue;
    private string _format = "dd/MM/yyyy";
    private string _placeholder = "Selecione uma data";
    private int _borderRadius = 8;
    private bool _hovering;
    private CalendarPopup? _popup;

    public event EventHandler? ValueChanged;

    public AcrDatePicker()
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
        Cursor = Cursors.Hand;
        TabStop = true;
        Size = new Size(180, 34);
    }

    [Category("Acr Custom")]
    [Description("Selected date (null = empty).")]
    [DefaultValue(null)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTime? Value
    {
        get => _value;
        set
        {
            DateTime? v = value?.Date;
            if (v.HasValue) v = v.Value < _minDate ? _minDate.Date : v.Value > _maxDate ? _maxDate.Date : v;
            if (v == _value) return;
            _value = v;
            Invalidate();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [Category("Acr Custom")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTime MinDate
    {
        get => _minDate;
        set { _minDate = value.Date; Value = _value; }
    }

    [Category("Acr Custom")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTime MaxDate
    {
        get => _maxDate;
        set { _maxDate = value.Date; Value = _value; }
    }

    private bool ShouldSerializeMinDate() => _minDate != DateTime.MinValue;
    private bool ShouldSerializeMaxDate() => _maxDate != DateTime.MaxValue;

    [Category("Acr Custom")]
    [Description("Display format of the date (e.g. dd/MM/yyyy, dd 'de' MMMM 'de' yyyy).")]
    [DefaultValue("dd/MM/yyyy")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Format
    {
        get => _format;
        set { _format = string.IsNullOrWhiteSpace(value) ? "dd/MM/yyyy" : value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Text shown when there is no date.")]
    [DefaultValue("Selecione uma data")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string PlaceholderText
    {
        get => _placeholder;
        set { _placeholder = value ?? string.Empty; Invalidate(); }
    }

    [Category("Acr Custom")]
    [DefaultValue(8)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = Math.Max(0, value); Invalidate(); }
    }

    [Browsable(false)]
    public bool IsOpen => _popup is { IsDisposed: false, Visible: true };

    private int RingWidth => LogicalToDeviceUnits(RingWidthLogical);

    public void OpenCalendar()
    {
        if (!Enabled || IsOpen) return;
        Focus();
        var popup = new CalendarPopup(this);
        popup.FormClosed += (_, _) =>
        {
            if (_popup == popup) _popup = null;
            popup.Dispose();
            Invalidate();
        };
        _popup = popup;

        var below = PointToScreen(new Point(0, Height));
        var area = Screen.FromControl(this).WorkingArea;
        int x = Math.Min(Math.Max(area.Left, below.X), area.Right - popup.Width);
        int y = below.Y + popup.Height > area.Bottom ? PointToScreen(Point.Empty).Y - popup.Height : below.Y;
        popup.Location = new Point(x, y);
        popup.Show(FindForm());
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        OpenCalendar();
    }

    protected override bool IsInputKey(Keys keyData) =>
        keyData is Keys.Enter or Keys.Space or Keys.Delete or Keys.Back || base.IsInputKey(keyData);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is Keys.Enter or Keys.Space || (e.Alt && e.KeyCode == Keys.Down)) { OpenCalendar(); e.Handled = true; }
        else if (e.KeyCode is Keys.Delete or Keys.Back) { Value = null; e.Handled = true; }
    }

    protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
    protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovering = true; Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovering = false; Invalidate(); }
    protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(AcrColors.ParentBack(this));

        bool active = Enabled && (Focused || IsOpen);
        var border = !Enabled ? AcrColors.BorderDisabled
            : active ? AcrColors.Primary
            : _hovering ? AcrColors.BorderHover
            : AcrColors.Border;
        var fill = Enabled ? BackColor : AcrColors.DisabledBack;
        AcrGraphics.DrawFieldFrame(g, ClientRectangle, fill, border, active, AcrColors.Primary, LogicalToDeviceUnits(_borderRadius), RingWidth);

        int iconArea = LogicalToDeviceUnits(IconAreaLogical);
        int pad = RingWidth + LogicalToDeviceUnits(10);
        var textRect = new Rectangle(pad, 0, Width - pad - iconArea - RingWidth, Height);
        string text = _value.HasValue ? _value.Value.ToString(_format, CultureInfo.CurrentCulture) : _placeholder;
        var color = !Enabled ? AcrColors.DisabledFore : _value.HasValue ? ForeColor : AcrColors.TextDisabled;
        TextRenderer.DrawText(g, text, Font, textRect, color, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        DrawCalendarIcon(g, new Rectangle(Width - RingWidth - iconArea, 0, iconArea, Height), active ? AcrColors.Primary : AcrColors.IconGlyph);
    }

    private void DrawCalendarIcon(Graphics g, Rectangle area, Color color)
    {
        int s = LogicalToDeviceUnits(14);
        var r = new Rectangle(area.X + (area.Width - s) / 2, area.Y + (area.Height - s) / 2, s, s);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(color, 1.4f);
        using var path = AcrGraphics.CreateRoundedRectPath(r, 2);
        g.DrawPath(pen, path);
        int top = r.Y + s / 3;
        g.DrawLine(pen, r.X, top, r.Right, top);
        g.DrawLine(pen, r.X + s / 3, r.Y - 2, r.X + s / 3, r.Y + 2);
        g.DrawLine(pen, r.Right - s / 3, r.Y - 2, r.Right - s / 3, r.Y + 2);
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        BackColor = AcrTheme.Swap(BackColor, o.Surface, n.Surface);
        ForeColor = AcrTheme.Swap(ForeColor, o.Text, n.Text);
        Invalidate();
    }

    private void SelectDate(DateTime date)
    {
        Value = date;
        _popup?.Close();
        Focus();
    }

    private sealed class CalendarPopup : Form
    {
        private readonly AcrDatePicker _owner;
        private DateTime _month;
        private DateTime _focusDate;
        private DateTime? _hovered;
        private int _hoverNav; // -1 anterior, 1 próximo

        private int Cell => LogicalToDeviceUnits(32);
        private int HeaderH => LogicalToDeviceUnits(40);
        private int WeekH => LogicalToDeviceUnits(24);
        private int FooterH => LogicalToDeviceUnits(34);
        private int Pad => LogicalToDeviceUnits(10);

        public CalendarPopup(AcrDatePicker owner)
        {
            _owner = owner;
            _focusDate = owner.Value ?? DateTime.Today;
            _month = new DateTime(_focusDate.Year, _focusDate.Month, 1);

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            KeyPreview = true;
            BackColor = AcrColors.Surface;
            Font = owner.Font;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            Size = new Size(Pad * 2 + Cell * 7, HeaderH + WeekH + Cell * 6 + FooterH + Pad);
            Deactivate += (_, _) => Close();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                var cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private DateTime GridStart
        {
            get
            {
                int firstDay = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                int offset = ((int)_month.DayOfWeek - firstDay + 7) % 7;
                return _month.AddDays(-offset);
            }
        }

        private Rectangle CellRect(int index) =>
            new(Pad + index % 7 * Cell, HeaderH + WeekH + index / 7 * Cell, Cell, Cell);

        private Rectangle PrevRect => new(Pad, 0, Cell, HeaderH);
        private Rectangle NextRect => new(Width - Pad - Cell, 0, Cell, HeaderH);
        private Rectangle TodayRect => new(Pad, Height - FooterH, (Width - Pad * 2) / 2, FooterH - Pad / 2);
        private Rectangle ClearRect => new(Width / 2, Height - FooterH, (Width - Pad * 2) / 2, FooterH - Pad / 2);

        private DateTime? DateAt(Point p)
        {
            for (int i = 0; i < 42; i++)
                if (CellRect(i).Contains(p)) return GridStart.AddDays(i);
            return null;
        }

        private bool IsSelectable(DateTime d) => d >= _owner._minDate && d <= _owner._maxDate;

        private void ChangeMonth(int delta)
        {
            _month = _month.AddMonths(delta);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _hovered = DateAt(e.Location);
            _hoverNav = PrevRect.Contains(e.Location) ? -1 : NextRect.Contains(e.Location) ? 1 : 0;
            bool clickable = (_hovered.HasValue && IsSelectable(_hovered.Value)) || _hoverNav != 0
                             || TodayRect.Contains(e.Location) || ClearRect.Contains(e.Location);
            Cursor = clickable ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hovered = null;
            _hoverNav = 0;
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            ChangeMonth(e.Delta > 0 ? -1 : 1);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (PrevRect.Contains(e.Location)) { ChangeMonth(-1); return; }
            if (NextRect.Contains(e.Location)) { ChangeMonth(1); return; }
            if (TodayRect.Contains(e.Location) && IsSelectable(DateTime.Today)) { _owner.SelectDate(DateTime.Today); return; }
            if (ClearRect.Contains(e.Location)) { _owner.Value = null; Close(); return; }

            var date = DateAt(e.Location);
            if (date.HasValue && IsSelectable(date.Value)) _owner.SelectDate(date.Value);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int days = keyData switch { Keys.Left => -1, Keys.Right => 1, Keys.Up => -7, Keys.Down => 7, _ => 0 };
            if (days != 0)
            {
                _focusDate = _focusDate.AddDays(days);
                _month = new DateTime(_focusDate.Year, _focusDate.Month, 1);
                Invalidate();
                return true;
            }
            switch (keyData)
            {
                case Keys.PageUp: ChangeMonth(-1); return true;
                case Keys.PageDown: ChangeMonth(1); return true;
                case Keys.Enter:
                case Keys.Space:
                    if (IsSelectable(_focusDate)) _owner.SelectDate(_focusDate);
                    return true;
                case Keys.Escape: Close(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var culture = CultureInfo.CurrentCulture;

            using (var border = new Pen(AcrColors.BorderSubtle))
                g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);

            // Cabeçalho: ‹  Setembro 2026  ›
            string title = culture.TextInfo.ToTitleCase(_month.ToString("MMMM yyyy", culture));
            TextRenderer.DrawText(g, title, AcrFonts.WithStyle(Font, FontStyle.Bold), new Rectangle(0, 0, Width, HeaderH), AcrColors.Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            DrawNav(g, PrevRect, "‹", _hoverNav == -1);
            DrawNav(g, NextRect, "›", _hoverNav == 1);

            // Dias da semana
            int firstDay = (int)culture.DateTimeFormat.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                string name = culture.DateTimeFormat.AbbreviatedDayNames[(firstDay + i) % 7];
                name = name.Length > 0 ? char.ToUpper(name[0]) + name[1..Math.Min(3, name.Length)].TrimEnd('.') : name;
                var r = new Rectangle(Pad + i * Cell, HeaderH, Cell, WeekH);
                TextRenderer.DrawText(g, name, AcrFonts.Get(7.5F, FontStyle.Bold), r, AcrColors.TextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // Dias
            var start = GridStart;
            for (int i = 0; i < 42; i++)
            {
                var date = start.AddDays(i);
                var rect = Rectangle.Inflate(CellRect(i), -2, -2);
                bool inMonth = date.Month == _month.Month;
                bool selectable = IsSelectable(date);
                bool selected = _owner.Value == date;
                bool today = date == DateTime.Today;
                bool hovered = _hovered == date && selectable;
                bool keyboard = _focusDate == date;

                Color? back = selected ? AcrColors.Primary : hovered || keyboard ? AcrColors.SurfaceHover : null;
                if (back.HasValue)
                {
                    using var brush = new SolidBrush(back.Value);
                    g.FillEllipse(brush, rect);
                }
                if (today && !selected)
                {
                    using var pen = new Pen(AcrColors.Primary, 1.4f);
                    g.DrawEllipse(pen, rect);
                }

                var color = selected ? AcrColors.OnPrimary
                    : !selectable ? AcrColors.TextDisabled
                    : inMonth ? AcrColors.Text
                    : AcrColors.TextMuted;
                TextRenderer.DrawText(g, date.Day.ToString(culture), selected || today ? AcrFonts.WithStyle(Font, FontStyle.Bold) : Font,
                    rect, color, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // Rodapé: Hoje | Limpar
            using (var sep = new Pen(AcrColors.Separator))
                g.DrawLine(sep, Pad, Height - FooterH, Width - Pad, Height - FooterH);
            TextRenderer.DrawText(g, "Hoje", Font, TodayRect, AcrColors.Primary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "Limpar", Font, ClearRect, AcrColors.TextMuted, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawNav(Graphics g, Rectangle rect, string glyph, bool hovered)
        {
            if (hovered)
            {
                using var brush = new SolidBrush(AcrColors.SurfaceHover);
                g.FillEllipse(brush, Rectangle.Inflate(rect, -4, -6));
            }
            TextRenderer.DrawText(g, glyph, AcrFonts.Get(13F), rect, hovered ? AcrColors.Primary : AcrColors.IconGlyph,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
