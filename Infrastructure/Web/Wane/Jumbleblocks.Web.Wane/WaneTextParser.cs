using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Parse which transforms text into list of tokens for tranformation into HTML
    /// </summary>
    public class WaneTextParser
    {
        public WaneTextParser(IDelimeterSet delimeterSet)
        {
            _delimeterSet = delimeterSet;
            _textReader = new WaneTextReader(delimeterSet.GetAllDelimeters());
        }

        /// <summary>
        /// DelimeterSet containing details on delimeters
        /// </summary>
        private IDelimeterSet _delimeterSet;

        /// <summary>
        /// Wane Text Reader to use
        /// </summary>
        private WaneTextReader _textReader;

        /// <summary>
        /// ParsesText to produce a list of Tokens
        /// </summary>
        /// <param name="waneText">text to parse</param>
        /// <returns>list of tokens</returns>
        public IEnumerable<Token> ParseText(string waneText)
        {
            _textReader.SetText(waneText);
            return ProcessTokenStream(_textReader);
        }

        /// <summary>
        /// Processes a stream or substream of tokens
        /// </summary>
        /// <param name="tokenStream">tokens to process</param>
        /// <returns>list of tokens</returns>
        private List<Token> ProcessTokenStream(IWaneTokenStream tokenStream)
        {
            var tokens = new List<Token>();
            bool isEscaping = false;

            while (!tokenStream.IsEndOfStream)
            {
                Token currentToken = tokenStream.ReadNextToken();

                //handle escaped token
                if (isEscaping)
                {
                    currentToken.ChangeTokenTypeToText();
                    isEscaping = false;
                }

                //check if escape character and is escaping something
                isEscaping = DelimeterCausesEscape(currentToken, tokenStream);

                if (IsCustomDelimeter(currentToken) && !isEscaping)
                {
                    List<Token> handledTokens = HandleCustomDelimeter(currentToken, tokenStream);
                    tokens.AddRange(handledTokens);
                }
                else if (currentToken.TokenType == TokenType.NewLine)
                    tokens.Add(currentToken);
                else if(!isEscaping)
                    AppendTextToTokens(tokens, currentToken);
            }

            return tokens;
        }

        /// <summary>
        /// Determines if a token causes the next token to be escaped
        /// </summary>
        /// <param name="currentToken">the current token to check if it's an escape char, and if the escape char is escaping</param>
        /// <param name="tokenStream">remaining token stream</param>
        /// <returns>true if causes escape, otherwise false</returns>
        private bool DelimeterCausesEscape(Token currentToken, IWaneTokenStream tokenStream)
        {
            if (IsEscapeDelimieter(currentToken))
            {
                Token nextToken = tokenStream.Peek();

                if (nextToken.IsDelimeter)
                    return true;
                else
                    currentToken.ChangeTokenTypeToText();
            }

            return false;
        }

        /// <summary>
        /// Handles a custom delimeter from it's start until it's end.
        /// Also handles tokens within those custom delimeters
        /// </summary>
        /// <param name="currentToken">current token (which is a delimeter)</param>
        /// <param name="tokenStream"></param>
        private List<Token> HandleCustomDelimeter(Token currentToken, IWaneTokenStream tokenStream)
        {
            if (!IsCustomDelimeter(currentToken))
                throw new ArgumentException("Expected custome delimeter", "currentToken");

            var tokens = new List<Token>();

            currentToken.IsStartingDelimeter = true;
            tokens.Add(currentToken);

            //find ending delimeter
            List<Token> tokensToInspect = GetRawTokensUntilAndIncludingEndingDelimeter(currentToken, tokenStream);

            if (tokensToInspect.Count > 0)
            {
                //remove ending token else this will not be processed properly
                Token closingToken = tokensToInspect.Last();
                tokensToInspect.RemoveAt(tokensToInspect.Count - 1);

                var internalStream = new WaneEnumerableTokenStream(tokensToInspect);

                //get and remove properties
                ExtractPropertiesFromTokenListAndApplyToCurrentToken(currentToken, internalStream);

                if (!internalStream.IsEndOfStream)
                {
                    List<Token> procesedInternalStreamTokens = ProcessTokenStream(internalStream);
                    tokens.AddRange(procesedInternalStreamTokens);
                }

                //add closing delimeter token
                closingToken.IsEndingDelimeter = true;
                tokens.Add(closingToken);

                WarnIfClosingTokenHasProperties(closingToken, tokenStream);
            }
            else 
            {
                //no closing delimeter
                currentToken.ChangeTokenTypeToText();
                currentToken.AddWarning("Delimeter start does not have a matching end delimeter, or the actual start delimeter has been escaped");
            }

            return tokens;
        }

        /// <summary>
        /// Appends text to list of tokens that already exists.
        /// 
        /// If previous token was text, then appends to that, otherwise appends as it's own token
        /// </summary>
        /// <param name="tokens">tokens to append to</param>
        /// <param name="currentToken">current token</param>
        /// <param name="allowCurrentTokenToBeAppended">if true if the previous token is not a TEXT then allows current token to be appended to stream, if false only allows token to be combined with previous token if that previous token is TEXT</param>
        private void AppendTextToTokens(List<Token> tokens, Token currentToken)
        {
            Token previousToken = tokens.LastOrDefault();

            if (previousToken != null && previousToken.IsText && !previousToken.HasWarnings)
                previousToken.Text += currentToken.Text;
            else if(!IsEscapeDelimieter(currentToken))
            {
                if(currentToken.TokenType != TokenType.Text)
                    currentToken.ChangeTokenTypeToText();

                tokens.Add(currentToken);
            }
        }

        /// <summary>
        /// Checks to see if token is custom delimeter
        /// </summary>
        /// <param name="token">token to check</param>
        /// <returns>true if is custom, otherwise false</returns>
        private bool IsCustomDelimeter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsCustomDelimeter(token.Text);
        }

        /// <summary>
        /// Checks to see if token represents the properties start delimeter
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>true if is, otherwise false</returns>
        private bool IsPropertiesStartDelimeter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsPropertiesStartDelimeter(token.Text);
        }

        /// <summary>
        /// Checks to see if token represents the properties end delimeter
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>true if is, otherwise false</returns>
        private bool IsPropertiesEndDelimeter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsPropertiesEndDelimeter(token.Text);
        }

        /// <summary>
        /// Checks to see if token represents the properties seperator delimeter
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>true if is, otherwise false</returns>
        private bool IsPropertiesSeperatorDelimeter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsPropertySeperatorDelimeter(token.Text);
        }

        /// <summary>
        /// Checks to see if token represents the seperator between property name and property value delimeter
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>true if is, otherwise false</returns>
        private bool IsPropertyNameValueSeperatorDelimeter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsPropertyNameValueSeperatorDelimeter(token.Text);
        }

        /// <summary>
        /// Checks to see if the token is an escape delimeter
        /// </summary>
        /// <param name="token">token to check</param>
        /// <returns>true if is, otherwise false</returns>
        private bool IsEscapeDelimieter(Token token)
        {
            return token.IsDelimeter && _delimeterSet.IsEscapeDelimeter(token.Text);
        }
         
        /// <summary>
        /// Gets a sub stream of tokens to parse
        /// </summary>
        /// <param name="findEndingTokenFor">searches for a matching token in the stream, the match will be the end of the sub-stream (or then end of the text will be)</param>
        /// <param name="tokenStream">token stream being read from</param>
        /// <returns>sub stream of tokens, or empty list if no ending delimeter</returns>
        private List<Token> GetRawTokensUntilAndIncludingEndingDelimeter(Token findEndingTokenFor, IWaneTokenStream tokenStream)
        {
            var subStream = new List<Token>();
            bool hasFoundEndingToken = false;
            string endingDelimeter = _delimeterSet.GetEndingDelimterForStartingDelimeter(findEndingTokenFor.Text);

            Token endingToken = tokenStream.Find(endingDelimeter);           

            if (endingToken != null)
            {
                while (!tokenStream.IsEndOfStream && !hasFoundEndingToken)
                {
                    Token token = tokenStream.ReadNextToken();
                    subStream.Add(token);

                    hasFoundEndingToken = token.Text == endingToken.Text;
                }
            }

            return subStream;
        }

        /// <summary>
        /// Extracts tokens which represent properties for a given token and apply put those properties onto the token
        /// </summary>
        /// <param name="currentToken">token that will have properties added to it</param>
        /// <param name="tokenStream">token stream to inspect</param>
        private void ExtractPropertiesFromTokenListAndApplyToCurrentToken(Token currentToken, IWaneTokenStream tokenStream)
        {
            if (IsPropertiesStartDelimeter(tokenStream.Peek()))
            {
                List<Token> rawPropertyTokens = GetRawTokensUntilAndIncludingEndingDelimeter(tokenStream.Peek(), tokenStream);

                if (rawPropertyTokens.Count == 0)
                    currentToken.AddWarning("Properties start delimeter found, but could not find ending properties delimeter.");
                else if(rawPropertyTokens.Count == 2)
                    currentToken.AddWarning("Properties declaration contains no key value pairs. Could not construct properties.");
                else
                    ProcessProperties(currentToken, rawPropertyTokens);         
            }
        }        

        private void ProcessProperties(Token currentToken, List<Token> propertyTokens)
        {
            //get rid of start and end.
            propertyTokens.RemoveAt(0);
            propertyTokens.RemoveAt(propertyTokens.Count - 1);

            List<List<Token>> foundPropertySets = FindPropertySets(propertyTokens);

            //check each property set has name, name value seperator and value
            AddPropertiesAndWarnings(currentToken, foundPropertySets);

        }

        /// <summary>
        /// Finds sets of tokens which belong together as a "property set" ie key, key value seperator and value
        /// </summary>
        /// <param name="propertyTokens">tokens to look through</param>
        /// <returns>list of list of tokens</returns>
        private List<List<Token>> FindPropertySets(List<Token> propertyTokens)
        {
            var foundPropertySets = new List<List<Token>>();

            var propertyTokenSet = new List<Token>();
            for (int i = 0; i < propertyTokens.Count; i++)
            {
                propertyTokenSet.Add(propertyTokens[i]);

                if ((i == propertyTokens.Count - 1) || IsPropertiesSeperatorDelimeter(propertyTokens[i]))
                {
                    foundPropertySets.Add(propertyTokenSet);
                    propertyTokenSet = new List<Token>();
                }
            }

            return foundPropertySets;
        }

        /// <summary>
        /// Adds each found property and any associated warnings to token it belongs to 
        /// </summary>
        /// <param name="currentToken">token to add warning to</param>
        /// <param name="foundPropertySets">property sets which have been found (list of list of tokens, where the inner list represents key, key value seperator and value)</param>
        private void AddPropertiesAndWarnings(Token currentToken, List<List<Token>> foundPropertySets)
        {
            foreach (var propertyTokenSet in foundPropertySets)
            {
                //check for errors
                if (propertyTokenSet.Count == 1)
                {
                    if (IsPropertyNameValueSeperatorDelimeter(propertyTokenSet[0]))
                        currentToken.AddWarning("Property added with no key or value");
                    else
                        currentToken.AddWarning(String.Format("Properties declaration contains no value for the key {0}", propertyTokenSet[0].Text));
                }
                else if (propertyTokenSet.Count == 2)
                {
                    if (IsPropertyNameValueSeperatorDelimeter(propertyTokenSet[0]))
                        currentToken.AddWarning(String.Format("A property has been added with the value {0} but no key", propertyTokenSet[1].Text));
                    else if (IsPropertyNameValueSeperatorDelimeter(propertyTokenSet[1]))
                        currentToken.AddWarning(String.Format("Properties declaration contains no value for the key {0}", propertyTokenSet[0].Text));
                    else
                        currentToken.AddWarning(String.Format("Can not determine key value seperator. {0} and {1} supplied", propertyTokenSet[0].Text, propertyTokenSet[1].Text));
                }
                else
                {
                     string propertyName = propertyTokenSet[0].Text;

                     if (_delimeterSet.IsDelimeterAllowedProperty(currentToken.Text, propertyName))
                     {
                         string propertyValue = propertyTokenSet[2].Text;
                         currentToken[propertyName] = propertyValue;
                     }
                     else
                         currentToken.AddWarning(String.Format("The property '{0}' is not allowed on this delimeter.", propertyName));
                }
            }
        }

        /// <summary>
        /// Checks to see if a given token (representing a closing delimeter has properties. If it does then adds warning to that token
        /// </summary>
        /// <param name="closingToken">token representing delimeter</param>
        /// <param name="tokenStream">remaining token stream</param>
        private void WarnIfClosingTokenHasProperties(Token closingToken, IWaneTokenStream tokenStream)
        {
            //check for closing token properties
            Token possiblePropertyToken = tokenStream.Peek();

            if (possiblePropertyToken != null && IsPropertiesStartDelimeter(possiblePropertyToken))
            {
                //check if has full properties
                List<Token> propertyTokens = GetRawTokensUntilAndIncludingEndingDelimeter(possiblePropertyToken, tokenStream);

                if (propertyTokens.Count > 0)
                    closingToken.AddWarning("Can not put properties on closing delimeters.");
            }
        }

    }
}
