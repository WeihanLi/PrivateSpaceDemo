using Model.APIModel;
using System.Web.Mvc;

namespace PrivateSpace.API
{
    public class ManageController :Controller
    {
        private Model.FormatModel.JsonMsgModel msg=null;
        private static Common.LogHelper logger = new Common.LogHelper(typeof(ManageController));

        public JsonResult DeleteMood(MoodOperateModel mood)
        {
            if (mood == null)
            {
                mood = Helper.OperateContext.RequestParams<MoodOperateModel>();
            }
            if (mood.Member==null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (mood.Member.userId<=0&&mood.Member.Mail!=null)
            {
                mood.Member = Helper.OperateContext.Current.SelectMember(mood.Member.Mail);
            }
            if (Helper.OperateContext.Current.DelMood(mood.Mood.MoodId))
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            else
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
            }
            return Json(msg);
        }

        public JsonResult DeleteDiary(DiaryOperateModel diary)
        {
            if (diary == null)
            {
                diary = Helper.OperateContext.RequestParams<DiaryOperateModel>();
            }
            if (diary.Member == null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (diary.Member.userId <= 0 && diary.Member.Mail != null)
            {
                diary.Member = Helper.OperateContext.Current.SelectMember(diary.Member.Mail);
            }
            if (Helper.OperateContext.Current.DelDiary(diary.Diary.DiaryId))
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            else
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
            }
            return Json(msg);
        }

        public JsonResult AddMood(MoodOperateModel mood)
        {
            //验证mood是否为空
            if (mood==null)
            {
                mood = Helper.OperateContext.RequestParams<MoodOperateModel>();
            }
            if (mood.Member==null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (mood.Member.userId <= 0 && mood.Member.Mail != null)
            {
                mood.Member = Helper.OperateContext.Current.SelectMember(mood.Member.Mail);
            }
            try
            {
                Model.ViewModel.MoodViewModel m = Helper.OperateContext.Current.AddMood(mood.Mood, mood.Member);
                msg = new Model.FormatModel.JsonMsgModel() { Data = m, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
            }
            return Json(msg);
        }

        public JsonResult AddDiary(Model.APIModel.DiaryOperateModel d)
        {
            //验证 d 是否为空
            if (d==null)
            {
                d = Helper.OperateContext.RequestParams<DiaryOperateModel>();
            }
            if (d.Member==null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (d.Member.userId <= 0 && d.Member.Mail != null)
            {
                d.Member = Helper.OperateContext.Current.SelectMember(d.Member.Mail);
            }
            Model.ViewModel.DiaryViewModel m = Helper.OperateContext.Current.AddDiary(d.Diary, d.Member);
            msg = new Model.FormatModel.JsonMsgModel() { Data = m, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            return Json(msg);
        }

        /// <summary>
        /// 加载心情列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public JsonResult LoadMoodList(Model.User u,int pageIndex =1, int pageSize=10)
        {
            if (u==null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (u.userId <= 0 && u.Mail != null)
            {
                u = Helper.OperateContext.Current.SelectMember(u.Mail);
            }
            msg = new Model.FormatModel.JsonMsgModel() { Data =Helper.OperateContext.Current.LoadMood(pageIndex, pageSize, m=>m.userId==u.userId, m=>m.MoodTime,false), Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            return Json(msg);
        }

        /// <summary>
        /// 加载日记列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public JsonResult LoadDiaryList(Model.User u,int pageIndex=1, int pageSize=10)
        {
            if (u == null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (u.userId <= 0 && u.Mail != null)
            {
                u = Helper.OperateContext.Current.SelectMember(u.Mail);
            }
            msg = new Model.FormatModel.JsonMsgModel() { Data = Helper.OperateContext.Current.LoadDiary(pageIndex, pageSize, d=>d.userId==u.userId, d=>d.diaryTime,false), Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            return Json(msg);
        }

        public JsonResult UpdateDiary(DiaryOperateModel diary)
        {
            if (diary==null)
            {
                diary=Helper.OperateContext.RequestParams<DiaryOperateModel>();
            }
            if (diary.Member==null)
            {
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
                return Json(msg);
            }
            if (diary.Member.userId <= 0 && diary.Member.Mail != null)
            {
                diary.Member = Helper.OperateContext.Current.SelectMember(diary.Member.Mail);
            }
            try
            {
                Helper.OperateContext.Current.UpdateDiary(diary.Diary);
                msg = new Model.FormatModel.JsonMsgModel() { Data = diary, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
            }
            catch(System.Exception ex)
            {
                logger.Error(ex);
                msg = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.error, Msg = "fail" };
            }
            return Json(msg);
        }
    }
}
