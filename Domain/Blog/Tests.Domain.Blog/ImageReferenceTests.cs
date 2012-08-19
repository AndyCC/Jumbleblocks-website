using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Core;
using Jumbleblocks.Domain.Blog;

namespace Tests.Jumbleblocks.Blog.Domain
{
    [TestClass]
    public class ImageReferenceTests 
    {
       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_url_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new ImageReference(null);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_url_Is_Empty_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new ImageReference(String.Empty);
        }

       [TestMethod]
        public void Ctor_WHEN_url_Has_Value_THEN_Sets_Url_Property()
        {
            const string url = "http://www.jumbleblocks.co.uk/noimage.jpg";

            var imageReference = new ImageReference(url);

            imageReference.Url.ShouldEqual(url);
        }

    }
}
