using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Domain.Entities;
using PetaPoco;
using Common.Models;
using Vacation.Web.Filters;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class NoticeController : BaseController
    {

        #region 管理模块

        [PowerFilter("notice_view")]
        public ActionResult Index()
        {
            return View();
        }

        [PowerFilter("notice_view")]
        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("title like @0", "%" + name + "%");

            var page = Notice.Page(pageIndex, pageSize, sql);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = page.Items
            });
        }

        [PowerFilter("notice_add")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [PowerFilter("notice_add")]
        public JsonResult Add(Notice notice)
        {
            notice.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }
        [PowerFilter("notice_update")]
        public ActionResult Update(int id)
        {
            var notice = Notice.SingleOrDefault(id);

            return View(notice);
        }

        [HttpPost]
        [PowerFilter("notice_update")]
        public JsonResult Update(Notice notice)
        {
            var old = Notice.SingleOrDefault(notice.ID);
            old.ID = notice.ID;
            old.Title = notice.Title;
            old.Content = notice.Content;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }
        [PowerFilter("notice_delete")]
        public JsonResult Delete(string id)
        {
            Notice.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

    }
}
