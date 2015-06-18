using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Web.AppCode;

namespace Vacation.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class RemoveCacheFilterAttribute : ActionFilterAttribute
    {
        public string CacheKey { get; set; }

        public RemoveCacheFilterAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            BasicDataCache.RemoveCache(CacheKey);
        }
    }
}