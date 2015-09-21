using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using PetaPoco;
using Common.Models;
using Vacation.Web.Filters;
using Vacation.Web.AppCode;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationTypeController : BaseController
    {

        #region 管理模块

        [PowerFilter("vacation_type_view")]
        public ActionResult Index()
        {
            return View();
        }
        [PowerFilter("vacation_type_view")]
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
        [PowerFilter("vacation_type_add")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationType_CacheKey)]
        [PowerFilter("vacation_type_add")]
        public JsonResult Add(VacationType vacationType)
        {
            vacationType.Type = EnumVacationTypeType.General;
            vacationType.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }
        [PowerFilter("vacation_type_update")]
        public ActionResult Update(int id)
        {
            var vacationType = VacationType.SingleOrDefault(id);

            return View(vacationType);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationType_CacheKey)]
        [PowerFilter("vacation_type_update")]
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

        [RemoveCacheFilter(BasicDataCache.VacationType_CacheKey)]
        [PowerFilter("vacation_type_delete")]
        public JsonResult Delete(string id)
        {
            VacationType.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
