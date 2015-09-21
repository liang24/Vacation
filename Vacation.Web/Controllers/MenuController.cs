using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using PetaPoco;
using Common.Models;
using Vacation.Web.AppCode;
using Vacation.Web.Filters;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        #region 管理模块

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNodes(string id)
        {
            bool? isVisible = null;

            var trees = BasicDataCache.GetMenuTrees(id.ToInt(), isVisible);

            return Json(trees.Select(t => new
            {
                id = t.ID,
                name = t.Name,
                isParent = t.IsParent,
                pid = t.ParentID,
                menu = t,
                tid = t.ID.ToString()
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(SysMenu menu)
        {
            menu.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpPost]
        public JsonResult Update(SysMenu menu)
        {
            var old = SysMenu.SingleOrDefault(menu.ID);
            old.Name = menu.Name;
            old.Sort = menu.Sort;
            old.Url = menu.Url;
            old.Tag = menu.Tag;
            old.ParentID = menu.ParentID;
            old.IsVisible = menu.IsVisible;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [TransactionFilter]
        public JsonResult Delete(string id)
        {
            SysMenu.Delete("where id in (@0)", id.ToIntList());
            SysFunction.Delete("where menu_id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

        #region Function

        public ActionResult Function(int menuId)
        {
            SysMenu menu = BasicDataCache.listMenus.SingleOrDefault(m => m.ID == menuId);

            menu.Functions = BasicDataCache.listFunctions.Where(f => f.MenuID == menu.ID);

            return View(menu);
        }

        public ActionResult FunctionAdd(int menuId)
        {
            SysMenu menu = BasicDataCache.listMenus.SingleOrDefault(m => m.ID == menuId);

            return View(menu);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.FunctionList_CacheKey)]
        public JsonResult FunctionAdd(SysFunction model)
        {
            model.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public ActionResult FunctionUpdate(int id)
        {
            SysFunction func = BasicDataCache.listFunctions.SingleOrDefault(f => f.ID == id);

            return View(func);
        }

        [HttpPost]
        [TransactionFilter]
        [RemoveCacheFilter(BasicDataCache.FunctionList_CacheKey)]
        public JsonResult FunctionUpdate(SysFunction model)
        {
            SysFunction func = BasicDataCache.listFunctions.SingleOrDefault(f => f.ID == model.ID);
            func.Name = model.Name;
            func.Tag = model.Tag;
            func.Sort = model.Sort;
            func.Update();

            // 更新SysPower的FunctionTag

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [HttpPost]
        [RemoveCacheFilter(BasicDataCache.FunctionList_CacheKey)]
        public JsonResult FunctionDelete(string id)
        {
            SysPower.Delete("where function_id in (@0)", id.ToIntList());
            SysFunction.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion
    }
}
