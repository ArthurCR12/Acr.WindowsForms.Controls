using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;

namespace AcrFormsTest
{
    public partial class Form1 : Form
    {
        private bool _formLocked = false;

        public Form1()
        {
            InitializeComponent();

            acrDataGridViewDemo.DataSource = GetSampleProdutos();
            cmbGridState.SelectedIndex = 0;
            cmbButtonState.SelectedIndex = 0;

            searchGridControl1.OnFormatGrid += FormatProdutosGrid;
            searchGridControl1.OnSearch += (s, term) =>
            {
                var produtos = GetSampleProdutos();
                searchGridControl1.DataSource = string.IsNullOrWhiteSpace(term)
                    ? produtos
                    : produtos
                        .Where(p => p.Nome.Contains(term, StringComparison.OrdinalIgnoreCase)
                                 || p.Categoria.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            };
            searchGridControl1.OnItemSelected += (s, item) =>
            {
                if (item is Produto p)
                    lblSelectedItem.Text = $"Selecionado: {p.Nome} ({p.Categoria}) — {p.Preco:C2}";
            };
            searchGridControl1.DataSource = GetSampleProdutos();
        }

        private static List<Produto> GetSampleProdutos() =>
        [
            new Produto { Id = 1, Nome = "Teclado Mecânico", Categoria = "Periféricos", Preco = 249.90m },
            new Produto { Id = 2, Nome = "Mouse Gamer", Categoria = "Periféricos", Preco = 129.90m },
            new Produto { Id = 3, Nome = "Monitor 24\"", Categoria = "Monitores", Preco = 899.00m },
            new Produto { Id = 4, Nome = "Cadeira Gamer", Categoria = "Móveis", Preco = 1199.00m },
            new Produto { Id = 5, Nome = "Headset Bluetooth", Categoria = "Áudio", Preco = 349.90m },
            new Produto { Id = 6, Nome = "Webcam Full HD", Categoria = "Periféricos", Preco = 199.90m },
            new Produto { Id = 7, Nome = "SSD 1TB", Categoria = "Armazenamento", Preco = 459.00m },
            new Produto { Id = 8, Nome = "Notebook 15\"", Categoria = "Computadores", Preco = 3899.00m },
        ];

        private static void FormatProdutosGrid(DataGridView grid)
        {
            if (grid.Columns["Id"] != null) grid.Columns["Id"].Visible = false;
            if (grid.Columns["Nome"] != null) grid.Columns["Nome"].HeaderText = "Produto";
            if (grid.Columns["Categoria"] != null) grid.Columns["Categoria"].HeaderText = "Categoria";
            if (grid.Columns["Preco"] != null)
            {
                grid.Columns["Preco"].HeaderText = "Preço";
                grid.Columns["Preco"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnValidarForm_Click(object sender, EventArgs e)
        {
            bool isValid = AcrValidationHelper.ValidateForm(tabPageTextBox, blockLeave: true);

            lblResult.Text = isValid
                ? "✔ Formulário válido!"
                : "✘ Existem campos obrigatórios pendentes.";
            lblResult.ForeColor = isValid ? Color.Green : Color.Red;
        }

        private void btnToggleLock_Click(object sender, EventArgs e)
        {
            _formLocked = !_formLocked;

            AcrStateHelper.ApplyState(tabPageTextBox, _formLocked ? EControlState.ReadOnly : EControlState.Edit);

            btnToggleLock.Text = _formLocked ? "Desbloquear Formulário" : "Bloquear Formulário";
        }

        private void btnValidarCombo_Click(object sender, EventArgs e)
        {
            AcrValidationHelper.ValidateRequired(acrComboBoxDemo, blockLeave: false);
        }

        private void cmbButtonState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbButtonState.SelectedItem is string text && Enum.TryParse<EControlState>(text, out var state))
                acrButtonDemo.ControlState = state;
        }

        private void cmbGridState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGridState.SelectedItem is string text && Enum.TryParse<EControlState>(text, out var state))
                acrDataGridViewDemo.ControlState = state;
        }

        private void btnNotifSuccess_Click(object sender, EventArgs e) =>
            NotificationHelper.Show("Operação realizada com sucesso!", NotificationType.Sucess);

        private void btnNotifError_Click(object sender, EventArgs e) =>
            NotificationHelper.Show("Ocorreu um erro ao processar a solicitação.", NotificationType.Error);

        private void btnNotifWarning_Click(object sender, EventArgs e) =>
            NotificationHelper.Show("Atenção: verifique os dados informados.", NotificationType.Warning);

        private void btnNotifInfo_Click(object sender, EventArgs e) =>
            NotificationHelper.Show("Esta é uma notificação informativa.", NotificationType.Info);

        private void acrPagination1_PageChanged(object? sender, int page) =>
            lblPaginationResult.Text = $"Página atual: {page} / {acrPagination1.PageCount}";
    }
}
