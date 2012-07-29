using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Linq;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Wane transformation
    /// </summary>
    public class WaneTransform : IWaneTransform
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="delimeterSet">the delimeter set to use to determine how to translate wanetext to HTML</param>
        public WaneTransform(IDelimeterSet delimeterSet)
        {
            Parser = new WaneTextParser(delimeterSet);
            TokenTransformer = new TokenTransformer(delimeterSet);
        }

        /// <summary>
        /// The parser to parse WaneText to tokens
        /// </summary>
        protected WaneTextParser Parser { get; private set; }

        /// <summary>
        /// The transformer which can transform tokens to HTML
        /// </summary>
        protected TokenTransformer TokenTransformer { get; private set; }

        /// <summary>
        /// Transforms Wane to html
        /// </summary>
        /// <param name="waneText">text to transform to HTML</param>
        /// <param name="shouldHtmlEncode">determines if waneText should be HTML encoded before the transformation to HTML occurs</param>
        /// <returns>IHtmlString</returns>
        public IHtmlString TransformToHtml(string waneText, bool shouldHtmlEncode = true)
        {
            return new HtmlString(TransformToRawHtml(waneText, shouldHtmlEncode));
        }

        /// <summary>
        /// Transforms Wane to html
        /// </summary>
        /// <param name="waneText">text to transform to HTML</param>
        /// <param name="shouldHtmlEncode">determines if waneText should be HTML encoded before the transformation to HTML occurs</param>
        /// <returns>String</returns>
        public string TransformToRawHtml(string waneText, bool shouldHtmlEncode = true)
        {
            if (String.IsNullOrWhiteSpace(waneText))
                return String.Empty;

            if (shouldHtmlEncode)
                waneText = HttpUtility.HtmlEncode(waneText);

            IEnumerable<Token> tokens = Parser.ParseText(waneText);
            string html = TokenTransformer.ToHtml(tokens);

            return html;
        }

    }
}
