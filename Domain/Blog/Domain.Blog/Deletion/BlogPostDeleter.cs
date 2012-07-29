using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog.Deletion
{
    public class BlogPostDeleter : IBlogPostDeleter
    {
        public BlogPostDeleter(IBlogPostRepository blogPostRepository, ILookupRepository lookupRepository)
        {
            if (blogPostRepository == null)
                throw new ArgumentNullException("blogPostRepository");

            if (lookupRepository == null)
                throw new ArgumentNullException("lookupRepository");

            BlogPostRepository = blogPostRepository;
            LookupRepository = lookupRepository;
        }

        public IBlogPostRepository BlogPostRepository { get; private set; }
        public ILookupRepository LookupRepository { get; private set; }

        public void MarkAsDeleted(BlogPost blogPost, int userId)
        {
            var deletionUser = LookupRepository.LoadForId<BlogUser>(userId);

            if (deletionUser == null)
                throw new UnknownUserException(userId);

            blogPost.MarkAsDeleted(deletionUser);
            BlogPostRepository.SaveOrUpdate(blogPost);
        }
    }
}
