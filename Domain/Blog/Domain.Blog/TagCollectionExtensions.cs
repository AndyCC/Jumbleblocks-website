using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Collections;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// Extensions for tag collection
    /// </summary>
    public static class TagCollectionExtensions
    {
        public static void Update(this ICollection<Tag> collection, IEnumerable<Tag> tagsToUpdateWith, bool removeUnfound)
        {
            if (removeUnfound)
                collection.ReplaceTagsCollection(tagsToUpdateWith);
            else
                collection.AddDistinctFromRange(tagsToUpdateWith);
        }

        private static void ReplaceTagsCollection(this ICollection<Tag> collection, IEnumerable<Tag> tags)
        {
            collection.Clear();
            collection.AddRange(tags);
        }
    }
}
