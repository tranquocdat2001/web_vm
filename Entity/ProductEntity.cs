using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    [Serializable]
    public class ProductEntity
    {
        public int ProductID { get; set; }
        public int CatalogID { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public int Status { get; set; }
    }
}
