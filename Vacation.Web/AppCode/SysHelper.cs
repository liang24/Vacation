/******************************************************************************
 *  作者：       [尹学良]
 *  创建时间：   2014/1/15 15:16:44
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WebLib;
using System.Web;
using System.Web.Security;
using NLog;
using System.IO;
using Vacation.Domain;
using Vacation.Domain.Entities;
using Vacation.Application;


namespace Vacation.Web.AppCode
{
    public class SysHelper
    {
        public static readonly string _UserSessionKey = "user";

        /// <summary>
        /// 获取当前用户
        /// </summary> 
        /// <param name="cookie">身份认证的cookie值，可空</param>
        /// <returns></returns> 
        /// 创建人：尹学良
        /// 2014/1/15 15:29
        public static SysUser GetCurrUser(string cookie = null)
        {
            SysUser currUser = null;

            if (!string.IsNullOrEmpty(cookie))
            {
                //cookie值不为空
                var ticket = TicketHelper.GetSysTicket(cookie);
                if (ticket != null)
                {
                    var user = GetUserByTicket(ticket);
                    if (user != null)
                    {
                        currUser = user;
                    }
                }
            }
            else
            {
                // cookie为空 
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var ticket = TicketHelper.GetSysTicket(null);
                    if (ticket != null)
                    {
                        currUser = HttpContext.Current.Session[_UserSessionKey] as SysUser;
                        if (currUser == null)
                        {
                            //session中为Null，由FormsCookie中获取 
                            var user = GetUserByTicket(ticket);
                            if (user != null)
                            {
                                currUser = user;

                                HttpContext.Current.Session[_UserSessionKey] = currUser;
                            }
                        }
                        else
                        {
                            // 判断Cookie中的与Session中的是否同一用户 
                            if (currUser.ID.ToString() != ticket.Name)
                            {
                                ClearSession();

                                return GetCurrUser(null);
                            }
                        }
                    }
                }
            }
            return currUser;
        }

        /// <summary>
        /// 根据票证获取用户
        /// </summary>
        /// <param name="ticket">票证</param>
        /// <returns></returns>
        /// 创建人：尹学良
        /// 2014/1/15 17:46
        private static SysUser GetUserByTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                return null;
            }
            int userId = ticket.Name.ToInt();

            return SysUser.SingleOrDefault(userId);
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns>是否已登录</returns>
        /// 创建人：尹学良
        /// 2014/1/15 17:31
        public static bool IsLogined(string cookie = null)
        {
            return GetCurrUser(cookie) != null;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="rememberMe">是否记住我</param>
        /// <param name="message">错误返回信息</param>
        /// <returns></returns>
        /// 创建人：尹学良
        /// 2014/1/15 17:45 
        public static bool Login(string userName, string password, bool rememberMe, out string message)
        {
            var mr = UserService.Login(userName, password);

            if (mr.IsComplete)
            {
                message = "";

                var user = mr.Entity as SysUser;

                System.Web.Security.FormsAuthentication.SetAuthCookie(user.ID.ToString(), rememberMe);
                HttpContext.Current.Session[_UserSessionKey] = user;

                return true;
            }
            else
            {
                message = mr.Message;

                return false;
            }
        }

        /// <summary>
        /// 退出
        /// </summary> 
        /// 创建人：尹学良
        /// 2014/1/15 17:37
        public static void Exit(bool isAutoRedirect = true)
        {
            ClearSession();
            FormsAuthentication.SignOut();

            if (isAutoRedirect)
            {
                HttpContext.Current.Response.Redirect("~/User/Login");
            }
        }

        /// <summary>
        /// 清空用户Session
        /// </summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session[_UserSessionKey] = null;
        }

        /// <summary>
        /// 获取返回路径
        /// </summary>
        /// <returns></returns>
        /// 创建人：尹学良
        /// 2014/1/15 17:46
        public static string GetReturnUrl()
        {
            var cookie = HttpContext.Current.Request.Cookies["ReturnUrl"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                return cookie.Value;
            }
            return null;
        }

        /// <summary>
        /// 设置返回路径
        /// </summary>
        /// <param name="url">The URL.</param>
        /// 创建人：尹学良
        /// 2014/1/15 17:47
        public static void SetReturnUrl(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = HttpContext.Current.Request.Url.PathAndQuery;
            }

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ReturnUrl", url));
        }

        /// <summary>
        /// 是否拥有权限
        /// </summary> 
        /// <param name="functionTags">访问标签</param>
        /// <returns>是否拥有权限</returns>
        /// 创建人：尹学良
        /// 2014/1/15 19:26
        public static bool HasPower(params string[] functionTags)
        {
            bool result = false;

            var currUser = GetCurrUser();
            if (currUser != null)
            {
                result = currUser.HasPower(functionTags);
            }

            return result;
        }

        /// <summary>
        /// 404错误
        /// </summary>
        /// 创建人：尹学良
        /// 2014/1/15 19:29
        public static void PageNotFound404()
        {
            HttpContext.Current.Response.Redirect("~/404");
        }
    }
}


