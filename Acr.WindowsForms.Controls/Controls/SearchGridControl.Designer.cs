using Acr.WindowsForms.Controls.Controls.CustomTextBox;

namespace Acr.WindowsForms.Controls.Controls
{
    partial class SearchGridControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _debounceTimer?.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            txt_Search = new AcrTextBox();
            dgv_Itens = new DataGridView();
            lbl_NoResults = new Label();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgv_Itens).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // txt_Search
            // 
            txt_Search.Dock = DockStyle.Top;
            txt_Search.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            txt_Search.ForeColor = Color.FromArgb(50, 50, 50);
            txt_Search.InputType = Enums.TextboxtInputType.All;
            txt_Search.LabelTitle = true;
            txt_Search.LabelTitleText = "";
            txt_Search.Location = new Point(0, 0);
            txt_Search.Name = "txt_Search";
            txt_Search.OnEnterBackColor = Color.AliceBlue;
            txt_Search.OnLeaveBackColor = Color.White;
            txt_Search.SelectAllTextOnEnter = false;
            txt_Search.Size = new Size(350, 27);
            txt_Search.TabIndex = 0;
            txt_Search.TabOnEnter = true;
            txt_Search.ValidateAsDate = false;
            txt_Search.WarningMessageDate = "Invalid date format.";
            txt_Search.KeyDown += txt_Search_KeyDown;
            // 
            // dgv_Itens
            // 
            dgv_Itens.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_Itens.Dock = DockStyle.Fill;
            dgv_Itens.Location = new Point(0, 27);
            dgv_Itens.MultiSelect = false;
            dgv_Itens.Name = "dgv_Itens";
            dgv_Itens.ReadOnly = true;
            dgv_Itens.RowHeadersWidth = 15;
            dgv_Itens.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Itens.ShowCellToolTips = false;
            dgv_Itens.Size = new Size(350, 86);
            dgv_Itens.TabIndex = 2;
            dgv_Itens.CellPainting += dgv_Itens_CellPainting;
            dgv_Itens.KeyPress += dgv_Itens_KeyPress;
            //
            // lbl_NoResults
            //
            lbl_NoResults.BackColor = Color.White;
            lbl_NoResults.Dock = DockStyle.Fill;
            lbl_NoResults.ForeColor = Color.Gray;
            lbl_NoResults.Location = new Point(0, 27);
            lbl_NoResults.Name = "lbl_NoResults";
            lbl_NoResults.Size = new Size(350, 86);
            lbl_NoResults.TabIndex = 3;
            lbl_NoResults.Text = "Nenhum resultado encontrado.";
            lbl_NoResults.TextAlign = ContentAlignment.MiddleCenter;
            lbl_NoResults.Visible = false;
            //
            // panel3
            //
            panel3.Controls.Add(lbl_NoResults);
            panel3.Controls.Add(dgv_Itens);
            panel3.Controls.Add(txt_Search);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(350, 113);
            panel3.TabIndex = 6;
            // 
            // SearchGridControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel3);
            Name = "SearchGridControl";
            Size = new Size(350, 113);
            ((System.ComponentModel.ISupportInitialize)dgv_Itens).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        public DataGridView dgv_Itens;
        public AcrTextBox txt_Search;
        private Label lbl_NoResults;
        private Panel panel3;
    }
}
