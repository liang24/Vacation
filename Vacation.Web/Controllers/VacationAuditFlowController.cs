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
using Vacation.Web.Models;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationAuditFlowController : BaseController
    {

        #region 管理模块

        public ActionResult Index()
        {
            ViewBag.Data = BasicDataCache.listVacationAuditFlowTypes.Select(type => new VacationAuditFlowTypeModel
            {
                ID = type.ID,
                Name = type.Name,
                UsedRoles = type.UsedRoleIDs.ToIntList().Select(role => role.ToRole().Name).Join(),
                Flows = BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == type.ID).OrderBy(flow => flow.Sort).Select(flow => new VacationAuditFlowModel
                {
                    AuditRole = flow.AuditRoleIDs.ToIntList().Select(role => role.ToRole().Name).Join(),
                    DeptLevel = flow.DeptLevel.EnumToName(),
                    ID = flow.ID,
                    Name = flow.Name,
                    Sort = flow.Sort
                })
            });

            return View();
        }

        #region 审核流程

        [HttpGet]
        public ActionResult AddFlow(int typeID)
        {
            ViewBag.DeptLevels = EnumExtension.GetComboBox<EnumDeptLevel>();
            ViewBag.Roles = BasicDataCache.listRoles;
            ViewBag.MaxSort = BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == typeID).Count() > 0 ? BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == typeID).Max(flow => flow.Sort) + 1 : 1;

            VacationAuditFlow model = new VacationAuditFlow();
            model.TypeID = typeID;

            return View(model);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        [TransactionFilter]
        public JsonResult AddFlow(VacationAuditFlow vacationAuditFlow)
        {
            vacationAuditFlow.AuditRoleIDs = Request.Form["AuditRoleIDs"];
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

        public ActionResult UpdateFlow(int id)
        {
            var vacationAuditFlow = VacationAuditFlow.SingleOrDefault(id);

            ViewBag.DeptLevels = EnumExtension.GetComboBox<EnumDeptLevel>();
            ViewBag.Roles = BasicDataCache.listRoles;

            return View(vacationAuditFlow);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        [TransactionFilter]
        public JsonResult UpdateFlow(VacationAuditFlow vacationAuditFlow)
        {
            vacationAuditFlow.AuditRoleIDs = Request.Form["AuditRoleIDs"];
            vacationAuditFlow.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [RemoveCacheFilter(BasicDataCache.VacationAuditFlow_CacheKey)]
        public JsonResult DeleteFlow(string id)
        {
            VacationAuditFlow.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }
        #endregion

        #region 审核流程类型

        [HttpGet]
        public ActionResult AddType()
        {
            ViewBag.Roles = BasicDataCache.listRoles;

            return View();
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlowType_CacheKey)]
        public JsonResult AddType(VacationAuditFlowType vacationAuditFlowType)
        {
            vacationAuditFlowType.UsedRoleIDs = Request.Form["UsedRoleIDs"];
            vacationAuditFlowType.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpGet]
        public ActionResult UpdateType(int id)
        {
            var vacationAuditFlowType = VacationAuditFlowType.SingleOrDefault(id);

            ViewBag.Roles = BasicDataCache.listRoles;

            return View(vacationAuditFlowType);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlowType_CacheKey)]
        public JsonResult UpdateType(VacationAuditFlowType vacationAuditFlowType)
        {
            vacationAuditFlowType.UsedRoleIDs = Request.Form["UsedRoleIDs"];
            vacationAuditFlowType.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.VacationAuditFlowType_CacheKey)]
        public JsonResult DeleteType(string id)
        {
            VacationAuditFlowType.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

        #endregion

    }
}
