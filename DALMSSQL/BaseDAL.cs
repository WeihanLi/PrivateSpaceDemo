using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace DALMSSQL
{
    public class BaseDAL<T> : IDAL.IBaseDAL<T> where T : class, new()
    {
        protected DbContext db = new DbContextFactory().GetDbContext();

        /// <summary>
        /// 添加数据实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>数据库更新受影响的行数</returns>
        public virtual int Add(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges();
        }

        /// <summary>
        /// 删除数据实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        public virtual int Del(T model)
        {
            RemoveHoldingEntityInContext(model);
            ////将 对象 添加到 EF中
            //DbEntityEntry entry = db.Entry<T>(model);
            ////设置 对象的包装 状态为 Unchanged
            //entry.State = EntityState.Deleted;
            //
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }

        /// <summary>
        /// 根据指定条件批量删除
        /// </summary>
        /// <param name="delWhere">删除条件 Linq表达式</param>
        /// <returns>数据库更新受影响行数</returns>
        public virtual int DelBy(Expression<Func<T, bool>> delWhere)
        {
            
            //查询要删除的数据
            List<T> listDeleting = db.Set<T>().Where(delWhere).ToList();
            //将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                RemoveHoldingEntityInContext(u);
                db.Set<T>().Attach(u);//先附加到 EF容器
                db.Set<T>().Remove(u);//标识为 删除 状态
            });
            //一次性 生成sql语句到数据库执行删除
            return db.SaveChanges();
        }

        public virtual bool Exist(Expression<Func<T, bool>> whereLambda)
        {
            int count = db.Set<T>().AsNoTracking().Where(whereLambda).Count();
            if (count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取所有数据列表
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<T> GetList()
        {
            return db.Set<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// 根据指定条件查询数据
        /// </summary>
        /// <param name="whereLambda">查询条件 Linq表达式</param>
        /// <returns>符合条件的数据列表</returns>
        public virtual List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsNoTracking().ToList();
        }

        #region 获取所有符合要求的列表   条件 + 排序
        /// <summary>
        /// 获取所有符合要求的列表   条件 + 排序
        /// </summary>
        /// <typeparam name="TKey">要用作排序 字段</typeparam>
        /// <param name="whereLambda">查询条件Linq表达式</param>
        /// <param name="orderLambda">排序条件Linq表达式</param>
        /// <returns>符合要求的数据列表</returns>
        public virtual List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda)
        {
            return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderLambda).ToList();
        }
        #endregion

        #region 根据条件和排序条件 查询分页数据 + List<T> GetPagedList
        /// <summary>
        /// 查询分页数据 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey">排序用到的键值</typeparam>
        /// <param name="pageIndex">页码索引，第几页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">查询条件Linq表达式</param>
        /// <param name="orderBy">排序条件Linq表达式</param>
        /// <param name="isAsc">是否是正向排序</param>
        /// <returns>符合要求的数据列表</returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        #endregion

        #region 查询分页数据（返回符合要求的记录总数）+ GetPagedList
        /// <summary>
        /// 查询分页数据（返回符合要求的记录总数）+ GetPagedList
        /// </summary>
        /// <typeparam name="TKey">排序用到的键值</typeparam>
        /// <param name="pageIndex">页码索引，第几页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowsCount">总记录数</param>
        /// <param name="whereLambda">查询条件Linq表达式</param>
        /// <param name="orderBy">排序条件Linq表达式</param>
        /// <param name="isAsc">是否正序排列</param>
        /// <returns>符合要求的列表</returns>
        public virtual List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowsCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            //查询总的记录数
            rowsCount = db.Set<T>().Where(whereLambda).Count();
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        #endregion

        public virtual int Modify(T model, params string[] proNames)
        {
            RemoveHoldingEntityInContext(model);
            //4.1将 对象 添加到 EF中
            DbEntityEntry entry = db.Entry(model);
            //4.2先设置 对象的包装 状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //4.3循环 被修改的属性名 数组
            foreach (string proName in proNames)
            {
                //4.4将每个 被修改的属性的状态 设置为已修改状态;后面生成update语句时，就只为已修改的属性 更新
                entry.Property(proName).IsModified = true;
            }
            //4.4一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }

        public virtual int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] PropertyNames)
        {
            RemoveHoldingEntityInContext(model);
            List<T> list = db.Set<T>().Where(whereLambda).AsNoTracking().ToList();
            foreach (T item in list)
            {
                //将对象添加到EF对象
                DbEntityEntry entry = db.Entry<T>(item);
                //设置状态为 Unchanged
                entry.State = EntityState.Unchanged;

                foreach (string property in PropertyNames)
                {
                    entry.Property(property).IsModified = true;
                }
            }
            return db.SaveChanges();
        }

        /// <summary>
        /// 执行SQL 语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>执行sql语句后受影响的行数</returns>
        public virtual int ExcuteSql(string strSql, params object[] paras)
        {
            return db.Database.ExecuteSqlCommand(strSql, paras);
        }

        /// <summary>
        /// 监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool RemoveHoldingEntityInContext(T entity)
        {
            ObjectContext objContext = ((IObjectContextAdapter)db).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);
            object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            if (exists)
            {
                objContext.Detach(foundEntity);
            }
            return (exists);
        }
    }
}
