using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Jumbleblocks.Domain.Blog;

namespace Jumbleblocks.DAL.Blog
{
    public class BlogUserMap : ClassMapping<BlogUser>
    {
        public BlogUserMap()
        {
            Table("Users");

            Id(a => a.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Native);
            });

            Property(p => p.Forenames, map =>
            {
                map.Column("Forenames");
                map.Length(255);
                map.NotNullable(true);
            });

            Property(p => p.Surname, map =>
            {
                map.Column("Surname");
                map.Length(255);
                map.NotNullable(true);
            });
        }
    }
}
