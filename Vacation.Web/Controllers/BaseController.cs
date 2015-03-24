using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using Vacation.Web.AppCode;

namespace Vacation.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            CurrUser = SysHelper.GetCurrUser();
        }

        protected SysUser CurrUser { get; private set; }

    }
}
