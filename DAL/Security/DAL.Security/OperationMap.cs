using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Domain.Security;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using NHibernate.Mapping.ByCode;

namespace Jumbleblocks.DAL.Security
{
    public class OperationMap : ClassMapping<Operation>
    {
        public OperationMap()
        {
            Table("Operations");

            Id(o => o.Id, map =>
                {
                    map.Column("Id");
                    map.Type(new Int32Type());
                    map.Generator(Generators.Native);
                });

            Property(p => p.Name, map =>
                {
                    map.Column("Name");
                    map.Length(100);
                    map.NotNullable(true);
                });
        }        
    }
}
