using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Core;
using Jumbleblocks.Domain.Blog;

namespace Tests.Jumbleblocks.Blog.Domain
{
    [TestFixture]
    public class ImageReferenceTests 
    {
        [Test]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_url_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new ImageReference(null);
        }

        [Test]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_url_Is_Empty_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new ImageReference(String.Empty);
        }

        [Test]
        public void Ctor_WHEN_url_Has_Value_THEN_Sets_Url_Property()
        {
            const string url = "http://www.jumbleblocks.co.uk/noimage.jpg";

            var imageReference = new ImageReference(url);

            imageReference.Url.ShouldEqual(url);
        }

    }
}
