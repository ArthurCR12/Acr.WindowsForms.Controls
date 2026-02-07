

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
            button1 = new Button();
            acrTextBox1 = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            acrTextBox2 = new Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(375, 132);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // acrTextBox1
            // 
            acrTextBox1.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBox1.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBox1.LabelTitle = false;
            acrTextBox1.LabelTitleText = "";
            acrTextBox1.Location = new Point(40, 107);
            acrTextBox1.Name = "acrTextBox1";
            acrTextBox1.OnEnterBackColor = Color.AliceBlue;
            acrTextBox1.OnLeaveBackColor = Color.White;
            acrTextBox1.RequiredField = true;
            acrTextBox1.SelectAllTextOnEnter = false;
            acrTextBox1.Size = new Size(206, 23);
            acrTextBox1.TabIndex = 2;
            acrTextBox1.TabOnEnter = true;
            acrTextBox1.ValidateAsDate = false;
            acrTextBox1.WarningMessageDate = "Invalid date format.";
            acrTextBox1.WarningMessageRequiredField = "Id Produto";
            // 
            // acrTextBox2
            // 
            acrTextBox2.ForeColor = Color.FromArgb(50, 50, 50);
            acrTextBox2.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acrTextBox2.LabelTitle = false;
            acrTextBox2.LabelTitleText = "";
            acrTextBox2.Location = new Point(40, 146);
            acrTextBox2.Name = "acrTextBox2";
            acrTextBox2.OnEnterBackColor = Color.AliceBlue;
            acrTextBox2.OnLeaveBackColor = Color.White;
            acrTextBox2.RequiredField = true;
            acrTextBox2.SelectAllTextOnEnter = false;
            acrTextBox2.Size = new Size(206, 23);
            acrTextBox2.TabIndex = 2;
            acrTextBox2.TabOnEnter = true;
            acrTextBox2.ValidateAsDate = false;
            acrTextBox2.WarningMessageDate = "Invalid date format.";
            acrTextBox2.WarningMessageRequiredField = "Descrição";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(acrTextBox2);
            Controls.Add(acrTextBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBox1;
        private Acr.WindowsForms.Controls.Controls.CustomTextBox.AcrTextBox acrTextBox2;
    }
}
