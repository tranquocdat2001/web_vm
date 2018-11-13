using Entity;
using PublicModel;
using System.Collections.Generic;

namespace BO.Category
{
    public interface ICategoryBo
    {
        List<CategoryEntity> GetList();
        List<CategoryEntity> GetListDesc();
        CategoryEntity GetByCateId(int cateId);

        List<BreadcrumbModel> GetBreadcrumb(string cateName, int cateId);
    }
}
