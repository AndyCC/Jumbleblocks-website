using Jumbleblocks.Web.Wane;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane.ParseRule.Properties
{
    [TestClass]
    public class AltParseRuleTests
    {
        [TestMethod]
        public void ConstructHtml_WHEN_Token_Has_Alt_Property_Name_THEN_Return_HTML_For_alt()
        {
            const string ImgSrc = "a.jpg";

            dynamic t = new Token(TokenType.Delimiter, "#img", 1, 1);
            t.Alt = ImgSrc;
            t.IsStartingDelimeter = true;

            var parseRule = new AltParseRule();
            string htmlFragment = parseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Format("alt='{0}'", ImgSrc));
        }

        [TestMethod]
        public void ConstructHtml_WHEN_Token_Does_Not_Have_Alt_Property_Name_THEN_Returns_Empty_String()
        {
            dynamic t = new Token(TokenType.Delimiter, "#img", 1, 1);
            t.IsStartingDelimeter = true;

            var parseRule = new AltParseRule();
            string htmlFragment = parseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Empty);
        }
    }
}
