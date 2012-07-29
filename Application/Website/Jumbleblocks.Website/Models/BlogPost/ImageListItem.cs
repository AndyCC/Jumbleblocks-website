using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.BlogPost
{
    /// <summary>
    /// View model for an image
    /// </summary>
    [Serializable]
    public class ImageListItem
    {
        /// <summary>
        /// unique id for the image
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// url for the image
        /// </summary>
        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ImageListItem)
                return ((ImageListItem)obj).Id == Id;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}