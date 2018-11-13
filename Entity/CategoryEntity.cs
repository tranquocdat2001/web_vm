using System;

namespace Entity
{
    public class CategoryEntity
    {
        public Int32 CatalogID { get; set; }
        public string Name { get; set; }
        public string DisplayUrl { get; set; }
        public Int32 ParrentID { get; set; }
        public Int32 Status { get; set; }
        public Int32 Order { get; set; }
        public Boolean IsHidden { get; set; }
    }
}
