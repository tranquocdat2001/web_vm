using Entity;
using FluentData;
using System;
using System.Collections.Generic;
using Utilities;

namespace DAL.Category
{
    public class CategoryDal : ContextBase, ICategoryDal
    {
        #region Constructor

        public CategoryDal()
        {
            _dbPosition = DBPosition.Master;
        }

        #endregion

        public List<CategoryEntity> GetList()
        {
            string storeName = "FE_GetListCatalog";
            List<CategoryEntity> lstObject = new List<CategoryEntity>();
            try
            {
                using (IDbContext context = Context())
                {
                    lstObject = context.StoredProcedure(storeName)
                        .QueryMany<CategoryEntity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }

            return lstObject;
        }
    }
}
