using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane.ParseRule.Properties
{
    [TestClass]
    public class SrcParseRuleTests
    {
        [TestMethod]
        public void ConstructHtml_WHEN_Token_Has_Src_Property_Name_THEN_Return_HTML_For_src()
        {
            const string ImgSrc = "a.jpg";

            dynamic t = new Token(TokenType.Delimiter, "#img", 1, 1);
            t.Src = ImgSrc;
            t.IsStartingDelimeter = true;

            var parseRule = new SrcParseRule();
            string htmlFragment = parseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Format("src='{0}'", ImgSrc));
        }

        [TestMethod]
        public void ConstructHtml_WHEN_Token_Does_Not_Have_Url_Property_Name_THEN_Returns_Empty_String()
        {
            dynamic t = new Token(TokenType.Delimiter, "#img", 1, 1);
            t.IsStartingDelimeter = true;

            var parseRule = new SrcParseRule();
            string htmlFragment = parseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Empty);
        }
    }
}
