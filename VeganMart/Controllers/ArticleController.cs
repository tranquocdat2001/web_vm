using BO.Article;
using BO.Category;
using BO.ParserLink;
using Entity;
using PublicModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utilities;

namespace VeganMart.Controllers
{
    public class ArticleController : BaseController
    {
        private IArticleBo _articleBo;
        private ICategoryBo _categoryBo;
        private IParserLinkBo _buildLinkBo;

        #region Constructors

        public ArticleController()
        {
            _articleBo = new ArticleBo();
            _categoryBo = new CategoryBo();
            _buildLinkBo = new ParserLinkBo();
        }

        public ArticleController(IArticleBo articleBo, ICategoryBo categoryBo, IParserLinkBo buildLinkBo)
        {
            _articleBo = articleBo;
            _categoryBo = categoryBo;
            _buildLinkBo = buildLinkBo;
        }

        #endregion

        #region Box trang chủ

        public ActionResult ArticleTop(int cateId, int top)
        {
            ArticleModelBox model = new ArticleModelBox();

            List<ArticleModel> lstArticle = _articleBo.GetTop(cateId, top);
            model = new ArticleModelBox
            {
                //TitleBox = cateInfo.Name,
                //UrlBox = cateInfo.DisplayUrl,
                ArticleList = lstArticle
            };

            return PartialView("_ArticleTop", model);
        }


        #endregion

        #region Trang danh sách

        public ActionResult Index(string alias, string textSearch, int pageIndex = 1)
        {
            int cateId = 0;
            if (!string.IsNullOrEmpty(textSearch))
            {
                textSearch = textSearch.Replace('-', ' ');
                textSearch = StringUtils.UnicodeToUnsignChar(textSearch);
            }

            SearchArticle searchInfo = new SearchArticle()
            {
                CateId = cateId,
                TextSearch = textSearch,
                OrderBy = CookieManager.Instance.Get<int>(Const.ArrangeArticle).ToInt(0),
                PageIndex = pageIndex,
                PageSize = Const.PageSizeArticle
            };

            #region Redirect Permanent 301

            string standardUrl = _buildLinkBo.ParserArticleUrl(alias, ref searchInfo);

            string url301 = standardUrl;
            string currentUrl = Request.RawUrl;

            if (currentUrl.Contains("?utm_source"))
            {
                string strUtm = currentUrl.Substring(currentUrl.IndexOf("?utm_source"), currentUrl.Length - currentUrl.IndexOf("?utm_source"));
                url301 = string.Concat(url301, strUtm);
            }
            if (!currentUrl.Equals(url301))
            {
                return Redirect301(url301);
            }

            #endregion

            ArticleListModel model = new ArticleListModel();
            int totalRows = _articleBo.GetListCount(searchInfo);
            if (totalRows > 0)
            {
                model.ArticleList = _articleBo.GetList(searchInfo);

                #region paging

                Pagings pageModel = new Pagings
                {
                    PageIndex = pageIndex,
                    Count = totalRows,
                    LinkSite = Utils.GetCurrentURL(standardUrl, pageIndex),
                    PageSize = Const.PageSizeArticle
                };
                model.PagingInfo = pageModel;
                ViewBag.MetaPagination = SEO.MetaPagination(pageModel.Count, pageIndex, pageModel.PageSize, pageModel.LinkSite);

                #endregion
            }
            if (pageIndex > 1 && totalRows > 0 && (model.ArticleList == null || model.ArticleList.Count == 0))
            {
                return Redirect(model.PagingInfo.LinkSite);
            }

            model.SearchInfo = searchInfo;

            #region Meta

            MetaModel metaTag = _buildLinkBo.BuildMetaArticle(searchInfo);
            string metaTags = SEO.Instance.BindingMeta(standardUrl, metaTag.MetaTitle, metaTag.MetaDescription, metaTag.MetaKeyword);

            model.TitlePage = metaTag.TitlePage;
            ViewBag.MetaTitle = metaTag.MetaTitle;
            ViewBag.Meta = metaTags;

            #endregion

            return View(model);
        }

        public ActionResult ArticleHot(int cateId, int top)
        {
            ArticleModelBox model = new ArticleModelBox();
            List<ArticleModel> lstArticle = _articleBo.GetTop(cateId, top);
            model = new ArticleModelBox
            {
                ArticleList = lstArticle
            };

            return PartialView("_ArticleHot", model);
        }

        #endregion

        #region Trang chi tiết

        public ActionResult ArticleDetail(int articleId)
        {
            ArticleModelDetail modelDetail = _articleBo.GetById(articleId);

            if (modelDetail != null && modelDetail.ArticleId > 0 && modelDetail.Status == (int)Enums.ArticleStatus.Active)
            {

                #region Redirect Permanent 301

                var currentUrl = Request.RawUrl;
                string standardUrl = modelDetail.URL;
                string url301 = standardUrl;

                if (currentUrl.Contains("?utm_source"))
                {
                    string strUtm = currentUrl.Substring(currentUrl.IndexOf("?utm_source"), currentUrl.Length - currentUrl.IndexOf("?utm_source"));
                    url301 = string.Concat(url301, strUtm);
                }
                if (!currentUrl.Equals(url301))
                {
                    return RedirectPermanent(string.Concat(Const.BaseUrlNoSlash, url301));
                }

                #endregion

                #region Meta

                string strTitle = modelDetail.Title;
                string seoTitle = !string.IsNullOrEmpty(modelDetail.SEOTitle) ? modelDetail.SEOTitle : modelDetail.Title;
                string seoDesc = !string.IsNullOrEmpty(modelDetail.SEODescription) ? modelDetail.SEODescription : modelDetail.Description;
                if (string.IsNullOrEmpty(seoDesc) && seoDesc.Length > 160)
                {
                    seoDesc = StringUtils.TrimText(seoDesc, 160);
                }
                string metaTags = SEO.Instance.BindingMeta(standardUrl, seoTitle, StringUtils.RemoveStrHtmlTags(seoDesc));
                ViewBag.MetaTitle = seoTitle;
                ViewBag.Meta = metaTags;
                ViewBag.MetaFacebook = SEO.AddMetaFacebook(modelDetail.Title, "article", seoDesc, Const.BaseUrlNoSlash + standardUrl, BuildLink.CropImage(modelDetail.Avatar, Const.FacebookAvatar));

                #endregion

                ViewBag.CateId = modelDetail.CateId;

                return View(modelDetail);
            }

            Response.StatusCode = 404;
            return null;
        }

        public ActionResult ArticleOther(int articleId, int cateId, int top)
        {
            ArticleModelBox model = new ArticleModelBox();
            List<ArticleModel> lstArticle = _articleBo.GetTop(cateId, top + 1);
            if (lstArticle != null && lstArticle.Count > 0)
            {
                model = new ArticleModelBox
                {
                    ArticleList = lstArticle.Where(x => x.ArticleId != articleId).Take(top).ToList()
                };
            }
            return PartialView("_ArticleOther", model);
        }


        #endregion
    }
}