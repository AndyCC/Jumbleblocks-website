using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    public class CodeParseRule : DelimeterParseRule
    {
        public CodeParseRule()
            : base("Code", DefaultDelimeterValues.DelimeterStart + "c", "code")
        {
        }
    }
}
