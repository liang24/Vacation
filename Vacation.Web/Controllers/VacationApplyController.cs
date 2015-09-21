using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using Common.Models;
using Vacation.Web.AppCode;
using PetaPoco;
using Vacation.Web.Models;
using Common.EnumLib;
using Common.WebLib;
using Vacation.Web.Filters;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class VacationApplyController : BaseController
    {
        #region 个人模块

        [HttpGet]
        [PowerFilter("my_vacation_apply_apply")]
        public ActionResult Apply()
        {
            ViewBag.VacationTypes = BasicDataCache.listVacationTypes;

            return View();
        }

        [HttpPost]
        [PowerFilter("my_vacation_apply_apply")]
        public ActionResult Apply(VacationApply apply)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            apply.UserID = CurrUser.ID;
            apply.ApplyTime = DateTime.Now;
            apply.Status = EnumVacationApplyStatus.Apply;

            var msg = GetNextFlow(CurrUser, null);

            if (msg.IsComplete)
            {
                apply.FlowID = ((VacationAuditFlow)msg.Entity).ID;
                apply.Insert();

                return Json(ArtDialogResponseResult.SuccessResult);
            }
            else
            {
                result.Message = msg.Message;
                return Json(result);
            }
        }

        [PowerFilter("vacation_my_record_delete")]
        public ActionResult MyDelete(string id)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            var ids = id.ToIntList();

            if (ids.HasElements())
            {
                foreach (var item in VacationApply.Fetch("where id in (@0)", ids))
                {
                    if (CanModify(item.Status) && item.UserID == CurrUser.ID)
                    {
                        item.Delete();
                    }
                }

                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "请选择至少一条记录操作！";
            }

            return Json(result);
        }

        [PowerFilter("vacation_my_record_view")]
        public ActionResult MyIndex()
        {
            return View();
        }

        [PowerFilter("vacation_my_record_view")]
        public JsonResult MyPage(int pageIndex, int pageSize, int? typeID)
        {
            var sql = Sql.Builder.Where("user_id = @0", CurrUser.ID);

            if (typeID.HasValue)
            {
                sql.Where("vacation_type_id=@0", typeID.Value);
            }

            var page = VacationApply.Page(pageIndex, pageSize, sql);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = page.Items.Select(flow => new VacationApplyModel
                {
                    ApplyTime = flow.ApplyTime.ToDateTimeString(),
                    EndTime = flow.EndTime.ToDateTimeString(),
                    ID = flow.ID,
                    Opinion = flow.Opinion,
                    Reason = flow.Reason,
                    StartTime = flow.StartTime.ToDateTimeString(),
                    Status = flow.Status.EnumToName(),
                    VacationTypeName = flow.VacationTypeID.ToVacationType().Name,
                    CanModify = CanModify(flow.Status)
                })
            });
        }

        private bool CanModify(EnumVacationApplyStatus status)
        {
            EnumVacationApplyStatus[] statuses = new EnumVacationApplyStatus[] { EnumVacationApplyStatus.Apply, EnumVacationApplyStatus.Nopass };

            return statuses.Contains(status);
        }

        [HttpGet]
        [PowerFilter("vacation_my_record_update")]
        public ActionResult Modify(int id)
        {
            var apply = VacationApply.Single(id);

            if (CanModify(apply.Status))
            {
                return View(apply);
            }
            else
            {
                return Redirect("~/404");
            }

        }

        [HttpPost]
        [PowerFilter("vacation_my_record_update")]
        public JsonResult Modify(VacationApply apply)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            var old = VacationApply.Single(apply.ID);

            if (!CanModify(old.Status))
            {
                result.Message = "此申请不允许修改，请刷新再操作！";
                return Json(result);
            }

            old.Status = EnumVacationApplyStatus.Apply;
            old.StartTime = apply.StartTime;
            old.EndTime = apply.EndTime;
            old.Reason = apply.Reason;
            old.VacationTypeID = apply.VacationTypeID;

            var msg = GetNextFlow(old.UserID.ToUser(), null);

            if (msg.IsComplete)
            {
                old.FlowID = ((VacationAuditFlow)msg.Entity).ID;
                old.Update();

                return Json(ArtDialogResponseResult.SuccessResult);
            }
            else
            {
                result.Message = msg.Message;
                return Json(result);
            }
        }

        #endregion

        #region 管理模块

        [PowerFilter("vacation_record_view")]
        public ActionResult Index()
        {
            return View();
        }

        [PowerFilter("vacation_record_view")]
        public JsonResult Page(int pageIndex, int pageSize, int? typeID, int? deptID)
        {
            var sql = Sql.Builder;

            if (typeID.HasValue)
            {
                sql.Where("vacation_type_id=@0", typeID.Value);
            }
            // 如果没有查看总站的权限时
            if (!SysHelper.HasPower("vacation_record_all"))
            {
                if (SysHelper.HasPower("vacation_record_site"))
                {
                    if (!deptID.HasValue)
                        deptID = BasicDataCache.listDepts.Single(d => d.ID == CurrUser.DeptID).ParentID;
                }
                else
                {
                    if (!deptID.HasValue)
                        deptID = CurrUser.DeptID;
                }
            }

            if (deptID.HasValue)
            {
                sql.Where("user_id in (select id from sys_users where dept_id in (@0))", BasicDataCache.GetSubDepts(deptID.Value).Select(d => d.ID));
            }

            var page = VacationApply.Page(pageIndex, pageSize, sql);

            List<VacationApplyModel> result = CreateModel(page);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = result
            });
        }

        [HttpGet]
        [PowerFilter("vacation_apply_aduit_view")]
        public ActionResult AuditIndex()
        {
            return View();
        }

        [HttpPost]
        [PowerFilter("vacation_apply_aduit_view")]
        public JsonResult AuditPage(int pageIndex, int pageSize, int? typeID, int? deptID)
        {
            var sql = Sql.Builder.Where("status in (@0,@1)", EnumVacationApplyStatus.Apply, EnumVacationApplyStatus.Audited);

            if (typeID.HasValue)
            {
                sql.Where("vacation_type_id=@0", typeID.Value);
            }

            // 如果没有查看总站的权限时
            if (!SysHelper.HasPower("vacation_apply_aduit_all"))
            {
                if (SysHelper.HasPower("vacation_apply_aduit_site"))
                {
                    if (!deptID.HasValue)
                        deptID = BasicDataCache.listDepts.Single(d => d.ID == CurrUser.DeptID).ParentID;
                }
                else
                {
                    if (!deptID.HasValue)
                        deptID = CurrUser.DeptID;
                }
            }

            if (deptID.HasValue)
            {
                sql.Where("user_id in (select id from sys_users where dept_id in (@0))", BasicDataCache.GetSubDepts(deptID.Value).Select(d => d.ID));
            }

            // 找出需要审核的流程
            var flows = BasicDataCache.listVacationAuditFlows.Where(flow => flow.AuditRoleIDs.ToIntList().Contains(CurrUser.RoleID));

            if (flows.HasElements())
            {
                // 判断范围

                List<string> scopes = new List<string>();

                foreach (var item in flows)
                {
                    switch (item.DeptLevel)
                    {
                        case EnumDeptLevel.TheOrSub:
                            {
                                scopes.Add(string.Format("flow_id={0} and user_id in (select id from sys_users where dept_id in({1}))", item.ID, BasicDataCache.GetSubDepts(CurrUser.DeptID).Select(d => d.ID).Join()));
                            }
                            break;
                        case EnumDeptLevel.TheSite:
                            {
                                scopes.Add(string.Format("flow_id={0} and user_id in (select id from sys_users where dept_id in({1}))", item.ID, BasicDataCache.GetSubDepts(BasicDataCache.GetSite(CurrUser.DeptID).ID).Select(d => d.ID).Join()));
                            }
                            break;
                        case EnumDeptLevel.All:
                            {
                                scopes.Add(string.Format("flow_id={0}", item.ID));
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                sql.Where(scopes.Join(" or "));
            }
            else
            {
                return Json(new ResponseData { Total = 0, Data = new List<VacationApplyModel>() });
            }

            var page = VacationApply.Page(pageIndex, pageSize, sql);

            List<VacationApplyModel> result = CreateModel(page);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = result
            });
        }

        private List<VacationApplyModel> CreateModel(Page<VacationApply> page)
        {
            List<VacationApplyModel> result = new List<VacationApplyModel>();
            foreach (var flow in page.Items)
            {
                VacationApplyModel model = new VacationApplyModel
                {
                    ApplyTime = flow.ApplyTime.ToDateTimeString(),
                    EndTime = flow.EndTime.ToDateTimeString(),
                    ID = flow.ID,
                    Opinion = flow.Opinion,
                    Reason = flow.Reason,
                    StartTime = flow.StartTime.ToDateTimeString(),
                    Status = flow.Status.EnumToName(),
                    VacationTypeName = flow.VacationTypeID.ToVacationType().Name,
                    CanModify = CanModify(flow.Status)
                };

                var user = flow.UserID.ToUser();

                model.RealName = user.RealName;
                model.UserName = user.UserName;
                model.DeptName = user.DeptID.ToDept().GetFullDeptName();

                result.Add(model);
            }
            return result;
        }

        [HttpGet]
        [PowerFilter("vacation_apply_aduit_aduit")]
        public ActionResult Audit(int id)
        {
            var apply = VacationApply.Single(id);

            if (CanAudit(apply.Status))
            {
                return View(apply);
            }
            else
            {
                return Redirect("~/404");
            }
        }

        [HttpPost]
        [PowerFilter("vacation_apply_aduit_aduit")]
        public JsonResult Audit(int id, bool isPass, string opinion)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            var apply = VacationApply.Single(id);

            if (!CanAudit(apply.Status))
            {
                result.Message = "此申请不允许审核，请刷新再操作！";
                return Json(result);
            }

            if (isPass)
            {
                var msg = GetNextFlow(apply.UserID.ToUser(), apply.FlowID);

                if (msg.IsComplete)
                {
                    var nextFlow = msg.Entity as VacationAuditFlow;

                    if (nextFlow == null)
                    {
                        apply.FlowID = null;
                        apply.Status = EnumVacationApplyStatus.Pass;
                        apply.Opinion = opinion;
                    }
                    else
                    {
                        apply.FlowID = nextFlow.ID;
                        apply.Status = EnumVacationApplyStatus.Audited;
                        apply.Opinion = opinion;
                    }

                    apply.Update();

                    return Json(ArtDialogResponseResult.SuccessResult);
                }
                else
                {
                    result.Message = msg.Message;
                    return Json(result);
                }
            }
            else
            {
                apply.Status = EnumVacationApplyStatus.Nopass;
                apply.Opinion = opinion;
                apply.Update();

                return Json(ArtDialogResponseResult.SuccessResult);
            }
        }

        [PowerFilter("vacation_record_delete")]
        public ActionResult Delete(string id)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            var ids = id.ToIntList();

            if (ids.HasElements())
            {
                foreach (var item in VacationApply.Fetch("where id in (@0)", ids))
                {
                    item.Delete();
                }

                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "请选择至少一条记录操作！";
            }

            return Json(result);
        }

        private bool CanAudit(EnumVacationApplyStatus status)
        {
            return new EnumVacationApplyStatus[] { EnumVacationApplyStatus.Apply, EnumVacationApplyStatus.Audited }.Contains(status);
        }

        #endregion

        #region 审核流程相关方法

        private VMessage GetNextFlow(SysUser user, int? prevFlowID)
        {
            VMessage result = new VMessage();

            // 根据当前用户角色获取审核流程类型  

            var flowType = BasicDataCache.listVacationAuditFlowTypes.FirstOrDefault(type => type.UsedRoleIDs.ToIntList().Contains(user.RoleID));

            if (flowType == null)
            {
                result.AddItem("您所属的角色尚未设置审核流程，请联系管理员！");
                return result;
            }

            if (prevFlowID.HasValue)
            {
                var prevFlow = BasicDataCache.listVacationAuditFlows.SingleOrDefault(flow => flow.ID == prevFlowID.Value);
                if (prevFlow == null)
                {
                    LogHelper.Log("流程缺失", string.Format("flow_id:{0}缺失", prevFlowID.Value));

                    result.AddItem("数据错误，请联系管理员！");
                    return result;
                }

                var nextFlow = BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == prevFlow.TypeID)
                   .OrderBy(flow => flow.Sort).FirstOrDefault(flow => flow.Sort > prevFlow.Sort);

                result.Entity = nextFlow;
            }
            else
            {
                int minSort = BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == flowType.ID).Min(flow => flow.Sort);

                var firstFlow = BasicDataCache.listVacationAuditFlows.Where(flow => flow.TypeID == flowType.ID)
                    .SingleOrDefault(flow => flow.Sort == minSort);
                if (firstFlow == null)
                {
                    result.AddItem("您所属的角色尚未设置审核流程，请联系管理员！");
                    return result;
                }

                result.Entity = firstFlow;
            }

            return result;
        }

        #endregion
    }
}
