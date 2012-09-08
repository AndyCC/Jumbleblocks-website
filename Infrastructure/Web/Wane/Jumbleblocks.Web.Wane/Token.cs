using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// A token found by the reader & manipulated by the parser
    /// </summary>
    public class Token : DynamicObject
    {
        public Token(TokenType tokenType, string text, int linePosition, int charPosition)
        {
            TokenType = tokenType;
            Text = text;
            LinePosition = linePosition;
            CharPosition = charPosition;
            IsStartingDelimeter = false;
            IsEndingDelimeter = false;
        }

        /// <summary>
        /// Contains properties about a token
        /// </summary>
        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        /// <summary>
        /// List of warnings associated with the token
        /// </summary>
        private List<string> _warnings = new List<string>();

        /// <summary>
        /// line token was found on
        /// </summary>
        public int LinePosition { get; private set; }
        
        /// <summary>
        /// char position (into line) of token
        /// </summary>
        public int CharPosition { get; private set; }

        /// <summary>
        /// Gets/Sets a value indicating if token represents starting delimeter 
        /// </summary>
        public bool IsStartingDelimeter { get; set; }

        /// <summary>
        /// Gets/Sets a value indicating if token represents ending delimeter 
        /// </summary>
        public bool IsEndingDelimeter { get; set; }

        /// <summary>
        /// Type of token
        /// </summary>
        public TokenType TokenType { get; private set; }

        /// <summary>
        /// The text which was found for the token
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// Checks to see if token represents a delimeter
        /// </summary>
        public bool IsDelimeter { get { return TokenType == TokenType.Delimiter; } }

        /// <summary>
        /// Checks to see if token represents text
        /// </summary>
        public bool IsText { get { return TokenType == TokenType.Text; } }

        /// <summary>
        /// Determines if tokens has warnings.
        /// </summary>
        public bool HasWarnings { get { return _warnings.Count > 0; } }

        /// <summary>
        /// Get's the property count
        /// </summary>
        public int PropertyCount { get { return _properties.Count; } }

        /// <summary>
        /// List of warning that have been added to the token
        /// </summary>
        public IEnumerable<string> Warnings
        {
            get { return _warnings; }
        }

        /// <summary>
        /// Changes the TokenType to text
        /// </summary>
        internal void ChangeTokenTypeToText()
        {
            TokenType = TokenType.Text;
        }

        /// <summary>
        /// Adds a warning to the list of warnings
        /// </summary>
        /// <param name="warning">warnings to add</param>
        internal void AddWarning(string warning)
        {
            if (warning == null)
                throw new ArgumentNullException("warning");
            else if (String.IsNullOrWhiteSpace(warning))
                throw new ArgumentException("Can not have an empty warning", "warning");

            if(!_warnings.Contains(warning)) 
                _warnings.Add(warning);
        }

        /// <summary>
        /// Indexer to retrieve a specific property value
        /// </summary>
        /// <param name="propertyName">name of property to retrieve</param>
        /// <returns>name of property</returns>
        public string this[string propertyName]
        {
            get   { return _properties[FormatPropertyName(propertyName)]; }
            set
            {
                string formattedPropertyName = FormatPropertyName(propertyName);
                string valueToSet = value.Trim();

                if (!_properties.ContainsKey(formattedPropertyName))
                    _properties.Add(formattedPropertyName, valueToSet);
                else
                {
                    _properties[formattedPropertyName] = valueToSet;

                    //we should not really be setting this again
                    string warningMessage = String.Format("Property '{0}' assigned multiple values.", propertyName);

                    if (!_warnings.Contains(warningMessage))
                        AddWarning(warningMessage);
                }
            }
        }

        /// <summary>
        /// Formats property name to be common, forces all names to be trimmed and lower case
        /// </summary>
        /// <param name="propertyName">name of property</param>
        /// <returns>new name for property</returns>
        private string FormatPropertyName(string propertyName)
        {
            return propertyName.Trim().ToLower();
        }

        /// <summary>
        /// Checks to see if a property exists
        /// </summary>
        /// <param name="propertyName">name of property to check for</param>
        /// <returns>true if does, otherwise false</returns>
        public bool HasProperty(string propertyName)
        {
            return _properties.ContainsKey(FormatPropertyName(propertyName));
        }

        /// <summary>
        /// Dynamic method to get a property value
        /// </summary>
        /// <param name="binder">binder to get value</param>
        /// <param name="result">result of trying to retrieve result</param>
        /// <returns>bool</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string propertyName = FormatPropertyName(binder.Name);

            bool hasProperty = HasProperty(propertyName);

            if (hasProperty)
                result = this[propertyName];
            else
                result = null;

            return hasProperty;
        }

        /// <summary>
        /// Tries to set a member
        /// </summary>
        /// <param name="binder">binder to set value</param>
        /// <param name="value">value to set</param>
        /// <returns>bool</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string propertyName = FormatPropertyName(binder.Name);

            if (value is string)
            {
                this[propertyName] = value as string;
                return true;
            }
            else
                throw new ArgumentException("Value should be a string", "value");
        }
    }
}
