using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog.Paging
{
    /// <summary>
    /// pages through blog posts
    /// </summary>
    public class BlogPostPager : IBlogPostPager
    {
        public BlogPostPager(int postsPerPage, IBlogPostRepository blogPostRepository)
        {
            if (postsPerPage <= 0)
                throw new ArgumentException("must be at least 1", "postsPerPage");

            if (blogPostRepository == null)
                throw new ArgumentNullException("blogPostRepository");

            PostsPerPage = postsPerPage;
            BlogPostRepository = blogPostRepository;
        }

        /// <summary>
        /// Posts to be shown on each page
        /// </summary>
        public int PostsPerPage { get; private set; }
        
        /// <summary>
        /// The blog post repository
        /// </summary>
        public IBlogPostRepository BlogPostRepository { get; private set; }

        /// <summary>
        /// Total Number of blog posts
        /// </summary>
        protected int NumberOfBlogPosts { get { return BlogPostRepository.Count; } }

        /// <summary>
        /// Number of pages that can be produced
        /// </summary>
        public int PageCount { get { return (int)Math.Ceiling((double)NumberOfBlogPosts / PostsPerPage); } }


        /// <summary>
        /// Gets the blog posts on a specific page
        /// </summary>
        /// <param name="pageNumber">page number to retrieve blog posts for</param>
        /// <param name="tags">List of tags names to constrain fetch by</param>
        /// <returns>IEnumerable of blog post</returns>
        public IEnumerable<BlogPost> FetchForPage(int pageNumber, IEnumerable<string> tags = null)
        {
            if (tags == null)
                tags = new string[0];

            int numberToSkip = CalculateNumberToSkip(pageNumber);

            if (HasBlogPostsToReturn(numberToSkip))
            {
                int numberToTake = CalculateNumberToTake(numberToSkip);
                return BlogPostRepository.GetPosts(numberToSkip, numberToTake, tags);
            }
            else
                return new BlogPost[0];
        }

        /// <summary>
        /// Calculate the number of pages to skip
        /// </summary>
        /// <param name="pageNumber">current page number</param>
        /// <returns>number of posts to skip</returns>
        private int CalculateNumberToSkip(int pageNumber)
        {
            return (pageNumber - 1) * PostsPerPage;
        }

        /// <summary>
        /// determines if there are still blog posts to return
        /// </summary>
        /// <param name="pageNumber">current page number</param>
        /// <returns>true if is, otherwise false</returns>
        private bool HasBlogPostsToReturn(int numberToSkip)
        {
            return numberToSkip < NumberOfBlogPosts;
        }

        /// <summary>
        /// Calculates the number to take
        /// </summary>
        /// <param name="numberToSkip">number to skip</param>
        /// <returns>number to take</returns>
        private int CalculateNumberToTake(int numberToSkip)
        {
            int numberVisitedAfterNextTake = numberToSkip + PostsPerPage;

            if (numberVisitedAfterNextTake > NumberOfBlogPosts)
                return NumberOfBlogPosts - numberToSkip;

            return PostsPerPage;
        }
    }
}
