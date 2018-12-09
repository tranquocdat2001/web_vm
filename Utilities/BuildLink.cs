using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class BuildLink
    {
        public static string CropImage(string urlImage, string sizeImage = "")
        {
            return BuildLinkImage(urlImage, "crop", sizeImage);
        }
        public static string ResizeImage(string urlImage, string sizeImage = "")
        {
            return BuildLinkImage(urlImage, "resize", sizeImage);
        }

        public static string BuildLinkImage(string urlImage, string type, string sizeImage = "")
        {
            if (string.IsNullOrEmpty(urlImage))
                urlImage = Const.NoImage;

            if (urlImage.StartsWith("http"))
                return urlImage;

            urlImage = urlImage.TrimStart('/');
            if (string.IsNullOrEmpty(sizeImage))
                return string.Format("{0}/{1}", Const.DomainImage, urlImage);

            return string.Format("{0}/{1}/{2}/{3}", Const.DomainImage, type, sizeImage, urlImage);
        }

        public static string BuildURL(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format)) return string.Empty;

            string outputUrl = format;

            if (!string.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                outputUrl = string.Format(outputUrl, args);
            }

            if (!outputUrl.StartsWith("http"))
            {
                if (!outputUrl.StartsWith("/"))
                {
                    outputUrl = string.Concat("/", outputUrl);
                }
            }

            return outputUrl;
        }
        public static string BuildURLForProduct(string cateUrl, string title, int productId)
        {
            title = StringUtils.UnicodeToUnsignCharAndDash(title).Trim('-');
            return BuildURL(ConstUrl.ProductDetailFormatUrl, new object[] { cateUrl, title, productId });
        }
        public static string BuildURLForArticle(string title, int articleId)
        {
            title = StringUtils.UnicodeToUnsignCharAndDash(title).Trim('-');
            return BuildURL(ConstUrl.ArticleDetailFormatUrl, new object[] { title, articleId });
        }

        public static string BuildLinkSeach(string textSearch)
        {
            return string.Format("/san-pham/k={0}", textSearch);
        }
    }
}
