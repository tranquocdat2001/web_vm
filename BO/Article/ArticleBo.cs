using BO.Category;
using DAL.Artilce;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using Utilities;

namespace BO.Article
{
    public class ArticleBo : BaseBo, IArticleBo
    {
        private IArticleDal _articleDal;
        ICategoryBo _categoryBo;

        #region Constructors

        public ArticleBo()
        {
            _articleDal = new ArticleDal();
            _categoryBo = new CategoryBo();
        }

        public ArticleBo(IArticleDal articleDal, ICategoryBo categoryBo)
        {
            _articleDal = articleDal;
            _categoryBo = categoryBo;
        }

        #endregion

        #region Box

        public List<ArticleModel> GetTop(int cateId, int top)
        {
            List<ArticleModel> lstObject = new List<ArticleModel>();
            try
            {
                lstObject = Execute(() => GetTopNoCache(cateId, top), "ArticleBo:GetTop", Const.MediumCacheTime, false, cateId, top);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return lstObject;
        }
        public List<ArticleModel> GetTopNoCache(int cateId, int top)
        {
            List<ArticleModel> lstModel = new List<ArticleModel>();
            List<ArticleEntity> lstArticle = _articleDal.GetTop(cateId, top);
            if (lstArticle != null && lstArticle.Count > 0)
            {
                //lstModel = lstArticle.Select(x => new ArticleModel(x)).ToList();

                CategoryEntity category = new CategoryEntity();
                foreach (ArticleEntity item in lstArticle)
                {
                    //category = _categoryBo.GetByCateId(item.CategoryID);
                    //if (category != null && category.CatalogID > 0)
                    //{
                    //}

                    ArticleModel articleModel = new ArticleModel(item);
                    lstModel.Add(articleModel);
                }
            }
            return lstModel;
        } 

        #endregion

        #region Page list

        public List<ArticleModel> GetList(SearchArticle search)
        {
            List<ArticleModel> lstObject = new List<ArticleModel>();
            try
            {
                lstObject = Execute(() => GetListNoCache(search), "ArticleBo:GetList", Const.MediumCacheTime, true, search);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return lstObject;
        }

        public List<ArticleModel> GetListNoCache(SearchArticle search)
        {
            List<ArticleModel> lstModel = new List<ArticleModel>();
            List<ArticleEntity> lstArticle = _articleDal.GetList(search);
            if (lstArticle != null && lstArticle.Count > 0)
            {
                //lstModel = lstArticle.Select(x => new ArticleModel(x)).ToList();
                CategoryEntity category = new CategoryEntity();
                foreach (ArticleEntity item in lstArticle)
                {
                    //category = _categoryBo.GetByCateId(item.CategoryID);
                    //if (category != null && category.CatalogID > 0)
                    //{
                    //    ArticleModel articleModel = new ArticleModel(item);
                    //    lstModel.Add(articleModel);
                    //}
                    ArticleModel articleModel = new ArticleModel(item);
                    lstModel.Add(articleModel);
                }
            }
            return lstModel;
        }

        public int GetListCount(SearchArticle search)
        {
            int totalCount = 0;
            try
            {
                totalCount = Execute(() => _articleDal.GetListCount(search), "ArticleBo:GetListCount", Const.MediumCacheTime, true, search);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return totalCount;
        }

        #endregion

        #region Page detail

        public ArticleModelDetail GetById(int articleId)
        {
            ArticleModelDetail obj = new ArticleModelDetail();
            //ArticleEntity obj = new ArticleEntity();
            try
            {
                obj = Execute(() => GetByIdNoCache(articleId), "ArticleBo:GetById", Const.MediumCacheTime, false, articleId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return obj;
        }

        public ArticleModelDetail GetByIdNoCache(int articleId)
        {
            ArticleModelDetail objModel = new ArticleModelDetail();
            ArticleEntity objArticle = _articleDal.GetById(articleId);
            if (objArticle != null && objArticle.NewsID > 0)
            {
                //CategoryEntity category = _categoryBo.GetByCateId(objArticle.CategoryID);
                //if (category != null && category.CatalogID > 0)
                //{
                //    objModel = new ArticleModelDetail(objArticle);
                //}
                objModel = new ArticleModelDetail(objArticle);
            }
            return objModel;
        }

        #endregion
    }
}
