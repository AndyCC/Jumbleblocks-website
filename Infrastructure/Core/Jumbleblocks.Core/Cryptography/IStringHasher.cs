using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Cryptography
{
    /// <summary>
    /// interface for hash operations on MD5 
    /// </summary>
    public interface IStringHasher
    {
        /// <summary>
        /// hashes password 
        /// </summary>
        /// <param name="input">plain text</param>
        /// <returns>32 character hexadecimal string</returns>
        string GetHash(string input);

        /// <summary>
        /// Checks to see if input matches hash
        /// </summary>
        /// <param name="input">the input</param>
        /// <param name="hash">hash to use</param>
        /// <returns>true if matches hash, otherwise false</returns>
        bool MatchesHash(string input, string hash);
    }
}
