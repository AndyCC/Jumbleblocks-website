using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog.Deletion
{
    /// <summary>
    /// Deletes a blog post
    /// </summary>
    public interface IBlogPostDeleter
    {
        /// <summary>
        /// Marks a blog post as deleted 
        /// </summary>
        /// <param name="blogPost">blog post to mark as deleted</param>
        /// <param name="userId">id of user deleting blogpost</param>
        void MarkAsDeleted(BlogPost blogPost, int userId);
    }
}
