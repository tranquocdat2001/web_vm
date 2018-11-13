using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.FB.InstantArticle
{
    public partial class FBInstantArticleModel
    {
        public long Id { get; set;}
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Avatar { get; set; }
        public string AvatarCaption { get; set; }
        public string Sapo { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PublishedDateStr { get; set; }
        public string ModifiedDateStr { get; set; }
        public string OriginalUrl { get; set; }
        public string Source { get; set; }
        /// <summary>
        /// Tương đương CatName
        /// </summary>
        public string Kicker { get; set; }
    }
}
