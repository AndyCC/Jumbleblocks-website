using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    /// <summary>
    /// base class which defines how to parse a custom delimeter
    /// </summary>
    public class DelimeterParseRule 
    {
        public DelimeterParseRule(string name, string delimeter, string htmlElement)
        {
            if(name == null)
                throw new ArgumentNullException("name");
            else if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Must have a value", "name");

            if(delimeter == null)
                throw new ArgumentNullException("delimeter");
            else if (String.IsNullOrWhiteSpace(delimeter))
                throw new ArgumentException("Must have a value", "delimeter");

            if (htmlElement == null)
                throw new ArgumentNullException("htmlElement");
            else if (String.IsNullOrWhiteSpace(htmlElement))
                throw new ArgumentException("Must have a value", "htmlElement");

            Name = name;
            Delimeter = delimeter;
            HTMLElement = htmlElement;
        }

        /// <summary>
        /// Name that identifies the delimeter parse rule
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The delimeter to find
        /// </summary>
        public string Delimeter { get; private set; }

        /// <summary>
        /// GEts/sets the HTML element e.g. "B" for bold
        /// </summary>
        public string HTMLElement { get; private set; }

        /// <summary>
        /// HTML open element string '<'
        /// </summary>
        protected string StartOpenElement { get { return "<"; } }

        /// <summary>
        /// Closes an element '</'
        /// </summary>
        protected string StartCloseElement { get { return "</"; } }

        /// <summary>
        /// Ends an element '>'
        /// </summary>
        protected string EndElement { get { return ">"; } }

        private List<PropertyParseRule> _localPropertyParseRules = new List<PropertyParseRule>();

        /// <summary>
        /// Gets the parse rules which will apply to all delimeter parse rules
        /// </summary>
        public IEnumerable<PropertyParseRule> LocalPropertyParseRules
        {
            get
            {
                return _localPropertyParseRules;
            }
        }

        /// <summary>
        /// Adds a parse rule for a property which will be applied to only this delimeter
        /// </summary>
        /// <param name="propertyParseRule">property parse rule to add</param>
        public void AddPropertyParseRule(PropertyParseRule propertyParseRule)
        {
            if (propertyParseRule == null)
                throw new ArgumentNullException("properyParseRule");

            if (_localPropertyParseRules.Count(ppr => ppr.PropertyName.Equals(propertyParseRule.PropertyName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                throw new InvalidOperationException(String.Format("PropertyParseRule for property name '{0}' has already been added to the global list", propertyParseRule.PropertyName));

            _localPropertyParseRules.Add(propertyParseRule);
        }

        /// <summary>
        /// Transforms a token into  HTML
        /// </summary>
        /// <param name="token">Token to transform</param>
        /// <param name="globalPropertyParseRules">set of global parse rules</param>
        /// <returns>HTML as a string</returns>
        public string TransformToHtml(Token token, IEnumerable<PropertyParseRule> globalPropertyParseRules)
        {
            if (token == null)
                throw new ArgumentNullException("token");

            if (globalPropertyParseRules == null)
                throw new ArgumentNullException("globalPropertyParseRules");

            if (!token.IsDelimeter)
                throw new InvalidOperationException(String.Format("Can not parse token type '{0}'", token.TokenType.ToString()));

            var htmlBuilder = new StringBuilder();

            if (token.IsEndingDelimeter)
                htmlBuilder.Append(StartCloseElement);
            else
                htmlBuilder.Append(StartOpenElement);

            htmlBuilder.Append(HTMLElement);

            string attributes = ParsePropertiesInAttributes(token, globalPropertyParseRules);
            htmlBuilder.Append(attributes);

            htmlBuilder.Append(EndElement);

            return htmlBuilder.ToString();
        }

        /// <summary>
        /// Parses Token properties into HTML attributes
        /// </summary>
        /// <param name="token">token to parse</param>
        /// <param name="globalPropertyParseRules">global property parse rules</param>
        /// <returns>HTML for attributes to add to element</returns>
        private string ParsePropertiesInAttributes(Token token, IEnumerable<PropertyParseRule> globalPropertyParseRules)
        {
            var propertiesHtmlBuilder = new StringBuilder();

            List<PropertyParseRule> propertyParseRules = new List<PropertyParseRule>(_localPropertyParseRules);

            foreach (var ppr in globalPropertyParseRules)
            {
                if (propertyParseRules.Count(ppr2 => ppr2.PropertyName == ppr.PropertyName) == 0)
                    propertyParseRules.Add(ppr);
            }

            foreach (PropertyParseRule ppr in propertyParseRules)
            {
                if (token.HasProperty(ppr.PropertyName))
                    propertiesHtmlBuilder.AppendFormat(" {0}", ppr.ConstructHtml(token));
            }

            return propertiesHtmlBuilder.ToString();
        }
    }
}
