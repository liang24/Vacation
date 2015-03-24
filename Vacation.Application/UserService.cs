using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Models;
using Vacation.Domain.Entities;

namespace Vacation.Application
{
    public class UserService
    { 
        public static VMessage Login(string userName, string password)
        {
            var result = new VMessage();

            var user = SysUser.SingleOrDefault("where user_name=@0 and password=@1", userName, password);

            if (user == null)
            {
                result.AddItem("用户名或密码错误！");
            }
            else
            {
                result.Entity = user;
            }

            return result;
        }
    }
}
