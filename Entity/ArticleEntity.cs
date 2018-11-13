using System;

namespace Entity
{
    public class ArticleEntity
    {
        public int NewsID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime DisApprovedDate { get; set; }
        public string DisApprovedBy { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedBy { get; set; }
        public string UnpublishedBy { get; set; }
        public DateTime UnpublishedDate { get; set; }
        public string EditorBy { get; set; }
        public DateTime EditorDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsFocus { get; set; }
        public int Mode { get; set; }
        public int Status { get; set; }
        public string NewsRelation { get; set; }
        public int NewsType { get; set; }
        public int ViewCount { get; set; }
        public string NewsContent { get; set; }
    }
}
