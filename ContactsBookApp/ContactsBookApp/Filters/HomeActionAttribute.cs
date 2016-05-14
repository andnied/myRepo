using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactsBookApp.Filters
{
    public class HomeActionAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Trace.WriteLine("Action Executed");
        }
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Trace.WriteLine("Action executing");
        }
    }
}