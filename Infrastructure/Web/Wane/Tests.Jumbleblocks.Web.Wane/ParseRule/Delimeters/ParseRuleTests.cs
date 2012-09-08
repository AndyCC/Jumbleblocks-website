using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane.ParseRule.Delimeters
{
    [TestClass]
    public class ParseRuleTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transform_WHEN_token_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.TransformToHtml(null, new List<PropertyParseRule>(0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transform_WHEN_globalPropertyParseRules_Is_Null_THEN_Throws_ArgumentNullException()
        {
            Token t = new Token(TokenType.Delimiter, "#b", 1, 1);

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.TransformToHtml(t, null);
        }

       [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transform_WHEN_token_TokenType_Is_Not_Delimeter_THEN_Throws_InvalidOperationException()
        {
            Token t = new Token(TokenType.Text, "text", 1, 1);

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.TransformToHtml(t, new List<PropertyParseRule>(0));
        }

       [TestMethod]
        public void Transform_WHEN_token_TokenType_Is_Delimeter_AND_Is_For_BOLD_And_Token_Is_For_Opening_Bold_THEN_Returns_HTML_To_Start_Bold()
        {
            Token t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.IsStartingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            string html = parseRule.TransformToHtml(t, new List<PropertyParseRule>(0));

            html.ShouldEqual("<b>");
        }

       [TestMethod]
        public void Transform_WHEN_token_TokenType_Is_Delimeter_AND_Is_For_BOLD_And_Token_Is_For_Closing_Bold_THEN_Returns_HTML_To_Close_Bold()
        {
            Token t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.IsEndingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            string html = parseRule.TransformToHtml(t, new List<PropertyParseRule>(0));

            html.ShouldEqual("</b>");
        }

       [TestMethod]
        public void Transform_GIVEN_ParseRule_Has_PropertyParseRule_For_StyleClass_WHEN_token_is_for_Bold_And_Is_Starting_Delimeter_With_No_Properties_THEN_Returns_HTML_For_Bold()
        {
            Token t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.IsStartingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.AddPropertyParseRule(new StyleClassParseRule());

            string html = parseRule.TransformToHtml(t, new List<PropertyParseRule>(0));

            html.ShouldEqual("<b>");
        }

       [TestMethod]
        public void Transform_GIVEN_ParseRule_Has_PropertyParseRule_For_StyleClass_WHEN_token_is_for_Bold_And_Is_Starting_Delimeter_With_Property_For_StyleClass_With_Value_ABC_THEN_Returns_HTML_For_Bold_With_Class_Attribute()
        {
            const string CssClass = "ABC";

            dynamic t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.StyleClass = CssClass;
            t.IsStartingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.AddPropertyParseRule(new StyleClassParseRule());

            string html = parseRule.TransformToHtml(t, new List<PropertyParseRule>(0));

            html.ShouldEqual(String.Format("<b class='{0}'>", CssClass));
        }

       [TestMethod]
        public void Transform_GIVEN_globalParseRule_For_StyleClass_WHEN_token_is_for_Bold_And_Is_Starting_Delimeter_With_No_Properties_THEN_Returns_HTML_For_Bold()
        {
            Token t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.IsStartingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");

            string html = parseRule.TransformToHtml(t, new PropertyParseRule[]{new StyleClassParseRule()});

            html.ShouldEqual("<b>");
        }

       [TestMethod]
        public void Transform_GIVEN_globalParseRule_StyleClass_WHEN_token_is_for_Bold_And_Is_Starting_Delimeter_With_Property_For_StyleClass_With_Value_ABC_THEN_Returns_HTML_For_Bold_With_Class_Attribute()
        {
            const string CssClass = "ABC";

            dynamic t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.StyleClass = CssClass;
            t.IsStartingDelimeter = true;

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");

            string html = parseRule.TransformToHtml(t, new PropertyParseRule[] { new StyleClassParseRule() });

            html.ShouldEqual(String.Format("<b class='{0}'>", CssClass));
        }

        [TestMethod]
        public void Transform_GIVEN_globalParseRule_For_StyleClass_Which_Outputs_id_And_localParseRule_For_StyleClass_Which_Outputs_class_WHEN_Token_Is_For_Bold_THEN_Returns_Result_Of_LocalPropertyParseRule_Which_Is_Bold_With_Class_Attribute()
        {
            const string CssClass = "ABC";

            dynamic t = new Token(TokenType.Delimiter, "#b", 1, 1);
            t.StyleClass = CssClass;
            t.IsStartingDelimeter = true;

            var styleClassRule = new StyleClassParseRule();

            var parseRule = new DelimeterParseRule("Bold", DefaultDelimeterValues.Bold, "b");
            parseRule.AddPropertyParseRule(styleClassRule);

            string html = parseRule.TransformToHtml(t, new PropertyParseRule[] { new PropertyParseRule(styleClassRule.PropertyName, "id") });

            html.ShouldEqual(String.Format("<b class='{0}'>", CssClass));
        }

    }
}
