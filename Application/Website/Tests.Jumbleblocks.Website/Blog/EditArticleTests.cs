using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using Moq;


using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Web.Wane;
using System.Web.Mvc;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Domain.Blog.Deletion;
using Tests.Jumbleblocks.Website.Helpers;
using System.Linq.Expressions;
using Jumbleblocks.Website.Models.BlogPost;


namespace Tests.Jumbleblocks.Website.Blog
{
    [TestFixture]
    public class EditArticleTests
    {
        [Test]
        public void BlogPostController_Edit_Returns_ViewResult()
        {
            const int BlogPostId = 1;

            var imageReference = new ImageReference(1, "/noimage.jpg");
            var author = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Blogs" };

            var blogPost = new BlogPost("Test", "This is the description", "Not much in this article", imageReference, DateTime.Now, author);
            blogPost.SetProperty("Id", BlogPostId);
            blogPost.UpdateSeries(new Series { Name = "Series A" });
            blogPost.UpdateTags(new Tag[] { new Tag { Text = "Tag 1" } });

            
            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            mockedBlogPostRepository.Setup(r => r.Load(BlogPostId)).Returns(blogPost);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var result = controller.Edit(1);

            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

        [Test]
        public void BlogPostController_Edit_Returns_View_Named_CreateEdit()
        {
            const int BlogPostId = 1;

            var imageReference = new ImageReference(1, "/noimage.jpg");
            var author = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Blogs" };

            var blogPost = new BlogPost("Test", "This is the description", "Not much in this article", imageReference, DateTime.Now, author);
            blogPost.SetProperty("Id", BlogPostId);
            blogPost.UpdateSeries(new Series { Name = "Series A" });
            blogPost.UpdateTags(new Tag[] { new Tag { Text = "Tag 1" } });

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.Load(BlogPostId)).Returns(blogPost);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Edit(1) as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("CreateEdit");
        }

        [Test]
        public void BlogPostController_Edit_GIVEN_BlogPostRepository_Contains_BlogPost_With_ID_1_WHEN_blogPostId_Is_1_THEN_Returns_ViewModel_For_BlogPost()
        {
            const int BlogPostId = 1;

            var imageReference = new ImageReference(1, "/noimage.jpg");
            var author = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Blogs" };

            var blogPost = new BlogPost("Test", "This is the description", "Not much in this article", imageReference, DateTime.Now, author);
            blogPost.SetProperty("Id", BlogPostId);
            blogPost.UpdateSeries(new Series { Name = "Series A" });
            blogPost.UpdateTags(new Tag[] { new Tag { Text = "Tag 1" } });

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.Load(BlogPostId)).Returns(blogPost);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Edit(blogPost.Id.Value) as ViewResult;
                 
            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(CreateEditModel));

            var model = (CreateEditModel)result.Model;

            model.BlogPostId.ShouldEqual(blogPost.Id);
            model.PublishedDate.ShouldEqual(blogPost.PublishedDate);
            model.Title.ShouldEqual(blogPost.Title);
            model.Description.ShouldEqual(blogPost.Description);
            model.FullArticle.ShouldEqual(blogPost.FullArticle);
            model.ImageId.ShouldEqual(blogPost.ImageReference.Id.Value);
            model.SeriesName.ShouldEqual(blogPost.Series.Name);
        }

        [Test]
        public void BlogPostController_Save_WHEN_viewModel_Has_Id_THEN_Loads_Existing_Article_From_BlogPostRepository_Updates_And_Saves()
        {
            const int BlogPostId = 1;
            const int ImageId = 1;
            const int AuthorId = 1;

            const int OldSeriesId = 1;
            const int NewSeriesId = 2;
            const string NewSeriesName = "Series B";

            const int OldTagId = 1;
            const int NewTagId = 2;

            Series newSeries = new Series{Name = NewSeriesName};
            newSeries.SetProperty(s => s.Id, NewSeriesId);

            Tag oldTag = new Tag { Text = "Tag 1" };
            oldTag.SetProperty(t => t.Id, OldTagId);

            Tag newTag = new Tag { Text = "Tag 2" };
            newTag.SetProperty(t => t.Id, NewTagId);

            BlogPost savedBlogPost = null;

            var imageReference = new ImageReference(ImageId, "/noimage.jpg");
            var author = new BlogUser { Id = AuthorId, Forenames = "Joe", Surname = "Blogs" };

            var blogPost = new BlogPost("Test", "This is the description", "Not much in this article", imageReference, DateTime.Now, author);
            blogPost.SetProperty("Id", BlogPostId);
            
            var series = new Series { Name = "Series A" };
            series.SetProperty(s => s.Id, OldSeriesId);

            blogPost.UpdateSeries(series); 
            blogPost.UpdateTags(new Tag[] { new Tag { Text = "Tag 1" } });

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            mockedBlogPostRepository.Setup(r => r.LoadFullArticle(BlogPostId)).Returns(blogPost).Verifiable();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();

            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Series>(It.IsAny<Expression<Func<Series, string>>>(), NewSeriesName)).Returns(newSeries);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), oldTag.Text)).Returns(oldTag);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), newTag.Text)).Returns(newTag);


            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var updatedBlogPost = new CreateEditModel
            {
                BlogPostId = BlogPostId,
                Description = "A different description",
                FullArticle = "A different Full Article",
                ImageId = ImageId,
                PublishedDate = blogPost.PublishedDate,
                SeriesName = NewSeriesName,
                TagTexts = "Tag 2",
                Title = blogPost.Title
            };

            controller.Save(updatedBlogPost);

            mockedBlogPostRepository.Verify(r => r.LoadFullArticle(BlogPostId), Times.Once());
            savedBlogPost.ShouldNotBeNull();

            savedBlogPost.Id.ShouldEqual(BlogPostId);
            savedBlogPost.Description.ShouldEqual(updatedBlogPost.Description);
            savedBlogPost.FullArticle.ShouldEqual(updatedBlogPost.FullArticle);
            savedBlogPost.ImageReference.ShouldEqual(imageReference);
            savedBlogPost.PublishedDate.ShouldEqual(updatedBlogPost.PublishedDate.Value);
            savedBlogPost.Series.ShouldEqual(newSeries);
            savedBlogPost.Tags.ShouldContain((t) => t.TextEquals(newTag));
        }
    }
}
