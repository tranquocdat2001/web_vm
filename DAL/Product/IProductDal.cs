using Entity;
using PublicModel;
using System.Collections.Generic;

namespace DAL.ProductDal
{
    public interface IProductDal
    {
        List<ProductEntity> GetListByCateId(int cateId, int top);

        List<ProductEntity> GetList(SearchProduct search);
        int GetListCount(SearchProduct search);

        ProductEntity GetByProductId(int productId);
        int CallMe_Insert(CallMeModel model);
        int ReceiveEmail_Insert(EmailModel model);
    }
}
