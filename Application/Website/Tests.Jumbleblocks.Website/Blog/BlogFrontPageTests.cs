using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Domain.Blog;
using Moq;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Website.Controllers.Blog;
using Tests.Jumbleblocks.Website.Helpers;
using Jumbleblocks.Website.Models.BlogPost;

namespace Tests.Jumbleblocks.Website.Blog
{
    /// <summary>
    /// Tests for retrieving Blog posts
    /// </summary>
    [TestFixture]
    public class BlogFrontPageTests
    {
        [Test]
        public void Index_Returns_ViewResult()
        {
            var controller = MockCreators.CreateBlogPostController();
            controller.Index().ShouldBeInstanceOfType(typeof(ViewResult), "Normal requests should return ViewResult");           
        }

        [Test]
        public void Index_Returns_View_Named_FrontPage()
        {
            var controller = MockCreators.CreateBlogPostController();
            var result = controller.Index() as ViewResult;

            result.ViewName.ShouldEqual("FrontPage");
        }

        [Test]
        public void Index_ViewBag_Title_Property_Is_Jumbleblocks()
        {            
            const string title = "Jumbleblocks";

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(title: title);

            var controller = MockCreators.CreateBlogPostController(configurationReader: mockedConfigurationReader.Object);
            var result = controller.Index() as ViewResult;

            ((string)result.ViewBag.Title).ShouldEqual(title);
        }
        
        [Test]
        public void Index_Returns_BlogViewModel()
        {
            var controller = MockCreators.CreateBlogPostController();
            var result = controller.Index() as ViewResult;

            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel));
        }

        [Test]
        public void Index_GIVEN_BlogPostRepository_Has_1_BlogSummary_THEN_Returns_BlogViewModel_Containing_1_Item()
        {
            var blogPosts = new BlogPost[]{  new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser()) };

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(blogPosts);
            mockedBlogPostRepository.Setup(r => r.Count).Returns(blogPosts.Length);
            
            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var result = controller.Index() as ViewResult;

            ((FrontPageModel)result.Model).Summaries.Count().ShouldEqual(1);
        }

        [Test]
        public void Index_GIVEN_BlogPostRepository_Has_2_BlogSummary_THEN_Returns_BlogViewModel_Containing_2_Items()
        {
            var blogPosts = new BlogPost[]
            {
                new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser()),
                new BlogPost("test2", "test2", "test2", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser())
            };

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(blogPosts);
            mockedBlogPostRepository.Setup(r => r.Count).Returns(blogPosts.Length);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var result = controller.Index() as ViewResult;

            ((FrontPageModel)result.Model).Summaries.Count().ShouldEqual(2);
        }

        [Test]
        public void Index_GIVEN_BlogPostRepository_Has_1_BlogSummary_THEN_Sets_BlogSummaryViewModel_To_Have_Same_Values()
        {
            const string title = "Title";
            const string description = "Description";
            var url = new Uri("http://www.jumbleblocks.co.uk/noimage.jpg");
            var publishedDate = DateTime.Now;
            var author = new BlogUser { Forenames = "Authors", Surname = "Name" };
            
            var blogPosts = new BlogPost[]
            {
                new BlogPost(title, description, "full article", new ImageReference(1, url:url.AbsoluteUri), publishedDate, author)
            };

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(blogPosts);
            mockedBlogPostRepository.Setup(r => r.Count).Returns(blogPosts.Length);


            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var result = controller.Index() as ViewResult;

            var model = (FrontPageModel)result.Model;
            model.Summaries.First().Title.ShouldEqual(title);
            model.Summaries.First().Description.ShouldEqual(description);
            model.Summaries.First().ImageUrl.ShouldEqual(url);
            model.Summaries.First().PublishedDate.ShouldEqual(publishedDate);
            model.Summaries.First().AuthorsName.ShouldEqual(author.FullName);
        }
        
        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_1_And_NoPage_Specified_THEN_Calls_Repository_GetSummaries_With_numberToSkip_0_AND_Count_1()
        {
            const int returnCount = 1;    
        
            int countAskedFor = -1;
            int numberToSkip = -1;

            var blogPost =  new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { numberToSkip = toSkip; countAskedFor = count; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(1);

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);

            controller.Index();

            numberToSkip.ShouldEqual(0, "numberToSkip passed through to repository is incorrect");
            countAskedFor.ShouldEqual(returnCount, "count passed through to repository is incorrect");
        }


        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_1_And_Page_Is_1_THEN_Calls_Repository_GetSummaries_With_numberToSkip_0_AND_Count_1()
        {            
            const int returnCount = 1;

            int countAskedFor = -1;
            int numberToSkip = -1;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { numberToSkip = toSkip; countAskedFor = count; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(1);

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader:mockedConfigurationReader.Object);

            controller.Index(1);

            numberToSkip.ShouldEqual(0, "numberToSkip passed through to repository is incorrect");
            countAskedFor.ShouldEqual(returnCount, "count passed through to repository is incorrect");
        }

        [Test]
        public void Index_GIVEN_Configuration_Has_Summaries_Per_Page_Set_To_1_And_Page_Is_2_THEN_Calls_Repository_GetSummaries_With_numberToSkip_1_AND_Count_1()
        {
            const int returnCount = 1;

            int countAskedFor = -1;
            int numberToSkip = -1;

            var blogPost1 = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            var blogPost2 = new BlogPost("test2", "test2", "test2", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost1, blogPost2 })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { numberToSkip = toSkip; countAskedFor = count; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(2);

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: returnCount);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);

            controller.Index(2);

            numberToSkip.ShouldEqual(1, "numberToSkip passed through to repository is incorrect");
            countAskedFor.ShouldEqual(returnCount, "count passed through to repository is incorrect");
        }

        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_1_And_Page_Is_3_THEN_Calls_Repository_GetSummaries_With_numberToSkip_2_AND_Count_1()
        {
            const int returnCount = 1;

            int countAskedFor = -1;
            int numberToSkip = -1;

            var blogPost1 = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            var blogPost2 = new BlogPost("test2", "test2", "test2", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
            var blogPost3 = new BlogPost("test3", "test3", "test3", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost1, blogPost2, blogPost3 })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { numberToSkip = toSkip; countAskedFor = count; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(3);

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: returnCount);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            controller.Index(3);

            numberToSkip.ShouldEqual(2, "numberToSkip passed through to repository is incorrect");
            countAskedFor.ShouldEqual(returnCount, "count passed through to repository is incorrect");
        }

        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_10_AND_Repository_BlogPostCount_Returns_20_THEN_Returns_2_PageLinks()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 20);
            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 10);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);

            var result = controller.Index() as ViewResult;

            result.ShouldNotBeNull("Result is null, or is not instance of ViewResult");
            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel), "Model is result is not BlogViewModel");
            ((FrontPageModel)result.Model).Paging.PageCount.ShouldEqual(2);
        }


        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_12_AND_Repository_BlogPostCount_Returns_20_THEN_Returns_2_PageLinks()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 20);
            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 12);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Index() as ViewResult;

            result.ShouldNotBeNull("Result is null, or is not instance of ViewResult");
            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel), "Model is result is not BlogViewModel");
            ((FrontPageModel)result.Model).Paging.PageCount.ShouldEqual(2);
        }


        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_5_AND_Repository_BlogPostCount_Returns_20_THEN_Returns_4_PageLinks()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 20);
            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 5);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Index() as ViewResult;

            result.ShouldNotBeNull("Result is null, or is not instance of ViewResult");
            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel), "Model is result is not BlogViewModel");
            ((FrontPageModel)result.Model).Paging.PageCount.ShouldEqual(4);
        }

        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_10_AND_Repository_BlogPostCount_Returns_20_WHEN_THEN_page_is_1_THEN_Returns_CurrentPage_As_1()
        {          
            const int page = 1;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 20);
            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 10);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Index(page) as ViewResult;

            result.ShouldNotBeNull("Result is null, or is not instance of ViewResult");
            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel), "Model is result is not BlogViewModel");
            ((FrontPageModel)result.Model).Paging.CurrentPage.ShouldEqual(page);
        }

        [Test]
        public void Index_GIVEN_Configuration_Has_PostsPerPage_Set_To_10_AND_Repository_BlogPostCount_Returns_20_WHEN_THEN_page_is_2_THEN_Returns_CurrentPage_As_2()
        {
            const int page = 2;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 20);
            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(postsPerPage: 10);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Index(page) as ViewResult;

            result.ShouldNotBeNull("Result is null, or is not instance of ViewResult");
            result.Model.ShouldBeInstanceOfType(typeof(FrontPageModel), "Model is result is not BlogViewModel");
            ((FrontPageModel)result.Model).Paging.CurrentPage.ShouldEqual(page);
        }

        [Test]
        public void Index_WHEN_No_Tags_THEN_Calls_GetSummaries_With_Empty_List()
        {
            IEnumerable<string> tagsEvaluated = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { tagsEvaluated = tags; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            controller.Index();

            tagsEvaluated.Count().ShouldEqual(0);
        }

        [Test]
        public void Index_WHEN_Tag_A_THEN_Calls_GetSummaries_Tag_Of_A()
        {
            const string FirstTag = "A"; 
            IEnumerable<string> tagsEvaluated = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { tagsEvaluated = tags; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            controller.Index(1, FirstTag);

            tagsEvaluated.First().ShouldEqual(FirstTag);
        }

        [Test]
        public void Index_WHEN_Tags_A_B_THEN_Calls_GetSummaries_Tag_Of_A_And_B()
        {
            const string FirstTag = "A";
            const string SecondTag = "B";
            IEnumerable<string> tagsEvaluated = null;

            var blogPost = new BlogPost("test", "test", "test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new BlogPost[] { blogPost })
                .Callback<int, int, IEnumerable<string>>((toSkip, count, tags) => { tagsEvaluated = tags; });

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(1);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            controller.Index(1, String.Format("{0}|{1}", FirstTag, SecondTag));

            tagsEvaluated.First().ShouldEqual(FirstTag);
            tagsEvaluated.Last().ShouldEqual(SecondTag);
        }

        [Test]
        public void Index_Passes_Description_To_WaneTextTransformer()
        {
            var mockBlogPostRepository = MockCreators.CreateMockedBlogPostRepository(blogPostCount: 1);

            var mockedWaneTextTransform = MockCreators.CreateMockedWaneTransform();
            mockedWaneTextTransform.Setup(t => t.TransformToRawHtml(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockBlogPostRepository.Object, waneTransform: mockedWaneTextTransform.Object);

            controller.Index();

            mockedWaneTextTransform.Verify(t => t.TransformToRawHtml(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }
    }
}
