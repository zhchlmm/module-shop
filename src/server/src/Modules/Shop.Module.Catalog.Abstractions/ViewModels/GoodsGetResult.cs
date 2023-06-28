using System;
using System.Collections.Generic;

namespace Shop.Module.Catalog.ViewModels
{
    public class GoodsGetResult
    {
        public int Id { get; set; }

        public int ParentGroupedProductId { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsAllowToOrder { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Sku { get; set; }

        public string Gtin { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public bool IsPublished { get; set; }

        public bool IsFeatured { get; set; }

        public int? MediaId { get; set; }

        public string MediaUrl { get; set; }

        public int? BrandId { get; set; }

        public string BrandName { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 备货期。取值范围:1-60;单位:天。
        /// </summary>
        public int? DeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets the order minimum quantity
        /// </summary>
        public int OrderMinimumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the order maximum quantity
        /// </summary>
        public int OrderMaximumQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this product is returnable (a customer is allowed to submit return request with this product)
        /// </summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        /// Gets or sets the weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the length
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Gets or sets the width
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public decimal Height { get; set; }

        public int? UnitId { get; set; }

        public string UnitName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? PublishedOn { get; set; }

        public bool StockTrackingIsEnabled { get; set; }

        public IList<int> CategoryIds { get; set; } = new List<int>();

        public IList<GoodsGetAttributeResult> Attributes { get; set; } = new List<GoodsGetAttributeResult>();

        public IList<ProductGetOptionResult> Options { get; set; } = new List<ProductGetOptionResult>();

        public IList<ProductGetVariationResult> Variations { get; set; } = new List<ProductGetVariationResult>();

        public IList<ProductGetMediaResult> ProductImages { get; set; } = new List<ProductGetMediaResult>();
        public IList<GoodsGetIssueResult> Issues { get; set; } = new List<GoodsGetIssueResult>()
        {
            new GoodsGetIssueResult (){ Id =1, Answer="自收到商品之日起24小时内，顾客可联系我们申请售后，我们将根据具体情况进行处理。", ProductId = 0, Question="1.如何申请退货？"},
            new GoodsGetIssueResult (){ Id =1, Answer="如需开具发票，请在付款后联系我们，并提供单位抬头等相关信息。", ProductId = 0, Question="2.如何开具发票？"},
            new GoodsGetIssueResult (){ Id =1, Answer="默认使用专车直配发货（个别商品使用其他快递），配送范围覆盖全郑州大部分地区。", ProductId = 0, Question="3.如何发货？"}
        };
    }
    public class GoodsGetAttributeResult
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class GoodsGetIssueResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DisplayOrder { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
