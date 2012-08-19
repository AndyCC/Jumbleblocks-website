using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using Jumbleblocks.Core.Configuration;
using Moq;
using System.Web.Mvc;
using System.Collections.Generic;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Web.Wane;
using System.Web;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Domain.Blog.Deletion;
using Jumbleblocks.Website.Configuration;
using Tests.Jumbleblocks.Website.Helpers;
using Jumbleblocks.Website.Models.BlogPost;


namespace Tests.Jumbleblocks.Website.Blog
{
    /// <summary>
    /// Tests to do with show an Blog article
    /// </summary>
    [TestClass]
    public class ShowArticleTests 
    {       
       [TestMethod]
        public void Show_Returns_View_Result()
        {
            var controller = MockCreators.CreateBlogPostController();  
            var result = controller.Show(2012, 1, 1, "Test");

            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

       [TestMethod]
        public void Show_Returns_View_With_Name_FullBlogPost()
        {
            var controller = MockCreators.CreateBlogPostController(); 
            var result = controller.Show(2012, 1, 1, "Test");

            result.ShouldBeInstanceOfType(typeof(ViewResult));
            ((ViewResult)result).ViewName.ShouldEqual("FullBlogPost");
        }

       [TestMethod]
        public void Show_Returns_View_With_ViewModel_Type_ArticleViewModel()
        {
            var controller = MockCreators.CreateBlogPostController(); 
            var result = controller.Show(2012, 1, 1, "Test");

            result.ShouldBeInstanceOfType(typeof(ViewResult), "result not a ViewResult");
            ((ViewResult)result).Model.ShouldBeInstanceOfType(typeof(FullBlogPostModel), "View model is not correct type");
        }

       [TestMethod]
        public void Show_WHEN_Year_Is_2012_Month_Is_12_AND_Day_Is_12_And_Title_is_ABC_THEN_Calls_Repository_Load_Method_With_Those_Values()
        {
            const int cYear = 2012;
            const int cMonth = 1;
            const int cDay = 1;
            const string cTitle = "ABC";

            int calledWithYear = -1;
            int calledWithMonth = -1;
            int calledWithDay = -1;
            string calledWithTitle = String.Empty;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            mockedBlogPostRepository
                .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new BlogPost("Test", "Description", "article", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser()))
                .Callback<int, int, int, string>((y, m, d, t) => { calledWithYear = y; calledWithMonth = m; calledWithDay = d; calledWithTitle = t; });

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            controller.Show(cYear, cMonth, cDay, cTitle);

            calledWithYear.ShouldEqual(cYear);
            calledWithMonth.ShouldEqual(cMonth);
            calledWithDay.ShouldEqual(cDay);
            calledWithTitle.ShouldEqual(cTitle);
        }

       [TestMethod]
        public void Show_GIVEN_Configuration_Returns_Jumbleblocks_For_Page_Title_WHEN_parameter_Title_Is_ABC_THEN_Sets_Title_To_Jumbleblocks_Colon_ABC()
        {
            const string configTitle = "Jumbleblocks";
            const string titleName = "ABC";

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(configTitle);
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            mockedBlogPostRepository
              .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
              .Returns(new BlogPost(titleName, "Description", "article", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser()));


            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Show(2012, 01, 01, titleName) as ViewResult;

            result.ShouldNotBeNull("Result not returned or not ViewResult");
            ((string)result.ViewBag.Title).ShouldEqual(String.Format("{0} : {1}", configTitle, titleName));

        }

       [TestMethod]
        public void WHEN_No_BlogPost_Returned_THEN_Returns_ViewName_BlogPostNotFound()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            mockedBlogPostRepository
                .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(null as BlogPost);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Show(2012, 10, 10, "Test") as ViewResult;

            ((ViewResult)result).ViewName.ShouldEqual("BlogPostNotFound");
        }

       [TestMethod]
        public void Show_GIVEN_Configuration_Returns_Jumbleblocks_For_Page_Title_And_title_parameter_Is_ABC_WHEN_No_BlogPost_Returned_From_Repository_THEN_Returns_Title_Jumbleblocks_colon_ABC_Not_Found()
        {
            const string configTitle = "Jumbleblocks";
            const string titleName = "Test";

            var mockedConfigurationReader = MockCreators.CreateMockedConfigurationReader(title: configTitle);
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            mockedBlogPostRepository
               .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .Returns(null as BlogPost);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, configurationReader: mockedConfigurationReader.Object);
            var result = controller.Show(2012, 01, 01, titleName) as ViewResult;

            result.ShouldNotBeNull("Result not returned or not ViewResult");
            ((string)result.ViewBag.Title).ShouldEqual(String.Format("{0} : {1} Not Found", configTitle, titleName));
        }

       [TestMethod]
        public void Show_WHEN_Title_On_BlogPost_Is_ABC_THEN_Sets_Title_On_ViewModel_To_ABC()
        {
            const string title = "ABC";
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            var post = new BlogPost(title, "Test", "article", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

           mockedBlogPostRepository
               .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .Returns(post);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Show(2012, 01, 01, title) as ViewResult;

            ((FullBlogPostModel)result.Model).Title.ShouldEqual(title);
        }


       [TestMethod]
        public void Show_WHEN_FullArticle_On_BlogPost_Is_ABC_THEN_Sets_FullArticle_On_ViewModel_To_ABC()
        {
            const string title = "Test";
            const string fullText = "ABC";
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            
            var post = new BlogPost(title, "Test", fullText, new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());

            mockedBlogPostRepository
               .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .Returns(post);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Show(2012, 01, 01, title) as ViewResult;

            ((FullBlogPostModel)result.Model).FullArticle.ShouldEqual(fullText);
        }

       [TestMethod]
        public void Show_WHEN_PublishedDate_On_BlogPost_Is_01_01_2012_THEN_Sets_PublishedDate_On_ViewModel_To_01_01_2012()
        {
            const string title = "Test";
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();

            var publishedDate = new DateTime(2012, 01, 01);

            var post = new BlogPost(title, "Test", "fullText", new ImageReference(1, "/noimage.jpg"), publishedDate, new BlogUser());

            mockedBlogPostRepository
               .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .Returns(post);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Show(publishedDate.Year, publishedDate.Month, publishedDate.Day, title) as ViewResult;

            ((FullBlogPostModel)result.Model).PublishedDate.ShouldEqual(publishedDate);
        }

       [TestMethod]
        public void Show_WHEN_Authors_Name_On_BlogPost_Is_Jonny_English_THEN_Sets_Authors_Name_On_ViewModel_To_Jonny_English()
        {
            const string title = "Test";

            const string forename = "Jonny";
            const string surname = "English";

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();         

            var post = new BlogPost(title, "Test", "fullText", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser { Forenames = forename, Surname = surname });

            mockedBlogPostRepository
               .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
               .Returns(post);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            var result = controller.Show(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, title) as ViewResult;

            ((FullBlogPostModel)result.Model).AuthorsName.ShouldEqual(String.Format("{0} {1}", forename, surname));
        }

        //[Test]
        //public void GIVEN_Post_Has_CommentsId_THEN_CommentsId_On_ViewModel_Is_Same_As_On_Post()
        //{
        //    const string title = "Test";
        //    var mockedConfigurationReader = CreateConfigurationReader();
        //    var mockedRepository = CreateBlogPostRepository();

        //    var post = new BlogPost(title, "Test", "fullText", GetImageReference(), DateTime.Now, new Author { Forenames = "me", Surname = "you" });

        //    mockedRepository
        //       .Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
        //       .Returns(post);

        //    var controller = CreateController(mockedConfigurationReader.Object, mockedRepository.Object, CreateMockedImageReferenceRepository().Object,
        //        CreateMockedTagRepository().Object, CreateMockedSeriesRepository().Object, CreateMockedWaneTransform().Object);
        //    var result = controller.Show(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, title) as ViewResult;

        //    ((ArticleViewModel)result.Model).CommentsId.ShouldEqual(post.CommentsIdentifier);
        //}
     }
}
