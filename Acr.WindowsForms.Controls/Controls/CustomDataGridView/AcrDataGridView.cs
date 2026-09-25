using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;

namespace Acr.WindowsForms.Controls.Controls.CustomDataGridView;

public class AcrDataGridView : DataGridView, IAcrBaseControl
{
    private EControlState _controlState = EControlState.Normal;
    private int _hoveredRow = -1;
    private bool _highlightHoverRow = true;
    private Color _hoverRowColor = Color.FromArgb(242, 247, 253);
    private string _emptyText = "Nenhum registro encontrado";

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

        DoubleBuffered = true;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AllowUserToResizeRows = false;

        CellPainting += OnCellPainting;
    }

    [Category("Acr Custom")]
    [Description("If true, the row under the mouse is highlighted.")]
    [Browsable(true)]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool HighlightHoverRow
    {
        get => _highlightHoverRow;
        set { _highlightHoverRow = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Background color of the row under the mouse.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverRowColor
    {
        get => _hoverRowColor;
        set { _hoverRowColor = value; Invalidate(); }
    }

    [Category("Acr Custom")]
    [Description("Text drawn in the middle of the grid when there are no rows. Empty = nothing.")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string EmptyText
    {
        get => _emptyText;
        set { _emptyText = value ?? string.Empty; Invalidate(); }
    }

    protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
    {
        base.OnCellMouseEnter(e);
        SetHoveredRow(e.RowIndex);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        SetHoveredRow(-1);
    }

    private void SetHoveredRow(int rowIndex)
    {
        if (!_highlightHoverRow || rowIndex == _hoveredRow) return;
        int old = _hoveredRow;
        _hoveredRow = rowIndex;
        if (old >= 0 && old < RowCount) InvalidateRow(old);
        if (rowIndex >= 0 && rowIndex < RowCount) InvalidateRow(rowIndex);
    }

    protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
    {
        base.OnCellFormatting(e);
        if (_highlightHoverRow && e.RowIndex == _hoveredRow && e.RowIndex >= 0 && e.CellStyle != null && !Rows[e.RowIndex].Selected)
            e.CellStyle.BackColor = _hoverRowColor;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (Rows.Count - (AllowUserToAddRows ? 1 : 0) <= 0 && !string.IsNullOrEmpty(_emptyText))
        {
            int top = ColumnHeadersVisible ? ColumnHeadersHeight : 0;
            var rect = new Rectangle(0, top, Width, Math.Max(0, Height - top));
            TextRenderer.DrawText(e.Graphics, _emptyText, DefaultCellStyle.Font ?? Font, rect, AcrColors.TextDisabled,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }
    }

    protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e) { base.OnRowsAdded(e); Invalidate(); }
    protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e) { base.OnRowsRemoved(e); Invalidate(); }

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
                Enabled = true;
                ReadOnly = false;
                break;
            case EControlState.Disabled:
                Enabled = false;
                ReadOnly = false;
                break;
            case EControlState.ReadOnly:
                Enabled = true;
                ReadOnly = true;
                break;
            case EControlState.Edit:
                Enabled = true;
                ReadOnly = false;
                break;
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
