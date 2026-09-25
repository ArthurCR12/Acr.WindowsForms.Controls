using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Controls.CustomButton;
using Acr.WindowsForms.Controls.Controls.CustomCard;
using Acr.WindowsForms.Controls.Controls.CustomDatePicker;
using Acr.WindowsForms.Controls.Controls.CustomNumericUpDown;
using Acr.WindowsForms.Controls.Controls.CustomTabControl;
using Acr.WindowsForms.Controls.Controls.CustomToolTip;
using Acr.WindowsForms.Controls.Enums;

namespace AcrFormsTest
{
    /// <summary>Seções "Tema" e "Componentes novos" da aba Novidades.</summary>
    public partial class Form1
    {
        private readonly AcrToolTip _toolTip = new();

        private static readonly AcrTheme PurpleTheme = AcrTheme.Light with
        {
            Name = "Roxo",
            Primary = Color.FromArgb(111, 66, 193),
            PrimaryHover = Color.FromArgb(128, 88, 206),
            PrimaryPressed = Color.FromArgb(92, 52, 166),
            Info = Color.FromArgb(111, 66, 193),
            GridSelectionBack = Color.FromArgb(236, 228, 250),
            GridHoverRow = Color.FromArgb(246, 242, 253),
        };

        // ───────────────────────────── Tema ─────────────────────────────
        private AcrCard BuildThemeSection()
        {
            var card = NewSection("Tema — claro, escuro ou personalizado", 110);

            var btnLight = new AcrButton { Text = "☀  Claro", Variant = AcrButtonVariant.Secondary, Location = new Point(16, ContentTop + 4), Size = new Size(120, 32) };
            var btnDark = new AcrButton { Text = "☾  Escuro", Variant = AcrButtonVariant.Secondary, Location = new Point(146, ContentTop + 4), Size = new Size(120, 32) };
            var btnPurple = new AcrButton { Text = "◆  Roxo", Variant = AcrButtonVariant.Secondary, Location = new Point(276, ContentTop + 4), Size = new Size(120, 32) };
            var lblCode = new Label
            {
                AutoSize = true,
                Location = new Point(410, ContentTop + 12),
                ForeColor = AcrColors.TextMuted,
                Text = "AcrTheme.Current = AcrTheme.Light",
                Font = new Font("Consolas", 9F),
            };

            btnLight.Click += (_, _) => { AcrTheme.Current = AcrTheme.Light; lblCode.Text = "AcrTheme.Current = AcrTheme.Light"; };
            btnDark.Click += (_, _) => { AcrTheme.Current = AcrTheme.Dark; lblCode.Text = "AcrTheme.Current = AcrTheme.Dark"; };
            btnPurple.Click += (_, _) => { AcrTheme.Current = PurpleTheme; lblCode.Text = "AcrTheme.Light with { Primary = ... }"; };

            _toolTip.SetToolTip(btnDark, "Troca todas as janelas abertas para o tema escuro.");
            _toolTip.SetToolTip(btnPurple, "Tema personalizado criado com AcrTheme.Light with { ... }");

            card.Controls.AddRange(new Control[] { btnLight, btnDark, btnPurple, lblCode });
            return card;
        }

        // ─────────────────────────── Componentes novos ───────────────────────────
        private AcrCard BuildNewComponentsSection()
        {
            var card = NewSection("Componentes novos — DatePicker, NumericUpDown, TabControl, ToolTip e toast com ação", 380);

            Label Caption(string text, int x, int y) => new()
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                ForeColor = AcrColors.TextMuted,
                Font = AcrFonts.Get(8.25F, FontStyle.Bold),
            };

            int rowA = ContentTop + 4;
            var lblResult = NewResultLabel(16, rowA + 64, 690);

            // DatePicker
            card.Controls.Add(Caption("AcrDatePicker", 16, rowA));
            var datePicker = new AcrDatePicker { Location = new Point(16, rowA + 20), Width = 200 };
            datePicker.ValueChanged += (_, _) =>
                lblResult.Text = datePicker.Value.HasValue ? $"Data: {datePicker.Value:dddd, dd/MM/yyyy}" : "Data limpa.";
            _toolTip.SetToolTip(datePicker, "Clique ou pressione Enter. Delete limpa a data.");

            // NumericUpDown
            card.Controls.Add(Caption("AcrNumericUpDown", 236, rowA));
            var numeric = new AcrNumericUpDown
            {
                Location = new Point(236, rowA + 20),
                Width = 150,
                Minimum = 0,
                Maximum = 1000,
                DecimalPlaces = 2,
                Increment = 0.5m,
                Value = 10,
            };
            numeric.ValueChanged += (_, _) => lblResult.Text = $"Quantidade: {numeric.Value:N2}";
            _toolTip.SetToolTip(numeric, "Use a roda do mouse ou as setas ↑ ↓.");

            // Toast com ação
            card.Controls.Add(Caption("Toast com ação", 406, rowA));
            var btnDelete = new AcrButton { Text = "Excluir item", Variant = AcrButtonVariant.Danger, Location = new Point(406, rowA + 21), Size = new Size(130, 32) };
            btnDelete.Click += (_, _) =>
            {
                lblResult.Text = "Item excluído.";
                NotificationHelper.ShowWithAction("O item foi excluído.", "Desfazer",
                    () => lblResult.Text = "Exclusão desfeita! ↩", NotificationType.Warning, "Item excluído");
            };

            // ToolTip
            card.Controls.Add(Caption("AcrToolTip", 556, rowA));
            var btnTip = new AcrButton { Text = "Passe o mouse", Variant = AcrButtonVariant.Outline, Location = new Point(556, rowA + 21), Size = new Size(140, 32) };
            _toolTip.SetToolTip(btnTip, "Tooltips arredondados, com espaçamento e cores do tema.");

            // TabControl
            var tabs = new AcrTabControl
            {
                Location = new Point(16, rowA + 96),
                Size = new Size(SectionWidth - 32, 220),
            };
            string[][] pages =
            {
                new[] { "Visão geral", "O AcrTabControl é um TabControl comum com visual moderno: abas planas, sublinhado na aba ativa e hover." },
                new[] { "Detalhes", "Use-o no Designer como qualquer TabControl — adicione TabPages e controles normalmente." },
                new[] { "Configurações", "A cor do sublinhado segue o tema (AccentColor) e muda junto com AcrTheme.Current." },
            };
            foreach (var p in pages)
            {
                var page = new TabPage(p[0]) { BackColor = AcrColors.Surface, Padding = new Padding(12) };
                page.Controls.Add(new Label { Text = p[1], Dock = DockStyle.Fill, Font = AcrFonts.Get(9.5F) });
                tabs.TabPages.Add(page);
            }

            card.Controls.AddRange(new Control[] { datePicker, numeric, btnDelete, btnTip, lblResult, tabs });
            return card;
        }
    }
}
