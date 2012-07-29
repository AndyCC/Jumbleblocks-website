using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Jumbleblocks.Core.Cryptography
{
    /// <summary>
    /// Operations for MD5 string hashing
    /// </summary>
    public class MD5StringHasher : IStringHasher
    {
        /// <summary>
        /// hashes password using MD5
        /// </summary>
        /// <param name="input">plain text</param>
        /// <returns>32 character hexadecimal string</returns>
        public string GetHash(string input)
        {
            if(String.IsNullOrWhiteSpace(input))
                throw new StringArgumentNullOrEmptyException("input");

            byte[] data;

            using (var md5Hasher = new MD5CryptoServiceProvider())
            {
                data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            }

            var hashBuilder = new StringBuilder();

            foreach(byte b in data)
                hashBuilder.Append(b.ToString("x2"));           

            return hashBuilder.ToString();
        }

        /// <summary>
        /// Checks to see if input matches hash
        /// </summary>
        /// <param name="input">the input</param>
        /// <param name="hash">hash to use</param>
        /// <returns>true if matches hash, otherwise false</returns>
        public bool MatchesHash(string input, string hash)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new StringArgumentNullOrEmptyException("input");

            if (String.IsNullOrWhiteSpace(hash))
                throw new StringArgumentNullOrEmptyException("hash");

            if (hash.Length != 32)
                throw new ArgumentException("Hash should be 32 charachters long", "hash");

            string hashedInput = GetHash(input);

            return hashedInput.Equals(hash, StringComparison.OrdinalIgnoreCase); 
        }
    }
}
