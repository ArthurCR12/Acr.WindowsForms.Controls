using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Controls.CustomButton;
using Acr.WindowsForms.Controls.Enums;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomModal;

public class AcrModal : Form
{
    private const int CornerRadius = 12;
    private const int HeaderHeight = 44;
    private const int FooterHeight = 56;
    private const int SidePadding = 20;

    private readonly Label _titleLabel;
    private readonly Label _closeButton;
    private readonly Label _messageLabel;
    private readonly Panel _headerPanel;
    private readonly Panel _footerPanel;
    private bool _dragging;
    private Point _dragStart;

    public Panel ContentPanel { get; }

    public AcrModal(string title, string message, AcrBadgeVariant variant = AcrBadgeVariant.Info)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F);
        Size = new Size(420, 220);
        MinimumSize = new Size(320, 150);

        _headerPanel = new Panel { Dock = DockStyle.Top, Height = HeaderHeight, BackColor = Color.Transparent };
        _titleLabel = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = AcrColors.Text,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Location = new Point(SidePadding, 0),
            Size = new Size(300, HeaderHeight),
        };
        _closeButton = new Label
        {
            Text = "✕",
            Font = new Font("Segoe UI", 10F),
            ForeColor = AcrColors.IconGlyph,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(28, 28),
            Cursor = Cursors.Hand,
        };
        _closeButton.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        _closeButton.MouseEnter += (_, _) => _closeButton.ForeColor = AcrColors.IconGlyphHover;
        _closeButton.MouseLeave += (_, _) => _closeButton.ForeColor = AcrColors.IconGlyph;
        _headerPanel.Controls.Add(_titleLabel);
        _headerPanel.Controls.Add(_closeButton);
        _headerPanel.MouseDown += Header_MouseDown;
        _headerPanel.MouseMove += Header_MouseMove;
        _headerPanel.MouseUp += (_, _) => _dragging = false;
        _titleLabel.MouseDown += Header_MouseDown;
        _titleLabel.MouseMove += Header_MouseMove;
        _titleLabel.MouseUp += (_, _) => _dragging = false;

        ContentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(SidePadding, 4, SidePadding, 4),
            BackColor = Color.Transparent,
        };

        _messageLabel = new Label
        {
            Text = message,
            AutoSize = false,
            Dock = DockStyle.Fill,
            ForeColor = AcrColors.Text,
            TextAlign = ContentAlignment.TopLeft,
            Visible = !string.IsNullOrEmpty(message),
        };
        ContentPanel.Controls.Add(_messageLabel);

        _footerPanel = new Panel { Dock = DockStyle.Bottom, Height = FooterHeight, BackColor = Color.Transparent };

        Controls.Add(ContentPanel);
        Controls.Add(_footerPanel);
        Controls.Add(_headerPanel);

        Load += (_, _) => PositionCloseButton();
        Resize += (_, _) => { PositionCloseButton(); UpdateRegion(); };

        UpdateRegion();
        SetAccentColor(variant);
    }

    public void SetButtons(params (string Text, DialogResult Result, bool IsPrimary)[] buttons)
    {
        _footerPanel.Controls.Clear();

        int x = Width - SidePadding;
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            var (text, result, isPrimary) = buttons[i];

            var button = new AcrButton
            {
                Text = text,
                Size = new Size(110, 32),
            };

            if (!isPrimary)
            {
                button.AccentColor = Color.FromArgb(240, 240, 240);
                button.ForeColor = AcrColors.Text;
            }

            x -= button.Width;
            button.Location = new Point(x, (FooterHeight - button.Height) / 2);
            button.Click += (_, _) => { DialogResult = result; Close(); };

            _footerPanel.Controls.Add(button);
            x -= 10;
        }
    }

    private void SetAccentColor(AcrBadgeVariant variant)
    {
        var accent = variant switch
        {
            AcrBadgeVariant.Success => AcrColors.Success,
            AcrBadgeVariant.Warning => AcrColors.Warning,
            AcrBadgeVariant.Error => AcrColors.Error,
            AcrBadgeVariant.Neutral => AcrColors.Neutral,
            AcrBadgeVariant.Primary => AcrColors.Primary,
            _ => AcrColors.Info,
        };
        _titleLabel.ForeColor = accent;
    }

    private void PositionCloseButton()
    {
        _closeButton.Location = new Point(Width - _closeButton.Width - 12, (HeaderHeight - _closeButton.Height) / 2);
        _titleLabel.Width = Math.Max(0, _closeButton.Left - SidePadding - 8);
    }

    private void Header_MouseDown(object? sender, MouseEventArgs e)
    {
        _dragging = true;
        _dragStart = e.Location;
    }

    private void Header_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_dragging) return;
        Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), CornerRadius);
        Region = new Region(path);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var borderPen = new Pen(Color.FromArgb(225, 225, 225), 1);
        using var borderPath = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius);
        e.Graphics.DrawPath(borderPen, borderPath);

        using var separatorPen = new Pen(Color.FromArgb(235, 235, 235), 1);
        e.Graphics.DrawLine(separatorPen, SidePadding, HeaderHeight, Width - SidePadding, HeaderHeight);
        e.Graphics.DrawLine(separatorPen, SidePadding, Height - FooterHeight, Width - SidePadding, Height - FooterHeight);
    }

    public static DialogResult ShowInfo(IWin32Window? owner, string message, string title = "Informação") =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Info);

    public static DialogResult ShowSuccess(IWin32Window? owner, string message, string title = "Sucesso") =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Success);

    public static DialogResult ShowWarning(IWin32Window? owner, string message, string title = "Atenção") =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Warning);

    public static DialogResult ShowError(IWin32Window? owner, string message, string title = "Erro") =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Error);

    private static DialogResult ShowSimple(IWin32Window? owner, string title, string message, AcrBadgeVariant variant)
    {
        using var modal = new AcrModal(title, message, variant);
        modal.SetButtons(("OK", DialogResult.OK, true));
        return owner != null ? modal.ShowDialog(owner) : modal.ShowDialog();
    }

    public static bool ShowConfirm(IWin32Window? owner, string message, string title = "Confirmação")
    {
        using var modal = new AcrModal(title, message, AcrBadgeVariant.Warning);
        modal.SetButtons(
            ("Cancelar", DialogResult.Cancel, false),
            ("Confirmar", DialogResult.Yes, true));
        var result = owner != null ? modal.ShowDialog(owner) : modal.ShowDialog();
        return result == DialogResult.Yes;
    }
}
