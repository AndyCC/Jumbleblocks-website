using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Blog.Paging;
using Jumbleblocks.Domain.Blog;
using Moq;
using System.Linq.Expressions;

namespace Tests.Jumbleblocks.Blog.Domain.Paging
{
    [TestFixture]
    public class BlogPostPagerTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_WHEN_postsPerPage_Is_0_THEN_Throws_ArgumentException()
        {
            new BlogPostPager(0, new Mock<IBlogPostRepository>().Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_blogPostRepository_Is_Null_THEN_Throws_ArgumentNullException()
        {
            new BlogPostPager(1, null);
        }

        [Test]
        public void Ctor_Sets_PostPerPage_Property_To_postsPerPage_parameter_AND_BlogPostRepository_Property_To_blogPostRepostitoryParameter()
        {
            const int PostsPerPage = 2;
            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);

            pager.PostsPerPage.ShouldEqual(PostsPerPage);
            pager.BlogPostRepository.ShouldEqual(mockedBlogPostRepository.Object);
        }

        private Mock<IBlogPostRepository> CreateMockedBlogPostRepository(int numberOfPosts)
        {            
            var blogPosts = new List<BlogPost>();

            for (int i = 0; i < numberOfPosts; i++)
            {
                var post = new BlogPost(i.ToString(), i.ToString(), i.ToString(), new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
                blogPosts.Add(post);
            }

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            mockedBlogPostRepository.SetupGet(r => r.Count).Returns(numberOfPosts);
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns<int, int, IEnumerable<string>>((skip, take, tags) => blogPosts.Skip(skip).Take(take));

            return mockedBlogPostRepository;
        }

        [Test]
        public void PageCount_GIVEN_BlogPostRepository_Has_2_Posts_And_Posts_Per_Page_Is_1_THEN_Returns_2()
        {
            const int PostsPerPage = 1;
            const int NumberOfPosts = 2;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);

            pager.PageCount.ShouldEqual(2);
        }

        [Test]
        public void PageCount_GIVEN_BlogPostRepository_Has_2_Posts_And_Posts_Per_Page_Is_2_THEN_Returns_1()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 2;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);

            pager.PageCount.ShouldEqual(1);
        }

        [Test]
        public void PageCount_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_3_THEN_Returns_2()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 3;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);

            pager.PageCount.ShouldEqual(2);
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_1_WHEN_pageNumber_Is_1_THEN_Calls_BlogPostRepository_GetPosts_With_numberToSkip_As_0_numberToTake_As_1()
        {
            const int PostsPerPage = 1;
            const int NumberOfPosts = 2;
            const int PageNumber = 1;

            const int ExpectedNumSkip = 0;
            const int ExpectedNumTake = 1;

            Expression<Action<IBlogPostRepository>> verifiableAction = r => r.GetPosts(ExpectedNumSkip, ExpectedNumTake, It.IsAny<IEnumerable<string>>());

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
            mockedBlogPostRepository.Setup(verifiableAction).Verifiable();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            pager.FetchForPage(PageNumber);

            mockedBlogPostRepository.Verify(verifiableAction, Times.Once());
        }
        
        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_1_WHEN_pageNumber_Is_2_THEN_Calls_BlogPostRepository_GetPosts_With_numberToSkip_As_1_numberToTake_As_1()
        {
            const int PostsPerPage = 1;
            const int NumberOfPosts = 2;
            const int PageNumber = 2;

            const int ExpectedNumSkip = 1;
            const int ExpectedNumTake = 1;

            Expression<Action<IBlogPostRepository>> verifiableAction = r => r.GetPosts(ExpectedNumSkip, ExpectedNumTake, It.IsAny<IEnumerable<string>>());
            
            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
            mockedBlogPostRepository.Setup(verifiableAction).Verifiable();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            pager.FetchForPage(PageNumber);

            mockedBlogPostRepository.Verify(verifiableAction, Times.Once());
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_1_WHEN_pageNumber_Is_3_THEN_Does_Not_Call_BlogPostRepository_GetPosts()
        {
            const int PostsPerPage = 1;
            const int NumberOfPosts = 2;
            const int PageNumber = 3;

            Expression<Action<IBlogPostRepository>> verifiableAction = r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>());

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
            mockedBlogPostRepository.Setup(verifiableAction).Verifiable();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            pager.FetchForPage(PageNumber);

            mockedBlogPostRepository.Verify(verifiableAction, Times.Never());
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_2_WHEN_pageNumber_Is_1_THEN_Calls_BlogPostRepository_GetPosts_With_numberToSkip_As_0_numberToTake_As_2()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 2;
            const int PageNumber = 1;

            const int ExpectedNumSkip = 0;
            const int ExpectedNumTake = 2;

            Expression<Action<IBlogPostRepository>> verifiableAction = r => r.GetPosts(ExpectedNumSkip, ExpectedNumTake, It.IsAny<IEnumerable<string>>());

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
            mockedBlogPostRepository.Setup(verifiableAction).Verifiable();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            pager.FetchForPage(PageNumber);

            mockedBlogPostRepository.Verify(verifiableAction, Times.Once());
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_3_Posts_And_PostsPerPage_Is2_WHEN_pageNumber_Is_2_THEN_Calls_BlogPostRepository_GetPosts_With_numberToSkip_As_2_numberToTake_As_1()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 3;
            const int PageNumber = 2;

            const int ExpectedNumSkip = 2;
            const int ExpectedNumTake = 1;

            Expression<Action<IBlogPostRepository>> verifiableAction = r => r.GetPosts(ExpectedNumSkip, ExpectedNumTake, It.IsAny<IEnumerable<string>>());

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
            mockedBlogPostRepository.Setup(verifiableAction).Verifiable();

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            pager.FetchForPage(PageNumber);

            mockedBlogPostRepository.Verify(verifiableAction, Times.Once());
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_1_WHEN_pageNumber_Is_1_THEN_Returns_1_BlogPost()
        {
            const int PostsPerPage = 1;
            const int NumberOfPosts = 2;
            const int PageNumber = 1;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);
        
            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            IEnumerable<BlogPost> posts = pager.FetchForPage(PageNumber);

            posts.Count().ShouldEqual(PostsPerPage);
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_2_Posts_And_PostsPerPage_Is_2_WHEN_pageNumber_Is_1_THEN_Returns_2_BlogPost()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 2;
            const int PageNumber = 1;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            IEnumerable<BlogPost> posts = pager.FetchForPage(PageNumber);

            posts.Count().ShouldEqual(PostsPerPage);
        }

        [Test]
        public void FetchForPage_GIVEN_BlogPostRepository_Has_3_Posts_And_PostsPerPage_Is_2_WHEN_pageNumber_Is_2_THEN_Returns_1_BlogPost()
        {
            const int PostsPerPage = 2;
            const int NumberOfPosts = 3;
            const int PageNumber = 2;

            const int ExpectedNumPosts = 1;

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(NumberOfPosts);

            var pager = new BlogPostPager(PostsPerPage, mockedBlogPostRepository.Object);
            IEnumerable<BlogPost> posts = pager.FetchForPage(PageNumber);

            posts.Count().ShouldEqual(ExpectedNumPosts);
        }

        [Test]
        public void FetchForPage_WHEN_No_Tags_THEN_Calls_BlogPostRepository_GetPosts_With_EmptyList_Of_Tags()
        {
            IEnumerable<string> tagsPassedToPosts = new string[0];

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(1);
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                                    .Callback<int, int, IEnumerable<string>>((skip, take, tags) => tagsPassedToPosts = tags);

            var pager = new BlogPostPager(1, mockedBlogPostRepository.Object);
            IEnumerable<BlogPost> posts = pager.FetchForPage(1, null);

            tagsPassedToPosts.Count().ShouldEqual(0);
        }

        [Test]
        public void FetchForPage_WHEN_2_Tags_THEN_Calls_BlogPostRepository_GetPosts_With_List_Of_2_Tags()
        {
            IEnumerable<string> tagsPassedToPosts = new string[0];

            var mockedBlogPostRepository = CreateMockedBlogPostRepository(1);
            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                                    .Callback<int, int, IEnumerable<string>>((skip, take, tags) => tagsPassedToPosts = tags);

            var pager = new BlogPostPager(1, mockedBlogPostRepository.Object);
            IEnumerable<BlogPost> posts = pager.FetchForPage(1, new string[] { "tag1", "tag2" });

            tagsPassedToPosts.Count().ShouldEqual(2);
        }
    }
}
