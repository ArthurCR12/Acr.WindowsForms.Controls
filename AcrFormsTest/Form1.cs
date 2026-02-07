namespace AcrFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (acr_TextBox1.RequiredField)
            {
                MessageBox.Show(acr_TextBox1.WarningMessageRequiredField);
            }
        }
    }
}
