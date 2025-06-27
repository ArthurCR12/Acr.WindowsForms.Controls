using Acr.WindowsForms.Controls.Controls;
using Acr.WindowsForms.Controls.Enums;

namespace Acr.WindowsForms.Controls.Class;

public static class NotificationHelper
{
    public static void Show(string message, NotificationType type = NotificationType.Info)
    {
        Frm_Notification f = new();
        f.ShowNotification(message, type);
    }
}
