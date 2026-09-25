

namespace AcrFormsTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblHeader = new Label();
            lblIdProduto = new Label();
            acrTextBox1 = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            lblDescricao = new Label();
            acrTextBox2 = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            lblEmail = new Label();
            acrTextBoxEmail = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintEmail = new Label();
            lblCpf = new Label();
            acrTextBoxCpf = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintCpf = new Label();
            lblCnpj = new Label();
            acrTextBoxCnpj = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintCnpj = new Label();
            lblPhone = new Label();
            acrTextBoxPhone = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintPhone = new Label();
            lblCustom = new Label();
            acrTextBoxCustom = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintCustom = new Label();
            btnValidarForm = new Button();
            lblResult = new Label();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHeader.Location = new Point(24, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(700, 40);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Teste dos controles Acr* — saia do campo (Tab/clique fora) para validar individualmente, ou clique em \"Validar Formulário\" para validar tudo de uma vez.";
            //
            // lblIdProduto
            //
            lblIdProduto.AutoSize = true;
            lblIdProduto.Location = new Point(30, 65);
            lblIdProduto.Name = "lblIdProduto";
            lblIdProduto.Size = new Size(150, 15);
            lblIdProduto.TabIndex = 1;
            lblIdProduto.Text = "Id Produto (obrigatório)";
            //
            // acrTextBox1
            //
            acrTextBox1.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBox1.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.Integer;
            acrTextBox1.LabelTitle = false;
            acrTextBox1.LabelTitleText = "";
            acrTextBox1.Location = new Point(30, 83);
            acrTextBox1.Name = "acrTextBox1";
            acrTextBox1.OnEnterBackColor = Color.AliceBlue;
            acrTextBox1.OnLeaveBackColor = Color.White;
            acrTextBox1.RequiredField = true;
            acrTextBox1.SelectAllTextOnEnter = false;
            acrTextBox1.Size = new Size(250, 23);
            acrTextBox1.TabIndex = 2;
            acrTextBox1.TabOnEnter = true;
            acrTextBox1.ValidateAsDate = false;
            acrTextBox1.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.None;
            acrTextBox1.WarningMessageDate = "Invalid date format.";
            acrTextBox1.WarningMessageRequiredField = "Id Produto é obrigatório";
            acrTextBox1.WarningMessagePattern = "Invalid value.";
            //
            // lblDescricao
            //
            lblDescricao.AutoSize = true;
            lblDescricao.Location = new Point(30, 116);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(150, 15);
            lblDescricao.TabIndex = 3;
            lblDescricao.Text = "Descrição (obrigatório)";
            //
            // acrTextBox2
            //
            acrTextBox2.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBox2.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBox2.LabelTitle = false;
            acrTextBox2.LabelTitleText = "";
            acrTextBox2.Location = new Point(30, 134);
            acrTextBox2.Name = "acrTextBox2";
            acrTextBox2.OnEnterBackColor = Color.AliceBlue;
            acrTextBox2.OnLeaveBackColor = Color.White;
            acrTextBox2.RequiredField = true;
            acrTextBox2.SelectAllTextOnEnter = false;
            acrTextBox2.Size = new Size(250, 23);
            acrTextBox2.TabIndex = 4;
            acrTextBox2.TabOnEnter = true;
            acrTextBox2.ValidateAsDate = false;
            acrTextBox2.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.None;
            acrTextBox2.WarningMessageDate = "Invalid date format.";
            acrTextBox2.WarningMessageRequiredField = "Descrição é obrigatória";
            acrTextBox2.WarningMessagePattern = "Invalid value.";
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 167);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(120, 15);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "E-mail";
            //
            // acrTextBoxEmail
            //
            acrTextBoxEmail.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxEmail.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxEmail.LabelTitle = false;
            acrTextBoxEmail.LabelTitleText = "";
            acrTextBoxEmail.Location = new Point(30, 185);
            acrTextBoxEmail.Name = "acrTextBoxEmail";
            acrTextBoxEmail.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxEmail.OnLeaveBackColor = Color.White;
            acrTextBoxEmail.RequiredField = false;
            acrTextBoxEmail.SelectAllTextOnEnter = false;
            acrTextBoxEmail.Size = new Size(250, 23);
            acrTextBoxEmail.TabIndex = 6;
            acrTextBoxEmail.TabOnEnter = true;
            acrTextBoxEmail.ValidateAsDate = false;
            acrTextBoxEmail.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.Email;
            acrTextBoxEmail.WarningMessageDate = "Invalid date format.";
            acrTextBoxEmail.WarningMessagePattern = "E-mail inválido";
            //
            // hintEmail
            //
            hintEmail.AutoSize = true;
            hintEmail.ForeColor = Color.Gray;
            hintEmail.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintEmail.Location = new Point(300, 189);
            hintEmail.Name = "hintEmail";
            hintEmail.Size = new Size(300, 15);
            hintEmail.TabIndex = 7;
            hintEmail.Text = "Ex.: usuario@dominio.com";
            //
            // lblCpf
            //
            lblCpf.AutoSize = true;
            lblCpf.Location = new Point(30, 218);
            lblCpf.Name = "lblCpf";
            lblCpf.Size = new Size(120, 15);
            lblCpf.TabIndex = 8;
            lblCpf.Text = "CPF";
            //
            // acrTextBoxCpf
            //
            acrTextBoxCpf.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxCpf.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxCpf.LabelTitle = false;
            acrTextBoxCpf.LabelTitleText = "";
            acrTextBoxCpf.Location = new Point(30, 236);
            acrTextBoxCpf.Name = "acrTextBoxCpf";
            acrTextBoxCpf.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxCpf.OnLeaveBackColor = Color.White;
            acrTextBoxCpf.RequiredField = false;
            acrTextBoxCpf.SelectAllTextOnEnter = false;
            acrTextBoxCpf.Size = new Size(250, 23);
            acrTextBoxCpf.TabIndex = 9;
            acrTextBoxCpf.TabOnEnter = true;
            acrTextBoxCpf.ValidateAsDate = false;
            acrTextBoxCpf.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.Cpf;
            acrTextBoxCpf.WarningMessageDate = "Invalid date format.";
            acrTextBoxCpf.WarningMessagePattern = "CPF inválido";
            //
            // hintCpf
            //
            hintCpf.AutoSize = true;
            hintCpf.ForeColor = Color.Gray;
            hintCpf.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintCpf.Location = new Point(300, 240);
            hintCpf.Name = "hintCpf";
            hintCpf.Size = new Size(360, 15);
            hintCpf.TabIndex = 10;
            hintCpf.Text = "Ex. válido: 123.456.789-09 (dígito verificador real)";
            //
            // lblCnpj
            //
            lblCnpj.AutoSize = true;
            lblCnpj.Location = new Point(30, 269);
            lblCnpj.Name = "lblCnpj";
            lblCnpj.Size = new Size(120, 15);
            lblCnpj.TabIndex = 11;
            lblCnpj.Text = "CNPJ";
            //
            // acrTextBoxCnpj
            //
            acrTextBoxCnpj.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxCnpj.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxCnpj.LabelTitle = false;
            acrTextBoxCnpj.LabelTitleText = "";
            acrTextBoxCnpj.Location = new Point(30, 287);
            acrTextBoxCnpj.Name = "acrTextBoxCnpj";
            acrTextBoxCnpj.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxCnpj.OnLeaveBackColor = Color.White;
            acrTextBoxCnpj.RequiredField = false;
            acrTextBoxCnpj.SelectAllTextOnEnter = false;
            acrTextBoxCnpj.Size = new Size(250, 23);
            acrTextBoxCnpj.TabIndex = 12;
            acrTextBoxCnpj.TabOnEnter = true;
            acrTextBoxCnpj.ValidateAsDate = false;
            acrTextBoxCnpj.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.Cnpj;
            acrTextBoxCnpj.WarningMessageDate = "Invalid date format.";
            acrTextBoxCnpj.WarningMessagePattern = "CNPJ inválido";
            //
            // hintCnpj
            //
            hintCnpj.AutoSize = true;
            hintCnpj.ForeColor = Color.Gray;
            hintCnpj.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintCnpj.Location = new Point(300, 291);
            hintCnpj.Name = "hintCnpj";
            hintCnpj.Size = new Size(360, 15);
            hintCnpj.TabIndex = 13;
            hintCnpj.Text = "Ex. válido: 11.222.333/0001-81";
            //
            // lblPhone
            //
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(30, 320);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(120, 15);
            lblPhone.TabIndex = 14;
            lblPhone.Text = "Telefone";
            //
            // acrTextBoxPhone
            //
            acrTextBoxPhone.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxPhone.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxPhone.LabelTitle = false;
            acrTextBoxPhone.LabelTitleText = "";
            acrTextBoxPhone.Location = new Point(30, 338);
            acrTextBoxPhone.Name = "acrTextBoxPhone";
            acrTextBoxPhone.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxPhone.OnLeaveBackColor = Color.White;
            acrTextBoxPhone.RequiredField = false;
            acrTextBoxPhone.SelectAllTextOnEnter = false;
            acrTextBoxPhone.Size = new Size(250, 23);
            acrTextBoxPhone.TabIndex = 15;
            acrTextBoxPhone.TabOnEnter = true;
            acrTextBoxPhone.ValidateAsDate = false;
            acrTextBoxPhone.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.Phone;
            acrTextBoxPhone.WarningMessageDate = "Invalid date format.";
            acrTextBoxPhone.WarningMessagePattern = "Telefone inválido";
            //
            // hintPhone
            //
            hintPhone.AutoSize = true;
            hintPhone.ForeColor = Color.Gray;
            hintPhone.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintPhone.Location = new Point(300, 342);
            hintPhone.Name = "hintPhone";
            hintPhone.Size = new Size(360, 15);
            hintPhone.TabIndex = 16;
            hintPhone.Text = "Ex. válido: (11) 91234-5678 (10 ou 11 dígitos)";
            //
            // lblCustom
            //
            lblCustom.AutoSize = true;
            lblCustom.Location = new Point(30, 371);
            lblCustom.Name = "lblCustom";
            lblCustom.Size = new Size(200, 15);
            lblCustom.TabIndex = 17;
            lblCustom.Text = "Placa (regex customizado)";
            //
            // acrTextBoxCustom
            //
            acrTextBoxCustom.CustomValidationPattern = "^[A-Z]{3}-\\d{4}$";
            acrTextBoxCustom.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxCustom.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxCustom.LabelTitle = false;
            acrTextBoxCustom.LabelTitleText = "";
            acrTextBoxCustom.Location = new Point(30, 389);
            acrTextBoxCustom.Name = "acrTextBoxCustom";
            acrTextBoxCustom.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxCustom.OnLeaveBackColor = Color.White;
            acrTextBoxCustom.RequiredField = false;
            acrTextBoxCustom.SelectAllTextOnEnter = false;
            acrTextBoxCustom.Size = new Size(250, 23);
            acrTextBoxCustom.TabIndex = 18;
            acrTextBoxCustom.TabOnEnter = true;
            acrTextBoxCustom.ValidateAsDate = false;
            acrTextBoxCustom.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.Custom;
            acrTextBoxCustom.WarningMessageDate = "Invalid date format.";
            acrTextBoxCustom.WarningMessagePattern = "Formato de placa inválido";
            //
            // hintCustom
            //
            hintCustom.AutoSize = true;
            hintCustom.ForeColor = Color.Gray;
            hintCustom.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintCustom.Location = new Point(300, 393);
            hintCustom.Name = "hintCustom";
            hintCustom.Size = new Size(360, 15);
            hintCustom.TabIndex = 19;
            hintCustom.Text = "Regex: ^[A-Z]{3}-\\d{4}$  —  ex.: ABC-1234";
            //
            // btnValidarForm
            //
            btnValidarForm.Location = new Point(30, 430);
            btnValidarForm.Name = "btnValidarForm";
            btnValidarForm.Size = new Size(180, 30);
            btnValidarForm.TabIndex = 20;
            btnValidarForm.Text = "Validar Formulário";
            btnValidarForm.UseVisualStyleBackColor = true;
            btnValidarForm.Click += btnValidarForm_Click;
            //
            // lblResult
            //
            lblResult.AutoSize = true;
            lblResult.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResult.Location = new Point(230, 437);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(400, 15);
            lblResult.TabIndex = 21;
            lblResult.Text = "";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 490);
            Controls.Add(lblHeader);
            Controls.Add(lblIdProduto);
            Controls.Add(acrTextBox1);
            Controls.Add(lblDescricao);
            Controls.Add(acrTextBox2);
            Controls.Add(lblEmail);
            Controls.Add(acrTextBoxEmail);
            Controls.Add(hintEmail);
            Controls.Add(lblCpf);
            Controls.Add(acrTextBoxCpf);
            Controls.Add(hintCpf);
            Controls.Add(lblCnpj);
            Controls.Add(acrTextBoxCnpj);
            Controls.Add(hintCnpj);
            Controls.Add(lblPhone);
            Controls.Add(acrTextBoxPhone);
            Controls.Add(hintPhone);
            Controls.Add(lblCustom);
            Controls.Add(acrTextBoxCustom);
            Controls.Add(hintCustom);
            Controls.Add(btnValidarForm);
            Controls.Add(lblResult);
            Name = "Form1";
            Text = "AcrControls - Teste de Validação";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Label lblIdProduto;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBox1;
        private Label lblDescricao;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBox2;
        private Label lblEmail;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxEmail;
        private Label hintEmail;
        private Label lblCpf;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxCpf;
        private Label hintCpf;
        private Label lblCnpj;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxCnpj;
        private Label hintCnpj;
        private Label lblPhone;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxPhone;
        private Label hintPhone;
        private Label lblCustom;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxCustom;
        private Label hintCustom;
        private Button btnValidarForm;
        private Label lblResult;
    }
}
