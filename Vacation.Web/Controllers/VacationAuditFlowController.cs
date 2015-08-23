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
using Common.EnumLib;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationAuditFlowController : BaseController
    {

        #region 管理模块

        public ActionResult Index()
        {
            ViewBag.Data = BasicDataCache.listVacationAuditFlows;

            return View();
        }

        public JsonResult GetList()
        {
            var sql = Sql.Builder;

            var list = BasicDataCache.listVacationAuditFlows;

            return Json(new ResponseData
            {
                Total = list.Count(),
                Data = list
            });
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.DeptLevels = EnumExtension.GetComboBox<EnumDeptLevel>();
            ViewBag.Roles = BasicDataCache.listRoles;
            ViewBag.MaxSort = BasicDataCache.listVacationAuditFlows.Max(flow => flow.Sort) + 1;

            return View();
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        [TransactionFilter]
        public JsonResult Add(VacationAuditFlow vacationAuditFlow)
        {
            vacationAuditFlow.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        ///// <summary>
        ///// 重置排序
        ///// </summary>
        //private void ResetSort()
        //{
        //    List<VacationAuditFlow> all = VacationAuditFlow.Fetch(Sql.Builder);

        //    VacationAuditFlow first = null;
        //    foreach (var item in all)
        //    {
        //        if (item.NextFlowID.HasValue)
        //        {
        //            if (all.Where(flow => flow.NextFlowID.HasValue).Select(flow => flow.NextFlowID).Contains(item.ID) == false)
        //            {
        //                first = item;
        //                break;
        //            }
        //        }
        //    }

        //    if (first != null)
        //    {
        //        int sort = 1;
        //        first.Sort = sort++;
        //        first.Update();

        //        VacationAuditFlow next = null, prev = first;

        //        while ((next = all.SingleOrDefault(flow => prev.NextFlowID.HasValue && flow.ID == prev.NextFlowID.Value)) != null)
        //        {
        //            next.Sort = sort++;
        //            next.Update();

        //            prev = next;
        //        }
        //    }
        //}

        public ActionResult Update(int id)
        {
            var vacationAuditFlow = VacationAuditFlow.SingleOrDefault(id);

            ViewBag.DeptLevels = EnumExtension.GetComboBox<EnumDeptLevel>();
            ViewBag.Roles = BasicDataCache.listRoles;

            return View(vacationAuditFlow);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        [TransactionFilter]
        public JsonResult Update(VacationAuditFlow vacationAuditFlow)
        {
            vacationAuditFlow.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        public JsonResult Delete(string id)
        {
            VacationAuditFlow.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
