using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class ConfigurationHelper
    {
        private static string siteroot = null;

        /// <summary>
        /// 获取配置文件中AppSetting节点的相对路径对应的绝对路径
        /// </summary>
        /// <param name="key">相对路径设置的键值</param>
        /// <returns>绝对路径</returns>
        public static string AppSettingMapPath(string key)
        {
            if (String.IsNullOrEmpty(siteroot))
            {
                siteroot = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            }
            //拼接路径
            string path = siteroot + ConfigurationManager.AppSettings[key].ToString();
            return path;
        }

        /// <summary>
        /// 获取配置文件中AppSetting节点的值
        /// </summary>
        /// <param name="key">设置的键值</param>
        /// <returns>键值对应的值</returns>
        public static string AppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 获取配置文件中ConnectionStrings节点的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>键值对应的连接字符串值</returns>
        public static string ConnectionString(string key)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// 添加或更新 AppSettings 节点设置
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public static bool AddAppSetting(string key, string value)
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings[key] == null)
                {
                    ConfigurationManager.AppSettings.Add(key, value);
                }
                else
                {
                    ConfigurationManager.AppSettings[key] = value;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
