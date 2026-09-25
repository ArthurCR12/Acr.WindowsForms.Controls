using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Controls.CustomAccordion;
using Acr.WindowsForms.Controls.Controls.CustomButton;
using Acr.WindowsForms.Controls.Controls.CustomCard;
using Acr.WindowsForms.Controls.Controls.CustomComboBox;
using Acr.WindowsForms.Controls.Controls.CustomDataGridView;
using Acr.WindowsForms.Controls.Controls.CustomDropdownMenu;
using Acr.WindowsForms.Controls.Controls.CustomModal;
using Acr.WindowsForms.Controls.Controls.CustomTextBox;
using Acr.WindowsForms.Controls.Enums;

namespace AcrFormsTest
{
    /// <summary>
    /// Aba "Novidades": demonstra as melhorias de Modal, Button, Notificações, TextBox,
    /// DataGrid, Card, DropdownMenu e Accordion. Montada por código para não mexer no Designer.
    /// </summary>
    public partial class Form1
    {
        private const int SectionLeft = 16;
        private const int SectionWidth = 730;
        private const int ContentTop = 44;

        private readonly AcrDropdownMenu _novidadesMenu = new();

        private void BuildNovidadesTab()
        {
            var page = new TabPage("✨ Novidades")
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(247, 248, 250),
                Padding = new Padding(0, 0, 0, 16),
            };

            int y = 16;
            y = AddSection(page, y, BuildModalSection());
            y = AddSection(page, y, BuildButtonSection());
            y = AddSection(page, y, BuildNotificationSection());
            y = AddSection(page, y, BuildTextBoxSection());
            y = AddSection(page, y, BuildGridSection());
            y = AddSection(page, y, BuildCardSection());
            y = AddSection(page, y, BuildDropdownSection());
            y = AddSection(page, y, BuildAccordionSection());

            // Espaço extra no fim da rolagem.
            page.Controls.Add(new Panel { Location = new Point(0, y), Size = new Size(1, 16) });

            tabControl1.TabPages.Insert(0, page);
            tabControl1.SelectedTab = page;
        }

        private static int AddSection(TabPage page, int y, AcrCard card)
        {
            card.Location = new Point(SectionLeft, y);
            card.Width = SectionWidth;
            page.Controls.Add(card);
            return y + card.Height + 16;
        }

        private static AcrCard NewSection(string title, int height) => new()
        {
            Title = title,
            Height = height,
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
        };

        private static Label NewResultLabel(int x, int y, int width = 400) => new()
        {
            AutoSize = false,
            Location = new Point(x, y),
            Size = new Size(width, 22),
            ForeColor = AcrColors.Neutral,
            TextAlign = ContentAlignment.MiddleLeft,
            Text = "—",
        };

        // ───────────────────────────── Modal ─────────────────────────────
        private AcrCard BuildModalSection()
        {
            var card = NewSection("Modal — fundo escurecido", 150);

            var chkDim = new CheckBox { Text = "Escurecer o resto da tela", Checked = true, AutoSize = true, Location = new Point(16, ContentTop + 4) };
            var lblOpacity = new Label { Text = "Opacidade (%):", AutoSize = true, Location = new Point(220, ContentTop + 6) };
            var numOpacity = new NumericUpDown { Minimum = 10, Maximum = 90, Increment = 5, Value = 45, Location = new Point(320, ContentTop + 2), Width = 60 };
            var lblResult = NewResultLabel(400, ContentTop + 4, 300);

            var btnInfo = new AcrButton { Text = "Informação", Location = new Point(16, ContentTop + 40), Size = new Size(120, 32) };
            var btnConfirm = new AcrButton { Text = "Confirmação", Location = new Point(146, ContentTop + 40), Size = new Size(120, 32), Variant = AcrButtonVariant.Danger };
            var btnCustom = new AcrButton { Text = "Personalizado", Location = new Point(276, ContentTop + 40), Size = new Size(130, 32), Variant = AcrButtonVariant.Outline };

            btnInfo.Click += (_, _) =>
            {
                var modal = new AcrModal("Informação", "O restante da tela fica escuro enquanto este modal está aberto.\nPressione Esc para fechar ou Enter para confirmar.")
                {
                    DimBackground = chkDim.Checked,
                    OverlayOpacity = (double)numOpacity.Value / 100,
                };
                using (modal)
                {
                    modal.SetButtons(("OK", DialogResult.OK, true));
                    modal.ShowModal(this);
                }
                lblResult.Text = "Modal de informação fechado.";
            };

            btnConfirm.Click += (_, _) =>
            {
                bool ok = AcrModal.ShowConfirm(this, "Tem certeza que deseja excluir este item?", dimBackground: chkDim.Checked);
                lblResult.Text = ok ? "Usuário confirmou." : "Usuário cancelou.";
            };

            btnCustom.Click += (_, _) =>
            {
                using var modal = new AcrModal("Selecionar categoria", string.Empty)
                {
                    DimBackground = chkDim.Checked,
                    OverlayOpacity = (double)numOpacity.Value / 100,
                    Size = new Size(420, 240),
                };
                var combo = new AcrComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Location = new Point(20, 10),
                    Size = new Size(340, 23),
                };
                combo.Items.AddRange(new object[] { "Periféricos", "Monitores", "Móveis", "Áudio" });
                combo.SelectedIndex = 0;
                modal.ContentPanel.Controls.Add(combo);
                modal.SetButtons(("Cancelar", DialogResult.Cancel, false), ("Salvar", DialogResult.OK, true));

                lblResult.Text = modal.ShowModal(this) == DialogResult.OK
                    ? $"Categoria salva: {combo.SelectedItem}"
                    : "Cancelado.";
            };

            card.Controls.AddRange(new Control[] { chkDim, lblOpacity, numOpacity, lblResult, btnInfo, btnConfirm, btnCustom });
            return card;
        }

        // ───────────────────────────── Button ─────────────────────────────
        private AcrCard BuildButtonSection()
        {
            var card = NewSection("Button — variantes e carregando", 150);

            var variants = new[]
            {
                AcrButtonVariant.Primary, AcrButtonVariant.Secondary, AcrButtonVariant.Outline,
                AcrButtonVariant.Ghost, AcrButtonVariant.Danger, AcrButtonVariant.Success,
            };

            int x = 16;
            foreach (var variant in variants)
            {
                card.Controls.Add(new AcrButton
                {
                    Text = variant.ToString(),
                    Variant = variant,
                    Location = new Point(x, ContentTop + 4),
                    Size = new Size(106, 32),
                });
                x += 116;
            }

            var btnLoading = new AcrButton
            {
                Text = "Salvar (clique)",
                LoadingText = "Salvando...",
                Location = new Point(16, ContentTop + 48),
                Size = new Size(150, 34),
            };
            btnLoading.Click += async (_, _) =>
            {
                btnLoading.IsLoading = true;
                await Task.Delay(2000);
                btnLoading.IsLoading = false;
                NotificationHelper.Success("Registro salvo!", "Button");
            };

            var btnDisabled = new AcrButton { Text = "Desabilitado", Enabled = false, Location = new Point(176, ContentTop + 48), Size = new Size(120, 34) };
            var btnRound = new AcrButton { Text = "Arredondado", CornerRadius = 17, Location = new Point(306, ContentTop + 48), Size = new Size(130, 34) };

            card.Controls.AddRange(new Control[] { btnLoading, btnDisabled, btnRound });
            return card;
        }

        // ─────────────────────────── Notificações ───────────────────────────
        private AcrCard BuildNotificationSection()
        {
            var card = NewSection("Notificações — título, duração, pausa ao passar o mouse", 140);

            var btnSuccess = new AcrButton { Text = "Sucesso", Variant = AcrButtonVariant.Success, Location = new Point(16, ContentTop + 4), Size = new Size(110, 32) };
            var btnError = new AcrButton { Text = "Erro", Variant = AcrButtonVariant.Danger, Location = new Point(136, ContentTop + 4), Size = new Size(110, 32) };
            var btnWarning = new AcrButton { Text = "Aviso", AccentColor = AcrColors.Warning, Location = new Point(256, ContentTop + 4), Size = new Size(110, 32) };
            var btnInfo = new AcrButton { Text = "Info (10s)", Location = new Point(376, ContentTop + 4), Size = new Size(110, 32) };
            var chkSound = new CheckBox { Text = "Tocar som", Checked = NotificationHelper.PlaySound, AutoSize = true, Location = new Point(500, ContentTop + 10) };

            var lblTip = new Label
            {
                AutoSize = true,
                Location = new Point(16, ContentTop + 48),
                ForeColor = AcrColors.Neutral,
                Text = "Passe o mouse sobre a notificação para pausar; clique na mensagem para fechar. A barra inferior mostra o tempo restante.",
            };

            chkSound.CheckedChanged += (_, _) => NotificationHelper.PlaySound = chkSound.Checked;
            btnSuccess.Click += (_, _) => NotificationHelper.Success("Operação realizada com sucesso!", "Tudo certo");
            btnError.Click += (_, _) => NotificationHelper.Error("Ocorreu um erro ao processar a solicitação.", "Falha");
            btnWarning.Click += (_, _) => NotificationHelper.Warning("Verifique os dados informados.", "Atenção");
            btnInfo.Click += (_, _) => NotificationHelper.Show("Esta notificação fica 10 segundos na tela.", NotificationType.Info, "Informação", 10000);

            card.Controls.AddRange(new Control[] { btnSuccess, btnError, btnWarning, btnInfo, chkSound, lblTip });
            return card;
        }

        // ───────────────────────────── TextBox ─────────────────────────────
        private AcrCard BuildTextBoxSection()
        {
            var card = NewSection("TextBox — novo visual (Outlined, Underline e Classic)", 260);

            const int colWidth = 220;
            int[] cols = { 16, 256, 496 };

            Label Caption(string text, int x, int y) => new()
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                ForeColor = AcrColors.Neutral,
                Font = new Font("Segoe UI", 8.25F, FontStyle.Bold),
            };

            AcrTextBox Field(string name, int x, int y, string placeholder, AcrTextBoxStyle style = AcrTextBoxStyle.Outlined) => new()
            {
                Name = name,
                Location = new Point(x, y),
                Width = colWidth,
                TextBoxStyle = style,
                PlaceholderText = placeholder,
                TabOnEnter = false,
            };

            // Linha 1: os três estilos
            int rowA = ContentTop + 4;
            card.Controls.Add(Caption("Outlined (padrão)", cols[0], rowA));
            card.Controls.Add(Caption("Underline", cols[1], rowA));
            card.Controls.Add(Caption("Classic", cols[2], rowA));
            card.Controls.Add(Field("txtNovOutlined", cols[0], rowA + 20, "Clique para ver o anel de foco"));
            card.Controls.Add(Field("txtNovUnderline", cols[1], rowA + 20, "Linha inferior", AcrTextBoxStyle.Underline));
            card.Controls.Add(Field("txtNovClassic", cols[2], rowA + 20, "Visual antigo", AcrTextBoxStyle.Classic));

            // Linha 2: validação, ícones e cores
            int rowB = ContentTop + 84;
            card.Controls.Add(Caption("Obrigatório (borda de erro)", cols[0], rowB));
            card.Controls.Add(Caption("Senha + botão limpar", cols[1], rowB));
            card.Controls.Add(Caption("Cores e cantos personalizados", cols[2], rowB));

            var txtRequired = Field("txtNovObrigatorio", cols[0], rowB + 20, "Campo obrigatório");
            txtRequired.RequiredField = true;
            txtRequired.WarningMessageRequiredField = "Preencha este campo!";

            var txtPassword = Field("txtNovSenha", cols[1], rowB + 20, "Digite a senha");
            txtPassword.IsPasswordField = true;
            txtPassword.ShowClearButton = true;

            var txtCustom = Field("txtNovCores", cols[2], rowB + 20, "Roxo no foco");
            txtCustom.BorderRadius = 16;
            txtCustom.BorderColor = Color.FromArgb(200, 190, 230);
            txtCustom.BorderHoverColor = Color.FromArgb(150, 120, 210);
            txtCustom.BorderFocusColor = Color.FromArgb(111, 66, 193);

            card.Controls.AddRange(new Control[] { txtRequired, txtPassword, txtCustom });

            // Linha 3: ação de validar
            int rowC = ContentTop + 166;
            var btnValidate = new AcrButton { Text = "Validar obrigatório", Location = new Point(cols[0], rowC), Size = new Size(150, 30) };
            var lblState = NewResultLabel(cols[0] + 160, rowC + 4, 400);
            btnValidate.Click += (_, _) =>
            {
                if (txtRequired.IsControlEmpty) txtRequired.ShowRequiredFieldError();
                else txtRequired.ClearError();
                lblState.Text = txtRequired.HasError ? "HasError = true (borda e anel vermelhos)" : "HasError = false";
            };

            card.Controls.AddRange(new Control[] { btnValidate, lblState });
            return card;
        }

        // ───────────────────────────── DataGrid ─────────────────────────────
        private AcrCard BuildGridSection()
        {
            var card = NewSection("DataGrid — linha em destaque no hover e texto de lista vazia", 300);

            var grid = new AcrDataGridView
            {
                Location = new Point(16, ContentTop + 44),
                Size = new Size(SectionWidth - 32, 200),
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                DataSource = GetSampleProdutos(),
            };
            grid.DataBindingComplete += (_, _) => FormatProdutosGrid(grid);

            var btnToggle = new AcrButton { Text = "Esvaziar", Variant = AcrButtonVariant.Secondary, Location = new Point(16, ContentTop + 4), Size = new Size(110, 30) };
            var chkHover = new CheckBox { Text = "Destacar linha no hover", Checked = true, AutoSize = true, Location = new Point(140, ContentTop + 10) };

            btnToggle.Click += (_, _) =>
            {
                bool empty = grid.RowCount == 0;
                grid.DataSource = empty ? GetSampleProdutos() : new List<Produto>();
                btnToggle.Text = empty ? "Esvaziar" : "Recarregar";
            };
            chkHover.CheckedChanged += (_, _) => grid.HighlightHoverRow = chkHover.Checked;

            card.Controls.AddRange(new Control[] { btnToggle, chkHover, grid });
            return card;
        }

        // ───────────────────────────── Card ─────────────────────────────
        private AcrCard BuildCardSection()
        {
            var card = NewSection("Card — faixa de destaque, hover e clique", 190);
            var lblResult = NewResultLabel(16, ContentTop + 110, 400);

            var cardClickable = new AcrCard
            {
                Title = "Card clicável",
                AccentColor = AcrColors.Primary,
                HoverEffect = true,
                BackColor = Color.White,
                Location = new Point(16, ContentTop + 4),
                Size = new Size(220, 100),
            };
            cardClickable.Controls.Add(new Label { Text = "Passe o mouse e clique.", AutoSize = true, Location = new Point(16, 44) });
            cardClickable.CardClick += (_, _) => lblResult.Text = $"CardClick às {DateTime.Now:HH:mm:ss}";

            var cardSuccess = new AcrCard
            {
                Title = "Vendas do mês",
                TitleColor = AcrColors.Success,
                AccentColor = AcrColors.Success,
                BorderWidth = 2,
                BorderColor = Color.FromArgb(200, 230, 210),
                Location = new Point(250, ContentTop + 4),
                Size = new Size(220, 100),
            };
            cardSuccess.Controls.Add(new Label { Text = "R$ 48.920,00", Font = new Font("Segoe UI", 14F, FontStyle.Bold), AutoSize = true, Location = new Point(16, 44) });

            var cardPlain = new AcrCard
            {
                Title = "Sem borda",
                BorderWidth = 0,
                CornerRadius = 16,
                BackColor = Color.FromArgb(235, 242, 252),
                Location = new Point(484, ContentTop + 4),
                Size = new Size(220, 100),
            };
            cardPlain.Controls.Add(new Label { Text = "BorderWidth = 0", AutoSize = true, BackColor = Color.Transparent, Location = new Point(16, 44) });

            card.Controls.AddRange(new Control[] { cardClickable, cardSuccess, cardPlain, lblResult });
            return card;
        }

        // ─────────────────────────── DropdownMenu ───────────────────────────
        private AcrCard BuildDropdownSection()
        {
            var card = NewSection("DropdownMenu — separadores, desabilitados, teclado e rolagem", 130);

            var btnActions = new AcrButton { Text = "Ações ▾", Location = new Point(16, ContentTop + 4), Size = new Size(160, 32) };
            var btnLong = new AcrButton { Text = "Lista longa ▾", Variant = AcrButtonVariant.Outline, Location = new Point(186, ContentTop + 4), Size = new Size(160, 32) };
            var lblResult = NewResultLabel(360, ContentTop + 8, 340);
            var lblTip = new Label
            {
                AutoSize = true,
                ForeColor = AcrColors.Neutral,
                Location = new Point(16, ContentTop + 48),
                Text = "Use ↑ ↓ Enter e Esc com o menu aberto. \"Arquivar\" está desabilitado.",
            };

            _novidadesMenu.ItemClicked += (_, e) => lblResult.Text = $"Selecionado: {e.Text} (índice {e.Index})";
            _novidadesMenu.Closed += (_, _) => { if (lblResult.Text == "—") lblResult.Text = "Menu fechado sem seleção."; };

            btnActions.Click += (_, _) =>
            {
                _novidadesMenu.Items.Clear();
                _novidadesMenu.Items.AddRange(new[] { "Editar", "Duplicar", AcrDropdownMenu.Separator, "Arquivar", AcrDropdownMenu.Separator, "Excluir" });
                _novidadesMenu.DisabledIndexes.Clear();
                _novidadesMenu.DisabledIndexes.Add(3);
                _novidadesMenu.MaxVisibleItems = 12;
                _novidadesMenu.ShowFor(btnActions);
            };

            btnLong.Click += (_, _) =>
            {
                _novidadesMenu.Items.Clear();
                _novidadesMenu.Items.AddRange(Enumerable.Range(1, 30).Select(i => $"Item {i:00}"));
                _novidadesMenu.DisabledIndexes.Clear();
                _novidadesMenu.MaxVisibleItems = 8;
                _novidadesMenu.ShowFor(btnLong);
            };

            card.Controls.AddRange(new Control[] { btnActions, btnLong, lblResult, lblTip });
            return card;
        }

        // ───────────────────────────── Accordion ─────────────────────────────
        private AcrCard BuildAccordionSection()
        {
            var card = NewSection("Accordion — animação e apenas um aberto por vez", 330);

            var flow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Location = new Point(16, ContentTop + 4),
                Size = new Size(SectionWidth - 32, 270),
                BackColor = Color.White,
            };

            string[] titles = { "O que é o AcrControls?", "Como instalar?", "Posso personalizar as cores?" };
            string[] texts =
            {
                "Uma biblioteca de controles Windows Forms modernos e personalizáveis.",
                "Pelo NuGet: dotnet add package AcrControls",
                "Sim! Quase todos os controles têm propriedades na categoria \"Acr Custom\".",
            };

            for (int i = 0; i < titles.Length; i++)
            {
                var accordion = new AcrAccordion
                {
                    HeaderText = titles[i],
                    Width = flow.Width - 8,
                    ExpandedContentHeight = 90,
                    Expanded = i == 0,
                    Margin = new Padding(0, 0, 0, 8),
                };
                accordion.ContentPanel.Controls.Add(new Label { Text = texts[i], Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9F) });
                accordion.CollapseSiblings = true;
                flow.Controls.Add(accordion);
            }

            card.Controls.Add(flow);
            return card;
        }
    }
}
