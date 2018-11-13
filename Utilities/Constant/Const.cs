using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Const
    {
        public static string DomainImage = AppSettings.Instance.GetString("DomainImage");

        public static string Domain = AppSettings.Instance.GetString("Domain");

        public static string BaseUrl = AppSettings.Instance.GetString("BaseUrl");
        public static string BaseUrlNoSlash = BaseUrl.TrimEnd('/');

        public static string Hotline = AppSettings.Instance.GetString("Hotline");
        public static string Companny = AppSettings.Instance.GetString("Companny");
        public static string Address = AppSettings.Instance.GetString("Address");
        public static string MapPositionX = AppSettings.Instance.GetString("MapPositionX");
        public static string MapPositionY = AppSettings.Instance.GetString("MapPositionY");

        public static string NoImage = AppSettings.Instance.GetString("NoImage");

        public static int WeekCacheTime = AppSettings.Instance.GetInt32("WeekCacheTime");
        public static int LongCacheTime = AppSettings.Instance.GetInt32("LongCacheTime");
        public static int MediumCacheTime = AppSettings.Instance.GetInt32("MediumCacheTime");
        public static int ShortCacheTime = AppSettings.Instance.GetInt32("ShortCacheTime");

        public static int PageSizeProduct = AppSettings.Instance.GetInt32("PageSizeProduct", 12);
        public static int PageSizeArticle = AppSettings.Instance.GetInt32("PageSizeArticle", 8);

        public static string ArrangeProduct = "ArrangeProduct";
        public static string ArrangeArticle = "ArrangeArticle";

        #region Crop Image

        public static string FacebookAvatar = "620x324";
        public static string Size73x55 = "73x55";
        public static string Size102x102 = "102x102";
        public static string Size350x350 = "350x350";
        public static string Size600x600 = "600x600";
        public static string Size885x241 = "885x241";
        public static string Size885x400 = "885x400";

        #endregion

        #region Email

        public static string EmailSupport = AppSettings.Instance.GetString("EmailSupport");
        public static string EmailMaster = AppSettings.Instance.GetString("EmailMaster", "noreply.topbank.vn@gmail.com");
        public static string EmailNoReply = AppSettings.Instance.GetString("EmailNoReply", "noreply.topbank.vn@gmail.com");
        public static string EmailContact = AppSettings.Instance.GetString("Mail-Contact");
        public static string PassEmailNoReply = AppSettings.Instance.GetString("PassEmailNoReply", "fintech2016");

        #endregion

        #region Meta tag

        public static string PrefixTitle = "";
        public static string DomainTitle = AppSettings.Instance.GetString("DomainTitle");

        public static string MetaMainTitle = AppSettings.Instance.GetString("MetaMainTitle");
        public static string MetaMainDescription = AppSettings.Instance.GetString("MetaMainDescription");
        public static string MetaMainKeyword = AppSettings.Instance.GetString("MetaMainKeyword");

        public static string MetaProductTitle = AppSettings.Instance.GetString("MetaProductTitle");
        public static string MetaProductDescription = AppSettings.Instance.GetString("MetaProductDescription");
        public static string MetaProductKeyword = AppSettings.Instance.GetString("MetaProductKeyword");

        public static string MetaNewsTitle = AppSettings.Instance.GetString("MetaNewsTitle");
        public static string MetaNewsDescription = AppSettings.Instance.GetString("MetaNewsDescription");
        public static string MetaNewsKeyword = AppSettings.Instance.GetString("MetaNewsKeyword");

        public static string MetaAboutTitle = AppSettings.Instance.GetString("MetaAboutTitle");
        public static string MetaAboutDescription = AppSettings.Instance.GetString("MetaAboutDescription");

        #endregion
    }
}
