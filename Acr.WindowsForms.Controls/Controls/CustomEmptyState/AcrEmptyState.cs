using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Controls.CustomButton;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomEmptyState;

public class AcrEmptyState : Control
{
    private const int IconSize = 48;

    private string _title = "Nenhum resultado encontrado";
    private string _description = "Tente ajustar os filtros ou a busca.";
    private string _actionText = string.Empty;
    private readonly AcrButton _actionButton;

    public event EventHandler? ActionClick;

    public AcrEmptyState()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = new Font("Segoe UI", 9F);

        _actionButton = new AcrButton
        {
            Visible = false,
            Size = new Size(140, 32),
            Anchor = AnchorStyles.None,
        };
        _actionButton.Click += (s, e) => ActionClick?.Invoke(this, e);
        Controls.Add(_actionButton);

        Size = new Size(320, 220);

        UpdateActionButton();
    }

    [Category("Acr Custom")]
    [Description("Main title text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => _title;
        set
        {
            _title = value ?? string.Empty;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Secondary, smaller description text below the title.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Description
    {
        get => _description;
        set
        {
            _description = value ?? string.Empty;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Text for the optional call-to-action button. Leave empty to hide it.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string ActionText
    {
        get => _actionText;
        set
        {
            _actionText = value ?? string.Empty;
            UpdateActionButton();
        }
    }

    private void UpdateActionButton()
    {
        _actionButton.Text = _actionText;
        _actionButton.Visible = !string.IsNullOrEmpty(_actionText);
        PositionChildren();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        PositionChildren();
    }

    private void PositionChildren()
    {
        if (!_actionButton.Visible) return;

        using var titleFont = new Font(Font, FontStyle.Bold);
        int titleHeight = TextRenderer.MeasureText(_title, titleFont).Height;
        int descHeight = TextRenderer.MeasureText(_description, Font, new Size(Width - 40, int.MaxValue), TextFormatFlags.WordBreak).Height;

        int contentTop = (Height - (IconSize + 12 + titleHeight + 6 + descHeight + 16 + _actionButton.Height)) / 2;
        int y = Math.Max(10, contentTop) + IconSize + 12 + titleHeight + 6 + descHeight + 16;

        _actionButton.Location = new Point((Width - _actionButton.Width) / 2, y);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? Color.White;
        pevent.Graphics.Clear(backColor.A == 0 ? Color.White : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using var titleFont = new Font(Font, FontStyle.Bold);
        int titleHeight = TextRenderer.MeasureText(_title, titleFont).Height;
        var descRect = new Rectangle(20, 0, Width - 40, int.MaxValue);
        int descHeight = TextRenderer.MeasureText(_description, Font, descRect.Size, TextFormatFlags.WordBreak).Height;

        int actionHeight = _actionButton.Visible ? _actionButton.Height + 16 : 0;
        int totalHeight = IconSize + 12 + titleHeight + 6 + descHeight + actionHeight;
        int top = Math.Max(10, (Height - totalHeight) / 2);

        var iconRect = new Rectangle((Width - IconSize) / 2, top, IconSize, IconSize);
        DrawPlaceholderIcon(e.Graphics, iconRect);

        var titleRect = new Rectangle(20, iconRect.Bottom + 12, Width - 40, titleHeight);
        TextRenderer.DrawText(e.Graphics, _title, titleFont, titleRect, AcrColors.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);

        var descriptionRect = new Rectangle(20, titleRect.Bottom + 6, Width - 40, descHeight);
        TextRenderer.DrawText(e.Graphics, _description, Font, descriptionRect, AcrColors.Neutral, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.WordBreak);
    }

    private static void DrawPlaceholderIcon(Graphics g, Rectangle rect)
    {
        using var backBrush = new SolidBrush(Color.FromArgb(240, 240, 242));
        g.FillEllipse(backBrush, rect);

        using var pen = new Pen(AcrColors.Neutral, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        int inset = rect.Width / 3;
        var glassRect = new Rectangle(rect.Left + inset / 2, rect.Top + inset / 2, rect.Width - inset, rect.Height - inset - 6);
        g.DrawEllipse(pen, glassRect);
        g.DrawLine(pen, glassRect.Right - 2, glassRect.Bottom - 2, rect.Right - 8, rect.Bottom - 8);
    }
}
