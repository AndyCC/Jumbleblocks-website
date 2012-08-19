using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using Tests.Jumbleblocks.Website.Helpers;
using Moq;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Core.Security;
using System.Web.Mvc;
using Jumbleblocks.Website.Models.BlogPost;

namespace Tests.Jumbleblocks.Website.Blog
{
    [TestClass]
    public class DeleteArticleTests
    {

        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Has_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_THEN_Saves_BlogPost_With_DeletedDate_Set()
        {
            const int BlogPostId = 1;

            BlogPost savedBlogPost = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            blogPost.SetProperty(bp => bp.Id, BlogPostId);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.LoadFullArticle(BlogPostId)).Returns(blogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, 1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);

            controller.Delete(BlogPostId);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.DeletedDate.ShouldBeWithinLast(new TimeSpan(0, 0, 1));
        }

        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Has_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_And_User_Is_Logged_In_THEN_Saves_BlogPost_With_DeletedByUser_Set_To_Logged_In_User()
        {
            const int UserId = 1;
            const int BlogPostId = 1;

            BlogPost savedBlogPost = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            blogPost.SetProperty(bp => bp.Id, BlogPostId);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.LoadFullArticle(BlogPostId)).Returns(blogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);

            controller.Delete(BlogPostId);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.DeletedByUser.Id.ShouldEqual(UserId);
        }


        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Has_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_THEN_Returns_DeletedModel()
        {
            const int UserId = 1;
            const int BlogPostId = 1;

            BlogPost savedBlogPost = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            blogPost.SetProperty(bp => bp.Id, BlogPostId);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.LoadFullArticle(BlogPostId)).Returns(blogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);

            var result = controller.Delete(BlogPostId);

            result.ShouldBeInstanceOfType(typeof(ViewResult));
            ((ViewResult)result).Model.ShouldBeInstanceOfType(typeof(DeletedModel));

            var model = (DeletedModel)((ViewResult)result).Model;
            model.BlogPostId.ShouldEqual(BlogPostId);
            model.Title.ShouldEqual(blogPost.Title);
        }


        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Has_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_THEN_Returns_Deleted_View()
        {
            const int UserId = 1;
            const int BlogPostId = 1;

            BlogPost savedBlogPost = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            blogPost.SetProperty(bp => bp.Id, BlogPostId);

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.LoadFullArticle(BlogPostId)).Returns(blogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);

            var result = controller.Delete(BlogPostId);

            result.ShouldBeInstanceOfType(typeof(ViewResult));
            ((ViewResult)result).ViewName.ShouldEqual("Deleted");
        }

        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Does_Not_Have_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_THEN_Does_Not_Call_Save_On_BlogPostRepository()
        {
            const int UserId = 1;
            const int BlogPostId = 1;

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.Load(BlogPostId)).Returns(null as BlogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Verifiable();

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);
            controller.Request.SetRequestReferrer("http://localhost/");

            controller.Delete(BlogPostId);

            mockedBlogPostRepository.Verify(r => r.SaveOrUpdate(It.IsAny<BlogPost>()), Times.Never());
        }


        [TestMethod]
        public void Delete_GIVEN_BlogPostRepository_Does_Not_Have_BlogPost_With_Id_1_WHEN_blogPostId_Parameter_Is_1_THEN_Returns_To_Referer()
        {
            const int UserId = 1;
            const int BlogPostId = 1;
            const string RefererUrl = "http://localhost/";

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.Load(BlogPostId)).Returns(null as BlogPost);
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Verifiable();

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            Mock<IJumbleblocksPrincipal> mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            controller.SetPrincipal(mockedPrincipal.Object);
            controller.Request.SetRequestReferrer(RefererUrl);

            var result = controller.Delete(BlogPostId) as RedirectResult;

            result.ShouldNotBeNull();
            result.Permanent.ShouldBeFalse();
            result.Url.ShouldEqual(RefererUrl);
        }

    }
}
