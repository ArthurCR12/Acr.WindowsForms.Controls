using Acr.WindowsForms.Controls.Class;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Controls.CustomDropzone;

public class AcrDropzone : Control
{
    private const int CornerRadius = 10;
    private const int IconSize = 40;

    private bool _dragOver = false;
    private string _title = "Arraste arquivos aqui";
    private string _subtitle = "ou clique para selecionar";
    private string[] _allowedExtensions = Array.Empty<string>();

    public event EventHandler<AcrDropzoneFilesEventArgs>? FilesDropped;

    public AcrDropzone()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Font = AcrFonts.Get(9.5F);
        Size = new Size(360, 140);
        Cursor = Cursors.Hand;
        AllowDrop = true;
    }

    [Category("Acr Custom")]
    [Description("Main instruction text.")]
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
    [Description("Secondary, smaller instruction text.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Subtitle
    {
        get => _subtitle;
        set
        {
            _subtitle = value ?? string.Empty;
            Invalidate();
        }
    }

    [Category("Acr Custom")]
    [Description("Comma-separated list of allowed extensions (e.g. \".png,.jpg\"). Leave empty to allow any file.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string AllowedExtensions
    {
        get => string.Join(",", _allowedExtensions);
        set
        {
            _allowedExtensions = (value ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(ext => ext.StartsWith('.') ? ext.ToLowerInvariant() : "." + ext.ToLowerInvariant())
                .ToArray();
        }
    }

    private bool IsAllowed(string filePath)
    {
        if (_allowedExtensions.Length == 0) return true;
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return _allowedExtensions.Contains(ext);
    }

    protected override void OnDragEnter(DragEventArgs drgevent)
    {
        base.OnDragEnter(drgevent);

        if (drgevent.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            var files = (string[])drgevent.Data.GetData(DataFormats.FileDrop)!;
            drgevent.Effect = files.All(IsAllowed) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        else
        {
            drgevent.Effect = DragDropEffects.None;
        }

        _dragOver = drgevent.Effect == DragDropEffects.Copy;
        Invalidate();
    }

    protected override void OnDragLeave(EventArgs e)
    {
        base.OnDragLeave(e);
        _dragOver = false;
        Invalidate();
    }

    protected override void OnDragDrop(DragEventArgs drgevent)
    {
        base.OnDragDrop(drgevent);
        _dragOver = false;

        if (drgevent.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            var files = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)!)
                .Where(IsAllowed)
                .ToArray();

            if (files.Length > 0)
                FilesDropped?.Invoke(this, new AcrDropzoneFilesEventArgs(files));
        }

        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);

        using var dialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = _allowedExtensions.Length > 0
                ? $"Arquivos permitidos|{string.Join(";", _allowedExtensions.Select(ext => "*" + ext))}"
                : "Todos os arquivos|*.*",
        };

        if (dialog.ShowDialog() == DialogResult.OK)
            FilesDropped?.Invoke(this, new AcrDropzoneFilesEventArgs(dialog.FileNames));
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        var backColor = Parent?.BackColor ?? AcrColors.Surface;
        pevent.Graphics.Clear(backColor.A < 255 ? AcrColors.Surface : backColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        var accent = AcrColors.Primary;

        using (var path = AcrGraphics.CreateRoundedRectPath(bounds, CornerRadius))
        {
            var backFill = _dragOver ? Color.FromArgb(24, accent.R, accent.G, accent.B) : AcrColors.SurfaceAlt;
            using (var backBrush = new SolidBrush(backFill))
                e.Graphics.FillPath(backBrush, path);

            using var dashedPen = new Pen(_dragOver ? accent : AcrColors.Border, 1.5f)
            {
                DashStyle = DashStyle.Dash,
            };
            e.Graphics.DrawPath(dashedPen, path);
        }

        int centerX = Width / 2;
        int iconTop = Math.Max(10, Height / 2 - 40);
        var iconRect = new Rectangle(centerX - IconSize / 2, iconTop, IconSize, IconSize);
        DrawUploadIcon(e.Graphics, iconRect, _dragOver ? accent : AcrColors.Neutral);

        var titleRect = new Rectangle(10, iconRect.Bottom + 8, Width - 20, 20);
        TextRenderer.DrawText(e.Graphics, _title, Font, titleRect, AcrColors.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);

        using var subtitleFont = new Font(Font.FontFamily, Font.Size - 1F);
        var subtitleRect = new Rectangle(10, titleRect.Bottom + 2, Width - 20, 18);
        TextRenderer.DrawText(e.Graphics, _subtitle, subtitleFont, subtitleRect, AcrColors.Neutral, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
    }

    private static void DrawUploadIcon(Graphics g, Rectangle rect, Color color)
    {
        using var pen = new Pen(color, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };

        int cx = rect.X + rect.Width / 2;
        int arrowTop = rect.Top;
        int arrowBottom = rect.Top + (int)(rect.Height * 0.55);

        g.DrawLine(pen, cx, arrowBottom, cx, arrowTop);
        g.DrawLine(pen, cx, arrowTop, cx - 10, arrowTop + 10);
        g.DrawLine(pen, cx, arrowTop, cx + 10, arrowTop + 10);

        var trayRect = new Rectangle(rect.X, rect.Bottom - (int)(rect.Height * 0.22), rect.Width, (int)(rect.Height * 0.22));
        g.DrawLine(pen, trayRect.Left, trayRect.Top, trayRect.Left, trayRect.Bottom);
        g.DrawLine(pen, trayRect.Left, trayRect.Bottom, trayRect.Right, trayRect.Bottom);
        g.DrawLine(pen, trayRect.Right, trayRect.Bottom, trayRect.Right, trayRect.Top);
    }
}

public class AcrDropzoneFilesEventArgs : EventArgs
{
    public string[] Files { get; }

    public AcrDropzoneFilesEventArgs(string[] files)
    {
        Files = files;
    }
}
