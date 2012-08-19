using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Blog;
using Moq;
using Jumbleblocks.Domain.Blog.Deletion;
using Jumbleblocks.Domain;

namespace Tests.Jumbleblocks.Blog.Domain.Deletion
{
    [TestClass]
    public class BlogPostDeleterTests
    {
        protected static ImageReference GetImageReference(int id = 1, string url = "http://www.jumbleblocks.co.uk/noimage.jpg")
        {
            return new ImageReference(id, url);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_blogPostRepository_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var mockedLookupRepository = new Mock<ILookupRepository>();

            new BlogPostDeleter(null, mockedLookupRepository.Object);
        }


       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_lookupRepository_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            new BlogPostDeleter(mockedBlogPostRepository.Object, null);
        }
        

       [TestMethod]
        public void Ctor_SetsAND_BlogPostRepository_Property_To_blogPostRepository_Parameter_LookupRepository_Property_To_lookupRepository_Parameter_()
        {
            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            var mockedLookupRepository = new Mock<ILookupRepository>();

            var blogPostDeleter = new BlogPostDeleter(mockedBlogPostRepository.Object, mockedLookupRepository.Object);

            blogPostDeleter.BlogPostRepository.ShouldEqual(mockedBlogPostRepository.Object);
            blogPostDeleter.LookupRepository.ShouldEqual(mockedLookupRepository.Object);
        }

       [TestMethod]
        public void MarkAsDeleted_GIVEN_Author_With_Id_1_Exists_In_AuthorRepository_WHEN_userId_Is_1_THEN_Saves_BlogPost_With_DeletedByUser_As_Author_With_Id_1()
        {
            const int UserId = 1;

            BlogPost savedBlogPost = null;

            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            var deletionUser = new BlogUser { Id = UserId, Forenames = "Joe", Surname = "Blogs" };

            var mockedLookupRepository = new Mock<ILookupRepository>();
            mockedLookupRepository.Setup(r => r.LoadForId<BlogUser>(It.IsAny<object>())).Returns(deletionUser);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var blogPostDeleter = new BlogPostDeleter(mockedBlogPostRepository.Object, mockedLookupRepository.Object);

            blogPostDeleter.MarkAsDeleted(post, UserId);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.DeletedByUser.ShouldEqual(deletionUser);
        }

       [TestMethod]
        [ExpectedException(typeof(UnknownUserException))]
        public void MarkAsDeleted_GIVEN_AuthorRepository_Has_No_Users_WHEN_userId_Is_1_THEN_Throws_UnknownUserException()
        {
            const int UserId = 1;

            BlogPost savedBlogPost = null;

            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            var mockedLookupRepository = new Mock<ILookupRepository>();
            mockedLookupRepository.Setup(r => r.LoadForId<BlogUser>(It.IsAny<object>())).Returns(null as BlogUser);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var blogPostDeleter = new BlogPostDeleter(mockedBlogPostRepository.Object, mockedLookupRepository.Object);

            blogPostDeleter.MarkAsDeleted(post, UserId);
        }
    }
}
