using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Web.Security;
using Jumbleblocks.Core.Collections;
using Jumbleblocks.Domain.Blog.Deletion;
using Jumbleblocks.Web.Wane;
using Castle.Windsor;
using Jumbleblocks.Domain;
using Jumbleblocks.Domain.Blog.Paging;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Website.Models.BlogPost;
using Jumbleblocks.Core.Logging;
using Jumbleblocks.Website.ActionFilters;
using Jumbleblocks.Core.Security;

namespace Jumbleblocks.Website.Controllers.Blog
{
    [HandleAndLogErrorAttribute(ExceptionType = typeof(Exception), View = "ErrorOccured")]
    public class BlogPostController : Controller
    {
        public BlogPostController(IBlogPostRepository blogPostSummaryRepository, ILookupRepository lookupRepository, IConfigurationReader configurationReader, IWaneTransform waneTransformer, IJumbleblocksLogger logger)
        {
            if (blogPostSummaryRepository == null)
                throw new ArgumentNullException("blogPostSummaryRepository");

            if (lookupRepository == null)
                throw new ArgumentNullException("lookupRepository");

            if (configurationReader == null)
                throw new ArgumentNullException("configurationReader");

            if (waneTransformer == null)
                throw new ArgumentNullException("waneTransformer");

            if (logger == null)
                throw new ArgumentNullException("logger");

            BlogPostRepository = blogPostSummaryRepository;
            LookupRepository = lookupRepository;
            ConfigurationReader = configurationReader;
            WaneTransformer = waneTransformer;
            Logger = logger;
        }

        protected const char TagSeperator = ';';

        /// <summary>
        /// Gets BlogPostRepository
        /// </summary>
        protected IBlogPostRepository BlogPostRepository { get; private set; }

        /// <summary>
        /// Repository to look up entities
        /// </summary>
        public ILookupRepository LookupRepository { get; private set; }

        /// <summary>
        /// Gets configuration reader
        /// </summary>
        protected IConfigurationReader ConfigurationReader { get; private set; }

        /// <summary>
        /// Gets the blog settings from config
        /// </summary>
        protected BlogConfigurationSection BlogSettings { get { return ConfigurationReader.GetSection<BlogConfigurationSection>(BlogContants.BlogSettingsConfigName); } }
        
        /// <summary>
        /// WaneText transformer
        /// </summary>
        protected IWaneTransform WaneTransformer { get; private set; }

        /// <summary>
        /// Logger
        /// </summary>
        protected IJumbleblocksLogger Logger { get; private set; }

        /// <summary>
        /// Gets the principal of the current user
        /// </summary>
        protected IJumbleblocksPrincipal JumbleblocksPrincipal
        {
            get { return (IJumbleblocksPrincipal)User; }
        }

        /// <summary>
        /// Transforms wane text to HTML
        /// </summary>
        /// <param name="text">text to transform</param>
        /// <returns>pure html</returns>
        protected string TransformWaneTextToHtml(string text)
        {
            return WaneTransformer.TransformToRawHtml(text, true);
        }

        /// <summary>
        /// Main view for index
        /// </summary>
        /// <param name="page">page being looked at (defaults to 1)</param>
        /// <param name="tags">tags to look for, seperated by a |</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        public ActionResult Index(int page = 1, string tags = "")
        {
            string[] splitTags = SplitTags(tags);

            IBlogPostPager pager = CreateBlogPostPager();

            var viewModel = CreateViewModelWithPagingInformation(page, pager);

            IEnumerable<BlogPost> blogPosts = pager.FetchForPage(page, splitTags);
            UpdateViewModelWithPosts(viewModel, blogPosts);

            ViewBag.Title = BlogSettings.Title;
            ViewData.Model = viewModel;

            return View("FrontPage");
        }

        protected IBlogPostPager CreateBlogPostPager()
        {
            int postsPerPage = BlogSettings.PagePostSummaryCount;
            return new BlogPostPager(postsPerPage, BlogPostRepository);
        }


        private string[] SplitTags(string tags)
        {
            return tags.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static FrontPageModel CreateViewModelWithPagingInformation(int page, IBlogPostPager pager)
        {
            var viewModel = new FrontPageModel();
            viewModel.Paging.CurrentPage = page;
            viewModel.Paging.PageCount = pager.PageCount;
            return viewModel;
        }

        private void UpdateViewModelWithPosts(FrontPageModel viewModel, IEnumerable<BlogPost> blogPosts)
        {
            foreach (BlogPost post in blogPosts)
            {
                string parsedDescription = TransformWaneTextToHtml(post.Description);

                var summaryViewModel = new FrontPageItemModel
                {
                    Title = post.Title,
                    Description = parsedDescription,
                    ImageUrl = post.ImageUrl,
                    PublishedDate = post.PublishedDate,
                    AuthorsName = post.Author.FullName
                };

                viewModel.Add(summaryViewModel);
            }
        }

        /// <summary>
        /// Shows an article
        /// </summary>
        /// <param name="year">year of article</param>
        /// <param name="month">month of article</param>
        /// <param name="day">day of article</param>
        /// <param name="title">title of article</param>
        /// <returns>ActionResult</returns>
        public ActionResult Show(int year, int month, int day, string title)
        {
            BlogPost post = BlogPostRepository.LoadFrom(year, month, day, title);
      
            if (post == null)
            {
                ViewBag.Title = String.Format("{0} : {1} Not Found", BlogSettings.Title, title);

                return View("BlogPostNotFound");
            }
            else
            {
                var parsedFullArticle = TransformWaneTextToHtml(post.FullArticle);             
                string imageUrl = String.Empty;

                if (post.ImageUrl != null)
                    imageUrl = post.ImageUrl.ToString();

                var viewModel = new FullBlogPostModel
                {
                     Title = post.Title,
                     PublishedDate = post.PublishedDate,
                     AuthorsName = post.Author.FullName,
                     FullArticle = parsedFullArticle,
                     ImageUrl = imageUrl
                };

                ViewData.Model = viewModel;
                ViewBag.Title = String.Format("{0} : {1}", BlogSettings.Title, title);

                return View("FullBlogPost");
            }
        }

        //TODO: eventually this will need paging
        [AuthorizeOperation("Edit Blog Post", "Delete Blog Post")]
        public ActionResult List()
        {
            var model = new List<BlogPostListingItemModel>();

            IEnumerable<BlogPost> allPosts = BlogPostRepository.LoadAll();

            foreach (BlogPost post in allPosts)
            {
                var listingModel = new BlogPostListingItemModel
                {
                    BlogPostId = post.Id.Value,
                    Title = post.Title,
                    PublishedDate = post.PublishedDate,
                    AuthorsName = post.Author.FullName
                };

                model.Add(listingModel);
            }

            ViewData.Model = model;

            return View("BlogPostListing");
        }

        [AuthorizeOperation("Create Blog Post")]
        public ActionResult CreateNew()
        {
            return View("CreateEdit");
        }

        [AuthorizeOperation("Edit Blog Post")]
        public ActionResult Edit(int blogPostId)
        {
            BlogPost post = BlogPostRepository.Load(blogPostId);

            string seriesName = post.Series == null ? String.Empty : post.Series.Name;
            
            int? imageReferenceId = post.GetImageReferenceId();
            string tagTexts = post.GetTagsAsSeperatedString(TagSeperator);           

            var viewModel = new CreateEditModel
            {
                BlogPostId = post.Id.Value,
                PublishedDate = post.PublishedDate,
                Title = post.Title,
                Description = post.Description,
                FullArticle = post.FullArticle,
                ImageId = imageReferenceId,
                SeriesName = seriesName,
                TagTexts = tagTexts
            };

            ViewData.Model = viewModel;

            return View("CreateEdit");
        }

        [ValidateAntiForgeryToken]
        [AuthorizeOperation("Delete Blog Post")]
        public ActionResult Delete(int blogPostId)
        {
            BlogPost blogPost = BlogPostRepository.LoadFullArticle(blogPostId); //nHibernate won't update objects with lazy load properties

            if (blogPost != null)
            {
                var blogPostDeleter = new BlogPostDeleter(BlogPostRepository, LookupRepository);
                blogPostDeleter.MarkAsDeleted(blogPost, JumbleblocksPrincipal.Identity.UserId.Value);

                ViewData.Model = new DeletedModel { BlogPostId = blogPost.Id.Value, Title = blogPost.Title };
                return View("Deleted");
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        protected IBlogPostDeleter CreateBlogPostDeleter()
        {
            return new BlogPostDeleter(BlogPostRepository, LookupRepository);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AuthorizeOperation("Create Blog Post")]      
        public ActionResult Save(CreateEditModel viewModel)
        {
            if (!viewModel.ImageId.HasValue || viewModel.ImageId < 1)
                viewModel.ImageId = 1;

            try
            {
                BlogPost blogPost = null;

                if (viewModel.BlogPostId.HasValue)
                {
                    blogPost = BlogPostRepository.LoadFullArticle(viewModel.BlogPostId.Value);

                    blogPost.UpdateTitle(viewModel.Title);
                    blogPost.UpdateDescription(viewModel.Description);
                    blogPost.UpdateFullArticle(viewModel.FullArticle);
                  
                    if (blogPost.ImageReference.Id.HasValue && blogPost.ImageReference.Id.Value != viewModel.ImageId)
                    {
                        ImageReference imageReference = LookupRepository.LoadForId<ImageReference>(viewModel.ImageId);
                        blogPost.UpdateImageReference(imageReference);
                    }
                }
                else
                {
                    ImageReference imageReference = LookupRepository.LoadForId<ImageReference>(viewModel.ImageId);

                    BlogUser user = LookupRepository.LoadForId<BlogUser>(JumbleblocksPrincipal.Identity.UserId);

                    blogPost = new BlogPost(viewModel.Title,
                                                viewModel.Description,
                                                viewModel.FullArticle,
                                                imageReference,
                                                DateTime.Now,
                                                user);
                }
              
                UpdateTagsOnBlogPost(blogPost, viewModel.TagTexts);
                UpdateSeriesOnBlogPost(blogPost, viewModel.SeriesName);

                BlogPostRepository.SaveOrUpdate(blogPost);

                return RedirectToAction("Index", "ControlPanel");
            }
            catch (Exception ex)
            {
                Logger.LogError("Could not save blog post", ex);
                ViewBag.ErrorMessage = "Exception occured while saving blog post. Please try again.";
                return View("CreateEdit");
            }
        }

        private void UpdateTagsOnBlogPost(BlogPost blogPost, string tagTexts)
        {
            var tagList = new List<Tag>();

            if (!String.IsNullOrWhiteSpace(tagTexts))
            {
                string[] seperatedTagTexts = tagTexts.Split(new char[] { TagSeperator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string tagText in seperatedTagTexts.Select(s => s.Trim()))
                {
                    Tag tag = LookupRepository.LoadForDescription<Tag>(t => t.Text, tagText);

                    if (tag == null && !String.IsNullOrWhiteSpace(tagTexts))
                        tag = new Tag { Text = tagText };

                    tagList.Add(tag);
                }
            }

            blogPost.UpdateTags(tagList, true);
        }


        private void UpdateSeriesOnBlogPost(BlogPost blogPost, string seriesName)
        {          
            if (!String.IsNullOrWhiteSpace(seriesName) && !blogPost.SeriesNameEqual(seriesName))
            {
                Series seriesFound = LookupRepository.LoadForDescription<Series>(s => s.Name, seriesName);

                if (seriesFound == null)
                    seriesFound = new Series { Name = seriesName };

                blogPost.UpdateSeries(seriesFound);
            }
        }
    }
}
