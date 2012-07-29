using System;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using System.Collections.Generic;
using Jumbleblocks.Web.Wane.ParseRules.Properties;
namespace Jumbleblocks.Web.Wane
{
    public interface IDelimeterSet : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Gets number of delimeters
        /// </summary>
        int DelimeterCount { get; }

        /// <summary>
        /// The delimeter to use to escape others
        /// </summary>
        string EscapeDelimeter { get; set; }

        /// <summary>
        /// The delimeter to use to start a property
        /// </summary>
        string PropertiesStartDelimeter { get; set; }

        /// <summary>
        /// The delimeter to use to end a property
        /// </summary>
        string PropertiesEndDelimeter { get; set; }

        /// <summary>
        /// The delimeter to seperate property values when in same set
        /// </summary>
        string PropertySeperatorDelimeter { get; set; }
        
        /// <summary>
        /// The delimeter to use to end a property
        /// </summary>
        string PropertyNameValueSeperatorDelimeter { get; set; }


        /// <summary>
        /// Gets/sets delimeters from underlying dictionary (case insensitive)
        /// </summary>
        /// <param name="delimeterName">name of delimeter to find [case insensitive]</param>
        /// <returns>the delimeter value</returns>
        string this[string delimeterName] { get; set; }

        /// <summary>
        /// Checks to see if delimeter set has been set up correctly and is valid
        /// </summary>
        /// <returns>true if is valid, otherwise false</returns>
        bool IsValid { get; }

        /// <summary>
        /// Checks to see if a delimeter already exists for the given name
        /// </summary>
        /// <param name="delimeterName">name of delimeter</param>
        /// <returns>true if has value, otherwise false</returns>
        bool HasDelimeterFor(string delimeterName);

        /// <summary>
        /// Checks to see if delimeter set has given delimeter value
        /// </summary>
        /// <param name="delimeterText">delimeter to check</param>
        /// <returns>true if has, otherwise false</returns>
        bool HasDelimeterText(string delimeterText);

        /// <summary>
        /// Adds a delimeter parse rule for use with this delimeter set
        /// </summary>
        /// <param name="delimeterParseRule">parse rule to use</param>
        void AddDelimeterParseRule(DelimeterParseRule delimeterParseRule);

        /// <summary>
        /// Gets all the delimeters values in the set
        /// </summary>
        /// <returns>array of string</returns>
        string[] GetAllDelimeters();

        /// <summary>
        /// Determines if the delimeter is a custom delimeter. i.e. not escape char, properties start, properties end, property name value seperator, property seperator
        /// </summary>
        /// <param name="delimeterText">text of delimeter</param>
        /// <returns>true if is custom delimeter, otherwise false</returns>
        bool IsCustomDelimeter(string delimeterText);

        /// <summary>
        /// Determines if delimeter represents PropertiesStart
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties start delimeter, otherwise false</returns>
        bool IsPropertiesStartDelimeter(string delimeterText);

        /// <summary>
        /// Determines if delimeter represents PropertiesEnd
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        bool IsPropertiesEndDelimeter(string delimeterText);

        /// <summary>
        /// Determines if delimeter represents PropertySeperator
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        bool IsPropertySeperatorDelimeter(string delimeterText);


        /// <summary>
        /// Determines if delimeter represents a seperator between a properties name and value
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents properties end delimeter, otherwise false</returns>
        bool IsPropertyNameValueSeperatorDelimeter(string delimeterText);

        /// <summary>
        /// Determines if delimeter represents Excape
        /// </summary>
        /// <param name="delimeterText">delimeter text</param>
        /// <returns>true if represents escape delimeter, otherwise false</returns>
        bool IsEscapeDelimeter(string delimeterText);
        
        /// <summary>
        /// Gets the matching ending delimeter for the given starting delimeter
        /// </summary>
        /// <param name="startingDelimeterText">starting delimeter text</param>
        /// <returns>ending delimeter text</returns>
        string GetEndingDelimterForStartingDelimeter(string startingDelimeterText);
        
        /// <summary>
        /// Gets the enumerator for underlying dictionary
        /// </summary>
        /// <returns>IEnumerable of KeyValuePair of strings</returns>
        IEnumerator<KeyValuePair<string, string>> GetEnumerator();

        /// <summary>
        /// Gets the parse rules which will apply to all delimeter parse rules
        /// </summary>
        IEnumerable<PropertyParseRule> GlobalPropertyParseRules { get; }

        /// <summary>
        /// Adds a parse rule for a property which will be applied to all delimeters
        /// </summary>
        /// <param name="propertyParseRule">property parse rule to add</param>
        void AddGlobalPropertyParseRule(PropertyParseRule propertyParseRule);
        
        /// <summary>
        /// Checks to see if the given delimeter is allowed to have the specified property
        /// </summary>
        /// <param name="delimeterText">text for the delimeter</param>
        /// <returns>true if delimeter allows property, otherwise false</returns>
        bool IsDelimeterAllowedProperty(string delimeterText, string propertyName);

        /// <summary>
        /// Parses a given token to HTML using the parse rules supplied to the delimeter set
        /// </summary>
        /// <param name="token">token to add</param>
        /// <returns>html fragment as a string</returns> 
        string ParseToHtml(Token token);      
    }
}
