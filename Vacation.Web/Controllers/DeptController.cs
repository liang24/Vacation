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
    public class DeptController : BaseController
    {

        #region 管理模块

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("dept_code like @0 or dept_name like @0", "%" + name + "%");

            var page = SysDept.Page(pageIndex, pageSize, sql);

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
        public JsonResult Add(SysDept dept)
        {
            dept.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public ActionResult Update(int id)
        {
            var dept = SysDept.SingleOrDefault(id);

            return View(dept);
        }

        [HttpPost]
        public JsonResult Update(SysDept dept)
        {
            var old = SysDept.SingleOrDefault(dept.ID);
            old.Code = dept.Code;
            old.Name = dept.Name;
            old.Sort = dept.Sort;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public JsonResult Delete(string id)
        {
            SysDept.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
