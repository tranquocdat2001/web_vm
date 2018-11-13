using Entity;
using System.Collections.Generic;
using Utilities;

namespace PublicModel
{
    public class ArticleModel
    {
        public ArticleModel(ArticleEntity item, CategoryEntity category)
        {
            this.ArticleId = item.NewsID;
            this.CateId = item.CategoryID;
            this.Title = item.Title;
            this.SubTitle = item.SubTitle;
            this.Description = item.Description;
            this.Avatar = item.Avatar;
            this.CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy");
            this.PublishedDate = item.PublishedDate.ToString("dd/MM/yyyy");
            this.URL = BuildLink.BuildURLForArticle(category.DisplayUrl, item.Title, item.NewsID);
        }
        public int ArticleId { get; set; }
        public int CateId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string CreatedDate { get; set; }
        public string PublishedDate { get; set; }
        public string URL { get; set; }
        public string GetAvatar(string crop)
        {
            return BuildLink.CropImage(this.Avatar, crop);
        }

    }

    public class ArticleModelBox
    {
        public string TitleBox { get; set; }
        public string UrlBox { get; set; }
        public List<ArticleModel> ArticleList { get; set; }
    }

    public class ArticleListModel
    {
        public ArticleListModel()
        {
            PagingInfo = new Pagings();
            SearchInfo = new SearchArticle();
        }
        public string TitlePage { get; set; }
        public List<ArticleModel> ArticleList { get; set; }
        public Pagings PagingInfo { get; set; }
        public SearchArticle SearchInfo { get; set; }
    }

    public class ArticleModelDetail
    {
        public ArticleModelDetail()
        {

        }
        public ArticleModelDetail(ArticleEntity item, CategoryEntity category)
        {
            this.ArticleId = item.NewsID;
            this.CateId = item.CategoryID;
            this.Title = item.Title;
            this.SubTitle = item.SubTitle;
            this.Description = item.Description;
            this.NewsContent = item.NewsContent;
            this.Avatar = item.Avatar;
            this.CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy");
            this.PublishedDate = item.PublishedDate.ToString("dd/MM/yyyy");
            this.URL = BuildLink.BuildURLForArticle(category.DisplayUrl, item.Title, item.NewsID);
        }

        public int ArticleId { get; set; }
        public int CateId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string NewsContent { get; set; }
        public string Avatar { get; set; }
        public string CreatedDate { get; set; }
        public string PublishedDate { get; set; }
        public bool IsFocus { get; set; }
        public int Mode { get; set; }
        public int Status { get; set; }
        public string NewsRelation { get; set; }
        public int ArticleType { get; set; }
        public int ViewCount { get; set; }
        public string URL { get; set; }
        public string GetAvatar(string crop)
        {
            return BuildLink.CropImage(this.Avatar, crop);
        }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
    }
}