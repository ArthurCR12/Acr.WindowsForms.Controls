using System.Collections.Concurrent;

namespace Acr.WindowsForms.Controls.Class;

/// <summary>
/// Fontes compartilhadas pelos controles Acr. Cada combinação de tamanho/estilo é criada
/// uma única vez e reutilizada, evitando criar (e vazar) um objeto Font por controle.
/// </summary>
/// <remarks>As fontes retornadas são compartilhadas: não chame Dispose nelas.</remarks>
public static class AcrFonts
{
    private static readonly ConcurrentDictionary<(string Family, float Size, FontStyle Style), Font> Cache = new();

    /// <summary>Família usada por padrão em todos os controles.</summary>
    public static string FamilyName { get; set; } = "Segoe UI";

    public static Font Get(float size, FontStyle style = FontStyle.Regular) => Get(FamilyName, size, style);

    public static Font Get(string family, float size, FontStyle style = FontStyle.Regular) =>
        Cache.GetOrAdd((family, size, style), key => new Font(key.Family, key.Size, key.Style));

    /// <summary>Versão com outro estilo de uma fonte existente (também compartilhada).</summary>
    public static Font WithStyle(Font font, FontStyle style) =>
        Get(font.FontFamily.Name, font.Size, style);

    public static Font Regular => Get(9F);
    public static Font Bold => Get(9F, FontStyle.Bold);
}
