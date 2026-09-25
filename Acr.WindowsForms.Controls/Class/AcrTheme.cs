namespace Acr.WindowsForms.Controls.Class;

/// <summary>
/// Paleta de cores usada pelos controles Acr. Use <see cref="Light"/>, <see cref="Dark"/>
/// ou crie o seu (ex.: <c>AcrTheme.Light with { Primary = Color.Purple }</c>) e atribua a
/// <see cref="Current"/>: todas as janelas abertas são atualizadas na hora.
/// </summary>
public sealed record AcrTheme
{
    public string Name { get; init; } = "Custom";
    public bool IsDark { get; init; }

    // Marca
    public Color Primary { get; init; }
    public Color PrimaryHover { get; init; }
    public Color PrimaryPressed { get; init; }
    /// <summary>Texto/ícone sobre a cor primária.</summary>
    public Color OnPrimary { get; init; } = Color.White;

    // Superfícies
    /// <summary>Fundo das janelas/páginas.</summary>
    public Color Background { get; init; }
    /// <summary>Fundo dos controles (cards, campos, menus).</summary>
    public Color Surface { get; init; }
    /// <summary>Superfície levemente destacada (sidebar, áreas de apoio).</summary>
    public Color SurfaceAlt { get; init; }
    /// <summary>Fundo de item sob o mouse.</summary>
    public Color SurfaceHover { get; init; }
    /// <summary>Superfície "afundada" (trilhos, segmented control, botão secundário).</summary>
    public Color SurfaceSunken { get; init; }

    // Bordas
    public Color Border { get; init; }
    public Color BorderHover { get; init; }
    public Color BorderSubtle { get; init; }
    public Color BorderDisabled { get; init; }
    public Color Separator { get; init; }
    /// <summary>Trilhos de slider, toggle desligado, linhas do stepper.</summary>
    public Color Track { get; init; }

    // Texto
    public Color Text { get; init; }
    public Color TextMuted { get; init; }
    public Color TextDisabled { get; init; }
    public Color IconGlyph { get; init; }
    public Color IconGlyphHover { get; init; }

    // Estados
    public Color DisabledBack { get; init; }
    public Color DisabledFore { get; init; }

    // Grid
    public Color GridHeaderBack { get; init; }
    public Color GridHeaderFore { get; init; }
    public Color GridLines { get; init; }
    public Color GridSelectionBack { get; init; }
    public Color GridSelectionFore { get; init; }
    public Color GridAltRow { get; init; }
    public Color GridHoverRow { get; init; }

    // Semânticas
    public Color Success { get; init; }
    public Color Warning { get; init; }
    public Color Error { get; init; }
    public Color Info { get; init; }
    public Color Neutral { get; init; }

    public static AcrTheme Light { get; } = new()
    {
        Name = "Light",
        Primary = Color.FromArgb(0, 120, 215),
        PrimaryHover = Color.FromArgb(23, 133, 220),
        PrimaryPressed = Color.FromArgb(0, 96, 172),

        Background = Color.FromArgb(247, 248, 250),
        Surface = Color.White,
        SurfaceAlt = Color.FromArgb(250, 250, 251),
        SurfaceHover = Color.FromArgb(242, 242, 244),
        SurfaceSunken = Color.FromArgb(240, 241, 243),

        Border = Color.FromArgb(200, 200, 200),
        BorderHover = Color.FromArgb(150, 150, 150),
        BorderSubtle = Color.FromArgb(225, 225, 225),
        BorderDisabled = Color.FromArgb(225, 225, 225),
        Separator = Color.FromArgb(235, 235, 235),
        Track = Color.FromArgb(220, 220, 220),

        Text = Color.FromArgb(50, 50, 50),
        TextMuted = Color.FromArgb(108, 117, 125),
        TextDisabled = Color.FromArgb(160, 160, 160),
        IconGlyph = Color.FromArgb(140, 140, 140),
        IconGlyphHover = Color.FromArgb(70, 70, 70),

        DisabledBack = Color.FromArgb(235, 235, 235),
        DisabledFore = Color.FromArgb(170, 170, 170),

        GridHeaderBack = Color.FromArgb(245, 246, 248),
        GridHeaderFore = Color.FromArgb(60, 60, 60),
        GridLines = Color.FromArgb(230, 230, 230),
        GridSelectionBack = Color.FromArgb(224, 238, 253),
        GridSelectionFore = Color.FromArgb(30, 30, 30),
        GridAltRow = Color.FromArgb(250, 250, 251),
        GridHoverRow = Color.FromArgb(242, 247, 253),

        Success = Color.FromArgb(32, 148, 87),
        Warning = Color.FromArgb(212, 140, 15),
        Error = Color.FromArgb(197, 48, 48),
        Info = Color.FromArgb(0, 120, 215),
        Neutral = Color.FromArgb(108, 117, 125),
    };

    public static AcrTheme Dark { get; } = new()
    {
        Name = "Dark",
        IsDark = true,
        Primary = Color.FromArgb(56, 152, 236),
        PrimaryHover = Color.FromArgb(84, 168, 240),
        PrimaryPressed = Color.FromArgb(36, 124, 204),

        Background = Color.FromArgb(24, 25, 28),
        Surface = Color.FromArgb(34, 35, 39),
        SurfaceAlt = Color.FromArgb(29, 30, 34),
        SurfaceHover = Color.FromArgb(46, 47, 52),
        SurfaceSunken = Color.FromArgb(44, 45, 50),

        Border = Color.FromArgb(78, 80, 88),
        BorderHover = Color.FromArgb(120, 123, 132),
        BorderSubtle = Color.FromArgb(56, 58, 64),
        BorderDisabled = Color.FromArgb(52, 54, 60),
        Separator = Color.FromArgb(50, 52, 58),
        Track = Color.FromArgb(72, 74, 82),

        Text = Color.FromArgb(232, 233, 236),
        TextMuted = Color.FromArgb(160, 165, 174),
        TextDisabled = Color.FromArgb(105, 108, 116),
        IconGlyph = Color.FromArgb(150, 154, 162),
        IconGlyphHover = Color.FromArgb(225, 227, 232),

        DisabledBack = Color.FromArgb(44, 45, 50),
        DisabledFore = Color.FromArgb(110, 113, 120),

        GridHeaderBack = Color.FromArgb(40, 41, 46),
        GridHeaderFore = Color.FromArgb(215, 217, 222),
        GridLines = Color.FromArgb(52, 54, 60),
        GridSelectionBack = Color.FromArgb(30, 64, 104),
        GridSelectionFore = Color.FromArgb(240, 242, 245),
        GridAltRow = Color.FromArgb(38, 39, 44),
        GridHoverRow = Color.FromArgb(44, 50, 60),

        Success = Color.FromArgb(62, 184, 118),
        Warning = Color.FromArgb(236, 170, 50),
        Error = Color.FromArgb(232, 88, 88),
        Info = Color.FromArgb(56, 152, 236),
        Neutral = Color.FromArgb(150, 156, 165),
    };

    private static AcrTheme _current = Light;

    /// <summary>Tema em uso. Ao trocar, o tema é reaplicado em todas as janelas abertas.</summary>
    public static AcrTheme Current
    {
        get => _current;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (ReferenceEquals(_current, value)) return;
            var old = _current;
            _current = value;

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                ApplyTo(form, old, value);

            ThemeChanged?.Invoke(null, new AcrThemeChangedEventArgs(old, value));
        }
    }

    /// <summary>Disparado depois que <see cref="Current"/> muda e as janelas abertas foram atualizadas.</summary>
    public static event EventHandler<AcrThemeChangedEventArgs>? ThemeChanged;

    /// <summary>Aplica o tema atual a um controle e seus filhos (útil para janelas criadas depois da troca).</summary>
    public static void ApplyTo(Control root) => ApplyTo(root, Light, _current);

    internal static void ApplyTo(Control root, AcrTheme oldTheme, AcrTheme newTheme)
    {
        root.SuspendLayout();
        try
        {
            ApplyToControl(root, oldTheme, newTheme);
            foreach (Control child in root.Controls)
                ApplyTo(child, oldTheme, newTheme);
        }
        finally
        {
            root.ResumeLayout();
        }
        root.Invalidate(true);
    }

    private static void ApplyToControl(Control c, AcrTheme o, AcrTheme n)
    {
        if (c is IAcrThemeable themeable)
        {
            themeable.ApplyTheme(o, n);
            return;
        }

        switch (c)
        {
            case Form or TabPage or UserControl or SplitContainer:
                c.BackColor = Swap(c.BackColor, o.Background, n.Background, SystemColors.Control, Color.White);
                c.ForeColor = Swap(c.ForeColor, o.Text, n.Text, SystemColors.ControlText, Color.Black);
                break;

            case TextBoxBase or ComboBox or ListBox or NumericUpDown or DateTimePicker:
                c.BackColor = Swap(c.BackColor, o.Surface, n.Surface, SystemColors.Window);
                c.ForeColor = Swap(c.ForeColor, o.Text, n.Text, SystemColors.WindowText);
                break;

            case Label or CheckBox or RadioButton or GroupBox or LinkLabel:
                c.ForeColor = Swap(c.ForeColor, o.Text, n.Text, SystemColors.ControlText, Color.Black);
                if (c.BackColor != Color.Transparent)
                    c.BackColor = Swap(c.BackColor, o.Background, n.Background, SystemColors.Control);
                break;

            case Panel or FlowLayoutPanel or TableLayoutPanel:
                c.BackColor = Swap(c.BackColor, o.Background, n.Background, SystemColors.Control);
                c.BackColor = Swap(c.BackColor, o.Surface, n.Surface, Color.White);
                break;
        }
    }

    /// <summary>
    /// Troca <paramref name="current"/> pelo valor do novo tema se ele for o valor do tema anterior
    /// (ou um dos padrões do sistema informados). Cores personalizadas são preservadas.
    /// </summary>
    public static Color Swap(Color current, Color oldValue, Color newValue, params Color[] alsoReplace)
    {
        if (current.ToArgb() == oldValue.ToArgb()) return newValue;
        foreach (var c in alsoReplace)
            if (current.ToArgb() == c.ToArgb()) return newValue;
        return current;
    }
}

public sealed class AcrThemeChangedEventArgs : EventArgs
{
    public AcrTheme OldTheme { get; }
    public AcrTheme NewTheme { get; }

    public AcrThemeChangedEventArgs(AcrTheme oldTheme, AcrTheme newTheme)
    {
        OldTheme = oldTheme;
        NewTheme = newTheme;
    }
}

/// <summary>Controles que guardam cores próprias e sabem se atualizar quando o tema muda.</summary>
public interface IAcrThemeable
{
    void ApplyTheme(AcrTheme oldTheme, AcrTheme newTheme);
}
