using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using PetaPoco;
using Common.Models;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationTypeController : BaseController
    {

        #region 管理模块

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("name like @0", "%" + name + "%");

            var page = VacationType.Page(pageIndex, pageSize, sql);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = page.Items
            });
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(VacationType vacationType)
        {
            vacationType.Type = EnumVacationTypeType.General;
            vacationType.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public ActionResult Update(int id)
        {
            var vacationType = VacationType.SingleOrDefault(id);

            return View(vacationType);
        }

        [HttpPost]
        public JsonResult Update(VacationType vacationType)
        {
            var old = VacationType.SingleOrDefault(vacationType.ID);
            old.Name = vacationType.Name;
            old.Sort = vacationType.Sort;
            old.Description = vacationType.Description;
            old.IsEnabled = vacationType.IsEnabled;

            if (CurrUser.IsSuperUser)
            {
                old.IsSystem = vacationType.IsSystem;
            }

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public JsonResult Delete(string id)
        {
            VacationType.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
