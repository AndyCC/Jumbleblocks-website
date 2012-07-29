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
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("Users");

            Id(u => u.Id, map =>
                {
                    map.Column("Id");
                    map.Type(new Int32Type());
                    map.Generator(Generators.Native);
                });

            Property(p => p.Username, map =>
                {
                    map.Column("Username");
                    map.Length(255);
                    map.NotNullable(true);
                });

            Property(p => p.Password, map =>
                {
                    map.Column("Password");
                    map.Length(32);
                    map.NotNullable(true);
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


            Bag<Role>(p => p.Roles, p =>
            {
                p.Table("UserRoles");
                p.Cascade(Cascade.All);
                p.Key(k => k.Column("UserId"));
                p.Lazy(CollectionLazy.NoLazy);
                p.Access(Accessor.Field);
                p.Inverse(true);
            },
             rel =>
             {
                 rel.ManyToMany(map =>
                   {
                       map.Column("RoleId");
                       map.Lazy(LazyRelation.NoLazy);
                       map.NotFound(NotFoundMode.Ignore);
                       map.Class(typeof(Role));
                   });
             }
            );

        }
    }
}
