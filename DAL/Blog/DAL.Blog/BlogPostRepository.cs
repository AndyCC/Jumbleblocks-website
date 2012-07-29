using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Jumbleblocks.nHibernate;
using Jumbleblocks.Domain.Blog;

namespace Jumbleblocks.DAL.Blog
{    
    /// <summary>
    /// Repository for BlogPost
    /// </summary>
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public IEnumerable<BlogPost> GetPosts(int numberToSkip, int numberToTake, IEnumerable<string> tags)
        {
            IQueryOver<BlogPost> query = null;

            if (tags != null && tags.Count() > 0)
            {
                query = Session.QueryOver<BlogPost>()
                                .Where(bp => bp.DeletedDate == null)
                                .And(bp => bp.DeletedByUser == null)
                               .OrderBy(bp => bp.PublishedDate).Desc
                               .Inner.JoinQueryOver<Tag>((bp) => bp.Tags)
                               .WhereRestrictionOn(t => t.Text).IsIn(tags.ToArray())            
                               .Skip(numberToSkip).Take(numberToTake);
            }
            else
            {
                query = Session.QueryOver<BlogPost>()
                               .Where(bp => bp.DeletedDate == null)
                               .And(bp => bp.DeletedByUser == null)
                               .OrderBy(bp => bp.PublishedDate).Desc
                               .Skip(numberToSkip).Take(numberToTake); 
            }

            return Transact<IEnumerable<BlogPost>>(() => query.List());
        }
        
        public BlogPost LoadFrom(int year, int month, int day, string title)
        {
            var lowDate = new DateTime(year, month, day);
            var highDate = lowDate.AddDays(1);

            var query = Session.QueryOver<BlogPost>()
                                .WhereRestrictionOn(bp => bp.PublishedDate).IsBetween(lowDate).And(highDate)
                                .And(bp => bp.Title == title)
                                .And(bp => bp.DeletedByUser == null)
                                .And(bp => bp.DeletedDate == null);

            return Transact<BlogPost>(() => query.SingleOrDefault());
        }

        /// <summary>
        /// Eager loads full article
        /// </summary>
        /// <param name="id">id of blog post to use</param>
        /// <returns>BlogPost</returns>
        public BlogPost LoadFullArticle(int id)
        {
            Func<BlogPost> query = () =>
            {
                var post = Load(id);
                NHibernateUtil.Initialize(post.Description);
                NHibernateUtil.Initialize(post.FullArticle);

                return post;
            };

            return Transact<BlogPost>(query);
        }
    }
}
