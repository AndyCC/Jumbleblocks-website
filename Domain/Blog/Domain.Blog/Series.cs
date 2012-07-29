using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog
{
    public class Series
    {
        public virtual int? Id { get; protected set; }
        public virtual string Name { get;  set; }

        public virtual bool NameEqual(string seriesName)
        {
            return Name.Equals(seriesName, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
