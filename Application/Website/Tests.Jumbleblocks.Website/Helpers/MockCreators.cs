using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Core.Configuration;
using Moq;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Domain;
using Jumbleblocks.Web.Wane;
using System.Web;
using System.Linq.Expressions;
using Jumbleblocks.Website.Controllers.Blog;
using Jumbleblocks.Core.Logging;
using Jumbleblocks.Core.Security;

namespace Tests.Jumbleblocks.Website.Helpers
{
    /// <summary>
    /// Creates article controller for tsts
    /// </summary>
    public class MockCreators
    {
        /// <summary>
        /// Creates article controller
        /// </summary>
        /// <param name="blogPostRepository">blogpost repository to use, leave null to use default mocked implementation</param>
        /// <param name="shouldBlogPostRepositoryAlwayReturnPost">if using default blogpost repository, if set to true then repository will always return BlogPost regardless of input. If however it is false posts titles are 0 to 19 and published date is current day</param>
        /// <param name="lookupRepository">repository to look things up, leave null for default mocked implementation</param>
        /// <param name="configurationReader">configuration reader to use, leave null for default mocked implementation</param>
        /// <param name="waneTransform">wane transform, leave null for default mocked implementaion</param>
        /// <param name="logger">Logger, leave null for default implementation</param>
        /// <param name="principal">principal used for security, leave null for default implementation</param>
        /// <returns></returns>
        public static BlogPostController CreateBlogPostController(IBlogPostRepository blogPostRepository = null, bool shouldBlogPostRepositoryAlwayReturnPost = true,
            ILookupRepository lookupRepository = null, IConfigurationReader configurationReader = null, IWaneTransform waneTransform = null, IJumbleblocksLogger logger = null,
            IJumbleblocksPrincipal principal = null)
        {
            if (blogPostRepository == null)
                blogPostRepository = CreateMockedBlogPostRepository(shouldBlogPostRepositoryAlwayReturnPost).Object;

            if (lookupRepository == null)
                lookupRepository = CreateMockedLookupRepository().Object;

            if (configurationReader == null)
                configurationReader = CreateMockedConfigurationReader().Object;

            if (waneTransform == null)
                waneTransform = CreateMockedWaneTransform().Object;

            if (logger == null)
                logger = new Mock<IJumbleblocksLogger>().Object;

            if (principal == null)
                principal = CreateMockedPrincipalAndAddBlogUserToLookUp(Mock.Get<ILookupRepository>(lookupRepository)).Object;

            var controller = new BlogPostController(blogPostRepository, lookupRepository, configurationReader, waneTransform, logger);
            controller.SetMockedControllerContext();
            controller.SetPrincipal(principal);

            return controller;
        }

        public static Mock<IBlogPostRepository> CreateMockedBlogPostRepository(bool shouldBlogPostRepositoryAlwayReturnPost = true, int blogPostCount = 20)
        {
            //TODO: may need to allow return of specific blog post if no match
            var blogPostList = new List<BlogPost>();

            for (int i = 0; i < blogPostCount; i++)
            {
                var blogPost = new BlogPost(i.ToString(), i.ToString(), i.ToString(), new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
                blogPost.SetProperty(p => p.Id, i);
                blogPostList.Add(blogPost);
            }

            var mockedBlogPostRepository = new Mock<IBlogPostRepository>();

            mockedBlogPostRepository.Setup(r => r.Count).Returns(blogPostCount);
            mockedBlogPostRepository.Setup(r => r.LoadAll()).Returns(blogPostList);

            mockedBlogPostRepository.Setup(r => r.LoadFrom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, int, int, string>((year, month, day, title) => ReturnBlogPost(year, month, day, title, blogPostList, shouldBlogPostRepositoryAlwayReturnPost));

            mockedBlogPostRepository.Setup(r => r.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Returns<int, int, IEnumerable<string>>((skip, take, tags) => blogPostList.Skip(skip).Take(take));

            return mockedBlogPostRepository;
        }

        private static BlogPost ReturnBlogPost(int year, int month, int day, string title, IList<BlogPost> blogPostList, bool shouldBlogPostRepositoryAlwayReturnPost)
        {
            if (shouldBlogPostRepositoryAlwayReturnPost)
            {
                var blogPost = new BlogPost("Test", "Test", "Test", new ImageReference(1, "/noimage.jpg"), DateTime.Now, new BlogUser());
                blogPost.SetProperty(p => p.Id, 1);

                return blogPost;
            }

            return blogPostList.SingleOrDefault(p => p.PublishedDate.Year == year && p.PublishedDate.Month == month && p.PublishedDate.Day == day && p.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Mock<ILookupRepository> CreateMockedLookupRepository()
        {
            var mockedLookupRepository = new Mock<ILookupRepository>();

            mockedLookupRepository.Setup(r => r.LoadForDescription<Tag>(It.IsAny<Expression<Func<Tag, string>>>(), It.IsAny<string>())).Returns(null as Tag);
            mockedLookupRepository.Setup(r => r.LoadForDescription<Series>(It.IsAny<Expression<Func<Series, string>>>(), It.IsAny<string>())).Returns(null as Series);

            return mockedLookupRepository;
        }


        public static Mock<IConfigurationReader> CreateMockedConfigurationReader(string title = "Jumbleblocks", int postsPerPage = 10)
        {
            var configurationReader = new Mock<IConfigurationReader>();

            var blogSettings = new BlogConfigurationSection
            {
                Title = title,
                PagePostSummaryCount = postsPerPage
            };

            configurationReader.Setup(r => r.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(blogSettings);

            return configurationReader;
        }

        public static Mock<IWaneTransform> CreateMockedWaneTransform()
        {
            var mockedWaneTextTransform = new Mock<IWaneTransform>();

            mockedWaneTextTransform.Setup(wtt => wtt.TransformToHtml(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, bool>((text, encode) =>
                {
                    if (encode)
                        return new HtmlString(HttpUtility.HtmlEncode(text));
                    else
                        return new HtmlString(text);
                });

            mockedWaneTextTransform.Setup(wtt => wtt.TransformToRawHtml(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, bool>((text, encode) =>
                {
                    if (encode)
                        return HttpUtility.HtmlEncode(text);
                    else
                        return text;
                });

            return mockedWaneTextTransform;
        }

        public static Mock<IJumbleblocksPrincipal> CreateMockedPrincipalAndAddBlogUserToLookUp(Mock<ILookupRepository> lookupRepository, int? userId = 99, string forename = "Joe", string surname = "Blogs")
        {
            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            mockedIdentity.SetupGet(i => i.Forename).Returns(forename);
            mockedIdentity.SetupGet(i => i.Surname).Returns(surname);
            mockedIdentity.SetupGet(i => i.Name).Returns(String.Format("{0} {1}",forename, surname));
            mockedIdentity.SetupGet(i => i.UserId).Returns(userId);

            var mockedPrincipal = new Mock<IJumbleblocksPrincipal>();
            mockedPrincipal.SetupGet(p => p.Identity).Returns(mockedIdentity.Object);

            if (userId.HasValue)
            {
                lookupRepository.Setup(r => r.LoadForId<BlogUser>(userId.Value))
                    .Returns<int>((id) => new BlogUser { Id = id, Forenames = forename, Surname = surname });
            }
            else
                lookupRepository.Setup(r => r.LoadForId<BlogUser>(It.IsAny<int?>())).Returns(null as BlogUser);

            return mockedPrincipal;
        }
    }
}
