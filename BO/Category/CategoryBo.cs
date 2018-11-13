using DAL.Category;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BO.Category
{
    public class CategoryBo : ICategoryBo
    {
        private ICategoryDal _categoryDal;
        private static List<CategoryEntity> ListCategoryStatic = new List<CategoryEntity>();
        private int indexBreadcrumb = 0;

        #region Constructors

        public CategoryBo()
        {
            _categoryDal = new CategoryDal();
        }

        public CategoryBo(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        #endregion

        public List<CategoryEntity> GetList()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.UserAgent != null)
            {
                if (HttpContext.Current.Request.UserAgent.Contains("refreshcache"))
                {
                    ListCategoryStatic = null;
                }
            }
            if (ListCategoryStatic != null && ListCategoryStatic.Any())
            {
                return ListCategoryStatic;
            }

            ListCategoryStatic = _categoryDal.GetList();
            return ListCategoryStatic;
        }

        public List<CategoryEntity> GetListDesc()
        {
            List<CategoryEntity> lstCateDesc = GetList().OrderByDescending(obj => obj.DisplayUrl.Length).ToList();
            return lstCateDesc;
        }

        public CategoryEntity GetByCateId(int cateId)
        {
            CategoryEntity objCate = new CategoryEntity();
            List<CategoryEntity> lstCate = GetList();
            if (lstCate != null && lstCate.Count > 0)
                objCate = lstCate.Find(x => x.CatalogID == cateId);
            return objCate;
        }

        public List<BreadcrumbModel> GetBreadcrumb(string cateName, int cateId)
        {
            List<BreadcrumbModel> lstBreadcrumb = new List<BreadcrumbModel>();

            BreadcrumbModel breadcrumb = new BreadcrumbModel()
            {
                Name = "Trang chủ",
                Url = "/",
                Priority = 0
            };

            if (cateId > 0)
            {
                lstBreadcrumb = GetSubBreadcrumb(cateId);
                if (lstBreadcrumb != null && lstBreadcrumb.Count > 0)
                {
                    lstBreadcrumb = lstBreadcrumb.OrderByDescending(x => x.Priority).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(cateName))
            {
                BreadcrumbModel breadcrumbEnd = new BreadcrumbModel()
                {
                    Name = cateName,
                    Priority = 1
                };
                lstBreadcrumb.Add(breadcrumbEnd);
            }

            lstBreadcrumb.Insert(0, breadcrumb);

            return lstBreadcrumb;
        }

        private List<BreadcrumbModel> GetSubBreadcrumb(int cateId)
        {
            List<BreadcrumbModel> lstBreadcrumb = new List<BreadcrumbModel>();
            if (cateId > 0)
            {
                CategoryEntity objCate = GetByCateId(cateId);
                if (objCate != null && objCate.CatalogID > 0)
                {
                    indexBreadcrumb++;
                    BreadcrumbModel breadcrumb = new BreadcrumbModel()
                    {
                        Name = objCate.Name,
                        //Url = indexBreadcrumb == 1 ? string.Empty : "/",
                        Url = string.Format("/{0}", objCate.DisplayUrl),
                        Priority = indexBreadcrumb
                    };

                    //indexBreadcrumb++;
                    //BreadcrumbModel objTest = new BreadcrumbModel()
                    //{
                    //    Name = "chan chan",
                    //    Url = "/",
                    //    Priority = indexBreadcrumb
                    //};
                    //List<BreadcrumbModel> lstTest = new List<BreadcrumbModel>();
                    //lstTest.Add(objTest);
                    //lstTest.Add(objTest);
                    //breadcrumb.ListSub = lstTest;

                    lstBreadcrumb.Add(breadcrumb);

                    if (objCate.ParrentID > 0)
                    {
                        lstBreadcrumb.AddRange(GetSubBreadcrumb(objCate.ParrentID));
                    }

                    //indexBreadcrumb++;
                    //breadcrumb = new BreadcrumbModel()
                    //{
                    //    Name = "Test",
                    //    Url = "/",
                    //    Priority = indexBreadcrumb
                    //};
                    //lstBreadcrumb.Add(breadcrumb);
                }
            }
            return lstBreadcrumb;
        }
    }
}
