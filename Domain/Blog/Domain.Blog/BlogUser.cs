using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog
{
    public class BlogUser
    {
        public BlogUser()
        {
        }

        public virtual int? Id { get; set; }

        public virtual string Forenames { get; set; }
        public virtual string Surname { get; set; }

        public virtual string FullName
        {
            get { return String.Format("{0} {1}", Forenames, Surname).Trim(); }
        }
    }
}
