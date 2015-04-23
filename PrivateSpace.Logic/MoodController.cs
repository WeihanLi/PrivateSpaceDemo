using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Model.ViewModel;

namespace PrivateSpace.Logic
{
    [Filters.PermissionRequire]
    public class MoodController : BaseController<Model.ViewModel.MoodViewModel>
    {
        private Model.FormatModel.JsonMsgModel jsonMsg = null;

        private static Model.User u = null;

        public override JsonResult Delete(int id)
        {
            if (Helper.OperateContext.Current.DelMood(id))
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            else
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
            }
            return Json(jsonMsg,JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Detail(int id)
        {
            Model.ViewModel.MoodViewModel d = Helper.OperateContext.Current.SelectMood(id);
            return View(d);
        }

        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult List(int pageIndex=1,int pageSize=15)
        {
            u = HttpContext.Session["Member"] as Model.User;
            int pageCount = -1,rowsCount=-1;
            try
            {
                rowsCount = Helper.OperateContext.Current.QueryMoodCount();
                pageCount = Convert.ToInt32(Math.Ceiling((rowsCount * 1.0) / pageSize));
                List<Model.ViewModel.MoodViewModel> listMoods = Helper.OperateContext.Current.LoadMood(pageIndex, pageSize, s => s.userId == u.userId, m => m.MoodTime, false);
                Model.FormatModel.PageListModel<MoodViewModel> pageListMoods = new Model.FormatModel.PageListModel<MoodViewModel>() { RowsCount = rowsCount, PageIndex = pageIndex, PageSize = pageSize, Items = listMoods, PageCount = pageCount };
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = pageListMoods, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            catch (Exception ex)
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = null, Status = Model.FormatModel.JsonResultStatus.error, Msg = ex.Message };
            }
            return Json(jsonMsg);
        }

        /// <summary>
        /// 发表新心情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ActionResult Add(MoodViewModel model)
        {
            u = HttpContext.Session["Member"] as Model.User;
            model.MoodTime = DateTime.Now;
            if (ModelState.IsValid || u == null)
            {
                Helper.OperateContext.Current.AddMood(model, u);
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = null, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            else
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = null, Status = Model.FormatModel.JsonResultStatus.error, Msg = "Fail" };
            }
            return Json(jsonMsg);
        }

        public ActionResult Create(MoodViewModel model)
        {
            u = HttpContext.Session["Member"] as Model.User;
            model.MoodTime = DateTime.Now;
            if (ModelState.IsValid || u == null)
            {
                Helper.OperateContext.Current.AddMood(model, u);
                return RedirectToAction("Index");
            }
            return Content("Fail");
        }
    }
}
