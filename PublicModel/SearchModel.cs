using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicModel
{
    public class SearchModel
    {
    }
    public class SearchProduct
    {
        public int TypeProduct { get; set; }
        public int CateId { get; set; }
        public int CityId { get; set; }
        //public int PriceId { get; set; }
        public string TextSearch { get; set; }
        public int OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SearchArticle
    {
        public int CateId { get; set; }
        public int Type { get; set; }
        public string TextSearch { get; set; }
        public int OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
