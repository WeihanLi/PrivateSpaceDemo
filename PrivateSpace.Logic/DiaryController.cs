using System;
using System.Web.Mvc;
using Model.ViewModel;
using System.Collections.Generic;

namespace PrivateSpace.Logic
{
    [Filters.PermissionRequire]
    public class DiaryController : BaseController<DiaryViewModel>
    {
        private Model.FormatModel.JsonMsgModel jsonMsg = null;

        private static Model.User u = null;

        public ActionResult Create()
        {
            return View();
        }

        public override JsonResult Delete(int id)
        {
            if (Helper.OperateContext.Current.DelDiary(id))
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
            DiaryViewModel d = Helper.OperateContext.Current.SelectDiary(id);
            return View(d);
        }

        public ActionResult Edit(int id)
        {
            DiaryViewModel d = Helper.OperateContext.Current.SelectDiary(id);
            return View(d);
        }
        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult List(int pageIndex=1, int pageSize = 10)
        {
            u = HttpContext.Session["Member"] as Model.User;
            int pageCount = -1, rowsCount = -1;
            try
            {
                rowsCount = Helper.OperateContext.Current.QueryDiaryCount();
                pageCount = Convert.ToInt32(Math.Ceiling((rowsCount * 1.0) / pageSize));
                List<DiaryViewModel> listDiary = Helper.OperateContext.Current.LoadDiary(pageIndex, pageSize, s => s.userId == u.userId, d => d.diaryTime, false);
                Model.FormatModel.PageListModel<DiaryViewModel> pageListItems = new Model.FormatModel.PageListModel<DiaryViewModel>() { RowsCount = rowsCount, PageIndex = pageIndex, PageSize = pageSize, Items = listDiary, PageCount = pageCount };
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = pageListItems, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            catch(Exception ex)
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Data = null, Status = Model.FormatModel.JsonResultStatus.error, Msg = ex.Message };
            }
            return Json(jsonMsg);
        }

        [ValidateInput(false)]
        public ActionResult Update(DiaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Helper.OperateContext.Current.UpdateDiary(model))
                {
                    return RedirectToAction("Detail",new { id=model.DiaryId});
                }
                else
                {
                    return Content("<script>alert('更新失败，请稍后重试')</script>");
                }
            }
            else
            {
                return Content("<script>alert('输入数据不符合要求')</script>");
            }
        }

        [ValidateInput(false)]
        public override ActionResult Add(DiaryViewModel model)
        {
            try
            {
                u = HttpContext.Session["Member"] as Model.User;
                model.DiaryTime = DateTime.Now;
                if (ModelState.IsValid && u != null)
                {
                    Helper.OperateContext.Current.AddDiary(model, u);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(DiaryController)).Error(ex.Message + ex.StackTrace);
            }
            return Content("<script>alert('添加失败，请稍后重试')</script>");
        }

        [ValidateInput(false)]
        public JsonResult AddDiary(DiaryViewModel model)
        {
            try
            {
                u = HttpContext.Session["Member"] as Model.User;
                model.DiaryTime = DateTime.Now;
                if (ModelState.IsValid && u != null)
                {
                    Helper.OperateContext.Current.AddDiary(model, u);
                    jsonMsg = new Model.FormatModel.JsonMsgModel() { Msg = "success", Status = Model.FormatModel.JsonResultStatus.ok, BackUrl = "/Diary/" };
                }
                else
                {
                    jsonMsg = new Model.FormatModel.JsonMsgModel() { Msg = "Fail to add", Status = Model.FormatModel.JsonResultStatus.error, BackUrl = "/Diary/" };
                }
            }
            catch (Exception ex)
            {
                jsonMsg = new Model.FormatModel.JsonMsgModel() { Msg = ex.Message, Status = Model.FormatModel.JsonResultStatus.error, BackUrl = "/Diary/" };
            }
            return Json(jsonMsg, JsonRequestBehavior.AllowGet);
        }
    }
}
