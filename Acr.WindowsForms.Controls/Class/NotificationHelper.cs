using Acr.WindowsForms.Controls.Controls;
using Acr.WindowsForms.Controls.Enums;

namespace Acr.WindowsForms.Controls.Class;

public static class NotificationHelper
{
    /// <summary>Tempo padrão (ms) que a notificação fica visível.</summary>
    public static int DefaultDuration { get; set; } = 5000;

    /// <summary>Se false, as notificações não tocam som.</summary>
    public static bool PlaySound { get; set; } = true;

    public static void Show(string message, NotificationType type = NotificationType.Info) =>
        Show(message, type, null, DefaultDuration);

    public static void Show(string message, NotificationType type, string? title, int durationMs = 0)
    {
        Frm_Notification f = new();
        f.ShowNotification(message, type, title, durationMs > 0 ? durationMs : DefaultDuration, PlaySound);
    }

    /// <summary>
    /// Notificação com um botão de ação (ex.: "Desfazer"). O botão executa <paramref name="onAction"/>
    /// e fecha a notificação.
    /// </summary>
    public static void ShowWithAction(string message, string actionText, Action onAction,
        NotificationType type = NotificationType.Info, string? title = null, int durationMs = 0)
    {
        Frm_Notification f = new();
        f.ShowNotification(message, type, title, durationMs > 0 ? durationMs : DefaultDuration, PlaySound, actionText, onAction);
    }

    public static void Success(string message, string? title = null) => Show(message, NotificationType.Success, title);
    public static void Error(string message, string? title = null) => Show(message, NotificationType.Error, title);
    public static void Warning(string message, string? title = null) => Show(message, NotificationType.Warning, title);
    public static void Info(string message, string? title = null) => Show(message, NotificationType.Info, title);
}
