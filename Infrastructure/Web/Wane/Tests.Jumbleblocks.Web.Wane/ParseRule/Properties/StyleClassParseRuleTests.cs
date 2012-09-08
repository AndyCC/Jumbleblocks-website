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
    public class StyleClassParseRuleTests
    {
        [TestMethod]
        public void ConstructHtml_WHEN_Token_Has_StyleClass_Property_Name_THEN_Return_HTML_For_class()
        {
            const string CssClass = "ABC";

            dynamic t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.StyleClass = CssClass;
            t.IsStartingDelimeter = true;

            var styleClassParseRule = new StyleClassParseRule();
            string htmlFragment = styleClassParseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Format("class='{0}'", CssClass));
        }

        [TestMethod]
        public void ConstructHtml_WHEN_Token_Does_Not_Have_StyleClass_Property_Name_THEN_Returns_Empty_String()
        {
            dynamic t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.IsStartingDelimeter = true;

            var styleClassParseRule = new StyleClassParseRule();
            string htmlFragment = styleClassParseRule.ConstructHtml(t);

            htmlFragment.ShouldEqual(String.Empty);
        }
    }
}
