using BO.ParserLink;
using BO.Category;
using BO.ProductBo;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace VeganMart.Controllers
{
    public class ProductController : BaseController
    {
        private IProductBo _productBo;
        private ICategoryBo _categoryBo;
        private IParserLinkBo _buildLinkBo;

        #region Constructors

        public ProductController()
        {
            _productBo = new ProductBo();
            _categoryBo = new CategoryBo();
            _buildLinkBo = new ParserLinkBo();
        }

        public ProductController(IProductBo productBo, ICategoryBo categoryBo, IParserLinkBo buildLinkBo)
        {
            _productBo = productBo;
            _categoryBo = categoryBo;
            _buildLinkBo = buildLinkBo;
        }

        #endregion

        #region Box trang chủ

        public ActionResult ProductTop(int cateId, int top)
        {
            ProductModelBox model = new ProductModelBox();
            CategoryEntity cateInfo = _categoryBo.GetByCateId(cateId);
            if (cateInfo != null && cateInfo.CatalogID > 0)
            {
                List<ProductModel> lstProduct = _productBo.GetListByCateId(cateId, top);
                model = new ProductModelBox
                {
                    TitleBox = cateInfo.Name,
                    UrlBox = cateInfo.DisplayUrl,
                    ProductList = lstProduct
                };
            }
            return PartialView("_ProductTop", model);
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

            SearchProduct searchInfo = new SearchProduct()
            {
                CateId = cateId,
                TextSearch = textSearch,
                OrderBy = CookieManager.Instance.Get<int>(Const.ArrangeProduct).ToInt(0),
                PageIndex = pageIndex,
                PageSize = Const.PageSizeProduct
            };

            #region Redirect Permanent 301

            string standardUrl = _buildLinkBo.ParserProductUrl(alias, ref searchInfo);

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

            ProductListModel model = new ProductListModel();
            int totalRows = _productBo.GetListCount(searchInfo);
            if (totalRows > 0)
            {
                model.ProductList = _productBo.GetList(searchInfo);

                #region paging

                Pagings pageModel = new Pagings
                {
                    PageIndex = pageIndex,
                    Count = totalRows,
                    LinkSite = Utils.GetCurrentURL(standardUrl, pageIndex),
                    PageSize = Const.PageSizeProduct
                };
                model.PagingInfo = pageModel;
                ViewBag.MetaPagination = SEO.MetaPagination(pageModel.Count, pageIndex, pageModel.PageSize, pageModel.LinkSite);

                #endregion
            }
            if (pageIndex > 1 && totalRows > 0 && (model.ProductList == null || model.ProductList.Count == 0))
            {
                return Redirect(model.PagingInfo.LinkSite);
            }

            model.SearchInfo = searchInfo;

            #region Meta

            //string titlePage = Const.MetaMainTitle;
            //string metaTitle = titlePage;
            //string metaDescription = Const.MetaMainDescription;
            //string metaKeyword = Const.MetaMainKeyword;

            MetaModel metaTag = _buildLinkBo.BuildMetaProduct(searchInfo);
            string metaTags = SEO.Instance.BindingMeta(standardUrl, metaTag.MetaTitle, metaTag.MetaDescription, metaTag.MetaKeyword);

            model.TitlePage = metaTag.TitlePage;
            ViewBag.MetaTitle = metaTag.MetaTitle;
            ViewBag.Meta = metaTags;

            #endregion

            return View(model);
        }

        public ActionResult ProductHot(int cateId, int top)
        {
            ProductModelBox model = new ProductModelBox();
            List<ProductModel> lstProduct = _productBo.GetListByCateId(cateId, top);
            model = new ProductModelBox
            {
                ProductList = lstProduct
            };

            return PartialView("_ProductHot", model);
        }

        #endregion

        #region Trang chi tiết

        public ActionResult ProductDetail(int productId)
        {
            ProductModelDetail modelDetail = _productBo.GetByProductId(productId);

            if (modelDetail != null && modelDetail.ProductId > 0 && modelDetail.Status == (int)Enums.ProductStatus.Active)
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

                string strTitle = modelDetail.Name;
                string seoTitle = !string.IsNullOrEmpty(modelDetail.SEOTitle) ? modelDetail.SEOTitle : modelDetail.Name;
                string seoDesc = !string.IsNullOrEmpty(modelDetail.SEODescription) ? modelDetail.SEODescription : modelDetail.ShortDescription;
                if (string.IsNullOrEmpty(seoDesc) && seoDesc.Length > 160)
                {
                    seoDesc = StringUtils.TrimText(seoDesc, 160);
                }
                string metaTags = SEO.Instance.BindingMeta(standardUrl, seoTitle, StringUtils.RemoveStrHtmlTags(seoDesc));
                ViewBag.MetaTitle = seoTitle;
                ViewBag.Meta = metaTags;
                ViewBag.MetaFacebook = SEO.AddMetaFacebook(modelDetail.Name, "article", seoDesc, Const.BaseUrlNoSlash + standardUrl, BuildLink.CropImage(modelDetail.Avatar, Const.FacebookAvatar));

                #endregion

                ViewBag.CateId = modelDetail.CateId;

                return View(modelDetail);
            }

            Response.StatusCode = 404;
            return null;
        }

        public ActionResult ProductOther(int productId, int cateId, int top)
        {
            ProductModelBox model = new ProductModelBox();
            List<ProductModel> lstProduct = _productBo.GetListByCateId(cateId, top + 1);
            if (lstProduct != null && lstProduct.Count > 0)
            {
                model = new ProductModelBox
                {
                    ProductList = lstProduct.Where(x => x.ProductId != productId).Take(top).ToList()
                };
            }
            return PartialView("_ProductOther", model);
        }


        #endregion
    }
}