using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog.Paging
{
    /// <summary>
    /// pages through blog posts
    /// </summary>
    public interface IBlogPostPager
    {
        /// <summary>
        /// Number of pages that can be produced
        /// </summary>
        int PageCount { get; }


        /// <summary>
        /// Gets the blog posts on a specific page
        /// </summary>
        /// <param name="pageNumber">page number to retrieve blog posts for</param>
        /// <param name="tags">List of tags names to constrain fetch by</param>
        /// <returns>IEnumerable of blog post</returns>
        IEnumerable<BlogPost> FetchForPage(int pageNumber, IEnumerable<string> tags = null);
    }
}
