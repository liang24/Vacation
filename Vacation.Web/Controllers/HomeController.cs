using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Models;
using Vacation.Web.AppCode;
using System.IO;

namespace Vacation.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (CurrUser.IsFirstVisit)
            {
                ViewBag.IsFirstVisit = true;

                CurrUser.IsFirstVisit = false;
                CurrUser.Update();
            }

            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public JsonResult Upload()
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();

            if (CurrUser == null)
            {
                result.Message = "非法操作...";
                return Json(result);
            }

            var file = Request.Files["Filedata"];
             
            var vm = UploadHelper.UploadFile(file, Server.MapPath("~/uploadfiles"));

            string filePath = string.Format("/uploadfiles/{0}", vm.Entity);

            result.IsSuccess = true;
            result.Entity = filePath;

            return Json(result);
        }
    }
}
