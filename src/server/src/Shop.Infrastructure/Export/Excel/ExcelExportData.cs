using System.Collections.Generic;

namespace Shop.Infrastructure.Export.Excel;

public class ExcelExportData<T> where T : new()
{
    /// <summary>
    /// 表格标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// sheet名称
    /// </summary>
    public string SheetName { get; set; }

    /// <summary>
    /// 数据起始行(含标题行)，从0开始
    /// </summary>
    public int StartRowIndex { get; set; } = 0;

    /// <summary>
    /// 是否在第一行显示标题
    /// </summary>
    public bool IsShowTitle { get; set; } = false;

    /// <summary>
    /// 是否显示表头行
    /// </summary>
    public bool IsShowHeader { get; set; } = true;

    /// <summary>
    /// 导出数据
    /// </summary>
    public IReadOnlyCollection<T> DataItems { get; set; } = new List<T>();

}

//public class ExcelExportItem
//{
//    public virtual string Name { get; set; }

//    public virtual string Value { get; set; }

//}