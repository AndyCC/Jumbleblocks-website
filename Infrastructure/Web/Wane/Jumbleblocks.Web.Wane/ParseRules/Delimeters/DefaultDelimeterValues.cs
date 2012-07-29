
namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    /// <summary>
    /// Delimiters which make up mark up
    /// </summary>
    public static class DefaultDelimeterValues
    {
        /// <summary>
        /// string which starts a delimeter
        /// </summary>
        public static string DelimeterStart { get { return "#"; } }

        /// <summary>
        /// Determines start of a section to define properties will begin
        /// </summary>
        public static string PropertiesStart { get { return "["; } }

        /// <summary>
        /// Determines end of section define properties will end
        /// </summary>
        public static string PropertiesEnd { get { return "]"; } }

        /// <summary>
        /// Seperates property values
        /// </summary>
        public static string PropertySeperator { get { return "|"; } }

        /// <summary>
        /// Value which seperates property name and value
        /// </summary>
        public static string PropertyNameValueSeperator { get { return ":"; } }

        /// <summary>
        /// Escape delimeter
        /// </summary>
        public static string Escape { get { return @"\"; } }

        /// <summary>
        /// bold delimeter
        /// </summary>
        public static string Bold { get { return DelimeterStart + "b"; } }

        /// <summary>
        /// italics delimeter
        /// </summary>
        public static string Italics { get { return DelimeterStart + "i"; } }

    }
}
