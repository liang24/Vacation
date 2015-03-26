using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.WebLib;
using Common.Models;

namespace Vacation.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());



        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }


        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

            Exception exception = null;

            if (Server.GetLastError().InnerException != null)
            {
                exception = Server.GetLastError().InnerException;
            }
            else
            {
                exception = Server.GetLastError();
            }

            LogHelper.Log(exception);

            // 如果是Ajax调用，返回错误信息
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Server.ClearError();

                Response.Clear();
                Response.Write(JsonHelper.Serialize(new ResponseResult { Message = "操作异常，请联系管理员...", IsSuccess = false }));
                Response.End();
            }
        }
    }
}