using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;

using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Web.Wane;
using Moq;
using System.Web.Mvc;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Domain.Blog.Deletion;
using Tests.Jumbleblocks.Website.Helpers;
using Jumbleblocks.Website.Models.BlogPost;

namespace Tests.Jumbleblocks.Website.Blog
{
    [TestFixture]
    public class ListArticleTests
    {
        [Test]
        public void BlogPostController_List_Returns_ViewResult()
        {
            var controller = MockCreators.CreateBlogPostController();

            var result = controller.List();

            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

        [Test]
        public void BlogPostController_List_Returns_View_Named_BlogPostListing()
        {
            var controller = MockCreators.CreateBlogPostController();

            var result = controller.List() as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("BlogPostListing");
        }

        [Test]
        public void BlogPostController_List_GIVEN_BlogPost_Has_1_Item_THEN_Returns_1_ArticleListingViewModel()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var result = controller.List() as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(IEnumerable<BlogPostListingItemModel>));

            var viewModel = (IEnumerable<BlogPostListingItemModel>)result.Model;
            viewModel.Count().ShouldEqual(1);            
        }

        [Test]
        public void BlogPostController_List_GIVEN_BlogPost_Has_1_Item_THEN_Returns_1_ArticleListingViewModel_With_Data_From_Post()
        {
            const int BlogPostId = 1;

            var imageReference = new ImageReference(1, "/noimage.jpg");
            var author = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Blogs" };

            var blogPost = new BlogPost("Test", "This is the description", "Not much in this article", imageReference, DateTime.Now, author);
            blogPost.SetProperty("Id", BlogPostId);
            blogPost.UpdateSeries(new Series { Name = "Series A" });
            blogPost.UpdateTags(new Tag[] { new Tag { Text = "Tag 1" } });

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            mockedBlogPostRepository.Setup(r => r.LoadAll()).Returns(new BlogPost[] { blogPost });

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.List() as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(IEnumerable<BlogPostListingItemModel>));

            var viewModelForPost = ((IEnumerable<BlogPostListingItemModel>)result.Model).First();

            viewModelForPost.BlogPostId.ShouldEqual(blogPost.Id.Value);
            viewModelForPost.Title.ShouldEqual(blogPost.Title);
            viewModelForPost.PublishedDate.ShouldEqual(blogPost.PublishedDate);
            viewModelForPost.AuthorsName.ShouldEqual(blogPost.Author.FullName);
        }

        //TODO: paging of listing
    }
}
