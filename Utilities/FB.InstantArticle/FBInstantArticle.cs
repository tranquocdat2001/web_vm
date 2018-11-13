using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.FB.InstantArticle
{
    public class FBInstantArticle
    {
        public static readonly bool FbPublished = AppSettings.Instance.GetBool("FbPublished", false);
        public static readonly bool FBDevelopmentMode = AppSettings.Instance.GetBool("FBDevelopmentMode", true);

        public static readonly string FbToken = AppSettings.Instance.GetString("FbToken", string.Empty);
        public static readonly string FbAppId = AppSettings.Instance.GetString("FbAppId", string.Empty);
        public static readonly string FbAppSecret = AppSettings.Instance.GetString("FbAppSecret", string.Empty);
        public static readonly string FbPageId = AppSettings.Instance.GetString("FbPageId", string.Empty);
        public static readonly string FbAPIVersion = AppSettings.Instance.GetString("FbAPIVersion", string.Empty);
        public static readonly string FbInstantArticleStyle = AppSettings.Instance.GetString("FbInstantArticleStyle", "default");

        public static readonly string SiteSlogun = AppSettings.Instance.GetString("SiteSlogun", string.Empty);
        public static readonly string DomainName = AppSettings.Instance.GetString("DomainName", string.Empty);


        public static ResponseData PublishArticle(FBInstantArticleModel articleInfo, string style, bool developmentMode = false)
        {
            ResponseData responseData = new ResponseData();

            if (articleInfo == null || articleInfo.Id <= 0)
            {
                responseData.Message = "Can not found this news";
                return responseData;
            }

            if (string.IsNullOrEmpty(articleInfo.Avatar))
            {
                responseData.Message = "This news has not avatar";
                return responseData;
            }

            try
            {
                if (string.IsNullOrEmpty(style))
                {
                    style = FbInstantArticleStyle;
                }

                var fbClient = new Facebook.FacebookClient(FbToken);
                fbClient.AppId = FbAppId;
                fbClient.AppSecret = FbAppSecret;

                Dictionary<string, object> fbParams = new Dictionary<string, object>();

                string newsContent = GenerateHtmlSource(articleInfo, style);

                responseData.Content = newsContent;

                if (!string.IsNullOrEmpty(responseData.Content))
                {
                    fbParams["access_token"] = FbToken;
                    fbParams["html_source"] = responseData.Content;
                    fbParams["published"] = FbPublished;
                    fbParams["development_mode"] = developmentMode;


                    var articleId = fbClient.Post(string.Format("/{0}/{1}/instant_articles", FbAPIVersion, FbPageId), fbParams);

                    if (articleId != null)
                    {
                        responseData.Data = articleId;
                        responseData.Success = true;
                        responseData.Message = "Update thành công: articleId = " + articleId;
                    }
                }
                else
                {
                    responseData.Message = "News content not allow null";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                responseData.Message = ex.ToString();
            }

            return responseData;
        }

        public static ResponseData DeleteArticle(string originalUrl)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                var fbClient = new Facebook.FacebookClient(FbToken)
                {
                    AppId = FbAppId,
                    AppSecret = FbAppSecret
                };
                //var fbParams = new Dictionary<string, object>{ ["fields"] = "instant_article" };

                Dictionary<string, object> fbParams = new Dictionary<string, object>();
                fbParams["fields"] = "instant_article";

                //get
                dynamic articleInfo = fbClient.Get("/" + FbAPIVersion + "/?id=" + originalUrl + "&fields=instant_article", fbParams);
                if (articleInfo != null && !string.IsNullOrEmpty(articleInfo.instant_article.id))
                {
                    //delete
                    var c = fbClient.Delete(string.Format("/{0}/{1}", FbAPIVersion, articleInfo.instant_article.id));
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return responseData;
        }

        public static ResponseData GetListArticle(int pageIndex, int pageSize)
        {
            ResponseData responseData = new ResponseData();

            var fbClient = new Facebook.FacebookClient(FbToken)
            {
                AppId = FbAppId,
                AppSecret = FbAppSecret
            };

            dynamic listArticles = fbClient.Get(string.Format("{0}/{1}/instant_articles?access_token={2}", FbAPIVersion, FbPageId, FbToken));

            if(listArticles != null)
            {
                responseData.Success = true;
                responseData.Data = listArticles;
            }

            return responseData;
        }

        #region private methods

        private static string GetFacebookToken()
        {
            var fb = new Facebook.FacebookClient();
            dynamic token = fb.Get("/" + FbAPIVersion + "/oauth/access_token", new
            {
                client_id = FbAppId,
                client_secret = FbAppSecret,
                grant_type = "client_credentials"
            });
            return token != null ? token.access_token : string.Empty;
        }

        public static string GenerateHtmlSource(FBInstantArticleModel articleInfo, string style)
        {
            StringBuilder strBuilder = new StringBuilder();

            string originalUrl = articleInfo.OriginalUrl;

            string title = StringUtils.RemoveStrHtmlTags(articleInfo.Title);
            string sapo = StringUtils.RemoveStrHtmlTags(articleInfo.Sapo);
            string author = articleInfo.Author;


            string avatar = articleInfo.Avatar;
            string captionAvatar = StringUtils.RemoveStrHtmlTags(articleInfo.AvatarCaption);

            string publishedDate = articleInfo.PublishedDate.ToString("yyyy-M-dTHH:mm");
            string publishedDateDisplay = articleInfo.PublishedDate.ToString("ddd, dd/MM/yyyy HH:mm");
            string modifiedDate = articleInfo.ModifiedDate.ToString("yyyy-M-dTHH:mm");
            string modifiedDateDisplay = articleInfo.ModifiedDate.ToString("ddd, dd/MM/yyyy HH:mm");
            string kicker = articleInfo.Kicker;

            string content = articleInfo.Content;

            #region Regex optimization content


            string splitReg = @"(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:p\s+[^>]*|p|div[^>]*)>(?:\s|\t|\n|\r|&nbsp;)*";
            string removeTagReg = @"(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:(?:p\s+|div|table|tbody|tr|th|td|ul|li|br|h1|h2|a|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*|p)>(?:\s|\t|\n|\r|&nbsp;)*|<\!--[\S\s]*-->";
            string styleTagReg = @"<\/?(?:(?:p\s+|em|u|b|strong|<i\/?>|font|span|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*)>";
            string trimReg = @"^(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+|(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+$";
            string photoReg = @"<img[^>]+>";
            string beginValidTag = @"^(?:\s|\t|\n|\r|&nbsp;)*<(?:div[^>]*|p\s+[^>]*|p|h\d[^>]*)>";
            string hasTag = @"<\/?[a-z0-9]+[^>]*>";

            string stand = @"^(?:(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:span|div|p|br)+[^>]*>(?:\s|\t|\n|\r|&nbsp;)*)+$";

            string[] paragraphs = Regex.Split(content, splitReg, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (paragraphs != null && paragraphs.Length > 0)
            {
                for (int i = 0; i < paragraphs.Length; i++)
                {
                    paragraphs[i] = Regex.Replace(paragraphs[i], trimReg, string.Empty).Trim();
                    Match match = Regex.Match(paragraphs[i], stand);
                    if (match.Success)
                    {
                        paragraphs[i] = string.Empty;
                    }
                    else
                    {
                        paragraphs[i] = Regex.Replace(paragraphs[i], removeTagReg, " ").Trim();
                        paragraphs[i] = Regex.Replace(paragraphs[i], styleTagReg, string.Empty).Trim();

                        if (!string.IsNullOrWhiteSpace(paragraphs[i]))
                        {
                            Match matchValidTag = Regex.Match(paragraphs[i], beginValidTag);
                            Match matchPhotoTag = Regex.Match(paragraphs[i], photoReg);
                            if (!matchValidTag.Success && !matchPhotoTag.Success)
                            {
                                paragraphs[i] = string.Format("<p>{0}</p>", paragraphs[i]);
                            }

                            //paragraphs[i] = Regex.Replace(paragraphs[i], photoReg, delegate (Match m)
                            //{
                            //    return "";
                            //});
                        }
                    }
                }

                content = string.Join("", paragraphs);
                content = Regex.Replace(content, trimReg, "");

            }

            content = Regex.Replace(content, @"<img.+?src=[\""'](?<SRC>.+?)[\""'].*?>", "<img src=\"${SRC}\" />");

            string patternReplaceEmptyTag = @"(?<empty>\<.?>(\s+)?</.?>)";
            string patternFindImage = @"(?<image>\<img.+?src=[\""'](?<SRC>.+?)[\""'].*?\>)";
            string replaceForImage = @"<figure data-feedback=""fb: likes, fb: comments"">${image}</figure>";

            content = Regex.Replace(content, @"<o:p></o:p>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = Regex.Replace(content, patternReplaceEmptyTag, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = Regex.Replace(content, patternFindImage, replaceForImage, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = Regex.Replace(content, ">(\\s+)<", ">\r\n<", RegexOptions.Singleline | RegexOptions.IgnoreCase);


            #endregion

            strBuilder.Append(@"<!doctype html>");
            strBuilder.Append(@"<html lang=""vi"" prefix=""op: http://media.facebook.com/op#"">");
            strBuilder.Append(@"<head>");
            strBuilder.Append(@"<meta charset=""utf-8"">");
            strBuilder.AppendFormat(@"<link rel=""canonical"" href=""{0}"">", originalUrl);
            strBuilder.AppendFormat(@"<title>{0}</title>", title);
            strBuilder.AppendFormat(@"<meta property=""fb:article_style"" content=""{0}"">", style);
            strBuilder.Append(@"</head>");
            strBuilder.Append(@"<body>");
            strBuilder.Append(@"<article>");

            strBuilder.Append(@"<header>");
            strBuilder.Append(@"<figure>");
            strBuilder.AppendFormat(@"<img src=""{0}"" />", avatar);
            if (!string.IsNullOrEmpty(captionAvatar))
            {
                strBuilder.AppendFormat(@"<figcaption>{0}</figcaption>", captionAvatar);
            }
            strBuilder.Append(@"</figure>");
            strBuilder.AppendFormat(@"<h1> {0} </h1>", title);
            strBuilder.AppendFormat(@"<h2> {0} </h2>", sapo);
            strBuilder.AppendFormat(@"<h3 class=""op-kicker"">{0}</h3>", kicker);
            strBuilder.Append(@"<address>");
            strBuilder.Append(author);
            strBuilder.Append(@"</address>");
            strBuilder.AppendFormat(@"<time class=""op-published"" dateTime=""{0}"">{1}</time>", publishedDate, publishedDateDisplay);
            strBuilder.AppendFormat(@"<time class=""op-modified"" dateTime=""{0}"">{1}</time>", modifiedDate, modifiedDateDisplay);
            strBuilder.Append(@"</header>");

            strBuilder.AppendFormat(@"{0}", content);

            strBuilder.Append(@"<footer>");
            strBuilder.Append(@"<aside>");
            strBuilder.Append(SiteSlogun);
            strBuilder.Append(@"</aside>");
            strBuilder.AppendFormat(@"<small>© {0}/small>", DomainName);
            strBuilder.Append(@"</footer>");
            strBuilder.Append(@"</article>");

            strBuilder.Append(@"</body>");
            strBuilder.Append(@"</html>");

            string newContent = strBuilder.ToString();

            //newContent = Regex.Replace(newContent, @"<p(\s[^>]*)?>(?<RPL>((?!</p>).)*<(div|figure)[^>]*>.*?)</p>", "${RPL}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //// Cách nữa: <p(\s[^>]*)?>(?<RPL>((?!</p>).)*<(div|figure)[^>]*>.*?)</p>

            //newContent = Regex.Replace(newContent, @"<div(\s[^>]*)?>(?<RPL>((?!</div>).)*<figure[^>]*>.*?)</div>", "${RPL}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //newContent = Regex.Replace(newContent, @"(<p(\s[^>]*)?>[\s\r\n]+(?<RPL><figure[^>].+?</figure>)[\s\r\n]+</p>)", "${RPL}", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //newContent = Regex.Replace(newContent, ">(\\s+)<", ">\r\n<", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return newContent;
        }

        #endregion
    }

}
