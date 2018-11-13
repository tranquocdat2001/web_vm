using Entity;
using PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Artilce
{
    public interface IArticleDal
    {
        List<ArticleEntity> GetTop(int cateId, int top);

        List<ArticleEntity> GetList(SearchArticle search);
        int GetListCount(SearchArticle search);

        ArticleEntity GetById(int articleId);

    }
}
