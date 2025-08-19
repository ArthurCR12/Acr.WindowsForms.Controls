using System.ComponentModel;
using System.Runtime.Versioning;

namespace Acr.WindowsForms.Controls.Controls;

/// <summary>
/// Component for searching and selecting item in a DataGridView.
/// Permits filtering items based on search input with debounce functionality.
/// </summary>
[SupportedOSPlatform("windows")]
public partial class SearchGridControl : UserControl
{
    private System.Windows.Forms.Timer _debounceTimer;
    private int _debounceInterval = 300;


    public SearchGridControl()
    {
        InitializeComponent();

        _debounceTimer = new System.Windows.Forms.Timer();
        _debounceTimer.Interval = _debounceInterval;
        _debounceTimer.Tick += (s, e) =>
        {
            _debounceTimer.Stop();
            OnSearch?.Invoke(this, txt_Search.Text.Trim());
        };

        txt_Search.TextChanged += (s, e) =>
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        };

        dgv_Itens.CellDoubleClick += (s, e) =>
        {
            if (e.RowIndex >= 0)
            {
                var item = dgv_Itens.Rows[e.RowIndex].DataBoundItem;
                OnItemSelected?.Invoke(this, item);
            }
        };

        txt_Search.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && dgv_Itens.CurrentRow != null)
            {
                var item = dgv_Itens.CurrentRow.DataBoundItem;
                OnItemSelected?.Invoke(this, item);
                e.Handled = true;
            }
        };

    }

    [Browsable(true)]
    [Category("Acr Custom")]
    [Description("Wait time (ms) after typing to trigger the search.")]
    [DefaultValue(300)]
    public int DebounceInterval
    {
        get => _debounceTimer.Interval;
        set
        {
            var newVal = value > 0 ? value : 300;
            if (_debounceInterval == newVal) return;
            _debounceInterval = newVal;
            _debounceTimer.Interval = _debounceInterval;
        }
    }

    /// <summary>
    /// Data source for the DataGridView.
    /// When set, it will trigger the event <see cref="OnFormatGrid"/> to customize the grid view.
    /// </summary>
    [Browsable(false)]
    [DefaultValue(null)]
    public object DataSource
    {
        get => dgv_Itens.DataSource;
        set
        {
            dgv_Itens.DataSource = value;
            OnFormatGrid?.Invoke(dgv_Itens);
        }
    }

    /// <summary>
    /// Triggered when the DataGridView is formatted.
    /// </summary>
    // This event will NOT be visible in the Properties window
    [Browsable(false)]
    public event Action<DataGridView>? OnFormatGrid;

    /// <summary>
    /// Triggered when the user types in the search box. After the interval defined in <see cref="DebounceInterval"/>.
    /// </summary>    
    public event EventHandler<string>? OnSearch;

    /// <summary>
    /// Triggered when the user selects an item from the DataGridView.
    /// Cold be triggered by double-clicking an item or pressing Enter when an item is selected.
    /// </summary>
    public event EventHandler<object>? OnItemSelected;

    private void txt_Search_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && dgv_Itens.CurrentRow != null)
        {
            var item = dgv_Itens.CurrentRow.DataBoundItem;
            OnItemSelected?.Invoke(this, item);
            e.Handled = true;
        }
        if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
        {
            if (dgv_Itens.Rows.Count == 0) return;
            e.Handled = true;
            dgv_Itens.Focus();
            if (e.KeyCode == Keys.Down && dgv_Itens.CurrentRow!.Index < dgv_Itens.Rows.Count - 1)
                dgv_Itens.CurrentCell = dgv_Itens.Rows[dgv_Itens.CurrentRow.Index + 1].Cells[0];
            else if (e.KeyCode == Keys.Up && dgv_Itens.CurrentRow!.Index > 0)
                dgv_Itens.CurrentCell = dgv_Itens.Rows[dgv_Itens.CurrentRow.Index - 1].Cells[0];
        }
    }
    public void dgv_Itens_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Back)
        {
            txt_Search.Focus();
            txt_Search.Text = txt_Search.Text.Length > 0 ? txt_Search.Text.Substring(0, txt_Search.Text.Length - 1) : string.Empty;
            txt_Search.SelectionStart = txt_Search.Text.Length;
            e.Handled = true;
            return;
        }
        if (!char.IsControl(e.KeyChar))
        {
            txt_Search.Focus();
            txt_Search.Text += e.KeyChar;
            txt_Search.SelectionStart = txt_Search.Text.Length;
        }
    }
    private void dgv_Itens_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == -1 && dgv_Itens.Rows[e.RowIndex].Selected)
        {
            e.PaintBackground(e.ClipBounds, true);
            string arrow = "►";
            //string number = ($"A{e.RowIndex + 1}").ToString();
            using (Font font = new Font("Arial", 8, FontStyle.Bold))
            {
                SizeF textSize = e.Graphics.MeasureString(arrow, font);
                PointF location = new PointF(
                    e.CellBounds.Left + (e.CellBounds.Width - textSize.Width) / 2,
                    e.CellBounds.Top + (e.CellBounds.Height - textSize.Height) / 2);
                e.Graphics.DrawString(arrow, font, Brushes.Black, location);
            }
            e.Handled = true;
        }

    }
}
