using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jumbleblocks.Website.Models.BlogPost
{
    public class BlogPostListingItemModel
    {
        public int BlogPostId { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string AuthorsName { get; set; }
    }
}