using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities
{
    public class SEO
    {
        private static SEO _instance;
        private static object syncLock = new object();

        public static SEO Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (syncLock)
                    {
                        if (null == _instance) _instance = new SEO();
                    }
                }
                return _instance;
            }
        }

        public string BindingMeta(string standardUrl, string title, string description, string keyword = "")
        {
            if (string.IsNullOrEmpty(title))
            {
                title = AppSettings.Instance.GetString(Const.MetaMainTitle);
            }
            if (string.IsNullOrEmpty(description))
            {
                description = AppSettings.Instance.GetString(Const.MetaMainDescription);
            }
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = AppSettings.Instance.GetString(Const.MetaMainKeyword);
            }

            title = StringUtils.RemoveStrHtmlTags(title);
            description = StringUtils.RemoveStrHtmlTags(description);
            keyword = StringUtils.RemoveStrHtmlTags(keyword);

            title = StringUtils.ReplaceSpecialCharater(title);
            description = StringUtils.ReplaceSpecialCharater(description);
            keyword = HttpUtility.HtmlDecode(keyword);

            string metaOgTitle = string.Format("<meta property=\"og:title\" content=\"{0}\" /> \r\n", title);
            string metaOgDesc = string.Format("<meta property=\"og:description\" content=\"{0}\" /> \r\n", description);
            string metaDesc = string.Format("<meta name=\"description\" content=\"{0}\" /> \r\n", description);
            string metaKeyword = string.Format("<meta name=\"keywords\" content=\"{0}\" />", keyword);

            string metaCanonical = string.Concat("\r\n", BindingLinkTags("canonical", Const.BaseUrlNoSlash + standardUrl));
            metaCanonical = string.Concat(metaCanonical, "\r\n", BindingLinkTags("alternate", Const.BaseUrlNoSlash + standardUrl, "only screen and (max-width: 640px)"));
            string metaAlternate = string.Concat("\r\n", BindingLinkTags("alternate", Const.BaseUrlNoSlash + standardUrl, "handheld"));

            string meta = string.Format("{0}{1}{2}{3}{4}{5}", metaOgTitle, metaOgDesc, metaDesc, metaKeyword, metaCanonical, metaAlternate);

            return meta;
        }

        public string BindingLinkTags(string rel, string href, string media = "")
        {
            if (string.IsNullOrEmpty(rel) || string.IsNullOrEmpty(href)) return string.Empty;
            Dictionary<string, string> dicts = new Dictionary<string, string>();
            dicts.Add("rel", rel);
            dicts.Add("href", href);
            if (!string.IsNullOrEmpty(media))
            {
                dicts.Add("media", media);
            }
            return AddMeta("link", dicts);
        }

        public static string AddMetaFacebook(string title, string type, string description, string url, string image, string domainAndCropSize = "", int imgWidth = 620, int imgHeight = 324)
        {
            description = description.Trim() == string.Empty ? " " : StringUtils.PlainText(description);
            title = title.Trim() == string.Empty ? " " : StringUtils.PlainText(title);
            type = type.Trim() == string.Empty ? " " : type;
            url = url.Trim() == string.Empty ? HttpContext.Current.Request.RawUrl : url;
            StringBuilder metaFormat = new StringBuilder();
            metaFormat.AppendFormat("<meta property=\"og:site_name\" content=\"{0}\" />", Const.Domain);
            metaFormat.AppendFormat("<meta property=\"og:title\" content=\"{0}\" />", title);
            metaFormat.AppendFormat("<meta property=\"og:type\" content=\"{0}\" />", type);
            metaFormat.AppendFormat("<meta property=\"og:description\" content=\"{0}\" />", description);
            metaFormat.AppendFormat("<meta property=\"og:url\" content=\"{0}\" />", url);
            if (!string.IsNullOrEmpty(image))
            {
                metaFormat.AppendFormat("<meta property=\"og:image\" content=\"{0}\" />", domainAndCropSize + image);
                metaFormat.Append("<meta property=\"og:image:type\" content=\"image/jpg\" />");
                metaFormat.AppendFormat("<meta property=\"og:image:width\" content=\"{0}\" />", imgWidth);
                metaFormat.AppendFormat("<meta property=\"og:image:height\" content=\"{0}\" />", imgHeight);
            }
            return metaFormat.ToString();
        }

        public static string MetaPagination(int totalCount, int pageIndex, int pageSize, string link)
        {
            string strMeta = string.Empty;
            int pageNum = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageIndex > 1)
                strMeta = string.Format("<link rel=\"prev\" href=\"{0}{1}/p{2}\" />", Const.BaseUrlNoSlash, link, (pageIndex - 1));
            if (pageIndex < pageNum)
                strMeta = string.Format("{0}<link rel=\"next\" href=\"{1}{2}/p{3}\" />", strMeta, Const.BaseUrlNoSlash, link, (pageIndex + 1));
            return strMeta;
        }

        public static string AddMeta(string tagName, string name, string value, Dictionary<string, string> attributeds = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) return string.Empty;

            string valueName = "content";
            if (name.ToLower().Contains("robots"))
                valueName = "rel";
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Format("<{0} name=\"{1}\" {2}=\"{3}\" />", tagName, name, valueName, value);
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            else
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" />", name, valueName, value);
            }
            return result;
        }

        public static string AddMeta(string name, string value, Dictionary<string, string> attributeds = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) return string.Empty;

            string valueName = "content";
            if (name.ToLower().Contains("robots"))
                valueName = "rel";
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" ", name, valueName, value);
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            else
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" />", name, valueName, value);
            }
            return result;
        }

        public static string AddMeta(string tagName, Dictionary<string, string> attributeds)
        {
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Concat("<", tagName, " ");
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            return result;
        }

        public static string AddMeta(Dictionary<string, string> attributeds)
        {
            string result = string.Empty;
            if (attributeds != null)
            {
                result = "<meta";
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            return result;
        }

        public static string GenarateTitle(string objectTitle)
        {
            if (string.IsNullOrEmpty(objectTitle)) return objectTitle;

            objectTitle = StringUtils.RemoveStrHtmlTags(objectTitle);

            return objectTitle;
        }
    }
}
