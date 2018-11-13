using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using System.Xml.Serialization;

namespace Utilities
{
    public static class BundleHelper
    {
        const string BundleConfigPath = "BundleConfigPath";
        const string BundleConfigCompressFunction = "BundleConfigCompressFunction";
        const string BundleConfigEncoding = "BundleConfigEncoding";
        const string BundleConfigFastDecode = "BundleConfigFastDecode";
        const string BundleConfigSpecialChars = "BundleConfigSpecialChars";

        public static string XmlPath { get; set; }

        public static ECMAScriptPacker.PackerEncoding Encoding { get; set; }
        public static bool FastDecode { get; set; }
        public static bool SpecialChars { get; set; }

        static BundleHelper()
        {
            string absolutePath = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).AbsolutePath;

            string xmlPath = !string.IsNullOrEmpty(AppSettings.Instance.GetString(BundleConfigPath))
                ? AppSettings.Instance.GetString(BundleConfigPath)
                : (absolutePath.Substring(0, absolutePath.IndexOf("/bin") + 1));

            XmlPath = string.Concat(xmlPath, "Config/BundleConfigs.xml");

            Encoding = (ECMAScriptPacker.PackerEncoding)AppSettings.Instance.GetInt32(BundleConfigEncoding, (int)ECMAScriptPacker.PackerEncoding.HighAscii);
            FastDecode = AppSettings.Instance.GetBool(BundleConfigFastDecode, false);
            SpecialChars = AppSettings.Instance.GetBool(BundleConfigSpecialChars, true);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            CssFixRewriteUrlTransform rewriteUrlTransform = new CssFixRewriteUrlTransform();

            BundleConfigRoot bundleConfig = GetXmlContentFromPath<BundleConfigRoot>();

            if (bundleConfig != null)
            {
                foreach (BundleConfigItem item in bundleConfig.styles)
                {
                    StyleBundle styleBundle = new StyleBundle(item.name);
                    foreach (BundleConfigItemConfig style in item.items)
                    {
                        if (style.css_fix_rewrite_url)
                            styleBundle.Include(style.path.Trim(), rewriteUrlTransform);
                        else
                            styleBundle.Include(style.path.Trim());
                    }
                    bundles.Add(styleBundle);
                }

                foreach (BundleConfigItem item in bundleConfig.scripts)
                {
                    ScriptBundle scriptBundle = new ScriptBundle(item.name);
                    foreach (BundleConfigItemConfig script in item.items)
                    {
                        if (script.css_fix_rewrite_url)
                            scriptBundle.Include(script.path.Trim(), rewriteUrlTransform);
                        else
                            scriptBundle.Include(script.path.Trim());
                    }
                    bundles.Add(scriptBundle);
                }
            }
        }


        public static T GetXmlContentFromPath<T>()
        {
            string path = XmlPath;

            if (string.IsNullOrEmpty(path)) return default(T);

            if (!File.Exists(path)) return default(T);

            using (StreamReader reader = new StreamReader(path))
            {
                string xmlContent = reader.ReadToEnd();

                return XmlDeserializeFromString<T>(xmlContent);
            }
        }

        public static T GetXmlContentFromPath<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return default(T);

            if (!File.Exists(path)) return default(T);

            using (StreamReader reader = new StreamReader(path))
            {
                string xmlContent = reader.ReadToEnd();

                return XmlDeserializeFromString<T>(xmlContent);
            }
        }

        private static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        private static object XmlDeserializeFromString(this string objectData, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        #region models

        [XmlRoot("bundle")]
        public class BundleConfigRoot
        {
            [XmlAttribute("name")]
            public string name { get; set; }
            [XmlArray("styles"), XmlArrayItem("style")]
            public BundleConfigItem[] styles { get; set; }
            [XmlArray("scripts"), XmlArrayItem("script")]
            public BundleConfigItem[] scripts { get; set; }
        }

        public class BundleConfigItem
        {
            [XmlAttribute("name")]
            public string name { get; set; }
            [XmlArray("items"), XmlArrayItem("item")]
            public BundleConfigItemConfig[] items { get; set; }
        }

        public class BundleConfigItemConfig
        {
            [XmlElement]
            public string path { get; set; }
            [XmlElement]
            public bool css_fix_rewrite_url { get; set; }
        }

        #endregion

        #region fix rewriteUrl

        public class CssFixRewriteUrlTransform : IItemTransform
        {

            private static string ConvertUrlsToAbsolute(string baseUrl, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }
                var regex = new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)");
                return regex.Replace(content, match => string.Concat("url(", RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value), ")"));
            }

            public string Process(string includedVirtualPath, string input)
            {
                if (includedVirtualPath == null)
                {
                    throw new ArgumentNullException("includedVirtualPath");
                }
                var directory = VirtualPathUtility.GetDirectory(includedVirtualPath);

                string content = ConvertUrlsToAbsolute(directory, input);

                string newContent = content;

                if (includedVirtualPath.GetExtensionFile().Equals(".js"))
                {

                    ECMAScriptPacker p = new ECMAScriptPacker(Encoding, FastDecode, SpecialChars);
                    newContent = p.Pack(newContent).Replace("\n", "\r\n");
                }
                return newContent;
            }

            private static string RebaseUrlToAbsolute(string baseUrl, string url)
            {
                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl) || url.StartsWith("/", StringComparison.OrdinalIgnoreCase)
                    ||
                    url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    return url;
                }
                if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = string.Concat(baseUrl, "/");
                }
                return VirtualPathUtility.ToAbsolute(string.Concat(baseUrl, url));
            }
        }

        #endregion
    }
}
