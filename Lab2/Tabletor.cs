using System;
using System.Collections.Generic;

class TableOptions<T> 
{
    public Column<T>[] Columns { get; set; }
}

public class Column<T>
{
    public string Header { get; set; }
    public Func<T, string> GetValue { get; set; }
    public Comparison<T> SortingCompare { get; set; }
}
