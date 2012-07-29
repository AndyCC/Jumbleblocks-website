using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace Jumbleblocks.Website.Models.Admin
{
    /// <summary>
    /// Link to an admin section
    /// </summary>
    /// <remarks>Dynamic object</remarks>
    public class AdminSectionLink 
    {
        public AdminSectionLink()
        {
            Parameters = new ExpandoObject();
        }

        /// <summary>
        /// Text to display to user
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Name of controller with action
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Name of action 
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Parameters to call link with
        /// </summary>
        public dynamic Parameters { get; private set; }

    }
}