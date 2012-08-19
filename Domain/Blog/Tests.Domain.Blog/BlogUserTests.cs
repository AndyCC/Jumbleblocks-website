using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Blog;

namespace Tests.Jumbleblocks.Blog.Domain
{
    [TestClass]
    public class BlogUserTests 
    {
       [TestMethod]
        public void FullName_WHEN_Forenames_Is_Jonny_AND_Surname_English_THEN_Returns_Jonny_English()
        {
            var blogPostUser = new BlogUser
            {
                Forenames = "Jonny",
                Surname = "English"
            };

            blogPostUser.FullName.ShouldEqual(String.Format("{0} {1}", blogPostUser.Forenames, blogPostUser.Surname));
        }
    }
}
