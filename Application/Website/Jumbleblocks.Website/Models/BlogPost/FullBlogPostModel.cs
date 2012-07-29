using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jumbleblocks.Website.Models.BlogPost
{
    /// <summary>
    /// View Model for an article
    /// </summary>
    public class FullBlogPostModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string FullArticle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string AuthorsName { get; set; }

        public string CommentsId { get; set; }
    }
}