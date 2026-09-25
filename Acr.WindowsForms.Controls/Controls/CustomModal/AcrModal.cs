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

    private Button? _primaryButton;

    public Panel ContentPanel { get; }

    /// <summary>
    /// Valor padrão de <see cref="DimBackground"/> para novos modais (inclusive os helpers estáticos).
    /// </summary>
    public static bool DefaultDimBackground { get; set; } = true;

    /// <summary>
    /// Se true, escurece o restante da tela (a janela dona) enquanto o modal estiver aberto.
    /// Use <see cref="ShowModal"/> para exibir o modal com o overlay.
    /// </summary>
    public bool DimBackground { get; set; } = DefaultDimBackground;

    /// <summary>Opacidade do overlay escuro (0.0 a 1.0).</summary>
    public double OverlayOpacity { get; set; } = 0.45;

    /// <summary>Cor do overlay.</summary>
    public Color OverlayColor { get; set; } = Color.Black;

    /// <summary>Se true, a tecla Esc fecha o modal com DialogResult.Cancel.</summary>
    public bool CloseOnEscape { get; set; } = true;

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

        KeyPreview = true;
        Load += (_, _) => PositionCloseButton();
        Resize += (_, _) => { PositionCloseButton(); UpdateRegion(); };

        UpdateRegion();
        SetAccentColor(variant);
    }

    public void SetButtons(params (string Text, DialogResult Result, bool IsPrimary)[] buttons)
    {
        _footerPanel.Controls.Clear();
        _primaryButton = null;

        int x = Width - SidePadding;
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            var (text, result, isPrimary) = buttons[i];

            var button = new AcrButton
            {
                Text = text,
                Size = new Size(110, 32),
            };

            if (!isPrimary) button.Variant = AcrButtonVariant.Secondary;

            x -= button.Width;
            button.Location = new Point(x, (FooterHeight - button.Height) / 2);
            button.Click += (_, _) => { DialogResult = result; Close(); };
            if (isPrimary) _primaryButton = button;

            _footerPanel.Controls.Add(button);
            x -= 10;
        }

        if (_primaryButton != null) AcceptButton = _primaryButton;
    }

    /// <summary>
    /// Exibe o modal. Se <see cref="DimBackground"/> for true, escurece a janela dona
    /// (ou a tela inteira, se não houver dona) enquanto o modal estiver aberto.
    /// </summary>
    public DialogResult ShowModal(IWin32Window? owner = null)
    {
        var ownerForm = ResolveOwnerForm(owner);

        if (!DimBackground)
            return ownerForm != null ? ShowDialog(ownerForm) : ShowDialog();

        using var overlay = new OverlayForm(OverlayColor, OverlayOpacity);
        if (ownerForm != null && ownerForm.WindowState != FormWindowState.Minimized)
        {
            overlay.Bounds = ownerForm.Bounds;
            overlay.Show(ownerForm);
        }
        else
        {
            overlay.Bounds = Screen.FromPoint(Cursor.Position).Bounds;
            overlay.Show();
        }

        try
        {
            // O overlay vira o dono do modal: garante que o modal fique acima dele e centralizado.
            return ShowDialog(overlay);
        }
        finally
        {
            overlay.Close();
            ownerForm?.Activate();
        }
    }

    private static Form? ResolveOwnerForm(IWin32Window? owner)
    {
        Form? form = owner switch
        {
            Form f => f,
            Control c => c.FindForm(),
            null => Form.ActiveForm,
            _ => Control.FromHandle(owner.Handle)?.FindForm(),
        };
        // Usa a janela de nível superior para cobrir a tela toda do app (inclusive MDI).
        while (form?.ParentForm != null) form = form.ParentForm;
        return form;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (CloseOnEscape && keyData == Keys.Escape)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private sealed class OverlayForm : Form
    {
        public OverlayForm(Color color, double opacity)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = color;
            Opacity = Math.Clamp(opacity, 0.0, 1.0);
            ControlBox = false;
            Text = string.Empty;
        }

        protected override bool ShowWithoutActivation => true;
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
        var old = Region;
        Region = new Region(path);
        old?.Dispose();
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

    public static DialogResult ShowInfo(IWin32Window? owner, string message, string title = "Informação", bool? dimBackground = null) =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Info, dimBackground);

    public static DialogResult ShowSuccess(IWin32Window? owner, string message, string title = "Sucesso", bool? dimBackground = null) =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Success, dimBackground);

    public static DialogResult ShowWarning(IWin32Window? owner, string message, string title = "Atenção", bool? dimBackground = null) =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Warning, dimBackground);

    public static DialogResult ShowError(IWin32Window? owner, string message, string title = "Erro", bool? dimBackground = null) =>
        ShowSimple(owner, title, message, AcrBadgeVariant.Error, dimBackground);

    private static DialogResult ShowSimple(IWin32Window? owner, string title, string message, AcrBadgeVariant variant, bool? dimBackground)
    {
        using var modal = new AcrModal(title, message, variant);
        if (dimBackground.HasValue) modal.DimBackground = dimBackground.Value;
        modal.SetButtons(("OK", DialogResult.OK, true));
        return modal.ShowModal(owner);
    }

    public static bool ShowConfirm(IWin32Window? owner, string message, string title = "Confirmação", bool? dimBackground = null)
    {
        using var modal = new AcrModal(title, message, AcrBadgeVariant.Warning);
        if (dimBackground.HasValue) modal.DimBackground = dimBackground.Value;
        modal.SetButtons(
            ("Cancelar", DialogResult.Cancel, false),
            ("Confirmar", DialogResult.Yes, true));
        return modal.ShowModal(owner) == DialogResult.Yes;
    }
}
