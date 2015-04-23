using IDAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLL
{
    /// <summary>
    /// BLL 层父类
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    public abstract class BaseBLL<T> : IBLL.IBaseBLL<T> where T : class, new()
    {
        /// <summary>
        /// 数据层访问接口
        /// </summary>
        protected IDAL.IBaseDAL<T> IDal = null;
        
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseBLL()
        {
            SetDal();
        }

        #region 初始化数据层访问接口 IDal
        /// <summary>
        /// 初始化数据层访问接口 IDal
        /// </summary>
        public abstract void SetDal(); 
        #endregion

        /// <summary>
        /// 数据仓储
        /// </summary>
        public IDbSession DbSession
        {
            get
            {
                return new DbSessionFactory().GetDbSession();
            }
        }

        public virtual int Add(T model)
        {
            return IDal.Add(model);
        }

        public virtual int Del(T model)
        {
            return IDal.Del(model);
        }

        public virtual bool Exist(Expression<Func<T, bool>> whereLambda)
        {
            return IDal.Exist(whereLambda);
        }

        public virtual List<T> GetList()
        {
            return IDal.GetList();
        }

        public virtual List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return IDal.GetListBy(whereLambda);
        }

        public virtual List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda)
        {
            return IDal.GetListBy<TKey>(whereLambda, orderLambda);
        }

        public virtual List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowsCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            return IDal.GetPagedList<TKey>(pageIndex, pageSize,ref rowsCount, whereLambda, orderBy, isAsc);
        }

        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            return IDal.GetPagedList<TKey>(pageIndex, pageSize, whereLambda, orderBy, isAsc);
        }

        public int Modify(T model, params string[] proNames)
        {
            return IDal.Modify(model, proNames);
        }

        public int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        {
            return IDal.ModifyBy(model, whereLambda, modifiedProNames);
        }
    }
}
