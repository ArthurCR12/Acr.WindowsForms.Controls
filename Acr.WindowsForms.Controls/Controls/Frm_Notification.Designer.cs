namespace Acr.WindowsForms.Controls.Controls
{
    partial class Frm_Notification
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lbl_Message = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            picB_Icon = new PictureBox();
            btn_Close = new Button();
            ((System.ComponentModel.ISupportInitialize)picB_Icon).BeginInit();
            SuspendLayout();
            // 
            // lbl_Message
            // 
            lbl_Message.AutoEllipsis = true;
            lbl_Message.Font = new Font("Cascadia Mono SemiBold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Message.Location = new Point(101, 9);
            lbl_Message.Name = "lbl_Message";
            lbl_Message.Size = new Size(247, 92);
            lbl_Message.TabIndex = 0;
            lbl_Message.Text = "label1";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // picB_Icon
            // 
            picB_Icon.Location = new Point(12, 23);
            picB_Icon.Name = "picB_Icon";
            picB_Icon.Size = new Size(67, 65);
            picB_Icon.SizeMode = PictureBoxSizeMode.StretchImage;
            picB_Icon.TabIndex = 2;
            picB_Icon.TabStop = false;
            // 
            // btn_Close
            // 
            btn_Close.FlatAppearance.BorderSize = 0;
            btn_Close.FlatStyle = FlatStyle.Flat;
            btn_Close.Image = Properties.Resources.cancel_32_Material;
            btn_Close.Location = new Point(354, 34);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new Size(37, 43);
            btn_Close.TabIndex = 1;
            btn_Close.UseVisualStyleBackColor = true;
            btn_Close.Click += btn_Close_Click;
            // 
            // Frm_Notification
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(403, 110);
            Controls.Add(picB_Icon);
            Controls.Add(btn_Close);
            Controls.Add(lbl_Message);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_Notification";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)picB_Icon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lbl_Message;
        private System.Windows.Forms.Timer timer1;
        private PictureBox picB_Icon;
        private Button btn_Close;
    }
}