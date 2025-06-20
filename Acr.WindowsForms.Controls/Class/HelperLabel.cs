using Acr.WindowsForms.Controls.Enums;

namespace Acr.WindowsForms.Controls.Class
{
    public class HelperLabel
    {
        public static void CreateLabel(Control ctr, string text, MessageType messageType = MessageType.Error, Color color = default)
        {
            var lbl = new Label
            {
                Name = "lbl_" + ctr.Name,
                Text = text,
                AutoSize = true,
                Font = new Font("Arial", 7, FontStyle.Bold),
                BackColor = Color.Transparent,
                Location = new Point(ctr.Location.X, ctr.Location.Y + ctr.Height)
            };

            switch (messageType)
            {
                case MessageType.Error:
                    lbl.ForeColor = Color.Red;
                    break;
                case MessageType.Warning:
                    lbl.ForeColor = Color.Orange;
                    break;
                case MessageType.Information:
                    lbl.ForeColor = Color.Blue;
                    break;
            }

            if (color != default)
            {
                lbl.ForeColor = color;
            }
            ctr.Parent.Controls.Add(lbl);
        }

        public static void RemoveLabel(Control ctr)
        {
            ctr.Parent.Controls.Remove(ctr.Parent.Controls["lbl_" + ctr.Name]);
        }

    }
}
