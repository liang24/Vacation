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
    public class NoticeController : BaseController
    {
       
        #region 管理模块

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("title like @0", "%" + name + "%");

            var page =Notice.Page(pageIndex, pageSize, sql);

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
        public JsonResult Add(Notice notice)
        {
            notice.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public ActionResult Update(int id)
        {
            var notice = Notice.SingleOrDefault(id);

            return View(notice);
        }

        [HttpPost]
        public JsonResult Update(Notice notice)
        {
            var old = Notice.SingleOrDefault(notice.ID);
            old.ID = notice.ID;
            old.Title = notice.Title;
            old.Content = notice.Content;
            old.CreateTime = notice.CreateTime;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public JsonResult Delete(string id)
        {
            Notice.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
