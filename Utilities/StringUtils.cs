using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    public class StringUtils
    {
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string QuoteString(string inputString)
        {
            string str = inputString.Trim();
            if (str != "")
            {
                str = str.Replace("'", "''");
            }
            return str;
        }

        public static string AddSlash(string input)
        {
            string str = !string.IsNullOrEmpty(input) ? input.Trim() : "";
            if (str != "")
            {
                str = str.Replace("'", "'").Replace("\"", "\\\"");
            }
            return str;
        }

        public static string RefreshText(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text.Trim())) return text;

            text = HttpUtility.HtmlDecode(text);

            text = HttpUtility.UrlDecode(text);

            return text;
        }

        public static string RemoveStrHtmlTags(object inputObject)
        {
            if (inputObject == null)
            {
                return string.Empty;
            }
            string input = Convert.ToString(inputObject).Trim();
            if (input != "")
            {
                input = Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
            }
            return input;
        }

        public static string ReplaceSpaceToPlus(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Regex.Replace(input, @"\s+", "+", RegexOptions.IgnoreCase);
            }
            return input;
        }

        public static string ReplaceSpecialCharater(object inputObject)
        {
            if (inputObject == null)
            {
                return string.Empty;
            }
            return Convert.ToString(inputObject).Trim().Trim().Replace(@"\", @"\\").Replace("\"", "&quot;").Replace("“", "&ldquo;").Replace("”", "&rdquo;").Replace("‘", "&lsquo;").Replace("’", "&rsquo;").Replace("'", "&#39;");
        }

        public static string JavaScriptSring(string input)
        {
            input = input.Replace("'", @"\u0027");
            input = input.Replace("\"", @"\u0022");
            return input;
        }

        public static int CountWords(string stringInput)
        {
            if (string.IsNullOrEmpty(stringInput))
            {
                return 0;
            }
            stringInput = RemoveStrHtmlTags(stringInput);
            return Regex.Matches(stringInput, @"[\S]+").Count;
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            //Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        public static string StripHtml(string html)
        {
            return (string.IsNullOrEmpty(html) ? string.Empty : Regex.Replace(html, "<.*?>", string.Empty));
        }

        public static string TrimText(object strIn, int intLength)
        {
            try
            {
                string str = StripHtml(Convert.ToString(strIn));
                if (str.Length > intLength)
                {
                    str = str.Substring(0, intLength - 4);
                    return (str.Substring(0, str.LastIndexOfAny(new char[] { ' ', '.', '?', ',', '!' })) + " ...");
                }
                return str;
            }
            catch (Exception)
            {
                return Convert.ToString(strIn);
            }
        }

        public static string FormatNumber(string sNumber, string sperator = ".")
        {
            int num = 3;
            int num2 = 0;
            for (int i = 1; i <= (sNumber.Length / 3); i++)
            {
                if ((num + num2) < sNumber.Length)
                {
                    sNumber = sNumber.Insert((sNumber.Length - num) - num2, sperator);
                }
                num += 3;
                num2++;
            }
            return sNumber;
        }

        public static string FormatNumberWithComma(string sNumber)
        {
            int num = 3;
            int num2 = 0;
            for (int i = 1; i <= (sNumber.Length / 3); i++)
            {
                if ((num + num2) < sNumber.Length)
                {
                    sNumber = sNumber.Insert((sNumber.Length - num) - num2, ",");
                }
                num += 3;
                num2++;
            }
            return sNumber;
        }

        public static bool IsValidWord(string input, char character)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }
            string[] arr = input.Split(character);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length > 30)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string GetMetaDescription(string format, params object[] args)
        {
            if (String.IsNullOrEmpty(format)) return String.Empty;

            string strDes = format;

            if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                strDes = String.Format(strDes, args);
            }

            return strDes;
        }

        public static string ConvertNumberToCurrency(double number, string sperator = ".", string currentcy = "")
        {
            if (number <= 0)
            {
                return "0";
            }

            number = Math.Round(number, 0);

            string output = StringUtils.FormatNumber(number.ToString(CultureInfo.CurrentCulture), sperator) + currentcy;

            return output;
        }

        public static string ReplaceCaseInsensitive(string input, string[] search, string[] replacement)
        {
            int lenSearch = search.Length, lenRepalace = replacement.Length;
            string result = string.Empty;
            for (int i = 0; i < lenSearch; i++)
            {
                for (int j = 0; j < lenRepalace; j++)
                {
                    result = Regex.Replace(
                        input,
                        Regex.Escape(search[i]),
                        replacement[j].Replace("$", "$$"),
                        RegexOptions.IgnoreCase
                    ).Trim();
                    input = result;
                }
            }

            return result;
        }

        public static string GetStringTreeview(int level)
        {
            if (level == 0) return string.Empty;

            string strLevel = "";
            for (int i = 0; i < level; i++)
            {
                strLevel = strLevel + "__ ";
            }
            return strLevel;
        }

        #region Content Process

        public static string UploadImageIncontent(string content, out string firstImage, Func<string, string> uploadImage)
        {
            firstImage = String.Empty;

            string newContent = content;

            if (String.IsNullOrEmpty(newContent)) return newContent;

            try
            {

                string strRegex = @"<img.+?src=[\""'](?<SRC>.+?)[\""'].*?>";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                Match match = myRegex.Match(newContent);
                if (match.Success)
                {
                    newContent = myRegex.Replace(newContent, m => string.Format("<p style=\"text-align:center\"><img src=\"{0}\" /></p>", uploadImage.Invoke(m.Groups["SRC"].Value)));
                }


                foreach (Match matchAvatar in myRegex.Matches(newContent))
                {
                    if (matchAvatar.Success)
                    {
                        firstImage = matchAvatar.Groups["SRC"].Value;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Todo
            }
            return newContent;
        }

        public static string ConvertToMobileContent(string content, string domainImage, string resizeSize = "resize/480x-")
        {
            string newContent = content;

            try
            {
                newContent = StringUtils.StandardizedContent(newContent, domainImage, resizeSize);
            }
            catch (Exception ex)
            {
                // Todo log
            }

            return newContent;
        }

        private static string StandardizedContent(string content, string currentViewDomain, string sizeCropOnMobile)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            try
            {
                content = HttpUtility.HtmlDecode(content);
                //content = content.Replace(Const.ServerLinkImage, currentViewDomain);

                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*"))
                {
                    if (node.Name == "div")
                    {
                        var iframNode = node.SelectSingleNode(".//iframe");
                        if (node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("video-container"))
                        {
                            if (iframNode == null)
                                node.RemoveClass("video-container");
                        }
                        else if (!node.Attributes.Contains("class") || !node.Attributes["class"].Value.Contains("video-container"))
                        {
                            if (iframNode != null)
                                node.AddClass("video-container");
                        }
                    }
                    if (node.Name == "p")
                    {
                        var iframNode = node.SelectSingleNode(".//iframe");
                        if (node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("video-container"))
                        {
                            if (iframNode == null)
                                node.RemoveClass("video-container");
                            else
                                node.Name = "div";
                        }
                        if (!node.Attributes.Contains("class") || !node.Attributes["class"].Value.Contains("video-container"))
                        {
                            if (iframNode != null)
                            {
                                node.AddClass("video-container");
                                node.Name = "div";
                            }
                        }
                    }
                    if (node.Name == "table")
                    {
                        if (node.ParentNode.Name != "div")
                        {
                            var newChild = HtmlNode.CreateNode("<div class='divresponsive'>" + node.OuterHtml + "</div>");
                            node.ParentNode.ReplaceChild(newChild, node);
                        }
                        else if (!node.ParentNode.Attributes.Contains("class") || !node.ParentNode.Attributes["class"].Value.Contains("divresponsive"))
                        {
                            node.ParentNode.AddClass("divresponsive");
                        }
                    }
                    if (node.Name == "img")
                    {
                        var src = node.Attributes["src"].Value;
                        if (src.StartsWith(currentViewDomain))
                        {
                            string patternImageCroped = string.Concat(currentViewDomain.TrimEnd('/'), "/", @"(crop\/\d+x\d+/|resize\/\d+x[0-9-]+\/)?");
                            Regex regexImageCroped = new Regex(patternImageCroped, RegexOptions.None);
                            string newSrc = regexImageCroped.Replace(src, string.Empty);
                            newSrc = string.Format("{0}/{1}/{2}", currentViewDomain.TrimEnd('/'), sizeCropOnMobile, newSrc.TrimStart('/'));
                            node.Attributes["src"].Value = newSrc;
                        }
                    }
                }
                content = doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return content;
        }


        #endregion

        public static string AddAttributeForAnchors(string htmlContent, string domainTarget = "http://banxehoi.com/diendan/seolink/?refer=", bool isEncrypt = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return htmlContent;
            try
            {
                htmlContent = Regex.Replace(htmlContent, @"rel=[""']nofollow[""']", string.Empty);
                htmlContent = htmlContent.Replace(@"target=[""']_blank[""']", string.Empty);

                string strRegex = @"(?<LINK><a[^>]href=[""'](?<url>[^""']+)[""'](?<attrs>[^>]*)>(?<Content>((?!<\/a>).)*)<\/a>)";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match match = myRegex.Match(htmlContent);
                //string strReplace = @"<a href=""" + domainTarget + @"${url}"" ${attrs} rel=""nofollow"" target=""_blank"">${Content}</a>";

                if (match.Success)
                {
                    htmlContent = myRegex.Replace(htmlContent, delegate (Match m)
                    {
                        string url = domainTarget + m.Groups["url"].Value;
                        string attrs = m.Groups["attrs"].Value;
                        string content = m.Groups["Content"].Value;
                        string link = string.Format(@"<a href=""{0}"" {1} rel=""nofollow"" target=""_blank"">{2}</a>", url, attrs,
                            content);
                        link = Regex.Replace(link, @"\s+", " ");
                        return link;
                    });
                    //htmlContent = myRegex.Replace(htmlContent, strReplace);
                }

            }
            catch
            {
                // Todo something
            }
            return htmlContent;
        }

        #region Unicode Process

        public const string uniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        public const string unsignChar =
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

        public static string UnicodeToUnsignChar(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string retVal = string.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += unsignChar[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }

        public static string UnicodeToUnsignCharAndDash(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            const string strChar = "abcdefghijklmnopqrstxyzuvxw0123456789 -";
            //string retVal = UnicodeToKoDau(s);
            s = UnicodeToUnsignChar(s.ToLower().Trim());
            string sReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (strChar.IndexOf(s[i]) > -1)
                {
                    if (s[i] != ' ')
                        sReturn += s[i];
                    else if (i > 0 && s[i - 1] != ' ' && s[i - 1] != '-')
                        sReturn += "-";
                }
            }
            while (sReturn.IndexOf("--") != -1)
            {
                sReturn = sReturn.Replace("--", "-");
            }
            return sReturn;
        }
        public static string RemoveSpecial(string s)
        {
            //const string REGEX = @"([^\w\dàáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ\.,\-_ ]+)";
            //s = Regex.Replace(s, REGEX, string.Empty, RegexOptions.IgnoreCase);

            return Regex.Replace(s, "[`~!@#$%^&*()_|+-=?;:'\"<>{}[]\\/]", string.Empty); //edited by vinhph

        }
        public static string RemoveSpecial4ModelDetail(string s)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                result = Regex.Replace(s, "[+*%/^&:]", string.Empty, RegexOptions.IgnoreCase);
            }
            return result;
        }
        public static string ReplaceSpecial4ModelDetail(string s)
        {
            string result = string.Empty;
            result = Regex.Replace(s, "plus", "+", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "star", "*", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "per", "%", RegexOptions.IgnoreCase);
            return result;
        }
        public static string UnicodeToKoDauAndSpace(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += unsignChar[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }
        /// <summary>
        /// loại bỏ các ký tự không phải chữ, số, dấu cách thành ký tự không dấu
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharToKhongDau(string s)
        {
            string retVal = UnicodeToUnsignChar(s);
            Regex regex = new Regex(@"[^\d\w]+");
            retVal = regex.Replace(retVal, " ");
            while (retVal.IndexOf("  ") != -1)
            {
                retVal = retVal.Replace("  ", " ");
            }
            return retVal;
        }

        #endregion

        public static string SubStringAlias(string urlAlias, string alias)
        {
            int strLenght = (alias + "-").Length;
            if (urlAlias.Length > strLenght)
                return urlAlias.Substring(strLenght, urlAlias.Length - strLenght);
            return null;
        }

        public static string RemoveHTMLTag(string htmlString)
        {
            string pattern = @"(<[^>]+>)";
            string text = Regex.Replace(htmlString, pattern, string.Empty);
            return text;
        }
        public static string PlainText(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return RemoveHTMLTag(input).Replace("\"", string.Empty).Replace("'", string.Empty).Trim();
            return string.Empty;
        }
    }
}
