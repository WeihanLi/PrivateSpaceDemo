using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PrivateSpace.Logic
{
    [Filters.PermissionRequire]
    public class ManageController : Controller
    {
        private Model.FormatModel.JsonMsgModel msg = null;
        private static Model.User u = null;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult DiaryPartial()
        {
            u = Session["Member"] as Model.User;
            List<Model.ViewModel.DiaryViewModel> moods = Helper.OperateContext.Current.LoadDiary(1, 10, m => m.userId == u.userId, m => m.diaryTime, false);
            return PartialView(moods);
        }

        public ActionResult MoodPartial()
        {
            u = Session["Member"] as Model.User;
            List<Model.ViewModel.MoodViewModel> moods = Helper.OperateContext.Current.LoadMood(1, 10, m => m.userId == u.userId, m => m.MoodTime, false);
            return PartialView(moods);
        }

        /// <summary>
        /// 加载心情列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public JsonResult LoadMoodList(int pageIndex=1, int pageSize=10)
        {
            u = Session["Member"] as Model.User;
            msg = new Model.FormatModel.JsonMsgModel() { Data = Helper.OperateContext.Current.LoadMood(pageIndex, pageSize, m=>m.userId==u.userId, m=>m.MoodTime,false), Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            return Json(msg);
        }

        /// <summary>
        /// 加载日记列表
        /// </summary>
        /// <param name="pageIndex">页码索引</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="u">用户</param>
        /// <returns></returns>
        public JsonResult LoadDiaryList(int pageIndex=1, int pageSize=10)
        {
            u = Session["Member"] as Model.User;
            msg = new Model.FormatModel.JsonMsgModel() { Data = Helper.OperateContext.Current.LoadDiary(pageIndex, pageSize, d=>d.userId==u.userId, d=>d.diaryTime,false), Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            return Json(msg);
        }
    }
}
