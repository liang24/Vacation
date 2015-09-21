using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using Vacation.Web.AppCode;

namespace Vacation.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class PowerFilterAttribute : ActionFilterAttribute
    {
        public string[] FunctionTags { get; set; }

        public PowerFilterAttribute(params string[] functionTags)
        {
            FunctionTags = functionTags;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (FunctionTags.HasElements())
            {
                foreach (var item in FunctionTags)
                {
                    if (SysHelper.HasPower(item))
                    {
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                }
            }

            filterContext.HttpContext.Response.Redirect("~/Home/NoPower");
        }
    }
}