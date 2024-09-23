using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Infrastructure.Export.Excel
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportHeaderAttribute : Attribute
    {
        /// <inheritdoc />
        public ExportHeaderAttribute(string displayName = null, int? columnIndex = null, bool isIgnore = false)
        {
            DisplayName = displayName;
            IsIgnore = isIgnore;
            ColumnIndex = columnIndex;

            //FontSize = fontSize;
            //Format = format;
            //IsBold = isBold;
            //IsAutoFit = isAutoFit;
            //AutoCenterColumn = autoCenterColumn;
            //Width = width;
            //if (fontColor != KnownColor.Empty)
            //{
            //    FontColor = Color.Parse(fontColor.ToString("G"));
            //}
        }

        /// <summary>
        ///     显示名称
        /// </summary>
        public string DisplayName { set; get; }

        ///// <summary>
        /////     字体大小
        ///// </summary>
        //public float FontSize { set; get; }

        /// <summary>
        /////     是否加粗
        ///// </summary>
        //public bool IsBold { set; get; }

        ///// <summary>
        /////     格式化（身份证'@'，日期'yyyy-MM-dd'、'yyyy-MM-dd HH:mm:ss'，数字'#,##0'）
        ///// </summary>
        //public string Format { get; set; }

        ///// <summary>
        /////     是否自适应
        ///// </summary>
        //public bool IsAutoFit { set; get; }

        ///// <summary>
        /////     自动居中
        ///// </summary>
        //public bool AutoCenterColumn { get; set; }

        /// <summary>
        ///     是否忽略
        /// </summary>
        public bool IsIgnore { get; set; }

        ///// <summary>
        /////     宽度
        ///// </summary>
        //public int Width { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Range(0, 65535)]
        public int? ColumnIndex { get; set; }

        ///// <summary>
        ///// 自动换行
        ///// </summary>
        //public bool WrapText { get; set; }

        ///// <summary>
        ///// Hidden
        ///// </summary>
        //public bool Hidden { get; set; }

        ///// <summary>
        ///// 字体颜色
        ///// </summary>
        //public Color? FontColor { get; set; }
    }
}
