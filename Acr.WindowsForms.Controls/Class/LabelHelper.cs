using Acr.WindowsForms.Controls.Enums;

namespace Acr.WindowsForms.Controls.Class
{
    public class LabelHelper
    {
        public static Label CreateLabel(
            Control ctr,
            string text,
            MessageType messageType = MessageType.Error,
            Color fcolor = default,
            Color bColor = default,
            string location = "bottom"
        )
        {
            var lbl = new Label
            {
                Name = "lbl_" + ctr.Name,
                Text = text,
                AutoSize = true,
                Font = new Font("Arial", 7, FontStyle.Bold),
                BackColor = Color.Transparent,                
            };


            var existingLabel = ctr.Parent.Controls["lbl_" + ctr.Name];
            if (existingLabel != null) ctr.Parent.Controls.Remove(existingLabel);


            lbl.CreateControl();
            lbl.Location = location switch
            {
                "top" => new Point(ctr.Location.X, ctr.Location.Y - lbl.Height + 5),
                _ => new Point(ctr.Location.X, ctr.Location.Y + ctr.Height),
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
                case MessageType.Title:
                    lbl.ForeColor = Color.Black;
                    lbl.Font = new Font("Arial", 8, FontStyle.Bold);
                    break;
            }

            if (fcolor != default) lbl.ForeColor = fcolor;

            if (bColor != default) lbl.BackColor = bColor;
            else lbl.BackColor = Color.Transparent;

            ctr.Parent.Controls.Add(lbl);
            return lbl;
        }

        public static void RemoveLabel(Control ctr)
        {
            var lbl = ctr.Parent.Controls["lbl_" + ctr.Name];
            if (lbl != null)
                ctr.Parent.Controls.Remove(lbl);
        }

    }
}
