using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.FormatModel
{
    /// <summary>
    /// Ajax请求返回消息类
    /// </summary>
    public class JsonMsgModel
    {
        public string Msg { get; set; }
        public JsonResultStatus Status { get; set; }
        public string BackUrl { get; set; }
        public object Data { get; set; } //数据对象
    }

    public enum JsonResultStatus { ok,error,noPermision}
}
