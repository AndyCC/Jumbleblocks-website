using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Core.Logging;
using Jumbleblocks.Web.Core;

namespace Jumbleblocks.Website.ActionFilters
{
    /// <summary>
    /// Handles errors and logs the using the supplied logger
    /// </summary>
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        public HandleAndLogErrorAttribute()
        {
            Logger = IocContext.Resolve<IJumbleblocksLogger>(); 
        }

        protected IJumbleblocksLogger Logger { get; private set; }


        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (Logger != null)
                Logger.LogError("Error caught by global handler", filterContext.Exception);
        }
    }
}