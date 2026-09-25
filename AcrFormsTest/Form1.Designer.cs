

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
            lblSenha = new Label();
            acrTextBoxSenha = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintSenha = new Label();
            lblValor = new Label();
            acrTextBoxValor = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            hintValor = new Label();
            btnValidarForm = new Button();
            btnToggleLock = new Button();
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
            acrTextBox2.ShowClearButton = true;
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
            acrTextBoxCpf.InputMask = "000.000.000-00";
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
            hintCpf.Text = "Máscara automática + dígito verificador real. Ex.: 12345678909";
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
            acrTextBoxCnpj.InputMask = "00.000.000/0000-00";
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
            hintCnpj.Text = "Máscara automática. Ex.: 11222333000181";
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
            acrTextBoxPhone.InputMask = "(00) 00000-0000";
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
            hintPhone.Text = "Máscara automática. Ex.: 11912345678";
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
            // lblSenha
            //
            lblSenha.AutoSize = true;
            lblSenha.Location = new Point(30, 422);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(200, 15);
            lblSenha.TabIndex = 20;
            lblSenha.Text = "Senha (modo password)";
            //
            // acrTextBoxSenha
            //
            acrTextBoxSenha.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxSenha.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBoxSenha.IsPasswordField = true;
            acrTextBoxSenha.LabelTitle = false;
            acrTextBoxSenha.LabelTitleText = "";
            acrTextBoxSenha.Location = new Point(30, 440);
            acrTextBoxSenha.Name = "acrTextBoxSenha";
            acrTextBoxSenha.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxSenha.OnLeaveBackColor = Color.White;
            acrTextBoxSenha.RequiredField = false;
            acrTextBoxSenha.SelectAllTextOnEnter = false;
            acrTextBoxSenha.Size = new Size(250, 23);
            acrTextBoxSenha.TabIndex = 21;
            acrTextBoxSenha.TabOnEnter = true;
            acrTextBoxSenha.ValidateAsDate = false;
            acrTextBoxSenha.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.None;
            acrTextBoxSenha.WarningMessageDate = "Invalid date format.";
            acrTextBoxSenha.WarningMessagePattern = "Invalid value.";
            //
            // hintSenha
            //
            hintSenha.AutoSize = true;
            hintSenha.ForeColor = Color.Gray;
            hintSenha.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintSenha.Location = new Point(300, 444);
            hintSenha.Name = "hintSenha";
            hintSenha.Size = new Size(360, 15);
            hintSenha.TabIndex = 22;
            hintSenha.Text = "Clique no ● à direita para mostrar/ocultar a senha";
            //
            // lblValor
            //
            lblValor.AutoSize = true;
            lblValor.Location = new Point(30, 473);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(200, 15);
            lblValor.TabIndex = 23;
            lblValor.Text = "Valor (formatação decimal)";
            //
            // acrTextBoxValor
            //
            acrTextBoxValor.DecimalPlaces = 2;
            acrTextBoxValor.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBoxValor.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.Decimal;
            acrTextBoxValor.LabelTitle = false;
            acrTextBoxValor.LabelTitleText = "";
            acrTextBoxValor.Location = new Point(30, 491);
            acrTextBoxValor.Name = "acrTextBoxValor";
            acrTextBoxValor.OnEnterBackColor = Color.AliceBlue;
            acrTextBoxValor.OnLeaveBackColor = Color.White;
            acrTextBoxValor.RequiredField = false;
            acrTextBoxValor.SelectAllTextOnEnter = false;
            acrTextBoxValor.Size = new Size(250, 23);
            acrTextBoxValor.TabIndex = 24;
            acrTextBoxValor.TabOnEnter = true;
            acrTextBoxValor.UseThousandsSeparator = true;
            acrTextBoxValor.ValidateAsDate = false;
            acrTextBoxValor.ValidationType = Acr.WindowsForms.Controls.Enums.TextValidationType.None;
            acrTextBoxValor.WarningMessageDate = "Invalid date format.";
            acrTextBoxValor.WarningMessagePattern = "Invalid value.";
            //
            // hintValor
            //
            hintValor.AutoSize = true;
            hintValor.ForeColor = Color.Gray;
            hintValor.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            hintValor.Location = new Point(300, 495);
            hintValor.Name = "hintValor";
            hintValor.Size = new Size(360, 15);
            hintValor.TabIndex = 25;
            hintValor.Text = "Digite 1234.5 e saia do campo → vira 1.234,50";
            //
            // btnValidarForm
            //
            btnValidarForm.Location = new Point(30, 530);
            btnValidarForm.Name = "btnValidarForm";
            btnValidarForm.Size = new Size(180, 30);
            btnValidarForm.TabIndex = 26;
            btnValidarForm.Text = "Validar Formulário";
            btnValidarForm.UseVisualStyleBackColor = true;
            btnValidarForm.Click += btnValidarForm_Click;
            //
            // btnToggleLock
            //
            btnToggleLock.Location = new Point(230, 530);
            btnToggleLock.Name = "btnToggleLock";
            btnToggleLock.Size = new Size(220, 30);
            btnToggleLock.TabIndex = 27;
            btnToggleLock.Text = "Bloquear Formulário";
            btnToggleLock.UseVisualStyleBackColor = true;
            btnToggleLock.Click += btnToggleLock_Click;
            //
            // lblResult
            //
            lblResult.AutoSize = true;
            lblResult.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResult.Location = new Point(30, 572);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(400, 15);
            lblResult.TabIndex = 28;
            lblResult.Text = "";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 610);
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
            Controls.Add(lblSenha);
            Controls.Add(acrTextBoxSenha);
            Controls.Add(hintSenha);
            Controls.Add(lblValor);
            Controls.Add(acrTextBoxValor);
            Controls.Add(hintValor);
            Controls.Add(btnValidarForm);
            Controls.Add(btnToggleLock);
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
        private Label lblSenha;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxSenha;
        private Label hintSenha;
        private Label lblValor;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBoxValor;
        private Label hintValor;
        private Button btnValidarForm;
        private Button btnToggleLock;
        private Label lblResult;
    }
}
