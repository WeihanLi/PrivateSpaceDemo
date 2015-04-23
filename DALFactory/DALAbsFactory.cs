using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace DALFactory
{
    /// <summary>
    /// Repository(DAL)层 抽象工厂
    /// </summary>
    public abstract class DALAbsFactory
    {
        //静态构造方法
        //static DALAbsFactory()
        //{
        //    string DALType = System.Configuration.ConfigurationManager.AppSettings["DALType"].ToString();
        //    switch (DALType)
        //    {
        //        case "DALMSSQL":
        //            CurrentFactory = new DALMSSQLFactory();
        //            DbSession= new DALMSSQL.DbSession();
        //            DbContext= new Model.PrivateEntity();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// EF上下文对象
        /// </summary>
        private static DbContext dbContext = null;

        #region 根据配置文件中 “DALType” 来创建工厂实体  + static DALAbsFactory GetDALFactory()
        /// <summary>
        /// 根据配置文件中 “DALType” 来创建工厂实体
        /// </summary>
        /// <returns>创建的工厂实体</returns>
        public static DALAbsFactory GetDALFactory()
        {
            string DALType = Common.ConfigurationHelper.AppSetting("DALType");
            DALAbsFactory DALFac = null;
            switch (DALType)
            {
                case "DALMSSQL":
                    DALFac = new DALMSSQLFactory();
                    break;
                default:
                    break;
            }
            return DALFac;
        }
        #endregion

        #region 创建 EF上下文 对象，在线程中共享 一个 上下文对象  +  DbContext
        /// <summary>
        /// 创建 EF上下文 对象，在线程中共享 一个 上下文对象
        /// </summary>
        /// <returns></returns>
        public static DbContext DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    //从当前线程中 获取 EF上下文对象
                    dbContext = CallContext.GetData(typeof(DALAbsFactory).Name) as DbContext;
                    if (dbContext == null)
                    {
                        dbContext = new Model.PrivateEntity();
                        dbContext.Configuration.ValidateOnSaveEnabled = false;
                        //将新创建的 ef上下文对象 存入线程
                        CallContext.SetData(typeof(DALAbsFactory).Name, dbContext);
                    }
                }
                return dbContext;
            }
        }
        #endregion

        #region EF 上下文 更新数据库
        /// <summary>
        /// EF 上下文 更新数据库
        /// </summary>
        /// <returns></returns>
        public abstract int SaveChanges(); 
        #endregion

        #region 创建类实体

        /// <summary>
        /// 创建 User 类实体
        /// </summary>
        /// <returns></returns>
        public abstract IDAL.IUser GetUser();

        /// <summary>
        /// 创建 Mood 类实体
        /// </summary>
        /// <returns></returns>
        public abstract IDAL.IMood GetMood();

        /// <summary>
        /// 创建 Diary 类实体
        /// </summary>
        /// <returns></returns>
        public abstract IDAL.IDiary GetDiary();

        /// <summary>
        /// 创建 Type 类实体
        /// </summary>
        /// <returns></returns>
        public abstract IDAL.IType GetType(); 
        #endregion
    }
}
