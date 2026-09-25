using Acr.WindowsForms.Controls.Enums;
using System.Runtime.CompilerServices;

namespace Acr.WindowsForms.Controls.Class
{
    /// <summary>
    /// Cria labels auxiliares (título, erro, aviso, informação) acima ou abaixo de um controle.
    /// As labels acompanham o controle: movem-se com ele, ficam ocultas junto com ele e são
    /// removidas quando ele é descartado.
    /// </summary>
    public class LabelHelper
    {
        private sealed class Tracking
        {
            public readonly Dictionary<MessageType, (Label Label, bool Top)> Labels = new();
        }

        private static readonly ConditionalWeakTable<Control, Tracking> Tracked = new();

        public static Label CreateLabel(
            Control ctr,
            string text,
            MessageType messageType = MessageType.Error,
            Color fcolor = default,
            Color bColor = default,
            string location = "bottom"
        )
        {
            // Substitui uma label anterior do mesmo tipo em vez de empilhar.
            RemoveLabel(ctr, messageType);

            var lbl = new Label
            {
                Name = LabelName(ctr, messageType),
                Text = text,
                AutoSize = true,
                Font = AcrFonts.Get(7.5F, FontStyle.Bold),
                BackColor = Color.Transparent,
            };

            switch (messageType)
            {
                case MessageType.Error:
                    lbl.ForeColor = AcrColors.Error;
                    break;
                case MessageType.Warning:
                    lbl.ForeColor = AcrColors.Warning;
                    break;
                case MessageType.Information:
                    lbl.ForeColor = AcrColors.Info;
                    break;
                case MessageType.Title:
                    lbl.ForeColor = AcrColors.Text;
                    lbl.Font = AcrFonts.Get(8F, FontStyle.Bold);
                    break;
            }

            if (fcolor != default) lbl.ForeColor = fcolor;
            lbl.BackColor = bColor != default ? bColor : Color.Transparent;

            bool top = location == "top";
            var tracking = Tracked.GetValue(ctr, Attach);
            tracking.Labels[messageType] = (lbl, top);

            // Sem pai ainda: a label é adicionada quando o controle entrar num container.
            if (ctr.Parent != null)
            {
                ctr.Parent.Controls.Add(lbl);
                lbl.BringToFront();
            }

            Reposition(ctr, lbl, top);
            return lbl;
        }

        public static void RemoveLabel(Control ctr, MessageType messageType = MessageType.Error)
        {
            if (Tracked.TryGetValue(ctr, out var tracking) && tracking.Labels.Remove(messageType, out var entry))
            {
                entry.Label.Parent?.Controls.Remove(entry.Label);
                entry.Label.Dispose();
                return;
            }

            // Compatibilidade: label criada sem rastreamento.
            var lbl = ctr.Parent?.Controls[LabelName(ctr, messageType)];
            if (lbl != null)
            {
                ctr.Parent!.Controls.Remove(lbl);
                lbl.Dispose();
            }
        }

        private static string LabelName(Control ctr, MessageType messageType) => $"lbl_{ctr.Name}_{messageType}";

        private static Tracking Attach(Control ctr)
        {
            ctr.LocationChanged += (_, _) => RepositionAll(ctr);
            ctr.SizeChanged += (_, _) => RepositionAll(ctr);
            ctr.VisibleChanged += (_, _) => RepositionAll(ctr);
            ctr.ParentChanged += (_, _) => Reparent(ctr);
            ctr.Disposed += (_, _) =>
            {
                if (!Tracked.TryGetValue(ctr, out var tracking)) return;
                foreach (var (label, _) in tracking.Labels.Values)
                {
                    label.Parent?.Controls.Remove(label);
                    label.Dispose();
                }
                tracking.Labels.Clear();
            };
            return new Tracking();
        }

        private static void Reparent(Control ctr)
        {
            if (!Tracked.TryGetValue(ctr, out var tracking)) return;
            foreach (var (label, top) in tracking.Labels.Values)
            {
                label.Parent?.Controls.Remove(label);
                if (ctr.Parent != null)
                {
                    ctr.Parent.Controls.Add(label);
                    label.BringToFront();
                }
                Reposition(ctr, label, top);
            }
        }

        private static void RepositionAll(Control ctr)
        {
            if (!Tracked.TryGetValue(ctr, out var tracking)) return;
            foreach (var (label, top) in tracking.Labels.Values)
                Reposition(ctr, label, top);
        }

        private static void Reposition(Control ctr, Label lbl, bool top)
        {
            lbl.Visible = ctr.Visible;
            int height = lbl.PreferredHeight;
            lbl.Location = top
                ? new Point(ctr.Left, ctr.Top - height + 1)
                : new Point(ctr.Left, ctr.Bottom + 1);
        }
    }
}
