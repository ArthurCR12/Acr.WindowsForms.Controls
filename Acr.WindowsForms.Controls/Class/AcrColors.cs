namespace Acr.WindowsForms.Controls.Class
{
    /// <summary>
    /// Atalhos para as cores do tema atual (<see cref="AcrTheme.Current"/>).
    /// </summary>
    public static class AcrColors
    {
        private static AcrTheme T => AcrTheme.Current;

        public static Color Primary => T.Primary;
        public static Color PrimaryHover => T.PrimaryHover;
        public static Color PrimaryPressed => T.PrimaryPressed;
        public static Color OnPrimary => T.OnPrimary;

        public static Color Background => T.Background;
        public static Color Surface => T.Surface;
        public static Color SurfaceAlt => T.SurfaceAlt;
        public static Color SurfaceHover => T.SurfaceHover;
        public static Color SurfaceSunken => T.SurfaceSunken;

        public static Color Border => T.Border;
        public static Color BorderHover => T.BorderHover;
        public static Color BorderFocused => T.Primary;
        public static Color BorderDisabled => T.BorderDisabled;
        public static Color BorderSubtle => T.BorderSubtle;
        public static Color Separator => T.Separator;
        public static Color Track => T.Track;

        public static Color Text => T.Text;
        public static Color TextMuted => T.TextMuted;
        public static Color TextDisabled => T.TextDisabled;

        public static Color GridHeaderBack => T.GridHeaderBack;
        public static Color GridHeaderFore => T.GridHeaderFore;
        public static Color GridGridLines => T.GridLines;
        public static Color GridSelectionBack => T.GridSelectionBack;
        public static Color GridSelectionFore => T.GridSelectionFore;
        public static Color GridAltRow => T.GridAltRow;
        public static Color GridHoverRow => T.GridHoverRow;

        public static Color Success => T.Success;
        public static Color Warning => T.Warning;
        public static Color Error => T.Error;
        public static Color Info => T.Info;

        public static Color IconGlyph => T.IconGlyph;
        public static Color IconGlyphHover => T.IconGlyphHover;

        public static Color DisabledBack => T.DisabledBack;
        public static Color DisabledFore => T.DisabledFore;

        public static Color Neutral => T.Neutral;

        /// <summary>Resolve a cor de fundo do pai para pintar cantos arredondados (transparente → Surface).</summary>
        public static Color ParentBack(Control control)
        {
            var back = control.Parent?.BackColor ?? Surface;
            return back.A < 255 ? Surface : back;
        }

        /// <summary>Mistura <paramref name="baseColor"/> com <paramref name="overlay"/> (amount de 0 a 1).</summary>
        public static Color Blend(Color baseColor, Color overlay, float amount)
        {
            amount = Math.Clamp(amount, 0f, 1f);
            return Color.FromArgb(
                baseColor.A,
                (int)(baseColor.R + (overlay.R - baseColor.R) * amount),
                (int)(baseColor.G + (overlay.G - baseColor.G) * amount),
                (int)(baseColor.B + (overlay.B - baseColor.B) * amount));
        }
    }
}
