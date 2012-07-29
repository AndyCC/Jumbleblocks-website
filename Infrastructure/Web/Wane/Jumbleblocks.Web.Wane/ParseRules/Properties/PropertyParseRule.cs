using System;

namespace Jumbleblocks.Web.Wane.ParseRules.Properties
{
    /// <summary>
    /// base class which determines how to parse a property
    /// </summary>
    public class PropertyParseRule
    {
        public PropertyParseRule(string propertyName, string propertyNameReplacement = "")
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            else if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Must have a value", "propertyName");

            PropertyName = propertyName;

            if (String.IsNullOrWhiteSpace(propertyNameReplacement))
                PropertyNameReplacement = PropertyName;
            else
                PropertyNameReplacement = propertyNameReplacement;
        }

        /// <summary>
        /// Name of property which this rule parses
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// determines what the property name is replaced with
        /// </summary>
        public string PropertyNameReplacement { get; private set; }

        /// <summary>
        /// Constructs HTML for the given parse rule by using values in the supplied token
        /// </summary>
        /// <param name="token">token to get values from</param>
        /// <returns>part html</returns>
        public virtual string ConstructHtml(Token token)
        {
            if (token.HasProperty(PropertyName))
                return String.Format("{0}='{1}'", PropertyNameReplacement, token[PropertyName]);
                        
            return String.Empty;
        }
    }
}
