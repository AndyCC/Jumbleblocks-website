using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Testing;

namespace Tests.Jumbleblocks.Web.Wane.ParseRule.Properties
{
    [TestClass]
    public class HrefsParseRuleTests
    {
        [TestMethod]
        public void ConstructHtml_WHEN_Token_Has_Url_Property_Name_THEN_Return_HTML_For_href()
        {
            const string Url = "http://www.jumbleblocks.com";

            dynamic t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.Url = Url;
            t.IsStartingDelimeter = true;

            var hrefParseRule = new HrefParseRule();
            string htmlFragment = hrefParseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Format("href='{0}'", Url));
        }

        [TestMethod]
        public void ConstructHtml_WHEN_Token_Does_Not_Have_Url_Property_Name_THEN_Returns_Empty_String()
        {
            dynamic t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;

            var hrefParseRule = new HrefParseRule();
            string htmlFragment = hrefParseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Empty);
        }
    }
}
