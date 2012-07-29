using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// reader to read wane text
    /// </summary>
    public class WaneTextReader : IWaneTokenStream
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="delimeters">delimeters that can be read</param>
        public WaneTextReader(string[] delimeters)
        {
            if (delimeters == null)
                throw new ArgumentNullException("delimeters");
            else if (delimeters.Length == 0)
                throw new ArgumentException("must have at least 1 delimiter", "delimeters");

            IsEndOfStream = false;
            IsSetup = false;

            BuildRegexes(delimeters);
        }

        private const string NewLineRegexOptions = "\r\n?|\n";

        /// <summary>
        /// Returns a value indicating if end of text file has been reached
        /// </summary>
        public bool IsEndOfStream { get; private set; }

        /// <summary>
        /// Returns a value indicating if reader has been set up
        /// </summary>
        public bool IsSetup { get; private set; }

        /// <summary>
        /// The text
        /// </summary>
        private string Text { get; set; }

        /// <summary>
        /// a regex to match delimeters into
        /// </summary>
        private Regex DelimetersRegexWithNewLine { get; set; }

        /// <summary>
        /// line position within text
        /// </summary>
        private int LinePosition { get; set; }

        /// <summary>
        /// char position in line
        /// </summary>
        private int CharPositionInLine { get; set; }

        /// <summary>
        /// sets up the text reader 
        /// </summary>
        /// <param name="text">text to read</param>
        /// <param name="delimeters">allowed delimeters</param>
        public void SetText(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                IsEndOfStream = true;
            else 
                IsEndOfStream = false;

            Text = text;
            LinePosition = 0;
            CharPositionInLine = 0;
            
            IsSetup = true;
        }

        /// <summary>
        /// Builds regex to find delimeter with
        /// </summary>
        /// <param name="delimeters">list of delimeters to find</param>
        private void BuildRegexes(string[] delimeters)
        {
            var regexBuidlerWithNewLine = new StringBuilder("(");

            foreach (string delimeter in delimeters)
            {
                string escaptedDelimeter = AddRegexEscapeCharToDelimeterIfRequired(delimeter);
                regexBuidlerWithNewLine.AppendFormat("{0}|", escaptedDelimeter);
            }

            regexBuidlerWithNewLine.Append(NewLineRegexOptions);
            regexBuidlerWithNewLine.Append(")");
        
            DelimetersRegexWithNewLine = new Regex(regexBuidlerWithNewLine.ToString());
        }

        /// <summary>
        /// Checks to see if delimeter will at any point be interpreted by regular expression as a special character, if so escapes it
        /// </summary>
        /// <param name="delimeter">delimeter to check</param>
        /// <returns>resulting delimeter</returns>
        private string AddRegexEscapeCharToDelimeterIfRequired(string delimeter)
        {
            const char RegexEscapeChar = '\\';
            var metaChars = new char[] { '[', ']', '\\', '^', '$', '.', '|', '?', '*', '+', '(', ')' };

            var delimeterReBuilder = new StringBuilder();

            foreach (char c in delimeter)
            {
                if (metaChars.Contains(c))
                    delimeterReBuilder.Append(RegexEscapeChar);

                delimeterReBuilder.Append(c);
            }

            return delimeterReBuilder.ToString();
        }

        /// <summary>
        /// Reads the next token, and increments the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        public Token ReadNextToken()
        {
            return NextToken(true);
        }

        /// <summary>
        /// Reads the next token but does not increment the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        public Token Peek()
        {
            return NextToken(false);
        }


        /// <summary>
        /// reads next token from stream
        /// </summary>
        /// <param name="incrementStream">if true increments stream to start of next token, false to just peek</param>
        /// <returns>Token that has been found</returns>
        private Token NextToken(bool incrementStream)
        {
            if (!IsSetup)
                throw new InvalidOperationException("WaneTextReader has not been setup");

            if (IsEndOfStream)
                return null;

            Tuple<int, int> nextIndexAndLength = IndexAndLengthOfNextDelimeterOrNewline();
            Token token = CreateToken(nextIndexAndLength);

            if (incrementStream)
            {
                RemoveTokenisedTextAndUpdatePositions(token);
                CheckIfIsEndOfText();
            }

            return token;
        }
    
        /// <summary>
        /// Gets index and length of next delimeter
        /// </summary>
        /// <returns>Tuple of index, length</returns>
        private Tuple<int, int> IndexAndLengthOfNextDelimeterOrNewline()
        {
            Match m = DelimetersRegexWithNewLine.Match(Text);

            int index = m.Index;

            if (m.Length == 0)
                index = Text.Length;

            return new Tuple<int, int>(index, m.Length);
        }

        /// <summary>
        /// Finds a token with the given delimeter
        /// </summary>
        /// <param name="delimeter">delimeter to find</param>
        /// <returns>found token, or null if no token found</returns>
        public Token Find(string delimeter)
        {
            Tuple<int, int> nextIndexAndLength = IndexAndLengthOfNextDelimeter(delimeter);

            if (nextIndexAndLength.Item1 == -1)
                return null;

            Token token = CreateTokenFromPosition(nextIndexAndLength);

            if (!token.Text.Equals(delimeter, StringComparison.CurrentCultureIgnoreCase))
                return null;
            else
                return token;
        }

        /// <summary>
        /// Gets the index and length of the next delimeter, reguardless of new lines
        /// </summary>
        /// <param name="delimeter">delimeter to find</param>
        /// <returns>Tuple of index, length. Index and length will be -1 if not found</returns>
        private Tuple<int, int> IndexAndLengthOfNextDelimeter(string delimeter)
        {
            string pattern = AddRegexEscapeCharToDelimeterIfRequired(delimeter);

            Match m = Regex.Match(Text, delimeter);

            if (m.Success)
                return new Tuple<int, int>(m.Index, m.Length);
            else
                return new Tuple<int, int>(-1, -1);
        }

        /// <summary>
        /// Creates a token from the provided index and length
        /// </summary>
        /// <param name="nextIndexAndLength">index (Item1) and length (Item2) of next delimeter or new line</param>
        /// <returns>Token</returns>
        private Token CreateToken(Tuple<int, int> nextIndexAndLength)
        {
            Token token = null;

            if(nextIndexAndLength.Item1 == 0)
            {
                token = CreateTokenFromPosition(nextIndexAndLength);
            }
            else
            {
                //looking at text up to delimeter 
                string foundText = Text.Substring(0, nextIndexAndLength.Item1);
                token = new Token(TokenType.Text, foundText, LinePosition + 1, CharPositionInLine + 1);
            }

           return token;
        }

        /// <summary>
        /// Create a token from 
        /// </summary>
        /// <param name="nextIndexAndLength"></param>
        /// <returns></returns>
        private Token CreateTokenFromPosition(Tuple<int, int> nextIndexAndLength)
        {
            string foundText = Text.Substring(nextIndexAndLength.Item1, nextIndexAndLength.Item2);
            TokenType tokenType = IsNewLine(foundText) ? TokenType.NewLine : TokenType.Delimiter;
            return new Token(tokenType, foundText, LinePosition + 1, CharPositionInLine + 1);
        }

        /// <summary>
        /// determines if text represents a new line
        /// </summary>
        /// <param name="text">text to check</param>
        /// <returns>true if new line, otherwise false</returns>
        private bool IsNewLine(string text)
        {
            const string regexPattern = "^(" + NewLineRegexOptions + ")$";
            return Regex.IsMatch(text, regexPattern);
        }

        /// <summary>
        /// Removes text which has been tokenised from the text in the reader
        /// Also updates line and char position counts
        /// </summary>
        /// <param name="token">Token containing the text to remove</param>
        private void RemoveTokenisedTextAndUpdatePositions(Token token)
        {
            Text = Text.Remove(0, token.Text.Length);

            if (token.TokenType == TokenType.NewLine)
            {
                LinePosition++;
                CharPositionInLine = 0;
            }
            else
                CharPositionInLine += token.Text.Length;
        }

        /// <summary>
        /// Checks to see if the end of the text file has been reaches
        /// </summary>
        private void CheckIfIsEndOfText()
        {
            if (Text.Length == 0)
                IsEndOfStream = true;
        }
    }
}
