using BO.Category;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BO.ParserLink
{
    public class ParserLinkBo : IParserLinkBo
    {
        private ICategoryBo _categoryBo;

        #region Constructors

        public ParserLinkBo()
        {
            _categoryBo = new CategoryBo();
        }

        public ParserLinkBo(ICategoryBo categoryBo)
        {
            _categoryBo = categoryBo;
        }

        #endregion

        #region Product

        public string ParserProductUrl(string urlAlias, ref SearchProduct searchInfo)
        {
            string trueAlias = string.Empty;
            urlAlias = string.Concat(urlAlias, "-");

            #region Category

            int cateId = 0;

            if (!string.IsNullOrEmpty(urlAlias) && urlAlias.StartsWith("san-pham"))
            {
                urlAlias = urlAlias.Replace("san-pham-", string.Empty);
                trueAlias = "san-pham";
            }
            else if (!string.IsNullOrEmpty(urlAlias))
            {
                List<CategoryEntity> lstCateDesc = _categoryBo.GetListDesc();
                foreach (var item in lstCateDesc)
                {
                    if (urlAlias.StartsWith(item.DisplayUrl + "-"))
                    {
                        searchInfo.CateId = cateId = item.CatalogID;
                        urlAlias = StringUtils.SubStringAlias(urlAlias, item.DisplayUrl);
                        trueAlias = string.Concat(trueAlias, item.DisplayUrl);
                        break;
                    }
                }
            }
            if (cateId == 0)
                trueAlias = "san-pham";

            #endregion

            if (!string.IsNullOrEmpty(searchInfo.TextSearch))
            {
                trueAlias = string.Format("{0}/k={1}", trueAlias, searchInfo.TextSearch);
            }

            if (searchInfo.PageIndex > 1)
                trueAlias = string.Format("{0}/p{1}", trueAlias, searchInfo.PageIndex);

            return string.Concat("/", trueAlias);
        }

        public MetaModel BuildMetaProduct(SearchProduct searchInfo)
        {
            MetaModel metaTag = new MetaModel()
            {
                TitlePage = Const.MetaProductTitle,
                MetaTitle = Const.MetaProductTitle,
                MetaDescription = Const.MetaProductDescription,
                MetaKeyword = Const.MetaProductKeyword
            };

            if (!string.IsNullOrEmpty(searchInfo.TextSearch))
            {
                metaTag.TitlePage = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
                metaTag.MetaTitle = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
                metaTag.MetaDescription = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
            }
            else if (searchInfo.CateId > 0)
            {
                CategoryEntity cateInfo = _categoryBo.GetByCateId(searchInfo.CateId);
                if (cateInfo != null && cateInfo.CatalogID > 0)
                {
                    metaTag.TitlePage = cateInfo.Name;
                    metaTag.MetaTitle = cateInfo.Name;
                    metaTag.MetaDescription = cateInfo.Name;
                }
            }

            if (searchInfo.PageIndex > 1)
            {
                metaTag.TitlePage = string.Concat(metaTag.TitlePage, " - trang " + searchInfo.PageIndex);
                if (!string.IsNullOrEmpty(Const.PrefixTitle))
                    metaTag.TitlePage = string.Concat(metaTag.TitlePage, " - ", Const.PrefixTitle);
            }

            return metaTag;
        }

        #endregion

        #region Article

        public string ParserArticleUrl(string urlAlias, ref SearchArticle searchInfo)
        {
            string trueAlias = string.Empty;
            urlAlias = string.Concat(urlAlias, "-");

            #region Category

            int cateId = 0;

            if (!string.IsNullOrEmpty(urlAlias) && urlAlias.StartsWith("tin-tuc"))
            {
                urlAlias = urlAlias.Replace("tin-tuc-", string.Empty);
                trueAlias = "tin-tuc";
            }
            else if (!string.IsNullOrEmpty(urlAlias))
            {
                List<CategoryEntity> lstCateDesc = _categoryBo.GetListDesc();
                foreach (var item in lstCateDesc)
                {
                    if (urlAlias.StartsWith(item.DisplayUrl + "-"))
                    {
                        searchInfo.CateId = cateId = item.CatalogID;
                        urlAlias = StringUtils.SubStringAlias(urlAlias, item.DisplayUrl);
                        trueAlias = string.Concat(trueAlias, item.DisplayUrl);
                        break;
                    }
                }
            }
            if (cateId == 0)
                trueAlias = "tin-tuc";

            #endregion

            if (!string.IsNullOrEmpty(searchInfo.TextSearch))
            {
                trueAlias = string.Format("{0}/k={1}", trueAlias, searchInfo.TextSearch);
            }

            if (searchInfo.PageIndex > 1)
                trueAlias = string.Format("{0}/p{1}", trueAlias, searchInfo.PageIndex);

            return string.Concat("/", trueAlias);
        }

        public MetaModel BuildMetaArticle(SearchArticle searchInfo)
        {
            MetaModel metaTag = new MetaModel()
            {
                TitlePage = Const.MetaNewsTitle,
                MetaTitle = Const.MetaNewsTitle,
                MetaDescription = Const.MetaNewsDescription,
                MetaKeyword = Const.MetaNewsKeyword
            };

            if (!string.IsNullOrEmpty(searchInfo.TextSearch))
            {
                metaTag.TitlePage = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
                metaTag.MetaTitle = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
                metaTag.MetaDescription = string.Format("Tìm kiếm {0}", searchInfo.TextSearch);
            }
            else if (searchInfo.CateId > 0)
            {
                CategoryEntity cateInfo = _categoryBo.GetByCateId(searchInfo.CateId);
                if (cateInfo != null && cateInfo.CatalogID > 0)
                {
                    metaTag.TitlePage = cateInfo.Name;
                    metaTag.MetaTitle = cateInfo.Name;
                    metaTag.MetaDescription = cateInfo.Name;
                }
            }

            if (searchInfo.PageIndex > 1)
            {
                metaTag.TitlePage = string.Concat(metaTag.TitlePage, " - trang " + searchInfo.PageIndex);
                if (!string.IsNullOrEmpty(Const.PrefixTitle))
                    metaTag.TitlePage = string.Concat(metaTag.TitlePage, " - ", Const.PrefixTitle);
            }

            return metaTag;
        }

        #endregion
    }
}
