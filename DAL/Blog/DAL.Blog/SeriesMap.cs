using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Jumbleblocks.Domain.Blog;
using NHibernate.Mapping.ByCode;

namespace Jumbleblocks.DAL.Blog
{
    public class SeriesMap : ClassMapping<Series>
    {
        public SeriesMap()
        {
            Table("Series");
            Id(s => s.Id, id => id.Generator(Generators.Native));

            Property(s => s.Name, map =>
                {
                    map.Length(50);
                });
        }
    }
}
