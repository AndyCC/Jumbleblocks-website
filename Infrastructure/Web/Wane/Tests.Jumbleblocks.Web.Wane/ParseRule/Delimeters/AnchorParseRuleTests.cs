using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane.ParseRule.Delimeters
{
    [TestClass]
    public class AnchorParseRuleTests
    {
        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimeter_Is_True_And_IsEndingDelimeter_Is_False_THEN_Returns_Html_For_Anchor()
        {
            Token t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;
            t.IsEndingDelimeter = false;

            var parseRule = new AnchorParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldStartWith("<a");
        }

        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimeter_Is_False_And_IsEndingDelimeter_Is_True_THEN_Returns_Html_For_Closing_Anchor()
        {
            Token t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = false;
            t.IsEndingDelimeter = true;

            var parseRule = new AnchorParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldEqual("</a>");
        }

        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimter_Is_True_And_Has_Property_Named_Url_THEN_Constructs_Anchor_Start_With_Href()
        {
            const string Url = "http://www.jumbleblocks.com/";

            dynamic t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;
            t.IsEndingDelimeter = false;
            t.Url = Url;

            var parseRule = new AnchorParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldEqual(String.Format("<a href='{0}'>", Url));
        }
    }
}
