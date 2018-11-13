using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace PublicModel
{
    public class ProductModel
    {
        public ProductModel(ProductEntity item, CategoryEntity category)
        {
            this.ProductId = item.ProductID;
            this.CateId = item.CatalogID;
            this.Name = item.Name;
            this.Price = item.Price;
            this.PriceFormat = string.Format("{0:n0} ₫", item.Price).Replace(",", ".");
            this.Discount = item.Discount;
            this.SaleOffPrice = item.SaleOffPrice;
            this.Code = item.Code;
            this.Avatar = item.Avatar;
            if (item.Weight.ToInt(0) > 0)
                this.Weight = string.Format("{0:n0}{1}", item.Weight, item.UnitType == 0 ? "g" : "kg");
            this.UnitType = item.UnitType;
            this.CreatedDate = item.CreatedDate;
            this.LastModifiedDate = item.LastModifiedDate;

            this.URL = BuildLink.BuildURLForProduct(category.DisplayUrl, item.Name, item.ProductID);
        }

        public int ProductId { get; set; }
        public int CateId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int SaleOffPrice { get; set; }
        public string Code { get; set; }
        public string Avatar { get; set; }
        public string Weight { get; set; }
        public int UnitType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string URL { get; set; }
        public string PriceFormat { get; set; }
        public string GetAvatar(string crop)
        {
            return BuildLink.CropImage(this.Avatar, crop);
        }
    }

    public class ProductModelBox
    {
        public string TitleBox { get; set; }
        public string UrlBox { get; set; }
        public List<ProductModel> ProductList { get; set; }
    }

    public class ProductListModel
    {
        public ProductListModel()
        {
            PagingInfo = new Pagings();
            SearchInfo = new SearchProduct();
        }
        public string TitlePage { get; set; }
        public List<ProductModel> ProductList { get; set; }
        public Pagings PagingInfo { get; set; }
        public SearchProduct SearchInfo { get; set; }
    }

    public class ProductModelDetail
    {
        public ProductModelDetail()
        {

        }
        public ProductModelDetail(ProductEntity item, CategoryEntity category)
        {
            this.ProductId = item.ProductID;
            this.CateId = item.CatalogID;
            this.Name = item.Name;
            this.Price = item.Price;
            this.PriceFormat = string.Format("{0:n0} ₫", item.Price).Replace(",", ".");
            this.Discount = item.Discount;
            this.SaleOffPrice = item.SaleOffPrice;
            this.Code = item.Code;
            this.Avatar = item.Avatar;
            if (item.Weight.ToInt(0) > 0)
                this.Weight = string.Format("{0:n0}{1}", item.Weight, item.UnitType == 0 ? "g" : "kg");
            this.UnitType = item.UnitType;
            this.ShortDescription = item.ShortDescription;
            this.Description = item.Description;
            this.CreatedDate = item.CreatedDate;
            this.LastModifiedDate = item.LastModifiedDate;
            this.Status = item.Status;
            this.URL = BuildLink.BuildURLForProduct(category.DisplayUrl, item.Name, item.ProductID);
            ListImage = new List<ProductImage>();
            ListImage.Add(new ProductImage { Link = item.Avatar });
            //ListImage.Add(new ProductImage { Link = item.Avatar });
            //ListImage.Add(new ProductImage { Link = item.Avatar });
            //ListImage.Add(new ProductImage { Link = item.Avatar });
            //ListImage.Add(new ProductImage { Link = item.Avatar });
            //ListImage.Add(new ProductImage { Link = item.Avatar });
        }

        public int ProductId { get; set; }
        public int CateId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int SaleOffPrice { get; set; }
        public string Code { get; set; }
        public string Avatar { get; set; }
        public string Weight { get; set; }
        public int UnitType { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Status { get; set; }

        public string URL { get; set; }
        public string PriceFormat { get; set; }
        public string GetAvatar(string crop)
        {
            return BuildLink.CropImage(this.Avatar, crop);
        }
        public List<ProductImage> ListImage { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
    }

    public class ProductImage
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}