using BO.Category;
using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VeganMart.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryBo _categoryBo;

        #region Constructors

        public CategoryController()
        {
            _categoryBo = new CategoryBo();
        }

        public CategoryController(ICategoryBo categoryBo)
        {
            _categoryBo = categoryBo;
        }

        #endregion

        public ActionResult MenuTop()
        {
            List<CategoryEntity> lstCate = _categoryBo.GetList();
            return PartialView("_MenuTop", lstCate);
        }

        public ActionResult Breadcrumb(string cateName, int cateId = 0)
        {
            List<BreadcrumbModel> lstBreadcrumb = _categoryBo.GetBreadcrumb(cateName, cateId);
            return PartialView("_Breadcrumb", lstBreadcrumb);
        }
    }
}