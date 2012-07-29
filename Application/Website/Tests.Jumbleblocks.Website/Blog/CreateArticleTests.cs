using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Testing.Web;
using Moq;
using System.Web.Mvc;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Domain.Blog.Deletion;
using Jumbleblocks.Website.Configuration;
using Tests.Jumbleblocks.Website.Helpers;
using Jumbleblocks.Domain;
using System.Linq.Expressions;
using Jumbleblocks.Website.Models.BlogPost;
using Jumbleblocks.Core.Security;

namespace Tests.Jumbleblocks.Website.Blog
{
    [TestFixture]
    public class CreateArticleTests
    {
        [Test]
        public void BlogPostController_CreateNew_Returns_ViewResult()
        {
            var controller = MockCreators.CreateBlogPostController();
            var result = controller.CreateNew();

            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

        [Test]
        public void BlogPostController_CreateNew_Returns_CreateEdit_ArticleView()
        {
            var controller = MockCreators.CreateBlogPostController();
            var result = controller.CreateNew() as ViewResult;

            result.ViewName.ShouldEqual("CreateEdit");
        }

        [Test]
        public void BlogPostController_Save_Returns_RedirectToRouteResult()
        {
            var model = new CreateEditModel()
            {
                Title = "ABC",
                Description = "ABC",
                FullArticle = "ABC",
                ImageId = 1,
                SeriesName = "ABC",
                TagTexts = "ABC"
            };

            var controller = MockCreators.CreateBlogPostController();
            var result = controller.Save(model);

            result.ShouldBeInstanceOfType(typeof(RedirectToRouteResult));
        }

        [Test]
        public void BlogPostController_Save_Calls_Save_On_BlogPostRepository()
        {
            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Verifiable();

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);
            
            var model = new CreateEditModel()
            {
                Title = "ABC",
                Description = "ABC",
                FullArticle = "ABC",
                ImageId = 1,
                SeriesName = "ABC",
                TagTexts ="ABC"
            };

            controller.Save(model);

            mockedBlogPostRepository.Verify(r => r.SaveOrUpdate(It.IsAny<BlogPost>()), Times.Once());
        }

        [Test]
        public void BlogPostController_Save_GIVEN_BlogPost_Has_Title_ABC_THEN_Saves_BlogPost_With_Title_ABC()
        {
            const string Title = "ABC";

            BlogPost savedBlogPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedBlogPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var model = new CreateEditModel()
            {
                Title = Title,
                Description = "DEF",
                FullArticle = "HIJ",
                ImageId = 1,
                SeriesName = "KLM",
                TagTexts = "NOP"
            };

            controller.Save(model);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.Title.ShouldEqual(Title);
        }

        [Test]
        public void BlogPostController_Save_GIVEN_BlogPost_Has_Description_ABC_THEN_Saves_BlogPost_With_Description_ABC()
        {
            const string Description = "ABC";

            BlogPost savedBlogPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedBlogPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = Description,
                FullArticle = "DEF",
                ImageId = 1,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            controller.Save(model);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.Description.ShouldEqual(Description);
        }

        [Test]
        public void BlogPostController_Save_GIVEN_BlogPost_Has_FullArticle_ABC_THEN_Saves_BlogPost_With_FullArticle_ABC()
        {
            const string FullArticle = "ABC";

            BlogPost savedBlogPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedBlogPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = FullArticle,
                ImageId = 1,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            controller.Save(model);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.FullArticle.ShouldEqual(FullArticle);
        }

        [Test]
        public void BlogPostController_Save_GIVEN_ImageId_Is_1_THEN_Calls_ILookupRepository_LoadForId_ImageReference_With_Id_1()
        {
            const int ImageId = 1;

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Verifiable();
            
            var controller = MockCreators.CreateBlogPostController(lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            controller.Save(model);

            mockedLookupRepository.Verify(r => r.LoadForId<ImageReference>(ImageId), Times.Once());
        }

        [Test]
        public void BlogPostController_Save_GIVEN_ILookupRepository_LoadForId_ImageReference_Contains_Entry_For_Id_1_WHEN_ImageId_Is_1_THEN_Sets_The_ImageReference_On_The_BlogPost_To_That_Image()
        {
            const int ImageId = 1;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            BlogPost savedBlogPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedBlogPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            controller.Save(model);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.ImageReference.ShouldEqual(imageReference);
        }

        [Test]
        public void BlogPostController_Save_WHEN_ImageId_Is_Less_Than_1_THEN_Sets_ImageId_To_1()
        {
            const int ExpectedImageId = 1;

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ExpectedImageId)).Verifiable();
            
            var controller = MockCreators.CreateBlogPostController(lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = -1,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            controller.Save(model);

            mockedLookupRepository.Verify(r => r.LoadForId<ImageReference>(ExpectedImageId), Times.Once());
        }

        [Test]
        public void BlogPostController_Save_WHEN_ILookupRepository_LoadForId_ImageReference_Throws_Exception_THEN_Returns_CreateEdit_View()
        {
            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(It.IsAny<int>())).Throws<Exception>();

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()));

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = 1,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            var result = controller.Save(model) as ViewResult;

            result.ViewName.ShouldEqual("CreateEdit");
        }


        [Test]
        public void BlogPostController_Save_WHEN_ILookupRepository_LoadForId_ImageReference_Throws_Exception_THEN_Returns_ErrorMessage_On_ViewBag()
        {
            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(It.IsAny<int>())).Throws<Exception>();

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()));

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = 1,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            var result = controller.Save(model) as ViewResult;

            string errorMessage = result.ViewBag.ErrorMessage;

            errorMessage.ShouldNotBeNullOrEmpty();
            errorMessage.ShouldEqual("Exception occured while saving blog post. Please try again.");
        }

        [Test]
        public void BlogPostController_Save_WHEN_BlogPostRepository_Throws_Exception_THEN_Returns_CreateEdit_View()
        {
            const int ImageId = 1;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Throws<Exception>();

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            var result = controller.Save(model) as ViewResult;

            result.ViewName.ShouldEqual("CreateEdit");
        }

        [Test]
        public void BlogPostController_Save_WHEN_BlogPostRepository_Throws_Exception_THEN_Returns_ErrorMessage_On_ViewBag()
        {
            const int ImageId = 1;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Throws<Exception>();

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = "KLM"
            };

            var result = controller.Save(model) as ViewResult;

            string errorMessage = result.ViewBag.ErrorMessage;

            errorMessage.ShouldNotBeNullOrEmpty();
            errorMessage.ShouldEqual("Exception occured while saving blog post. Please try again.");
        }

        [Test]
        public void BlogPostController_Save_WHEN_No_Tags_THEN_Saves_No_Tags()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);
            
            var model = new CreateEditModel()
            {
                Title = "Title",
                Description = "Description",
                FullArticle = "Full Article",
                ImageId = ImageId,
                SeriesName = "Series",
                TagTexts = String.Empty
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Tags.Count().ShouldEqual(0);            
        }


        [Test]
        public void BlogPostController_Save_GIVEN_TagRepository_Has_Tag_ABC_WHEN_Tag_ABC_THEN_Loads_ABC_Tag_From_TagRepository_AND_Adds_To_BlogPost()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;
            Tag tag = new Tag { Text = "ABC" };

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), tag.Text)).Returns(tag);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);
            
            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = tag.Text
            };

            controller.Save(model);
            
            savedPost.ShouldNotBeNull();
            savedPost.Tags.First().ShouldEqual(tag);
        }

        [Test]
        public void BlogPostController_Save_GIVEN_TagRepository_Does_Not_Have_Tag_ABC_WHEN_Tag_ABC__WHEN_Tag_ABC_THEN_Creates_New_Tag_ABC_AND_Adds_To_BlogPost()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;
            Tag tag = new Tag { Text = "ABC" };

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), tag.Text)).Returns(null as Tag);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = tag.Text
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Tags.First().ShouldNotEqual(tag);
            savedPost.Tags.First().Text.ShouldEqual(tag.Text);
        }

        [Test]
        public void BlogPostController_Save_WHEN_viewModel_Has_Two_Tags_Seperated_By_SemiColon_THEN_Adds_Both_To_Saved_BlogPost()
        {
            const string Tag1Text = "tag1";
            const string Tag2Text = "tag2";

            const int ImageId = 1;
            BlogPost savedPost = null;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), It.IsAny<string>())).Returns(null as Tag);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);
            
            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = String.Format("{0};{1}", Tag1Text, Tag2Text)
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Tags.Count().ShouldEqual(2);
            savedPost.Tags.ShouldContain( t => t.Text == Tag1Text, String.Format("Does not contain {0}", Tag1Text));
            savedPost.Tags.ShouldContain( t => t.Text == Tag2Text, String.Format("Does not contain {0}", Tag2Text));
        }

        [Test]
        public void BlogPostController_Save_WHEN_TagTexts_Is_Null_THEN_Still_Saves_BlogPost()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                SeriesName = "HIJ",
                TagTexts = null
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
        }
                
        [Test]
        public void BlogPostController_Save_WHEN_No_Series_THEN_Series_On_Saved_Object_Is_Null()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "Title",
                Description = "Description",
                FullArticle = "Full Article",
                ImageId = ImageId,
                TagTexts = String.Empty
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Series.ShouldBeNull();
        }


        [Test]
        public void BlogPostController_Save_GIVEN_SeriesRepository_Has_Series_ABC_WHEN_Series_ABC_THEN_Loads_ABC_Series_From_SeriesRepository_AND_Adds_To_BlogPost()
        {
            const int ImageId = 1;
            BlogPost savedPost = null;
            Series series = new Series { Name = "ABC" };

            var imageReference = new ImageReference(ImageId, "~/1.jpg");

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForId<ImageReference>(ImageId)).Returns(imageReference);
            mockedLookupRepository.Setup(r => r.LoadForDescription(It.IsAny<Expression<Func<Series, string>>>(), series.Name)).Returns(series);

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = ImageId,
                TagTexts = String.Empty,
                SeriesName = series.Name
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Series.ShouldEqual(series);
        }

        [Test]
        public void BlogPostController_Save_WHEN_Series_Is_Null_THEN_Does_Not_Call_SeriesRepository_LoadForName()
        {
            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();

            Expression<Func<ILookupRepository, Series>> verifiableMethod = r => r.LoadForDescription(It.IsAny<Expression<Func<Series, string>>>(), It.IsAny<string>());
            mockedLookupRepository.Setup(verifiableMethod).Verifiable();

            var controller = MockCreators.CreateBlogPostController(lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = 1,
                TagTexts = String.Empty,
                SeriesName = null
            };

            controller.Save(model);

            mockedLookupRepository.Verify(verifiableMethod, Times.Never());
        }

        [Test]
        public void BlogPostController_Save_WHEN_Series_Is_EmptyString_THEN_Does_Not_Call_SeriesRepository_LoadForName()
        {
            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();

            Expression<Func<ILookupRepository, Series>> verifiableMethod = r => r.LoadForDescription(It.IsAny<Expression<Func<Series, string>>>(), It.IsAny<string>());
            mockedLookupRepository.Setup(verifiableMethod).Verifiable();

            var controller = MockCreators.CreateBlogPostController(lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = 1,
                TagTexts = String.Empty,
                SeriesName = String.Empty
            };

            controller.Save(model);

            mockedLookupRepository.Verify(verifiableMethod, Times.Never());
        }

        [Test]
        public void BlogPostController_Save_GIVEN_SeriesRepository_Does_Not_Have_Series_ABC_WHEN_Series_Is_ABC__THEN_Creates_New_Series_ABC_AND_Adds_To_BlogPost()
        {      
            const string SeriesName = "ABC";
            BlogPost savedPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(bp => savedPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            mockedLookupRepository.Setup(r => r.LoadForDescription<Series>(It.IsAny<Expression<Func<Series, string>>>(), It.IsAny<string>())).Returns(null as Series);

            var controller = MockCreators.CreateBlogPostController(blogPostRepository:mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object);

            var model = new CreateEditModel()
            {
                Title = "123",
                Description = "456",
                FullArticle = "ABC",
                ImageId = 2,
                SeriesName = SeriesName
            };

            controller.Save(model);

            savedPost.ShouldNotBeNull();
            savedPost.Series.ShouldNotBeNull();
            savedPost.Series.Name.ShouldEqual(SeriesName);
        }

        [Test]
        public void BlogPostController_Save_GIVEN_LoggedIn_User_Has_Id_1_WHEN_UserRepository_Has_User_With_Id_1_THEN_Sets_Author_On_BlogPost_To_That_User()
        {
            const int UserId = 1;
            const string Forename = "Joe";
            const string Surname = "Blogs";

            BlogPost savedBlogPost = null;

            var mockedBlogPostRepository = MockCreators.CreateMockedBlogPostRepository();
            mockedBlogPostRepository.Setup(r => r.SaveOrUpdate(It.IsAny<BlogPost>())).Callback<BlogPost>(bp => savedBlogPost = bp);

            var mockedLookupRepository = MockCreators.CreateMockedLookupRepository();
            var mockedPrincipal = MockCreators.CreateMockedPrincipalAndAddBlogUserToLookUp(mockedLookupRepository, UserId, Forename, Surname);
            
            var controller = MockCreators.CreateBlogPostController(blogPostRepository: mockedBlogPostRepository.Object, lookupRepository: mockedLookupRepository.Object, principal: mockedPrincipal.Object);

            var viewModel = new CreateEditModel
            {
                 Title = "test",
                 Description = "test",
                 FullArticle = "Test",
                 ImageId = 1
            };

            controller.Save(viewModel);

            savedBlogPost.ShouldNotBeNull();
            savedBlogPost.Author.ShouldNotBeNull();

            savedBlogPost.Author.Id.ShouldEqual(UserId);
            savedBlogPost.Author.Forenames.ShouldEqual(Forename);
            savedBlogPost.Author.Surname.ShouldEqual(Surname);
        }

        //TODO: test when can not find user
    }
}
