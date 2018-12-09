using Entity;
using PublicModel;
using System.Collections.Generic;

namespace BO.ProductBo
{
    public interface IProductBo
    {
        List<ProductModel> GetListByCateId(int cateId, int top);

        List<ProductModel> GetList(SearchProduct search);
        int GetListCount(SearchProduct search);

        ProductModelDetail GetByProductId(int productId);
        ProductModelDetail GetByProductIdNoCache(int productId);
        int CallMe_Insert(CallMeModel model);
        int ReceiveEmail_Insert(EmailModel model);
    }
}
