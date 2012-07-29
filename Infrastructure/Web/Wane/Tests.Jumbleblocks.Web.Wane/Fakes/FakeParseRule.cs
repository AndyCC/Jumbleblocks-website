using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;

namespace Tests.Jumbleblocks.Web.Wane.Fakes
{
    public class FakeParseRule : DelimeterParseRule
    {
        public FakeParseRule(string name, string delimeter, string htmlElement)
            : base(name, delimeter, htmlElement)
        {
        }
    }
}
