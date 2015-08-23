using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using Common.Models;
using Vacation.Web.AppCode;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationApplyController : BaseController
    {
        [HttpGet]
        public ActionResult Apply()
        {
            ViewBag.VacationTypes = BasicDataCache.listVacationTypes;

            return View();
        }

        [HttpPost]
        public ActionResult Apply(VacationApply apply)
        {
            apply.UserID = CurrUser.ID;
            apply.ApplyTime = DateTime.Now;
            apply.Status = EnumVacationApplyStatus.Apply;

            var firstFlow = BasicDataCache.listVacationAuditFlows.SingleOrDefault(flow => flow.Sort == 1);
            if (firstFlow != null)
            {
                apply.FlowID = firstFlow.ID;
            }
            apply.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }
    }
}
