using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.BlogPost
{
    /// <summary>
    /// Details about how many pages there are
    /// </summary>
    [Serializable]
    public class PagingDetail
    {
        /// <summary>
        /// Gets the current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets the total number of pages
        /// </summary>
        public int PageCount { get; set; }
    }
}