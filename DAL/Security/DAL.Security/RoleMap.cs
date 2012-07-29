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
    public class RoleMap : ClassMapping<Role>
    {
        public RoleMap()
        {
            Table("Roles");

            Id(r => r.Id, map =>
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

            Bag<Operation>(p => p.Operations, p =>
                {
                    p.Table("RoleOperations");
                    p.Cascade(Cascade.All);
                    p.Key(k => k.Column("RoleId"));
                    p.Lazy(CollectionLazy.NoLazy);
                    p.Access(Accessor.Field);
                    p.Inverse(true);
                },
                rel =>
                {
                    rel.ManyToMany(map =>
                        {
                            map.Column("OperationId");
                            map.Lazy(LazyRelation.NoLazy);
                            map.NotFound(NotFoundMode.Ignore);
                            map.Class(typeof(Operation));
                        });
                });
        }

    }
}
