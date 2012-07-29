using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Jumbleblocks.Domain.Blog;

namespace Jumbleblocks.DAL.Blog
{
    /// <summary>
    /// mapping for image reference
    /// </summary>
    public class ImageReferenceMap : ClassMapping<ImageReference>
    {
        public ImageReferenceMap()
        {
            Table("ImageReferences");
            Id(ir => ir.Id, id => id.Generator(Generators.Native));
            Property(ir => ir.Url, map =>
            {
                map.NotNullable(true);
                map.Length(350);
            });
        }
    }
}
