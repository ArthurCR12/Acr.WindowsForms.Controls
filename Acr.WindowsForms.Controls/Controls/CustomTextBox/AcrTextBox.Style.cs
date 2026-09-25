using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Acr.WindowsForms.Controls.Controls.CustomTextBox;

/// <summary>
/// Visual moderno do AcrTextBox. O TextBox nativo não permite cantos arredondados nem
/// espaçamento interno, então a área não-cliente (WM_NCCALCSIZE) é aumentada para criar
/// o espaçamento, e a moldura é desenhada nela (WM_NCPAINT).
/// </summary>
public partial class AcrTextBox : TextBox, IAcrValidatableControl, IAcrThemeable
{
    private const int WM_NCHITTEST = 0x0084;
    private const int HTCLIENT = 1;
    private const int HTHSCROLL = 6;
    private const int HTVSCROLL = 7;
    private const int WS_BORDER = 0x00800000;
    private const int WS_EX_CLIENTEDGE = 0x00000200;

    private const int RingWidthLogical = 3;

    private int RingWidth => LogicalToDeviceUnits(RingWidthLogical);

    private AcrTextBoxStyle _textBoxStyle = AcrTextBoxStyle.Outlined;
    private int _borderRadius = 8;
    private int _horizontalPadding = 10;
    private int _verticalPadding = 8;

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    [Category("Acr Custom")]
    [Description("Visual style: Outlined (rounded, default), Underline or Classic (square 1px border).")]
    [Browsable(true)]
    [DefaultValue(AcrTextBoxStyle.Outlined)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public AcrTextBoxStyle TextBoxStyle
    {
        get => _textBoxStyle;
        set
        {
            if (_textBoxStyle == value) return;
            _textBoxStyle = value;
            if (IsHandleCreated) RecreateHandle();
            ApplyModernHeight();
        }
    }

    [Category("Acr Custom")]
    [Description("Corner radius of the Outlined style.")]
    [Browsable(true)]
    [DefaultValue(8)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = Math.Max(0, value); RedrawBorder(); }
    }

    [Category("Acr Custom")]
    [Description("Inner horizontal spacing, in pixels, between the frame and the text (Outlined/Underline).")]
    [Browsable(true)]
    [DefaultValue(10)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int HorizontalPadding
    {
        get => _horizontalPadding;
        set { _horizontalPadding = Math.Max(0, value); UpdateFrame(); }
    }

    [Category("Acr Custom")]
    [Description("Inner vertical spacing, in pixels, above and below the text. Defines the height of single-line fields (Outlined/Underline).")]
    [Browsable(true)]
    [DefaultValue(8)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int VerticalPadding
    {
        get => _verticalPadding;
        set { _verticalPadding = Math.Max(2, value); ApplyModernHeight(); UpdateFrame(); }
    }

    private bool IsModernStyle => _textBoxStyle != AcrTextBoxStyle.Classic;

    private int TextLineHeight => FontHeight + 2;

    /// <summary>Altura de um campo de uma linha no estilo moderno.</summary>
    private int ModernHeight => TextLineHeight + LogicalToDeviceUnits(_verticalPadding) * 2;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            if (IsModernStyle)
            {
                // A moldura é desenhada por nós: remove a borda nativa.
                cp.Style &= ~WS_BORDER;
                cp.ExStyle &= ~WS_EX_CLIENTEDGE;
            }
            return cp;
        }
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (IsModernStyle && !Multiline) height = ModernHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        ApplyModernHeight();
        UpdateFrame();
    }

    protected override void OnMultilineChanged(EventArgs e)
    {
        base.OnMultilineChanged(e);
        ApplyModernHeight();
        UpdateFrame();
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (_clearButtonIcon != null) _clearButtonIcon.BackColor = BackColor;
        if (_passwordToggleIcon != null) _passwordToggleIcon.BackColor = BackColor;
        RedrawBorder();
    }

    protected override void OnParentBackColorChanged(EventArgs e)
    {
        base.OnParentBackColorChanged(e);
        RedrawBorder();
    }

    private void ApplyModernHeight()
    {
        if (IsModernStyle && !Multiline && Height != ModernHeight) Height = ModernHeight;
    }

    /// <summary>Força o Windows a recalcular a área não-cliente (espaçamento).</summary>
    private void UpdateFrame()
    {
        if (!IsHandleCreated) return;
        const uint SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_NOZORDER = 0x0004, SWP_NOACTIVATE = 0x0010, SWP_FRAMECHANGED = 0x0020;
        SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
        RedrawBorder();
    }

    /// <summary>Espaçamento (em pixels) entre a borda da janela e a área de texto, para uma altura de janela.</summary>
    private Padding GetFramePadding(int windowHeight)
    {
        int left = LogicalToDeviceUnits(_horizontalPadding) + (_textBoxStyle == AcrTextBoxStyle.Outlined ? RingWidth : 0);
        int top, bottom;

        if (Multiline)
        {
            top = LogicalToDeviceUnits(_verticalPadding) + (_textBoxStyle == AcrTextBoxStyle.Outlined ? RingWidth : 0);
            bottom = top;
        }
        else
        {
            // Centraliza verticalmente a linha de texto.
            top = Math.Max(0, (windowHeight - TextLineHeight) / 2);
            bottom = Math.Max(0, windowHeight - top - TextLineHeight);
        }

        return new Padding(left, top, left, bottom);
    }

    /// <summary>Trata as mensagens do estilo moderno. Retorna true se a mensagem já foi tratada.</summary>
    private bool HandleModernMessage(ref Message m)
    {
        if (!IsModernStyle) return false;

        switch (m.Msg)
        {
            case WM_NCCALCSIZE:
            {
                base.WndProc(ref m);
                // Tanto para wParam=TRUE (NCCALCSIZE_PARAMS) quanto FALSE (RECT), o primeiro campo é o RECT a ajustar.
                var rect = Marshal.PtrToStructure<RECT>(m.LParam);
                var pad = GetFramePadding(rect.Bottom - rect.Top);
                rect.Left += pad.Left;
                rect.Right -= pad.Right;
                rect.Top += pad.Top;
                rect.Bottom -= pad.Bottom;
                if (rect.Right < rect.Left) rect.Right = rect.Left;
                if (rect.Bottom < rect.Top) rect.Bottom = rect.Top;
                Marshal.StructureToPtr(rect, m.LParam, false);
                return true;
            }

            case WM_NCPAINT:
                base.WndProc(ref m); // desenha barras de rolagem (multiline), se houver
                PaintModernFrame();
                m.Result = IntPtr.Zero;
                return true;

            case WM_NCHITTEST:
            {
                base.WndProc(ref m);
                // O espaçamento se comporta como área de texto: cursor I-beam, clique foca, hover funciona.
                int hit = m.Result.ToInt32();
                if (hit != HTVSCROLL && hit != HTHSCROLL) m.Result = (IntPtr)HTCLIENT;
                return true;
            }
        }

        return false;
    }

    private void PaintModernFrame()
    {
        if (!IsHandleCreated || Width <= 0 || Height <= 0) return;

        var hdc = GetWindowDC(Handle);
        if (hdc == IntPtr.Zero) return;

        try
        {
            using var g = Graphics.FromHdc(hdc);
            var pad = GetFramePadding(Height);
            var inner = new Rectangle(pad.Left, pad.Top, Width - pad.Horizontal, Height - pad.Vertical);
            g.ExcludeClip(inner);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var parentBack = AcrColors.ParentBack(this);
            var fill = Enabled ? BackColor : AcrColors.DisabledBack;
            var accent = _hasError ? _borderErrorColor : _borderFocusColor;
            bool focused = Focused && Enabled;

            var borderColor = !Enabled
                ? AcrColors.BorderDisabled
                : _hasError
                    ? _borderErrorColor
                    : focused
                        ? _borderFocusColor
                        : _hovering
                            ? _borderHoverColor
                            : _borderColor;

            g.Clear(parentBack);

            if (_textBoxStyle == AcrTextBoxStyle.Underline)
            {
                using (var fillBrush = new SolidBrush(fill))
                    g.FillRectangle(fillBrush, 0, 0, Width, Height);

                int lineWidth = focused || _hasError ? 2 : 1;
                using var lineBrush = new SolidBrush(borderColor);
                g.FillRectangle(lineBrush, 0, Height - lineWidth, Width, lineWidth);
                return;
            }

            // Outlined
            var box = new Rectangle(RingWidth, RingWidth, Width - RingWidth * 2 - 1, Height - RingWidth * 2 - 1);
            int radius = Math.Min(LogicalToDeviceUnits(_borderRadius), box.Height / 2);

            if (focused || _hasError)
            {
                // Anel suave em volta do campo.
                var ring = Rectangle.Inflate(box, RingWidth - 1, RingWidth - 1);
                using var ringPath = AcrGraphics.CreateRoundedRectPath(ring, radius + RingWidth - 1);
                using var ringBrush = new SolidBrush(Color.FromArgb(focused ? 55 : 35, accent));
                g.FillPath(ringBrush, ringPath);
            }

            using (var boxPath = AcrGraphics.CreateRoundedRectPath(box, radius))
            {
                using (var fillBrush = new SolidBrush(fill))
                    g.FillPath(fillBrush, boxPath);

                using var pen = new Pen(borderColor, focused || _hasError ? 1.6f : 1f);
                g.DrawPath(pen, boxPath);
            }
        }
        finally
        {
            ReleaseDC(Handle, hdc);
        }
    }

    public void ApplyTheme(AcrTheme o, AcrTheme n)
    {
        _onEnterBackColor = AcrTheme.Swap(_onEnterBackColor, o.Surface, n.Surface, Color.White);
        _onLeaveBackColor = AcrTheme.Swap(_onLeaveBackColor, o.Surface, n.Surface, Color.White);
        _borderColor = AcrTheme.Swap(_borderColor, o.Border, n.Border);
        _borderHoverColor = AcrTheme.Swap(_borderHoverColor, o.BorderHover, n.BorderHover);
        _borderFocusColor = AcrTheme.Swap(_borderFocusColor, o.Primary, n.Primary);
        _borderErrorColor = AcrTheme.Swap(_borderErrorColor, o.Error, n.Error);

        if (Enabled)
        {
            BackColor = Focused ? _onEnterBackColor : _onLeaveBackColor;
            ForeColor = AcrTheme.Swap(ForeColor, o.Text, n.Text);
        }
        else
        {
            BackColor = n.DisabledBack;
            ForeColor = n.DisabledFore;
        }
        RedrawBorder();
    }
}
