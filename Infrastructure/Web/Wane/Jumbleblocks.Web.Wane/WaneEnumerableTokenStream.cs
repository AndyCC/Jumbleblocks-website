using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// A token stream based on an enumerable list
    /// </summary>
    public class WaneEnumerableTokenStream : IWaneTokenStream
    {
        public WaneEnumerableTokenStream(IEnumerable<Token> tokenStream)
        {
            if (tokenStream == null)
                throw new ArgumentNullException("tokenStream");

            _tokenStream = new List<Token>(tokenStream);
        }

        private List<Token> _tokenStream;

        /// <summary>
        /// Returns a value indicating if end of text file has been reached
        /// </summary>
        public bool IsEndOfStream
        {
            get { return _tokenStream.Count == 0; }
        }

        /// <summary>
        /// Reads the next token, and increments the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        public Token ReadNextToken()
        {
            Token token = _tokenStream.FirstOrDefault();

            if(token != null)
                _tokenStream.RemoveAt(0);

            return token;
        }

        /// <summary>
        /// Reads the next token but does not increment the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        public Token Peek()
        {
            return _tokenStream.FirstOrDefault();
        }

        /// <summary>
        /// Finds a token with the given delimeter
        /// </summary>
        /// <param name="delimeter">delimeter to find</param>
        /// <returns>found token, or null if no token found</returns>
        public Token Find(string delimeter)
        {
            foreach (Token token in _tokenStream)
            {
                if (token.Text.Equals(delimeter, StringComparison.CurrentCultureIgnoreCase))
                    return token;
            }

            return null;
        }
    }
}
