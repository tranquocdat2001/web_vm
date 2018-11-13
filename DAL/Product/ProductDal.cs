using Entity;
using FluentData;
using PublicModel;
using System;
using System.Collections.Generic;
using Utilities;

namespace DAL.ProductDal
{
    public class ProductDal : ContextBase, IProductDal
    {
        public List<ProductEntity> GetListByCateId(int cateId, int top)
        {
            string storeName = "FE_Product_GetListByCateId";
            List<ProductEntity> lstObject = new List<ProductEntity>();
            try
            {
                using (IDbContext context = Context())
                {
                    lstObject = context.StoredProcedure(storeName)
                        .Parameter("CateId", cateId, DataTypes.Int32)
                        .Parameter("Top", top, DataTypes.Int32)
                        .QueryMany<ProductEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return lstObject;
        }

        public List<ProductEntity> GetList(SearchProduct search)
        {
            string storeName = "FE_Product_GetList";
            List<ProductEntity> lstObject = new List<ProductEntity>();
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
                        .QueryMany<ProductEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return lstObject;
        }
        public int GetListCount(SearchProduct search)
        {
            string storeName = "FE_Product_GetList_Count";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure("FE_Product_GetList_Count")
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

        public ProductEntity GetByProductId(int productId)
        {
            string storeName = "FE_Product_GetByProductId";
            ProductEntity obj = new ProductEntity();
            try
            {
                using (IDbContext context = Context())
                {
                    obj = context.StoredProcedure(storeName)
                        .Parameter("ProductId", productId, DataTypes.Int32)
                        .QuerySingle<ProductEntity>();
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
