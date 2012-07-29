using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using Jumbleblocks.Web.Wane.ParseRules.Properties;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Contains all the individual delimeters used to tokenise a string  
    /// </summary>
    public class DelimeterSet : IDelimeterSet
    {
        const int NumberOfDefaultDelimeters = 5;
        const string EscapeDelimeterKey = "Escape";
        const string PropertiesStartKey = "StartPropertySet";
        const string PropertiesEndKey = "EndPropertySet";
        const string PropertySeperatorKey = "PropertySeperator";
        const string PropertyNameValueSeperatorKey = "PropertyNameValueSeperator";

        Dictionary<string, string> _delimeters;
        List<DelimeterParseRule> _parseRules = new List<DelimeterParseRule>();

        public DelimeterSet()
        { 
            _delimeters = new Dictionary<string, string>();
            _delimeters.Add(EscapeDelimeterKey, DefaultDelimeterValues.Escape);
            _delimeters.Add(PropertiesStartKey, DefaultDelimeterValues.PropertiesStart);
            _delimeters.Add(PropertiesEndKey, DefaultDelimeterValues.PropertiesEnd);
            _delimeters.Add(PropertySeperatorKey, DefaultDelimeterValues.PropertySeperator);
            _delimeters.Add(PropertyNameValueSeperatorKey, DefaultDelimeterValues.PropertyNameValueSeperator);
        }

        /// <summary>
        /// Gets number of delimeters
        /// </summary>
        public int DelimeterCount { get { return _delimeters.Count; } }

        /// <summary>
        /// The delimeter to use to escape others
        /// </summary>
        public string EscapeDelimeter
        {
            get { return _delimeters[EscapeDelimeterKey]; }
            set { _delimeters[EscapeDelimeterKey] = value; }
        }

        /// <summary>
        /// The delimeter to use to start a property
        /// </summary>
        public string PropertiesStartDelimeter
        {
            get { return _delimeters[PropertiesStartKey]; }
            set { _delimeters[PropertiesStartKey] = value; }
        }

        /// <summary>
        /// The delimeter to use to end a property
        /// </summary>
        public string PropertiesEndDelimeter
        {
            get { return _delimeters[PropertiesEndKey]; }
            set { _delimeters[PropertiesEndKey] = value; }
        }

        /// <summary>
        /// The delimeter to seperate property values when in same set
        /// </summary>
        public string PropertySeperatorDelimeter
        {
            get { return _delimeters[PropertySeperatorKey]; }
            set { _delimeters[PropertySeperatorKey] = value; }
        }

        /// <summary>
        /// The delimeter to use to end a property
        /// </summary>
        public string PropertyNameValueSeperatorDelimeter
        {
            get { return _delimeters[PropertyNameValueSeperatorKey]; }
            set { _delimeters[PropertyNameValueSeperatorKey] = value; }
        }

        /// <summary>
        /// Gets/sets delimeters from underlying dictionary (case insensitive)
        /// </summary>
        /// <param name="delimeterName">name of delimeter to find [case insensitive]</param>
        /// <returns>the delimeter value</returns>
        public string this[string delimeterName]
        {
            get { return _delimeters[FormatDelimeterName(delimeterName)]; }
            set
            {
                delimeterName = FormatDelimeterName(delimeterName);

                if (!_delimeters.ContainsKey(delimeterName))
                    _delimeters.Add(delimeterName, value);
                else
                    _delimeters[FormatDelimeterName(delimeterName)] = value;
            }
        }

        /// <summary>
        /// Checks to see if delimeter set has been set up correctly and is valid
        /// </summary>
        /// <returns>true if is valid, otherwise false</returns>
        public bool IsValid
        {
            get
            {
                return !String.IsNullOrWhiteSpace(EscapeDelimeter) &&
                       !String.IsNullOrWhiteSpace(PropertiesStartDelimeter) &&
                       !String.IsNullOrWhiteSpace(PropertiesEndDelimeter) &&
                       !String.IsNullOrWhiteSpace(PropertySeperatorDelimeter) &&
                       !String.IsNullOrWhiteSpace(PropertyNameValueSeperatorDelimeter) &&
                       DelimeterCount > NumberOfDefaultDelimeters;
            }
        }        

        /// <summary>
        /// Checks to see if a delimeter already exists for the given name
        /// </summary>
        /// <param name="delimeterName">name of delimeter</param>
        /// <returns>true if has value, otherwise false</returns>
        public bool HasDelimeterFor(string delimeterName)
        {
            return _delimeters.ContainsKey(FormatDelimeterName(delimeterName));
        }

        /// <summary>
        /// formats delimeter name so it is case insensitive
        /// </summary>
        /// <param name="delimeterName">name of delimeter</param>
        /// <returns>string</returns>
        private string FormatDelimeterName(string delimeterName)
        {
            return delimeterName.Trim().ToLower();
        }

        /// <summary>
        /// Checks to see if delimeter set has given delimeter value
        /// </summary>
        /// <param name="delimeterText">delimeter to check</param>
        /// <returns>true if has, otherwise false</returns>
        public bool HasDelimeterText(string delimeterText)
        {
            return IsPropertiesStartDelimeter(delimeterText) ||
                   IsPropertyNameValueSeperatorDelimeter(delimeterText) ||
                   IsPropertySeperatorDelimeter(delimeterText) ||
                   IsPropertiesEndDelimeter(delimeterText) ||
                   IsEscapeDelimeter(delimeterText) ||
                   IsCustomDelimeter(delimeterText);
        }

        /// <summary>
        /// Adds a delimeter parse rule for use with this delimeter set
        /// </summary>
        /// <param name="delimeterParseRule">parse rule to use</param>
        public void AddDelimeterParseRule(DelimeterParseRule delimeterParseRule)
        { 
            if (delimeterParseRule == null)
                throw new ArgumentNullException("delimeterParseRule");

            string parseRuleName = FormatDelimeterName(delimeterParseRule.Name);

            if (HasDelimeterFor(parseRuleName))
                throw new InvalidOperationException(String.Format("DelimeterParseRule with name '{0}' has already been added", parseRuleName));

            if(HasDelimeterText(delimeterParseRule.Delimeter))
                throw new InvalidOperationException(String.Format("DelimeterParseRule with text '{0}' has already been added", delimeterParseRule.Delimeter));

            _delimeters.Add(parseRuleName, delimeterParseRule.Delimeter);
            _parseRules.Add(delimeterParseRule);
        }

        /// <summary>
        /// Gets all the delimeters values in the set
        /// </summary>
        /// <returns>array of string</returns>
        public string[] GetAllDelimeters()
        {
            return _delimeters.Select(d => d.Value).ToArray();
        }

        /// <summary>
        /// Determines if the delimeter is a custom delimeter. i.e. not escape char, properties start, properties end, property name value seperator, property seperator
        /// </summary>
        /// <param name="delimeterText">text of delimeter</param>
        /// <returns>true if is custom delimeter, otherwise false</returns>
        public bool IsCustomDelimeter(string delimeterText)
        {
            bool isCoreDelimeter = EscapeDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase) ||
                                     PropertiesStartDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase) ||
                                     PropertiesEndDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase) ||
                                     PropertySeperatorDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase) ||
                                     PropertyNameValueSeperatorDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);

            if (isCoreDelimeter)
                return false;

            foreach (KeyValuePair<string, string> delimeter in _delimeters)
            {
                if (delimeter.Value.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if delimeter represents PropertiesStart
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties start delimeter, otherwise false</returns>
        public bool IsPropertiesStartDelimeter(string delimeterText)
        {
            return PropertiesStartDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Determines if delimeter represents PropertiesEnd
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        public bool IsPropertiesEndDelimeter(string delimeterText)
        {
            return PropertiesEndDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Determines if delimeter represents PropertySeperator
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        public bool IsPropertySeperatorDelimeter(string delimeterText)
        {
            return PropertySeperatorDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Determines if delimeter represents a seperator between a properties name and value
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        public bool IsPropertyNameValueSeperatorDelimeter(string delimeterText)
        {
            return PropertyNameValueSeperatorDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Determines if delimeter represents Excape
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents escape delimeter, otherwise false</returns>
        public bool IsEscapeDelimeter(string delimeterText)
        {
            return EscapeDelimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the matching ending delimeter for the given starting delimeter
        /// </summary>
        /// <param name="startingDelimeterText">starting delimeter text</param>
        /// <returns>ending delimeter text</returns>
        public string GetEndingDelimterForStartingDelimeter(string startingDelimeterText)
        {
            if (startingDelimeterText.Equals(PropertiesStartDelimeter, StringComparison.CurrentCultureIgnoreCase))
                return PropertiesEndDelimeter;
            else
                return startingDelimeterText;
        }

        /// <summary>
        /// Gets the enumerator for underlying dictionary
        /// </summary>
        /// <returns>IEnumerable of KeyValuePair of strings</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _delimeters.GetEnumerator();
        }

        /// <summary>
        /// Gets enumerator of underlying dictionary
        /// </summary>
        /// <returns>IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _delimeters.GetEnumerator();
        }


        private List<PropertyParseRule> _globalPropertyParseRules = new List<PropertyParseRule>();

        /// <summary>
        /// Gets the parse rules which will apply to all delimeter parse rules
        /// </summary>
        public IEnumerable<PropertyParseRule> GlobalPropertyParseRules
        {
            get
            {
                return _globalPropertyParseRules;
            }
        }

        /// <summary>
        /// Adds a parse rule for a property which will be applied to all delimeters
        /// </summary>
        /// <param name="propertyParseRule">property parse rule to add</param>
        public void AddGlobalPropertyParseRule(PropertyParseRule propertyParseRule)
        {
            if (propertyParseRule == null)
                throw new ArgumentNullException("properyParseRule");

            if (_globalPropertyParseRules.Count(ppr => ppr.PropertyName.Equals(propertyParseRule.PropertyName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                throw new InvalidOperationException(String.Format("PropertyParseRule for property name '{0}' has already been added to the global list", propertyParseRule.PropertyName));

            _globalPropertyParseRules.Add(propertyParseRule);
        }

        /// <summary>
        /// Checks to see if the given delimeter is allowed to have the specified property
        /// </summary>
        /// <param name="delimeterText">text for the delimeter</param>
        /// <returns>true if delimeter allows property, otherwise false</returns>
        public bool IsDelimeterAllowedProperty(string delimeterText, string propertyName)
        {
            DelimeterParseRule parseRule = _parseRules.SingleOrDefault(pr => pr.Delimeter.Equals(delimeterText, StringComparison.CurrentCultureIgnoreCase));

            if (parseRule == null)
                return false;

            return parseRule.LocalPropertyParseRules.Count(lpr => lpr.PropertyName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase)) > 0 ||
                   GlobalPropertyParseRules.Count(gpr => gpr.PropertyName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }

        /// <summary>
        /// Parses a given token to HTML using the parse rules supplied to the delimeter set
        /// </summary>
        /// <param name="token">token to add</param>
        /// <returns>html fragment as a string</returns>       
        public string ParseToHtml(Token token)
        {
            DelimeterParseRule parseRule = _parseRules.SingleOrDefault(pr => pr.Delimeter.Equals(token.Text, StringComparison.CurrentCultureIgnoreCase));

            if (parseRule == null)
                return String.Empty;
            else
                return parseRule.TransformToHtml(token, _globalPropertyParseRules);
        }   
    }
}
