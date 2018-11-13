using Entity;
using FluentData;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DAL.Artilce
{
    public class ArticleDal : ContextBase, IArticleDal
    {
        public List<ArticleEntity> GetTop(int cateId, int top)
        {
            string storeName = "FE_News_GetTop";
            List<ArticleEntity> lstObject = new List<ArticleEntity>();
            try
            {
                using (IDbContext context = Context())
                {
                    lstObject = context.StoredProcedure(storeName)
                        .Parameter("CateId", cateId, DataTypes.Int32)
                        .Parameter("Top", top, DataTypes.Int32)
                        .QueryMany<ArticleEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return lstObject;
        }
        public List<ArticleEntity> GetList(SearchArticle search)
        {
            string storeName = "FE_News_GetList";
            List<ArticleEntity> lstObject = new List<ArticleEntity>();
            try
            {
                using (IDbContext context = Context())
                {
                    lstObject = context.StoredProcedure(storeName)
                        .Parameter("CateId", search.CateId, DataTypes.Int32)
                        .Parameter("Keyword", search.TextSearch, DataTypes.String)
                        .Parameter("OrderBy", search.OrderBy, DataTypes.Int32)
                        .Parameter("PageIndex", search.PageIndex, DataTypes.Int32)
                        .Parameter("PageSize", search.PageSize, DataTypes.Int32)
                        .QueryMany<ArticleEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return lstObject;
        }
        public int GetListCount(SearchArticle search)
        {
            string storeName = "FE_News_GetList_Count";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure("FE_News_GetList_Count")
                        .Parameter("CateId", search.CateId, DataTypes.Int32)
                        .Parameter("Keyword", search.TextSearch, DataTypes.String)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

        }

        public ArticleEntity GetById(int articleId)
        {
            string storeName = "FE_News_GetByNewsId";
            ArticleEntity obj = new ArticleEntity();
            try
            {
                using (IDbContext context = Context())
                {
                    obj = context.StoredProcedure(storeName)
                        .Parameter("ArticleId", articleId, DataTypes.Int32)
                        .QuerySingle<ArticleEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return obj;
        }
    }
}
