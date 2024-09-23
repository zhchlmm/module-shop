using Aspose.Cells;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shop.Infrastructure.Export.Excel
{
    /// <summary>
    /// 基于Aspose.Cells的Excel导出类
    /// </summary>
    /// <typeparam name="T">导出数据类</typeparam>
    public class ExcelExporter<T> where T : new()
    {
        /// <summary>
        /// 导出数据
        /// </summary>
        private ExcelExportData<T> Data { get; init; }

        public ExcelExporter(ExcelExportData<T> data)
        {
            Data = data;
        }

        /// <summary>
        /// 保存WorkBook
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private Workbook SaveToWorkBook()
        {
            ColumnData[] properties = CheckDataProperties();

            var workBook = new Workbook();
            var firstSheet = workBook.Worksheets[0];
            if (!string.IsNullOrEmpty(Data.SheetName))
            {
                firstSheet.Name = Data.SheetName;
            }

            var cells = firstSheet.Cells;
            if (Data.IsShowTitle)
            {
                cells.Merge(0, 0, 1, properties.Length);

                var cell = cells[0, 0];
                cell.Value = Data.Title ?? "";

                var style = cell.GetStyle();
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.Font.Size = 24;
                style.Font.IsBold = true;
            }

            if (Data.IsShowHeader)
            {
                var headColumnIndex = 0;
                foreach (var property in properties)
                {
                    if (property.ColumnIndex < 0)
                    {
                        headColumnIndex = GetNextColumnIndex(properties, headColumnIndex);
                    }

                    cells[Data.StartRowIndex, headColumnIndex++].Value = property.HeaderName;
                }
            }

            for (var i = 0; i < Data.DataItems.Count; i++)
            {
                var dataColumnIndex = 0;
                foreach (var property in properties)
                {
                    var dataValue = property.Property.GetValue(Data.DataItems.ElementAt(i));
                    if (property.ColumnIndex < 0)
                    {
                        dataColumnIndex = GetNextColumnIndex(properties, dataColumnIndex);
                    }

                    cells[Data.StartRowIndex + 1 + i, dataColumnIndex++].Value = dataValue;
                }
            }

            return workBook;
        }

        private ColumnData[] CheckDataProperties()
        {
            var rowIndex = Data.IsShowTitle ? 1 : 0;
            if (Data.StartRowIndex <= rowIndex)
            {
                throw new ArgumentException($"StartRowIndex:{Data.StartRowIndex}必须大于{rowIndex}！");
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public)
                          .Select(p => new
                          {
                              Property = p,
                              ExportHeaderAttribute = p.GetCustomAttributes(false).OfType<ExportHeaderAttribute>().FirstOrDefault()
                          })
                          .Select(p => new ColumnData
                          {
                              Property = p.Property,
                              IsIgnore = p.ExportHeaderAttribute.IsIgnore,
                              HeaderName = string.IsNullOrEmpty(p.ExportHeaderAttribute.DisplayName) ? p.Property.Name : p.ExportHeaderAttribute.DisplayName,
                              ColumnIndex = p.ExportHeaderAttribute?.ColumnIndex ?? -1
                          })
                          .OrderBy(p => p.ColumnIndex)
                          .ToArray();

            if (!properties.Any())
            {
                throw new ArgumentException($"导出数据不存在Public属性！");
            }

            var columanIndexCount = properties.Where(p => p.ColumnIndex != -1).Select(p => p.ColumnIndex).Distinct().Count();
            if (properties.Length > 1 && properties.Length != columanIndexCount) // 如部分设置或者设置有重复值则抛出异常，如未设置或全部设置相同值，则按照Properties排序进行
            {
                throw new ArgumentException($"ColumnIndex不能设置相同值！");
            }

            return properties;
        }

        /// <summary>
        /// 检查默认ColumnIndex是否和已指定ColumnIndex冲突
        /// </summary>
        /// <param name="columnDatas"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private int GetNextColumnIndex(ColumnData[] columnDatas, int columnIndex)
        {
            var tempColumnIndex = columnIndex;
            var result = !Array.Exists(columnDatas, cd => cd.ColumnIndex == tempColumnIndex);
            if (result)
            {
                return tempColumnIndex;
            }

            tempColumnIndex++;
            return GetNextColumnIndex(columnDatas, tempColumnIndex);
        }

        public MemoryStream ExportToStream()
        {
            using var workBook = SaveToWorkBook();

            var stream = workBook.SaveToStream();
            return stream;
        }

        public void Export(string fileName)
        {
            using var workBook = SaveToWorkBook();
            workBook.Save(fileName);
        }

        public void Export(string fileName, SaveFormat saveFormat)
        {
            using var workBook = SaveToWorkBook();
            workBook.Save(fileName, saveFormat);
        }

        public void Export(string fileName, SaveOptions saveOptions)
        {
            using var workBook = SaveToWorkBook();
            workBook.Save(fileName, saveOptions);
        }

        public void Export(Stream stream, SaveOptions saveOptions)
        {
            using var workBook = SaveToWorkBook();
            workBook.Save(stream, saveOptions);
        }

        public void Export(Stream stream, SaveFormat saveFormat)
        {
            using var workBook = SaveToWorkBook();
            workBook.Save(stream, saveFormat);
        }
    }

    internal class ColumnData
    {
        public PropertyInfo Property { get; set; }
        public bool IsIgnore { get; set; }
        public string HeaderName { get; set; }
        public int ColumnIndex { get; set; }
    }
}
