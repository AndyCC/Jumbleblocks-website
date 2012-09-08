using Jumbleblocks.Web.Wane.ParseRules.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    public class ImageParseRule : DelimeterParseRule
    {
        public ImageParseRule()
            : base("Image", DefaultDelimeterValues.DelimeterStart + "img", "img")
        {
            AddPropertyParseRule(new SrcParseRule());
            AddPropertyParseRule(new AltParseRule());
        }
    }
}
