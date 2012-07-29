using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// enum keeps track of which part of property is currently being parsed
    /// </summary>
    enum PropertyParseElements
    {
        PropertyName = 1,
        PropertyNameValueSeperator = 2,
        PropertyValue = 3,
        PropertySeperator = 4
    }
}
