using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// References an image
    /// </summary>
    public class ImageReference
    {
        protected ImageReference()
        {
            Url = String.Empty;
        }

        public ImageReference(string url)
        {
            if (String.IsNullOrEmpty(url))
                throw new StringArgumentNullOrEmptyException("url");

            Url = url;
        }

        public ImageReference(int? id, string url)
            : this(url)
        {
            Id = id;
        }

        /// <summary>
        /// id of image reference
        /// </summary>
        public virtual int? Id { get; protected set; }

        /// <summary>
        /// Url to image as string
        /// </summary>
        public virtual string Url { get; protected set; }

        /// <summary>
        /// Checks to see if image reference id's match
        /// </summary>
        /// <param name="other">other Image reference to check</param>
        /// <returns>true if equal, otherwise false</returns>
        public virtual bool IdsEqual(ImageReference other)
        {
            return Id.Equals(other.Id);
        }
    }
}
