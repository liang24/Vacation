using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using Common.Models;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationApplyController : BaseController
    {
        [HttpGet]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(VacationApply apply)
        {
            apply.UserID = CurrUser.ID;
            apply.ApplyTime = DateTime.Now;
            apply.Status = EnumVacationApplyStatus.Apply;

            apply.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }
    }
}
