using Entity;
using System.Collections.Generic;

namespace DAL.Category
{
    public interface ICategoryDal
    {
        List<CategoryEntity> GetList();
    }
}
