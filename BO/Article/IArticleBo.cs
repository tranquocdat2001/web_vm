using PublicModel;
using System.Collections.Generic;

namespace BO.Article
{
    public interface IArticleBo
    {
        List<ArticleModel> GetTop(int cateId, int top);

        List<ArticleModel> GetList(SearchArticle search);
        int GetListCount(SearchArticle search);

        ArticleModelDetail GetById(int articleId);
    }
}
