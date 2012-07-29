using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// Represents a Tag from the view of a blof
    /// </summary>
    public class Tag 
    {
        /// <summary>
        /// Id of the tag
        /// </summary>
        public virtual int? Id { get; protected set; }

        /// <summary>
        /// The tag's text
        /// </summary>
        public virtual string Text { get; set; }

        public virtual bool TextEquals(Tag other)
        {
            return Text.Equals(other.Text, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
