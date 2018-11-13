using PublicModel;

namespace BO.ParserLink
{
    public interface IParserLinkBo
    {
        string ParserProductUrl(string urlAlias, ref SearchProduct objSearch);
        MetaModel BuildMetaProduct(SearchProduct objSearch);

        string ParserArticleUrl(string urlAlias, ref SearchArticle objSearch);
        MetaModel BuildMetaArticle(SearchArticle objSearch);
    }
}
