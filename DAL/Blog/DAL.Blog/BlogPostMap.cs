using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using NHibernate.Mapping.ByCode;
using Jumbleblocks.Domain.Blog;
using NHibernate;

namespace Jumbleblocks.DAL.Blog
{
    /// <summary>
    /// Mapping for blogpost
    /// </summary>
    public class BlogPostMap : ClassMapping<BlogPost>
    {
        public BlogPostMap()
        {
            Table("BlogPosts");
            
            Id(bp => bp.Id, map =>
            {
                map.Column("Id");
                map.Type(new Int32Type());
                map.Generator(Generators.Native);
            });

            Property(p => p.Title, map =>
            {
                map.Column("Title");
                map.Length(255);
                map.NotNullable(true);
            });

            Property(p => p.Description, map =>
            {
                map.Column("Description");
                map.Length(2000);
                map.NotNullable(true);
                map.Lazy(true);
            });

            Property(p => p.FullArticle, map =>
            {
                map.Column("FullArticle");
                map.NotNullable(true);
                map.Lazy(true);
                map.Type(NHibernateUtil.StringClob);
                map.Column(c => c.SqlType("VARCHAR(MAX)"));
            });

            Property(p => p.PublishedDate, map =>
            {
                map.Column("PublishedDate");
                map.NotNullable(false);
            });

            ManyToOne(p => p.ImageReference, map =>
            {
                map.Column("ImageReferenceId");
                map.Cascade(Cascade.Detach | Cascade.Merge | Cascade.Persist | Cascade.ReAttach | Cascade.Refresh);
                map.NotNullable(false);
            });

            ManyToOne(p => p.Author, map =>
            {
                map.Column("AuthorId");
                map.Cascade(Cascade.Detach | Cascade.Merge | Cascade.Persist | Cascade.ReAttach | Cascade.Refresh);
                map.NotNullable(true);
            });

            Bag<Tag>(p => p.Tags, p =>
            {
                p.Table("BlogPostTags");
                p.Access(Accessor.Field);
                p.Cascade(Cascade.All);
                p.Key(k => k.Column("BlogPostId"));
                p.Lazy(CollectionLazy.Lazy);
            },
            rel =>
            {
                rel.ManyToMany(map =>
                {
                    map.Column("TagId");
                    map.Lazy(LazyRelation.Proxy);
                    map.NotFound(NotFoundMode.Ignore);
                    map.Class(typeof(Tag));
                });
            });

            ManyToOne(p => p.Series, map =>
                {
                    map.Column("SeriesId");
                    map.Cascade(Cascade.Detach | Cascade.Merge | Cascade.Persist | Cascade.ReAttach | Cascade.Refresh);
                    map.NotNullable(false);
                });

            Property(p => p.DeletedDate, map =>
                {
                    map.Column("DeletedDate");
                });

            ManyToOne(p => p.DeletedByUser, map =>
            {
                map.Column("DeletedByUserId");
                map.Cascade(Cascade.None);
            });
        
        }
    }
}
