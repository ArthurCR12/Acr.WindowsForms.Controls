using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomDropdownMenu;

public class AcrDropdownMenu : Component
{
    public List<string> Items { get; } = new();

    public event EventHandler<AcrDropdownMenuItemEventArgs>? ItemClicked;

    public Font MenuFont { get; set; } = new Font("Segoe UI", 9F);

    public void ShowFor(Control anchor)
    {
        var location = anchor.PointToScreen(new Point(0, anchor.Height));
        Show(location);
    }

    public void Show(Point screenLocation)
    {
        if (Items.Count == 0) return;

        var popup = new PopupForm(Items, MenuFont, OnItemClicked);
        popup.FormClosed += (_, _) => popup.Dispose();
        popup.ShowPopup(screenLocation);
    }

    private void OnItemClicked(int index, string text) =>
        ItemClicked?.Invoke(this, new AcrDropdownMenuItemEventArgs(index, text));

    private sealed class PopupForm : Form
    {
        private const int ItemHeight = 28;
        private const int Padding = 6;

        private readonly List<string> _items;
        private readonly Action<int, string> _onClick;
        private int _hoveredIndex = -1;

        public PopupForm(List<string> items, Font font, Action<int, string> onClick)
        {
            _items = items;
            _onClick = onClick;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.White;
            Font = font;

            int textWidth = items.Select(i => TextRenderer.MeasureText(i, font).Width).DefaultIfEmpty(80).Max();
            Size = new Size(textWidth + Padding * 2 + 20, ItemHeight * items.Count + Padding * 2);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            MouseMove += OnMouseMoveHandler;
            MouseClick += OnMouseClickHandler;
            Deactivate += (_, _) => Close();
        }

        public void ShowPopup(Point screenLocation)
        {
            Location = screenLocation;
            Show();
            Activate();
        }

        private int IndexAt(Point p)
        {
            if (p.X < 0 || p.X > Width || p.Y < Padding || p.Y > Height - Padding) return -1;
            int index = (p.Y - Padding) / ItemHeight;
            return index >= 0 && index < _items.Count ? index : -1;
        }

        private void OnMouseMoveHandler(object? sender, MouseEventArgs e)
        {
            int index = IndexAt(e.Location);
            if (index != _hoveredIndex)
            {
                _hoveredIndex = index;
                Invalidate();
            }
        }

        private void OnMouseClickHandler(object? sender, MouseEventArgs e)
        {
            int index = IndexAt(e.Location);
            if (index >= 0)
            {
                _onClick(index, _items[index]);
                Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = AcrGraphics.CreateRoundedRectPath(bounds, 6))
            {
                using var backBrush = new SolidBrush(Color.White);
                e.Graphics.FillPath(backBrush, path);

                using var borderPen = new Pen(Color.FromArgb(220, 220, 220), 1);
                e.Graphics.DrawPath(borderPen, path);
            }

            for (int i = 0; i < _items.Count; i++)
            {
                var itemRect = new Rectangle(Padding, Padding + i * ItemHeight, Width - Padding * 2, ItemHeight);

                if (i == _hoveredIndex)
                {
                    using var hoverBrush = new SolidBrush(Color.FromArgb(240, 240, 240));
                    using var hoverPath = AcrGraphics.CreateRoundedRectPath(itemRect, 4);
                    e.Graphics.FillPath(hoverBrush, hoverPath);
                }

                var textRect = new Rectangle(itemRect.X + 10, itemRect.Y, itemRect.Width - 20, itemRect.Height);
                TextRenderer.DrawText(e.Graphics, _items[i], Font, textRect, AcrColors.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
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
