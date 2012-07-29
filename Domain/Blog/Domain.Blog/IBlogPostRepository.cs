using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// interface for a blog post repository
    /// </summary>
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        /// <summary>
        /// Gets a list of summaries
        /// </summary>
        /// <param name="numberToSkip">Number of post summarys to skip</param>
        /// <param name="numberToTake">number of posts to return</param>
        /// <param name="tags">any tags to constrain the search</param>
        /// <returns>IEnumerable of BlogPost</returns>
        IEnumerable<BlogPost> GetPosts(int numberToSkip, int numberToTake, IEnumerable<string> tags);

        /// <summary>
        /// Loads a blog post with given details
        /// </summary>
        /// <param name="year">year blog post published</param>
        /// <param name="month">month blog post published</param>
        /// <param name="day">day blog post published</param>
        /// <param name="title">title of blog post</param>
        /// <returns>Fully filled blogpost </returns>
        BlogPost LoadFrom(int year, int month, int day, string title);

        /// <summary>
        /// Eager loads full article
        /// </summary>
        /// <param name="id">id of blog post to use</param>
        /// <returns>BlogPost</returns>
        BlogPost LoadFullArticle(int id);
    }
}
