using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPoco;
using Common.Models;
using Vacation.Domain.Entities;
using Vacation.Web.Filters;
using Vacation.Web.Models;
using Vacation.Domain.Views;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        #region 管理模块

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("role_name like @0", "%" + name + "%");

            var page = SysRole.Page(pageIndex, pageSize, sql);

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
        public JsonResult Add(SysRole role)
        {
            role.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public ActionResult Update(int id)
        {
            var role = SysRole.SingleOrDefault(id);

            return View(role);
        }

        [HttpPost]
        public JsonResult Update(SysRole role)
        {
            var old = SysRole.SingleOrDefault(role.ID);
            old.Name = role.Name;
            old.Description = role.Description;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public JsonResult Delete(string id)
        {
            SysRole.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpGet]
        public ActionResult Assign(int id)
        {
            SysRole role = SysRole.SingleOrDefault(id);

            return View(role);
        }

        [HttpPost]
        public JsonResult Assign(string userIds, int id)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                var users = SysUser.Fetch("where id in (@0)", userIds.ToIntList());
                var role = SysRole.SingleOrDefault(id);

                foreach (var user in users)
                {
                    try
                    {
                        SysUserRole userRole = new SysUserRole
                        {
                            RoleID = role.ID,
                            UserID = user.ID
                        };

                        DB.GetInstance().Insert(userRole);
                    }
                    catch { }
                }

            }
            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpPost]
        public JsonResult CancelAssign(string userIds, int id)
        {
            if (!string.IsNullOrEmpty(userIds))
            {

                DB.GetInstance().Delete<SysUserRole>("where user_id in (@0) and role_id = @1", userIds.ToIntList(), id);

            }
            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpGet]
        public ActionResult Authorize(int id)
        {
            RoleAuthorizeModel model = new RoleAuthorizeModel
            {
                Role = SysRole.SingleOrDefault(id),
                Funcs = SysFunction.Fetch(Sql.Builder),
                Trees = MenuTree.Fetch(Sql.Builder),
                SelectedFuncIds = SysPower.Fetch("where master_id=@0 and master_type=@1", id, MasterType.Role.ToString()).Select(power => power.FunctionID)
            };

            return View(model);
        }

        [HttpPost]
        [TransactionFilter]
        public JsonResult Authorize(string functionIds, int id)
        {
            var role = SysRole.SingleOrDefault(id);

            // 删除权限
            SysPower.Delete("where master_id in (@0) and master_type=@1", role.ID, MasterType.Role.ToString());

            // 角色授权
            if (string.IsNullOrEmpty(functionIds) == false)
            {
                var funcs = SysFunction.Fetch("where id in (@0)", functionIds.ToIntList());
                foreach (var func in funcs)
                {
                    SysPower power = new SysPower
                    {
                        FunctionID = func.ID,
                        MasterID = role.ID,
                        MasterType = MasterType.Role.ToString()
                    };

                    power.Insert();
                }
            }

            // 刷新用户权限
            var users = SysUser.Fetch("where id in (select user_id from sys_user_roles where role_id=@0)", role.ID);
            foreach (var user in users)
            {
                SysPower.Delete("where master_id in (@0) and master_type=@1", user.ID, MasterType.User.ToString());

                var powers = SysPower.Fetch("where master_id in (select role_id from sys_user_roles where user_id=@0) and master_type=@1", user.ID, MasterType.Role.ToString());

                foreach (var power in powers)
                {
                    try
                    {
                        SysPower newPower = new SysPower
                        {
                            FunctionID = power.FunctionID,
                            MasterID = user.ID,
                            MasterType = MasterType.User.ToString()
                        };
                        newPower.Insert();
                    }
                    catch { }
                }
            }

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion
    }
}
