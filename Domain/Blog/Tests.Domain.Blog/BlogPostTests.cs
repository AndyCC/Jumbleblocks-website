using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Core;
using Jumbleblocks.Domain.Blog;
using System.Threading;

namespace Tests.Jumbleblocks.Blog.Domain
{
    /// <post>
    /// Unit tests for BlogPostSummary
    /// </post>
    [TestClass]
    public class BlogPostTests 
    {
        protected static ImageReference GetImageReference(int id = 1, string url = "http://www.jumbleblocks.co.uk/noimage.jpg")
        {
            return new ImageReference(id, url);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_title_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost(null, "a", "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_title_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost(String.Empty, "a", "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
        }

       [TestMethod]
        public void Ctor_WHEN_title_Is_Abc_THEN_Sets_Title_Property_To_Abc()
        {
            const string title = "ABC";

            var post = new BlogPost(title, "a", "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
            post.Title.ShouldEqual(title);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_description_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost("a", null, "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_description_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost("a", String.Empty, "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
        }

       [TestMethod]
        public void Ctor_WHEN_description_Is_Abc_THEN_Sets_Description_Property_To_Abc()
        {
            const string description = "ABC";

            var post = new BlogPost("a", description, "fullArticle", GetImageReference(), DateTime.Now, new BlogUser());
            post.Description.ShouldEqual(description);
        }


       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_fullArticle_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost("a", "description", String.Empty, GetImageReference(), DateTime.Now, new BlogUser());
        }

       [TestMethod]
        public void Ctor_WHEN_fullArticle_Is_Abc_THEN_Sets_FullArticle_Property_To_Abc()
        {
            const string fullArticle = "ABC";

            var post = new BlogPost("a", "description", fullArticle, GetImageReference(), DateTime.Now, new BlogUser());
            post.FullArticle.ShouldEqual(fullArticle);
        }

       [TestMethod]
        public void Ctor_WHEN_publishedDate_Is_Now_THEN_Sets_PublishDate_Property_To_Now()
        {
            var now = DateTime.Now;

            var post = new BlogPost("a", "b", "c", GetImageReference(), now, new BlogUser());
            post.PublishedDate.ShouldEqual(now);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_author_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new BlogPost("a", "b", "c", GetImageReference(), DateTime.Now, null);
        }

       [TestMethod]
        public void Ctor_WHEN_author_has_value_THEN_Sets_Author_Property_To_That_Value()
        {
            var author = new BlogUser();

            var post = new BlogPost("a", "b", "c", GetImageReference(), DateTime.Now, author);
            post.Author.ShouldEqual(author);
        }

        //[Test]
        //public void Ctor_When_Title_Is_ABC_AND_Date_Is_2012_12_12_THEN_Sets_CommentsIdentifier_To_BLOGPOST_20121112_ABC()
        //{
        //    var author = new BlogPostUser();

        //    var dateTime = new DateTime(2012,11,12);
        //    string title = "ABC";

        //    var post = new BlogPost(title, "description", "fullText", GetImageReference(), dateTime, author);

        //    post.CommentsIdentifier.ShouldEqual(String.Format("BLOGPOST_{0}{1}{2}_{3}", dateTime.Year, dateTime.Month, dateTime.Day, title));

        //}

       [TestMethod]
        public void Ctor_ImageUrl_Returns_Same_Url_As_In_ImageReference()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());
            post.ImageUrl.ShouldEqual(new Uri(imageReference.Url));
        }

       [TestMethod]
        public void ImageUrl_If_ImageReference_Is_Null_THEN_Returns_NULL()
        {
            BlogPost postAsIfCreatedFromDb = TestHelpers.CreateClassFromCtor<BlogPost>();
            postAsIfCreatedFromDb.ImageReference.ShouldBeNull("Pre Check: ImageReference should be null");
            postAsIfCreatedFromDb.ImageUrl.ShouldBeNull("ImageUrl Should Be Null");
        }

       [TestMethod]
        public void ImageUrl_If_ImageReference_Url_Is_Absolute_THEN_Returns_Absolute_Url()
        {
            var imageReference = GetImageReference(url:"http://www.jumbleblocks.co.uk/noimage.jpg");
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());
            post.ImageUrl.ShouldEqual(new Uri(imageReference.Url));
        }

       [TestMethod]
        public void ImageUrl_If_ImageReference_Url_Is_Relative_THEN_Returns_Relative_Url()
        {
            var imageReference = GetImageReference(url: "/noimage.jpg");
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());
            post.ImageUrl.ShouldEqual(new Uri(imageReference.Url, UriKind.Relative));
        }

       [TestMethod]
        public void UpdateTags_GIVEN_No_Tags_THEN_Adds_tags_Parameter_To_Tags_Property()
        {
            var tag = new Tag { Text = "Tag1" };
            tag.SetProperty(t => t.Id, 1);

            var tags = new Tag[]{ tag };

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTags(tags);

            post.Tags.ShouldContainAll(tags);
        }

       [TestMethod]
        public void UpdateTags_GIVEN_No_Tags_WHEN_tags_Parameter_Contains_two_Tags_With_Differnt_Texts_THEN_Only_Adds_1_Tag()
        {
            var tag1 = new Tag { Text = "Tag1" };
            tag1.SetProperty(t => t.Id, 1);

            var tag2 = new Tag { Text = "Tag1" };
            tag2.SetProperty(t => t.Id, 2);
            
            var tags = new Tag[] { tag1, tag2 };

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTags(tags);

            post.Tags.Count().ShouldEqual(1);
        }

       [TestMethod]
        public void UpdateTags_GIVEN_1_Tag_Exists_With_Text_Tag1_WHEN_Adding_Tag_With_Text_Tag2_THEN_Tags_Property_Contains_Tag1_And_Tag2()
        {
            var tag1 = new Tag { Text = "Tag1" };
            tag1.SetProperty(t => t.Id, 1);

            var tag2 = new Tag { Text = "Tag2" };
            tag2.SetProperty(t => t.Id, 2);

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTags(new Tag[] { tag1 });

            post.Tags.ShouldContain(tag1, "Pre-check");

            post.UpdateTags(new Tag[] { tag2 });

            post.Tags.ShouldContain(tag1);
            post.Tags.ShouldContain(tag2);
        }

       [TestMethod]
        public void UpdateTags_GIVEN_1_Tag_Exists_With_Tag1_WHEN_Adding_Tag2_And_removeUnfound_Is_True_THEN_Tags_Property_Contains_Tag2_Only()
        {
            var tag1 = new Tag { Text = "Tag1" };
            tag1.SetProperty(t => t.Id, 1);

            var tag2 = new Tag { Text = "Tag2" };
            tag2.SetProperty(t => t.Id, 2);

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTags(new Tag[] { tag1 }, false);

            post.Tags.ShouldContain(tag1, "Pre-check");

            post.UpdateTags(new Tag[] { tag2 }, true);

            post.Tags.ShouldNotContain(tag1);
            post.Tags.ShouldContain(tag2);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateTags_WHEN_tags_Parameter_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTags(null, false);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateSeries_WHEN_series_parameter_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateSeries(null);
        }

       [TestMethod]
        public void UpdateSeries_Sets_Series_Property_To_series_parameter()
        {
            var series = new Series { Name = "Test" };

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateSeries(series);
            post.Series.ShouldEqual(series);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void UpdateTitle_WHEN_newTitle_parameter_is_null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTitle(null);
        }

       [TestMethod]
        public void UpdateTitle_WHEN_sets_Title_Property_To_Trimmed_Text_Of_newTitle_parameter()
        {
            const string NewTitle = "New Title  ";

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateTitle(NewTitle);

            post.Title.ShouldEqual(NewTitle.Trim());
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void UpdateDescription_WHEN_newDescription_parameter_is_null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateDescription(null);
        }

       [TestMethod]
        public void UpdateDescription_WHEN_sets_Description_Property_To_Trimmed_Text_Of_newDescription_parameter()
        {
            const string NewDescription = "New Description  ";

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateDescription(NewDescription);

            post.Description.ShouldEqual(NewDescription.Trim());
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void UpdateFullArticle_WHEN_newFullArticle_parameter_is_null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateFullArticle(null);
        }

       [TestMethod]
        public void UpdateFullArticle_WHEN_sets_FullArticle_Property_To_Trimmed_Text_Of_newFullArticle_parameter()
        {
            const string NewFullText = "New Full Text  ";

            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateFullArticle(NewFullText);

            post.FullArticle.ShouldEqual(NewFullText.Trim());
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateImageReference_WHEN_newImageReference_parameter_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var imageReference = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference, DateTime.Now, new BlogUser());

            post.UpdateImageReference(null);
        }

       [TestMethod]
        public void UpdateImageReference_Sets_ImageReference_Property_To_newImageReference_parameter()
        {
            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            var imageReference2 = GetImageReference(2, "/anotherNoneImage.jpg");
            post.UpdateImageReference(imageReference2);

            post.ImageReference.ShouldEqual(imageReference2);
        }

       [TestMethod]
        public void UpdateImageReference_WHEN_newImageReference_parameter_Has_Same_Id_As_Existing_ImageReference_THEN_Does_Not_Update_ImageReference_Property()
        {
            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            var imageReference2 = GetImageReference();
            post.UpdateImageReference(imageReference2);

            post.ImageReference.ShouldEqual(imageReference1);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MarkAsDeleted_WHEN_User_Is_Null_Throws_ArgumentNullException()
        {
            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            post.MarkAsDeleted(null as BlogUser);
        }

       [TestMethod]
        public void MarkAsDeleted_Sets_DeletedDate_To_Now_And_DeletedByUser_Property_To_deltedByUser_parameter()
        {
            var deletionUser = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Bloggs" };

            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            post.MarkAsDeleted(deletionUser);

            post.DeletedDate.ShouldBeWithinLast(new TimeSpan(0, 0, 10));
            post.DeletedByUser.ShouldEqual(deletionUser);
        }

       [TestMethod]
        public void MarkAsDeleted_GIVEN_Already_Marked_As_Deleted_WHEN_MarkAsDeleted_Called_A_Second_Time_THEN_Does_Not_Update_DeletedDate()
        {
            TimeSpan waitBetweenUpdates = new TimeSpan(0, 0, 0, 0, 100);

            var deletionUser = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Bloggs" };

            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            post.MarkAsDeleted(deletionUser);

            Thread.Sleep(waitBetweenUpdates);

            post.MarkAsDeleted(deletionUser);

            post.DeletedDate.ShouldNotBeWithinLast(waitBetweenUpdates);
        }

       [TestMethod]
        public void MarkAsDeleted_GIVEN_Already_Marked_As_Deleted_WHEN_MarkAsDeleted_Called_A_Second_Time_With_A_Differnt_User_THEN_Does_Not_Update_DeletedByUser()
        {
            TimeSpan waitBetweenUpdates = new TimeSpan(0, 0, 0, 0, 500);

            var deletionUser1 = new BlogUser { Id = 1, Forenames = "Joe", Surname = "Bloggs" };
            var deletionUser2 = new BlogUser { Id = 2, Forenames = "Scrooge", Surname = "Bloggs" };

            var imageReference1 = GetImageReference();
            var post = new BlogPost("ABC", "description", "fullText", imageReference1, DateTime.Now, new BlogUser());

            post.MarkAsDeleted(deletionUser1);
            post.MarkAsDeleted(deletionUser2);

            post.DeletedByUser.ShouldEqual(deletionUser1);
        }
    }
}
