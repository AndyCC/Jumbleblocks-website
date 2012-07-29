using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    public class ItalicsParseRule : DelimeterParseRule
    {
        public ItalicsParseRule()
            : base("Italics", DefaultDelimeterValues.Italics, "i")
        {
        }
    }
}
