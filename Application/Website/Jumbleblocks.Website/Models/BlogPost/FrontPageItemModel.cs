using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.BlogPost
{
    [Serializable]
    public class FrontPageItemModel
    {
        /// <summary>
        /// Title of Blog Post
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of what the article is about
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Image url
        /// </summary>
        public Uri ImageUrl { get; set; }

        /// <summary>
        /// Date/Time publised
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets/Sets name of author
        /// </summary>
        public string AuthorsName { get; set; }

        /// <summary>
        /// The year to return for the link to show the article
        /// </summary>
        public int LinkYear { get { return PublishedDate.Year; } }

        /// <summary>
        /// The month to return for the link to show the article
        /// </summary>
        public int LinkMonth { get { return PublishedDate.Month; } }

        /// <summary>
        /// The day to return for the link to show the article
        /// </summary>
        public int LinkDay { get { return PublishedDate.Day; } }
    }
}