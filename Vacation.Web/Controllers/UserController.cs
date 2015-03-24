﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation.Web.AppCode;
using Vacation.Domain.Entities;
using Common.Models;
using PetaPoco;

namespace Vacation.Web.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "用户名或密码不能为空！";

                return View();
            }

            string error;
            if (SysHelper.Login(userName, password, rememberMe, out error))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = error;

            return View();
        }

        public void Exit()
        {
            SysHelper.Exit();
        }

        [Authorize]
        public ActionResult Info()
        {
            var user = SysHelper.GetCurrUser();

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public JsonResult Modify(SysUser model)
        {
            CurrUser.Email = model.Email;
            CurrUser.Phone = model.Phone;
            CurrUser.Sex = model.Sex;
            CurrUser.RealName = model.RealName;
            CurrUser.HeadImage = model.HeadImage;

            CurrUser.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [Authorize]
        public ActionResult ChangePwd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangePwd(string oldPwd, string newPwd, string confirmPwd)
        {
            ArtDialogResponseResult result = new ArtDialogResponseResult();
            if (CurrUser.Password != oldPwd)
            {
                result.Message = "旧密码错误！";
            }
            else if (newPwd != confirmPwd)
            {
                result.Message = "两次密码不一致！";
            }
            else if (oldPwd == newPwd)
            {
                result.Message = "新密码不能与旧密码一样！";
            }
            else
            {
                CurrUser.Password = newPwd;
                CurrUser.Update();

                result.IsSuccess = true;
            }
            return Json(result);
        }

        #region 消息

        public JsonResult ReadMessage(string ids)
        {
            Vacation.Domain.Entities.MessageReceiver.Update("set status=@0 where id in (@1)", (int)MessageStatus.Readeded, ids.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        public JsonResult DeleteMessage(string id)
        {
            Vacation.Domain.Entities.MessageReceiver.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        #endregion

        [Authorize]
        public ActionResult Message()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetMessage(int pageIndex, int pageSize)
        {
            var page = CurrUser.GetMessages(pageIndex, pageSize);

            return Json(new ResponseData { Data = page.Items, Total = page.TotalItems });
        }

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="param">用户名</param>
        /// <returns></returns>
        /// 创建人：尹学良
        /// 5/25/2013 8:49 AM        
        public JsonResult CheckName(string param)
        {
            ValidformResponseResult result = new ValidformResponseResult();
            if (string.IsNullOrEmpty(param))
            {
                result.Message = "用户名不能为空";
            }
            else
            {
                if (SysUser.SingleOrDefault("where user_name=@0", param) != null)
                {
                    result.Message = "用户名已存在";
                }
                else
                {
                    result.IsSuccess = true;
                }
            }

            return Json(result);
        }


        #region 管理模块

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult Page(int pageIndex, int pageSize, string name)
        {
            var sql = Sql.Builder.Where("real_name like @0 or user_name like @0", "%" + name + "%");

            var page = SysUser.Page(pageIndex, pageSize, sql);

            return Json(new ResponseData
            {
                Total = page.TotalItems,
                Data = page.Items.Select(user => new
                {
                    user.ID,
                    user.Email,
                    user.UserName,
                    user.RealName
                })
            });
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult Add(SysUser user)
        {
            user.Insert();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [Authorize]
        public ActionResult Update(int id)
        {
            var user = SysUser.SingleOrDefault(id);

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public JsonResult Update(SysUser user)
        {
            var old = SysUser.SingleOrDefault(user.ID);
            old.RealName = user.RealName;
            old.Email = user.Email;
            old.Phone = user.Phone;

            old.Update();

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        [Authorize]
        public JsonResult Delete(string id)
        {
            SysUser.Delete("where id in (@0)", id.ToIntList());

            return Json(ArtDialogResponseResult.SuccessResult);
        }

        //[Authorize]
        //public ArtDialogResponseResult Authorize(string functionIds, int userId)
        //{
        //    //new PowerRepository().Authorize(MasterType.User, userId, string.IsNullOrEmpty(functionIds) ? new int[] { } : StringHelper.ChageStringToIntArray(functionIds));
        //    //SysHelper.CreateSession(CurrUser);
        //    return ArtDialogResponseResult.SuccessResult;
        //}

        #endregion
    }
}