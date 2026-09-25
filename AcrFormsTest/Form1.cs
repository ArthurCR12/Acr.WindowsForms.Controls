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
        }

        private void btnValidarForm_Click(object sender, EventArgs e)
        {
            bool isValid = AcrValidationHelper.ValidateForm(this, blockLeave: true);

            lblResult.Text = isValid
                ? "✔ Formulário válido!"
                : "✘ Existem campos obrigatórios pendentes.";
            lblResult.ForeColor = isValid ? Color.Green : Color.Red;
        }

        private void btnToggleLock_Click(object sender, EventArgs e)
        {
            _formLocked = !_formLocked;

            AcrStateHelper.ApplyState(this, _formLocked ? EControlState.ReadOnly : EControlState.Edit);

            btnToggleLock.Text = _formLocked ? "Desbloquear Formulário" : "Bloquear Formulário";
        }
    }
}
