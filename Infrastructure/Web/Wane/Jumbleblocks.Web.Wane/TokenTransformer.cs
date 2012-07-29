using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// transform a list of parsed tokens into HTML
    /// </summary>
    public class TokenTransformer
    {
        public TokenTransformer(IDelimeterSet delimeterSet)
        {
            if (delimeterSet == null)
                throw new ArgumentNullException("delimeterSet");

            _delimeterSet = delimeterSet;
        }

        private IDelimeterSet _delimeterSet;

        /// <summary>
        /// Converts the tokens into a string of HTML
        /// </summary>
        /// <param name="tokens">tokens to transform</param>
        /// <returns>HTML as a string</returns>
        public string ToHtml(IEnumerable<Token> tokens)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Text)
                    htmlBuilder.Append(token.Text);
                else if (token.TokenType == TokenType.NewLine)
                    htmlBuilder.Append("<br>");
                else
                {
                    if (_delimeterSet.HasDelimeterText(token.Text))
                        htmlBuilder.Append(_delimeterSet.ParseToHtml(token));
                }
            }
      
            return htmlBuilder.ToString();
        }
    }
}
