using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using System.Web;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Tests for transforming 'Wane' syntax to HTML
    /// </summary>
    [TestClass]
    public class TransformTests
    {
        /// <summary>
        /// returns a delimeter set with bold and italics and with the style class property parse rule
        /// </summary>
        /// <returns></returns>
        private DelimeterSet GetBasicDelimeterSet()
        {
            var set = new DelimeterSet();
            set.AddDelimeterParseRule(new BoldParseRule());
            set.AddDelimeterParseRule(new ItalicsParseRule());

            set.AddGlobalPropertyParseRule(new StyleClassParseRule());

            return set;
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_Null_THEN_Returns_Empty_HtmlString()
        {
            var wane = new WaneTransform(GetBasicDelimeterSet());
            IHtmlString htmlString = wane.TransformToHtml(null);
            htmlString.ToHtmlString().ShouldEqual(String.Empty);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_Empty_String_THEN_Returns_Empty_HtmlString()
        {
            var wane = new WaneTransform(GetBasicDelimeterSet());
            IHtmlString htmlString = wane.TransformToHtml(String.Empty);
            htmlString.ToHtmlString().ShouldEqual(String.Empty);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_hashB_text_hashB_THEN_returns_text_Wrapped_In_HTML_Bold_Tags()
        {
            const string text = "text";
            const string htmlBoldStart = "<b>";
            const string htmlBoldEnd = "</b>";

            string waneText = String.Format("{0}{1}{0}", DefaultDelimeterValues.Bold, text);
            string expectedHtml = String.Format("{0}{1}{2}", htmlBoldStart, text, htmlBoldEnd);

            IHtmlString htmlString = new WaneTransform(GetBasicDelimeterSet()).TransformToHtml(waneText);

            htmlString.ToHtmlString().ShouldEqual(expectedHtml);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_hashB_text_THEN_returns_text_as_barSlashB_text()
        {
            const string text = "#btext";

            IHtmlString htmlString = new WaneTransform(GetBasicDelimeterSet()).TransformToHtml(text);
            htmlString.ToHtmlString().ShouldEqual(text);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_hashB_class_equals_abc_In_SquareBrackets_text_hashB_THEN_returns_text_wrapped_in_Html_Bold_Tags_With_CSS_Class_Abc()
        {
            const string @class = "abc";

            const string text = "text"; 
            const string htmlBoldStart = "<b class='"+ @class +"'>";
            const string htmlBoldEnd = "</b>";

            string waneText = String.Format("{0}[{1}:{2}]{3}{0}", DefaultDelimeterValues.Bold, StyleClassParseRule.NameOfProperty, @class, text);
            string expectedHtml = String.Format("{0}{1}{2}", htmlBoldStart, text, htmlBoldEnd);

            Console.WriteLine(waneText);

            IHtmlString htmlString = new WaneTransform(GetBasicDelimeterSet()).TransformToHtml(waneText);

            htmlString.ToHtmlString().ShouldEqual(expectedHtml);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_Escape_hashB_text_hashB_THEN_returns_hashB_text_hashB()
        {
            const string text = "text";

            string waneText = String.Format("{0}{1}{2}{1}", DefaultDelimeterValues.Escape, DefaultDelimeterValues.Bold, text);
            string expectedHtml = String.Format("{0}{1}{0}", DefaultDelimeterValues.Bold, text);

            IHtmlString htmlString = new WaneTransform(GetBasicDelimeterSet()).TransformToHtml(waneText);
            htmlString.ToHtmlString().ShouldEqual(expectedHtml);
        }

       [TestMethod]
        public void WaneTransform_Transform_WHEN_waneText_Is_Escape_hashB_class_equals_abc_In_SquareBrackets_text_hashB_THEN_returns_hashB_class_equals_abc_In_SquareBrackets_text_hashB()
        {
            const string @class = "abc";
            const string text = "text";

            string waneText = String.Format("{0}{1}[{2}:{3}]{4}{1}", DefaultDelimeterValues.Escape, DefaultDelimeterValues.Bold, StyleClassParseRule.NameOfProperty, @class, text);
            string expectedHtml = String.Format("{0}[{1}:{2}]{3}{0}", DefaultDelimeterValues.Bold, StyleClassParseRule.NameOfProperty, @class, text);


            IHtmlString htmlString = new WaneTransform(GetBasicDelimeterSet()).TransformToHtml(waneText);
            htmlString.ToHtmlString().ShouldEqual(expectedHtml);
        }
    }
}
