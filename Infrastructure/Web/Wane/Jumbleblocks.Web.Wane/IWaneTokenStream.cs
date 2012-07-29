using System;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// interface for wane text reader
    /// </summary>
    public interface IWaneTokenStream
    {
        /// <summary>
        /// Returns a value indicating if end of text file has been reached
        /// </summary>
        bool IsEndOfStream { get; }

        /// <summary>
        /// Reads the next token, and increments the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        Token ReadNextToken();

        /// <summary>
        /// Reads the next token but does not increment the stream
        /// </summary>
        /// <returns>Token, or null if no more tokens</returns>
        Token Peek();

        /// <summary>
        /// Finds a token with the given delimeter
        /// </summary>
        /// <param name="delimeter">delimeter to find</param>
        /// <returns>found token, or null if no token found</returns>
        Token Find(string delimeter);
    }
}
