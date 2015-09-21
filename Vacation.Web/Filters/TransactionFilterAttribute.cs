using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace Vacation.Web.Filters
{ 
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}