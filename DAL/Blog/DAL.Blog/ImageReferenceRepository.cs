using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.nHibernate;
using NHibernate;

namespace Jumbleblocks.DAL.Blog
{
    public class ImageReferenceRepository : Repository<ImageReference>, IImageReferenceRepository
    {
        public ImageReferenceRepository(ISessionFactory sessionFactory)
            :base(sessionFactory)
        {
        }

        public IEnumerable<ImageReference> GetImageReferences(int numberToSkip, int numberToTake)
        {
            var query = Session.QueryOver<ImageReference>().Skip(numberToSkip).Take(numberToTake);
            return Transact<IEnumerable<ImageReference>>(() => query.List());
        }
    }
}
