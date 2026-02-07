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
            acr_TextBox1 = new Acr.WindowsForms.Controls.Controls.Acr_TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // acr_TextBox1
            // 
            acr_TextBox1.ForeColor = Color.FromArgb(50, 50, 50);
            acr_TextBox1.InputType = Acr.WindowsForms.Controls.Enums.TextboxtInputType.All;
            acr_TextBox1.LabelTitle = false;
            acr_TextBox1.LabelTitleText = "";
            acr_TextBox1.Location = new Point(12, 55);
            acr_TextBox1.Name = "acr_TextBox1";
            acr_TextBox1.OnEnterBackColor = Color.AliceBlue;
            acr_TextBox1.OnLeaveBackColor = Color.White;
            acr_TextBox1.RequiredField = true;
            acr_TextBox1.SelectAllTextOnEnter = false;
            acr_TextBox1.Size = new Size(100, 23);
            acr_TextBox1.TabIndex = 0;
            acr_TextBox1.TabOnEnter = true;
            acr_TextBox1.ValidateAsDate = false;
            acr_TextBox1.WarningMessageDate = "Invalid date format.";
            acr_TextBox1.WarningMessageRequiredField = "This field is required.";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(acr_TextBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Acr.WindowsForms.Controls.Controls.Acr_TextBox acr_TextBox1;
        private Button button1;
    }
}
