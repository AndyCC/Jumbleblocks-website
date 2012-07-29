using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Domain.Blog;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Jumbleblocks.DAL.Blog
{
    /// <summary>
    /// Db mapping for tag map
    /// </summary>
    public class TagMap : ClassMapping<Tag>
    {
        public TagMap()
        {
            Table("Tags");
            Id(t => t.Id, id => id.Generator(Generators.Native));
            Property(t => t.Text, map =>
            {
                map.Length(50);
                map.NotNullable(true);
            });
        }
    }
}
