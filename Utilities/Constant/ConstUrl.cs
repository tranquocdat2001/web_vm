using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class ConstUrl
    {
        public const string ArticleUrl = "/tin-tuc";
        public const string ArticlePagingUrl = "/tin-tuc/p{0}";
        public const string ArticleCateUrl = "/{0}";
        public const string ArticleCatePagingUrl = "/{0}/p{1}";
        public const string ArticleDetailFormatUrl = "/tin-tuc/{0}-ar{1}";
        public const string ArticleDetailAMPUrl = "/{0}/{1}-ar{2}/amp";

        public const string ProductUrl = "/san-pham";
        public const string ProductDetailFormatUrl = "/{0}/{1}-id{2}";
    }
}
