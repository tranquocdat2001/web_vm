using BO.Category;
using DAL.ProductDal;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BO.ProductBo
{
    public class ProductBo : BaseBo, IProductBo
    {
        private IProductDal _productDal;
        ICategoryBo _categoryBo;

        #region Constructors

        public ProductBo()
        {
            _productDal = new ProductDal();
            _categoryBo = new CategoryBo();
        }

        public ProductBo(IProductDal productDal, ICategoryBo categoryBo)
        {
            _productDal = productDal;
            _categoryBo = categoryBo;
        }

        #endregion

        public List<ProductModel> GetListByCateId(int cateId, int top)
        {
            List<ProductModel> lstObject = new List<ProductModel>();
            try
            {
                lstObject = Execute(() => GetListByCateIdNoCache(cateId, top), "ProductBo:GetListByCateId", Const.MediumCacheTime, false, cateId, top);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return lstObject;
        }

        public List<ProductModel> GetListByCateIdNoCache(int cateId, int top)
        {
            List<ProductModel> lstModel = new List<ProductModel>();
            List<ProductEntity> lstProduct = _productDal.GetListByCateId(cateId, top);
            if (lstProduct != null && lstProduct.Count > 0)
            {
                //lstModel = lstProduct.Select(x => new ProductModel(x)).ToList();

                CategoryEntity category = new CategoryEntity();
                foreach (ProductEntity item in lstProduct)
                {
                    category = _categoryBo.GetByCateId(item.CatalogID);
                    if (category != null && category.CatalogID > 0)
                    {
                        ProductModel productModel = new ProductModel(item, category);
                        lstModel.Add(productModel);
                    }
                }
            }
            return lstModel;
        }


        #region Page list

        public List<ProductModel> GetList(SearchProduct search)
        {
            List<ProductModel> lstObject = new List<ProductModel>();
            try
            {
                lstObject = Execute(() => GetListNoCache(search), "ProductBo:GetList", Const.MediumCacheTime, true, search);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return lstObject;
        }

        public List<ProductModel> GetListNoCache(SearchProduct search)
        {
            List<ProductModel> lstModel = new List<ProductModel>();
            List<ProductEntity> lstProduct = _productDal.GetList(search);
            if (lstProduct != null && lstProduct.Count > 0)
            {
                //lstModel = lstProduct.Select(x => new ProductModel(x)).ToList();
                CategoryEntity category = new CategoryEntity();
                foreach (ProductEntity item in lstProduct)
                {
                    category = _categoryBo.GetByCateId(item.CatalogID);
                    if (category != null && category.CatalogID > 0)
                    {
                        ProductModel productModel = new ProductModel(item, category);
                        lstModel.Add(productModel);
                    }
                }
            }
            return lstModel;
        }

        public int GetListCount(SearchProduct search)
        {
            int totalCount = 0;
            try
            {
                totalCount = Execute(() => _productDal.GetListCount(search), "ProductBo:GetListCount", Const.MediumCacheTime, true, search);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return totalCount;
        }

        #endregion

        #region Page detail

        public ProductModelDetail GetByProductId(int productId)
        {
            ProductModelDetail obj = new ProductModelDetail();
            //ProductEntity obj = new ProductEntity();
            try
            {
                obj = Execute(() => GetByProductIdNoCache(productId), "ProductBo:GetByProductId", Const.MediumCacheTime, false, productId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return obj;
        }

        public ProductModelDetail GetByProductIdNoCache(int productId)
        {
            ProductModelDetail objModel = new ProductModelDetail();
            ProductEntity objProduct = _productDal.GetByProductId(productId);
            if (objProduct != null && objProduct.ProductID > 0)
            {
                CategoryEntity category = _categoryBo.GetByCateId(objProduct.CatalogID);
                if (category != null && category.CatalogID > 0)
                {
                    objModel = new ProductModelDetail(objProduct, category);
                }
            }
            return objModel;
        }

        public int CallMe_Insert(CallMeModel model)
        {
            return _productDal.CallMe_Insert(model);
        }

        #endregion

        public int ReceiveEmail_Insert(EmailModel model)
        {
            return _productDal.ReceiveEmail_Insert(model);
        }
    }
}
