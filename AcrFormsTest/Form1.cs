using Acr.WindowsForms.Controls.Helpers;

namespace AcrFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnValidarForm_Click(object sender, EventArgs e)
        {
            bool isValid = AcrValidationHelper.ValidateForm(this, blockLeave: true);

            lblResult.Text = isValid
                ? "✔ Formulário válido!"
                : "✘ Existem campos obrigatórios pendentes.";
            lblResult.ForeColor = isValid ? Color.Green : Color.Red;
        }
    }
}
