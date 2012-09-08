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
    public class ImageParseRuleTests
    {
        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimeter_Is_True_And_IsEndingDelimeter_Is_False_THEN_Returns_Html_For_Image()
        {
            Token t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;
            t.IsEndingDelimeter = false;

            var parseRule = new ImageParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldStartWith("<img");
        }

        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimeter_Is_False_And_IsEndingDelimeter_Is_True_THEN_Returns_Html_For_Closing_Image()
        {
            Token t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = false;
            t.IsEndingDelimeter = true;

            var parseRule = new ImageParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldEqual("</img>");
        }

        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimter_Is_True_And_Has_Property_Named_Scr_THEN_Constructs_Anchor_Start_With_Src()
        {
            const string Src = "a.jpg";

            dynamic t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;
            t.IsEndingDelimeter = false;
            t.Src = Src;

            var parseRule = new ImageParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldEqual(String.Format("<img src='{0}'>", Src));
        }

        [TestMethod]
        public void TransformToHtml_WHEN_Token_IsStartingDelimter_Is_True_And_Has_Property_Named_Scr_And_Property_Named_Alt_THEN_Constructs_Anchor_Start_With_Src_And_Alt()
        {
            const string Src = "a.jpg";
            const string Alt = "An image";

            dynamic t = new Token(TokenType.Delimiter, "#a", 1, 1);
            t.IsStartingDelimeter = true;
            t.IsEndingDelimeter = false;
            t.Src = Src;
            t.Alt = Alt;

            var parseRule = new ImageParseRule();
            string html = parseRule.TransformToHtml(t, new PropertyParseRule[0]);

            html.ShouldEqual(String.Format("<img src='{0}' alt='{1}'>", Src, Alt));
        }
    }
}
