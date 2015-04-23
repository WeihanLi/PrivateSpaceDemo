using System.Web.Mvc;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System;

namespace PrivateSpace.Logic
{
    [Filters.PermissionRequire]
    public class ChatController :Controller
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(ChatController));
        //private const string APIKey = "73b4e7f0-75a8-4d6b-bccf-fa180ccec35e";
        private static string url = "http://www.xiaohuangji.com/ajax.php";
        private static string urlFormat = "http://www.xiaohuangji.com/ajax.php/para={0}";

        /// <summary>
        /// GET方法获取消息的回复
        /// </summary>
        /// <returns>返回结果的Json数据</returns>
        [HttpGet]
        public async Task<JsonResult> GetReply(string msg)
        //public JsonResult GetReply(string msg)
        {
            Model.FormatModel.JsonMsgModel jsonMsg = new Model.FormatModel.JsonMsgModel();
            try
            {
                //GET
                //string response = await Common.Helper.DoGetRequest(string.Format(urlFormat, msg));
                //POST
                string response = await Common.Helper.DoPostRequestAsync(url, "para=" + msg);
                //string response = "Test Success";//测试
                //Model.FormatModel.ResponseMsgModel responseMsg = Common.Helper.JsonToObject<Model.FormatModel.ResponseMsgModel>(response);
                //if (responseMsg.result == 100 && responseMsg.msg == "OK")
                //{
                //    jsonMsg.Data = responseMsg.response;
                //}
                //else
                //{
                //    jsonMsg.Data = "听不懂 -_-";
                //}
                jsonMsg.Data = response;
                jsonMsg.Status = Model.FormatModel.JsonResultStatus.ok;
                jsonMsg.Msg = "success";
            }
            catch (Exception ex)
            {
                jsonMsg.Data = "内部发生错误了，-_-";
                jsonMsg.Status = Model.FormatModel.JsonResultStatus.error;
                jsonMsg.Msg = ex.Message;
            }
            return Json(jsonMsg,JsonRequestBehavior.AllowGet);
        }

    }
}
