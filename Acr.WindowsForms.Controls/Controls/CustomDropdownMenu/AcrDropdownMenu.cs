using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomDropdownMenu;

[ToolboxBitmap(typeof(ContextMenuStrip))]
[DefaultEvent(nameof(AcrDropdownMenu.ItemClicked))]
public class AcrDropdownMenu : Component
{
    /// <summary>Texto usado em <see cref="Items"/> para desenhar uma linha separadora.</summary>
    public const string Separator = "-";

    /// <summary>Itens do menu. Use <see cref="Separator"/> ("-") para inserir um separador.</summary>
    public List<string> Items { get; } = new();

    /// <summary>Índices de itens que aparecem desabilitados (não clicáveis).</summary>
    public HashSet<int> DisabledIndexes { get; } = new();

    public event EventHandler<AcrDropdownMenuItemEventArgs>? ItemClicked;

    /// <summary>Disparado quando o menu é fechado (com ou sem seleção).</summary>
    public event EventHandler? Closed;

    /// <summary>Fonte do menu. A fonte padrão é compartilhada (AcrFonts) e não é descartada pelo menu.</summary>
    [Category("Acr Custom")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Font MenuFont { get; set; } = AcrFonts.Get(9F);

    /// <summary>Cor de destaque do item sob o mouse / selecionado pelo teclado. Vazia = cor do tema.</summary>
    [Category("Acr Custom")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverColor { get; set; } = Color.Empty;

    /// <summary>Se true, o menu tem pelo menos a largura do controle âncora em <see cref="ShowFor"/>.</summary>
    [Category("Acr Custom")]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool MatchAnchorWidth { get; set; } = true;

    /// <summary>Quantidade máxima de itens visíveis antes de rolar.</summary>
    [Category("Acr Custom")]
    [DefaultValue(12)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int MaxVisibleItems { get; set; } = 12;

    public bool IsOpen => _popup is { IsDisposed: false, Visible: true };

    private PopupForm? _popup;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _popup?.Close();
        }
        base.Dispose(disposing);
    }

    public void ShowFor(Control anchor)
    {
        var bottom = anchor.PointToScreen(new Point(0, anchor.Height + 2));
        var top = anchor.PointToScreen(Point.Empty);
        Show(bottom, MatchAnchorWidth ? anchor.Width : 0, top.Y - 2);
    }

    public void Show(Point screenLocation) => Show(screenLocation, 0, null);

    public void Close() => _popup?.Close();

    private void Show(Point screenLocation, int minWidth, int? flipAboveY)
    {
        if (Items.Count == 0) return;
        _popup?.Close();

        var popup = new PopupForm(this, minWidth);
        popup.FormClosed += (_, _) =>
        {
            if (_popup == popup) _popup = null;
            popup.Dispose();
            Closed?.Invoke(this, EventArgs.Empty);
        };
        _popup = popup;
        popup.ShowPopup(screenLocation, flipAboveY);
    }

    private void OnItemClicked(int index, string text) =>
        ItemClicked?.Invoke(this, new AcrDropdownMenuItemEventArgs(index, text));

    private sealed class PopupForm : Form
    {
        private const int ItemHeightLogical = 28;
        private int ItemHeight => LogicalToDeviceUnits(ItemHeightLogical);
        private const int SeparatorHeightLogical = 9;
        private int SeparatorHeight => LogicalToDeviceUnits(SeparatorHeightLogical);
        private const int PadLogical = 6;
        private int Pad => LogicalToDeviceUnits(PadLogical);

        private readonly AcrDropdownMenu _owner;
        private readonly List<string> _items;
        private readonly int[] _tops;
        private readonly int _contentHeight;
        private int _hoveredIndex = -1;
        private int _scroll;

        public PopupForm(AcrDropdownMenu owner, int minWidth)
        {
            _owner = owner;
            _items = owner.Items.ToList();

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            KeyPreview = true;
            BackColor = AcrColors.Surface;
            Font = owner.MenuFont;

            _tops = new int[_items.Count];
            int y = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                _tops[i] = y;
                y += IsSeparator(i) ? SeparatorHeight : ItemHeight;
            }
            _contentHeight = y;

            int textWidth = _items.Where(i => i != Separator).Select(i => TextRenderer.MeasureText(i, Font).Width).DefaultIfEmpty(80).Max();
            int maxHeight = Math.Max(1, owner.MaxVisibleItems) * ItemHeight;
            Size = new Size(Math.Max(minWidth, textWidth + Pad * 2 + 20), Math.Min(_contentHeight, maxHeight) + Pad * 2);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            MouseMove += OnMouseMoveHandler;
            MouseClick += OnMouseClickHandler;
            MouseLeave += (_, _) => { _hoveredIndex = -1; Invalidate(); };
            MouseWheel += (_, e) => ScrollBy(-Math.Sign(e.Delta) * ItemHeight * 2);
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

        private bool IsSeparator(int i) => _items[i] == Separator;
        private bool IsSelectable(int i) => i >= 0 && i < _items.Count && !IsSeparator(i) && !_owner.DisabledIndexes.Contains(i);
        private int ViewHeight => Height - Pad * 2;

        public void ShowPopup(Point screenLocation, int? flipAboveY)
        {
            var area = Screen.FromPoint(screenLocation).WorkingArea;
            int x = Math.Min(Math.Max(area.Left, screenLocation.X), area.Right - Width);
            int y = screenLocation.Y;
            if (y + Height > area.Bottom)
                y = flipAboveY.HasValue ? flipAboveY.Value - Height : area.Bottom - Height;
            Location = new Point(x, Math.Max(area.Top, y));
            Show();
            Activate();
        }

        private void ScrollBy(int delta)
        {
            int max = Math.Max(0, _contentHeight - ViewHeight);
            int value = Math.Clamp(_scroll + delta, 0, max);
            if (value == _scroll) return;
            _scroll = value;
            Invalidate();
        }

        private void EnsureVisible(int index)
        {
            int top = _tops[index];
            int bottom = top + ItemHeight;
            if (top < _scroll) ScrollBy(top - _scroll);
            else if (bottom > _scroll + ViewHeight) ScrollBy(bottom - (_scroll + ViewHeight));
        }

        private int IndexAt(Point p)
        {
            if (p.X < 0 || p.X > Width || p.Y < Pad || p.Y > Height - Pad) return -1;
            int y = p.Y - Pad + _scroll;
            for (int i = _items.Count - 1; i >= 0; i--)
                if (y >= _tops[i]) return IsSelectable(i) ? i : -1;
            return -1;
        }

        private void OnMouseMoveHandler(object? sender, MouseEventArgs e)
        {
            int index = IndexAt(e.Location);
            if (index != _hoveredIndex)
            {
                _hoveredIndex = index;
                Cursor = index >= 0 ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        private void OnMouseClickHandler(object? sender, MouseEventArgs e) => SelectItem(IndexAt(e.Location));

        private void SelectItem(int index)
        {
            if (!IsSelectable(index)) return;
            var text = _items[index];
            Close();
            _owner.OnItemClicked(index, text);
        }

        private void MoveHover(int direction)
        {
            if (_items.Count == 0) return;
            int i = _hoveredIndex;
            for (int n = 0; n < _items.Count; n++)
            {
                i = i < 0 ? (direction > 0 ? 0 : _items.Count - 1) : (i + direction + _items.Count) % _items.Count;
                if (IsSelectable(i))
                {
                    _hoveredIndex = i;
                    EnsureVisible(i);
                    Invalidate();
                    return;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Down: MoveHover(1); return true;
                case Keys.Up: MoveHover(-1); return true;
                case Keys.Enter:
                case Keys.Space: SelectItem(_hoveredIndex); return true;
                case Keys.Escape: Close(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = AcrGraphics.CreateRoundedRectPath(bounds, 6))
            {
                using var backBrush = new SolidBrush(AcrColors.Surface);
                e.Graphics.FillPath(backBrush, path);

                using var borderPen = new Pen(AcrColors.BorderSubtle, 1);
                e.Graphics.DrawPath(borderPen, path);
            }

            e.Graphics.SetClip(new Rectangle(1, Pad, Width - 2, ViewHeight));

            for (int i = 0; i < _items.Count; i++)
            {
                int top = Pad + _tops[i] - _scroll;

                if (IsSeparator(i))
                {
                    using var sepPen = new Pen(AcrColors.Separator, 1);
                    int sy = top + SeparatorHeight / 2;
                    e.Graphics.DrawLine(sepPen, Pad + 4, sy, Width - Pad - 4, sy);
                    continue;
                }

                var itemRect = new Rectangle(Pad, top, Width - Pad * 2, ItemHeight);

                if (i == _hoveredIndex)
                {
                    using var hoverBrush = new SolidBrush(_owner.HoverColor.IsEmpty ? AcrColors.SurfaceHover : _owner.HoverColor);
                    using var hoverPath = AcrGraphics.CreateRoundedRectPath(itemRect, 4);
                    e.Graphics.FillPath(hoverBrush, hoverPath);
                }

                var color = _owner.DisabledIndexes.Contains(i) ? AcrColors.TextDisabled : AcrColors.Text;
                var textRect = new Rectangle(itemRect.X + 10, itemRect.Y, itemRect.Width - 20, itemRect.Height);
                TextRenderer.DrawText(e.Graphics, _items[i], Font, textRect, color, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }

            e.Graphics.ResetClip();

            if (_contentHeight > ViewHeight)
            {
                int thumbHeight = Math.Max(20, ViewHeight * ViewHeight / _contentHeight);
                int thumbY = Pad + (ViewHeight - thumbHeight) * _scroll / Math.Max(1, _contentHeight - ViewHeight);
                using var thumbBrush = new SolidBrush(AcrColors.Border);
                using var thumbPath = AcrGraphics.CreateRoundedRectPath(new Rectangle(Width - 6, thumbY, 3, thumbHeight), 1);
                e.Graphics.FillPath(thumbBrush, thumbPath);
            }
        }
    }
}

public class AcrDropdownMenuItemEventArgs : EventArgs
{
    public int Index { get; }
    public string Text { get; }

    public AcrDropdownMenuItemEventArgs(int index, string text)
    {
        Index = index;
        Text = text;
    }
}
