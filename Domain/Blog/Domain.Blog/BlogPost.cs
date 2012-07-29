using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core;
using Jumbleblocks.Core.Collections;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// Summary of a post
    /// </summary>
    public class BlogPost
    {
        protected BlogPost()
        {
        }

        /// <summary>
        /// Blog Post Summary
        /// </summary>
        /// <param name="title">title of a blog post</param>
        /// <param name="description">description for the blog post</param>
        /// <param name="fullArticle">The full article</param>
        /// <param name="imageReference">reference details about image to use</param>
        /// <param name="author">Author of blog post</param>
        public BlogPost(string title, string description, string fullArticle, ImageReference imageReference, DateTime publishedDate, BlogUser author)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new StringArgumentNullOrEmptyException("must have a value", "title");

            if (String.IsNullOrWhiteSpace(description))
                throw new StringArgumentNullOrEmptyException("must have a value", "description");

            if (String.IsNullOrWhiteSpace(fullArticle))
                throw new StringArgumentNullOrEmptyException("must have a value", "fullArticle");

            if (author == null)
                throw new ArgumentNullException("author");

            this.Title = title;
            this.Description = description;
            this.ImageReference = imageReference;
            this.PublishedDate = publishedDate;
            this.Author = author;
            this.FullArticle = fullArticle;
        }


        private IList<Tag> _tags = new List<Tag>();
        private LlamdaEqualityComparer<Tag> _tagTextComparer = new LlamdaEqualityComparer<Tag>((t1, t2) => t1.TextEquals(t2), t => t.Text.GetHashCode());

        /// <summary>
        /// Database identifier for BlogPost
        /// </summary>
        public virtual int? Id { get; protected set; }

        /// <summary>
        /// Title of Blog Post
        /// </summary>
        public virtual string Title { get; protected set; }

        /// <summary>
        /// Description of what the article is about
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Gets full article to display
        /// </summary>
        public virtual string FullArticle { get; protected set; }

        /// <summary>
        /// Gets the image reference
        /// </summary>
        public virtual ImageReference ImageReference { get; protected set; }

        /// <summary>
        /// Image url
        /// </summary>
        public virtual Uri ImageUrl
        { 
            get
            {
                if (ImageReference == null)
                    return null;
                else
                    return new Uri(ImageReference.Url, UriKind.RelativeOrAbsolute);
            } 
        }

        /// <summary>
        /// Date/Time publised
        /// </summary>
        public virtual DateTime PublishedDate { get; protected set; }

        /// <summary>
        /// Gets the author
        /// </summary>
        public virtual BlogUser Author { get; protected set; }
        
        /// <summary>
        /// Gets/Sets Tags Blog is matched against
        /// </summary>
        public virtual IEnumerable<Tag> Tags { get { return _tags; } }

        /// <summary>
        /// Gets/sets series
        /// </summary>
        public virtual Series Series { get; protected set; }

        /// <summary>
        /// user who deleted post
        /// </summary>
        public virtual BlogUser DeletedByUser { get; protected set; }
        
        /// <summary>
        /// Gets the date the article was deleted
        /// </summary>
        public virtual DateTime? DeletedDate { get; protected set; }

        /// <summary>
        /// Updates the title of the blog post
        /// </summary>
        /// <param name="newTitle">title to update to</param>
        public virtual void UpdateTitle(string newTitle)
        {
            if (String.IsNullOrWhiteSpace(newTitle))
                throw new StringArgumentNullOrEmptyException("title");

            if (!Title.Equals(newTitle, StringComparison.CurrentCultureIgnoreCase)) 
                Title = newTitle.Trim();
        }

        /// <summary>
        /// Updates the description of the blog post 
        /// </summary>
        /// <param name="newDescription">new description</param>
        public virtual void UpdateDescription(string newDescription)
        {
            if (String.IsNullOrWhiteSpace(newDescription))
                throw new StringArgumentNullOrEmptyException("description");

            if(!Description.Equals(newDescription, StringComparison.CurrentCultureIgnoreCase))
                Description = newDescription.Trim();
        }

        /// <summary>
        /// Updates the full text of the blog post
        /// </summary>
        /// <param name="newFullArticle">new full text</param>
        public virtual void UpdateFullArticle(string newFullArticle)
        {
            if (String.IsNullOrWhiteSpace(newFullArticle))
                throw new StringArgumentNullOrEmptyException("fullArticle");

            if (!FullArticle.Equals(newFullArticle, StringComparison.CurrentCultureIgnoreCase))
                FullArticle = newFullArticle.Trim();
        }

        /// <summary>
        /// Updates the image reference used by the blog post
        /// </summary>
        /// <param name="newImageReference">image reference to use</param>
        public virtual void UpdateImageReference(ImageReference newImageReference)
        {
            if (newImageReference == null)
                throw new ArgumentNullException("imageReference");

            if(!ImageReference.IdsEqual(newImageReference))
                ImageReference = newImageReference;
        }
        
        /// <summary>
        /// Updates tags with new tags
        /// </summary>
        /// <param name="tags">tags to update with</param>
        /// <param name="removeUnfound">If true removes tags from blogpost if they are not in tags parameter. Default is false</param>
        public virtual void UpdateTags(IEnumerable<Tag> tags, bool removeUnfound = false)
        {
            if (tags == null)
                throw new ArgumentNullException("tags");

            var distinctNewTags = tags.Distinct(_tagTextComparer);
            _tags.Update(distinctNewTags, removeUnfound);
        }

        /// <summary>
        /// Updates the current series
        /// </summary>
        /// <param name="series">series the blog post belongs to</param>
        public virtual void UpdateSeries(Series series)
        {
            if (series == null)
                throw new ArgumentNullException("series");

            Series = series;
        }

        /// <summary>
        /// Checks to see if the provided series name equals the one on the blogpost
        /// </summary>
        /// <param name="seriesName">name of series to check</param>
        /// <returns>true if series name equals, otherwise false</returns>
        public virtual bool SeriesNameEqual(string seriesName)
        {
            return Series != null && Series.NameEqual(seriesName);
        }

        /// <summary>
        /// Marks blog post as deleted
        /// </summary>
        /// <param name="deletedBy">user who deleted post</param>
        public virtual void MarkAsDeleted(BlogUser deletedByUser)
        {
            if (deletedByUser == null)
                throw new ArgumentNullException("deletedByUser");

            if (DeletedByUser == null)
            {
                DeletedDate = DateTime.Now;
                DeletedByUser = deletedByUser;
            }
        }

        ///// <summary>
        ///// Gets identifier for comments
        ///// </summary>
        //public virtual string CommentsIdentifier
        //{
        //    get { return String.Format("BLOGPOST_{0}{1}{2}_{3}", PublishedDate.Year, PublishedDate.Month, PublishedDate.Day, Title); }
        //}
       
    }
}
