using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomDataGridView;

public class AcrDataGridView : DataGridView, IAcrBaseControl
{
    private EControlState _controlState = EControlState.Normal;

    public AcrDataGridView()
    {
        BorderStyle = BorderStyle.None;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        GridColor = AcrColors.GridGridLines;
        BackgroundColor = Color.White;

        EnableHeadersVisualStyles = false;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        ColumnHeadersDefaultCellStyle.BackColor = AcrColors.GridHeaderBack;
        ColumnHeadersDefaultCellStyle.ForeColor = AcrColors.GridHeaderFore;
        ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        ColumnHeadersDefaultCellStyle.SelectionBackColor = AcrColors.GridHeaderBack;
        ColumnHeadersDefaultCellStyle.SelectionForeColor = AcrColors.GridHeaderFore;
        ColumnHeadersDefaultCellStyle.Padding = new Padding(4);

        RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        RowHeadersVisible = false;

        DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        DefaultCellStyle.ForeColor = AcrColors.Text;
        DefaultCellStyle.SelectionBackColor = AcrColors.GridSelectionBack;
        DefaultCellStyle.SelectionForeColor = AcrColors.GridSelectionFore;
        DefaultCellStyle.Padding = new Padding(2);

        AlternatingRowsDefaultCellStyle.BackColor = AcrColors.GridAltRow;

        RowTemplate.Height = 28;
        ColumnHeadersHeight = 32;

        CellPainting += OnCellPainting;
    }

    [Category("Acr Custom")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public EControlState ControlState
    {
        get => _controlState;
        set
        {
            if (_controlState == value) return;
            _controlState = value;
            ApplyState();
        }
    }

    public void ApplyState()
    {
        switch (_controlState)
        {
            case EControlState.Normal:
                ReadOnly = false;
                Enabled = true; break;
            case EControlState.Disabled:
                Enabled = false; break;
            case EControlState.ReadOnly:
                ReadOnly = true; break;
            case EControlState.Edit:
                ReadOnly = false;
                Enabled = true; break;
        }
    }

    private void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex == -1 && e.ColumnIndex >= 0)
        {
            using var bottomBorder = new Pen(AcrColors.GridGridLines);
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);
            e.Graphics!.DrawLine(bottomBorder, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
            e.Handled = true;
        }
    }
}
