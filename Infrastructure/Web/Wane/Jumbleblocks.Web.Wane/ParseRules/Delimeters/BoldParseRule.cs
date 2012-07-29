using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    /// <summary>
    /// Parse rule for bold
    /// </summary> 
    public class BoldParseRule : DelimeterParseRule
    {
        public BoldParseRule()
            : base("Bold", DefaultDelimeterValues.Bold, "b")
        {
        }
    }
}
