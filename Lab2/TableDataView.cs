using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

class TableDataView<TData>: ListView
where TData: class
{
    private TableOptions<TData> _tableOptions;
    private TData[] _dataItems;
    private IDisposable _unsubscribe;
    private Comparison<TData> _lastUsedComparison;
    private int _sortingColumnIndex = -1;
    private bool _sortingReversed = false;
    private int _defaultColumnWidth = -2;

    public int DefaultColumnWidth {
        get {
            return this._defaultColumnWidth;
        }
        set {
            if (value != this._defaultColumnWidth && value > 0 && value < 32768) {
                this._defaultColumnWidth = value;
                this.ResetHeader();
            }
        }
    }

    private TableDataView(): base() 
    { 
        this.View = View.Details;
        this.FullRowSelect = true;
    }

    // Automatically updated view
    public TableDataView(Observable<List<TData>> dataSource, TableOptions<TData> tableOptions): this()
    {
        this._tableOptions = tableOptions;
        this.ResetHeader();
        this._unsubscribe = dataSource.Subscribe((list) => {
            this._dataItems = list.ToArray();
            this.ResetRows();
        }, ASAP: true);
    }

    // Static view
    public TableDataView(List<TData> dataSource, TableOptions<TData> tableOptions): this()
    {
        this._tableOptions = tableOptions;
        this._dataItems = dataSource.ToArray();
        this.ResetHeader();
        this.ResetRows();
    }

    protected void ResetHeader()
    {
        if (this._tableOptions == null) return;
        this.Columns.Clear();
        int i = 0;
        foreach (Column<TData> column in this._tableOptions.Columns) {
            string columnHeaderText = (i == this._sortingColumnIndex) ? 
                column.Header + (this._sortingReversed ? " ▽" : " △") : 
                column.Header;
            this.Columns.Add(columnHeaderText, this._defaultColumnWidth, HorizontalAlignment.Left);
            i++;
        }
    }

    protected void UpdateHeader()
    {
        if (this._tableOptions == null) return;
        int i = 0;
        foreach (Column<TData> column in this._tableOptions.Columns) {
            if (i == this._sortingColumnIndex) {
                this.Columns[i].Text = column.Header + (this._sortingReversed ? " ▽" : " △");
            }
            else {
                this.Columns[i].Text = column.Header;
            }
            i++;
        }
    }

    protected void ResetRows()
    {
        if (this._dataItems == null || this._tableOptions == null) return;
        if (this._lastUsedComparison != null) {
            Array.Sort(this._dataItems, this._lastUsedComparison);
        } 
        this.Items.Clear();
        foreach (TData item in this._dataItems) {
            this.AddRow(item);
        }
    }

    private void AddRow(TData item)
    {
        string[] thisRowValues = new string[this._tableOptions.Columns.Length];
        int index = 0;
        foreach (Column<TData> column in this._tableOptions.Columns) {
            thisRowValues[index++] = column.GetValue(item);
        }
        this.Items.Add(new ListViewItem(thisRowValues));
    }

    protected override void OnColumnClick(ColumnClickEventArgs eventArgs)
    {
        base.OnColumnClick(eventArgs);

        int colIndex = eventArgs.Column;
        var column = this._tableOptions.Columns[colIndex];
        if (column == null || column.SortingCompare == null) return;

        if (column.SortingCompare == this._lastUsedComparison) {
            this._sortingReversed = true;
            this._lastUsedComparison = column.SortingCompare.Reversed();
        }
        else {
            this._sortingReversed = false;
            this._lastUsedComparison = column.SortingCompare;
        }

        this._sortingColumnIndex = colIndex;
        this.UpdateHeader();
        this.ResetRows();
    }
}
