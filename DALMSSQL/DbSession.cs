using IDAL;
using System;

namespace DALMSSQL
{
    public class DbSession : IDbSession
    {
        #region Fields
        private IDiary iDiary = null;
        private IMood iMood = null;
        private IUser iUser = null;
        #endregion

        public IDiary IDiary
        {
            get
            {
                if (iDiary == null)
                {
                    iDiary = new Diary();
                }
                return iDiary;
            }
            set
            {
                iDiary = value;
            }
        }

        public IMood IMood
        {
            get
            {
                if (iMood == null)
                {
                    iMood = new Mood();
                }
                return iMood;
            }

            set
            {
                iMood = value;
            }
        }

        public IUser IUser
        {
            get
            {
                if (iUser == null)
                {
                    iUser = new User();
                }
                return iUser;
            }

            set
            {
                iUser = value;
            }
        }

        public int SaveChanges()
        {
            //获取上下文对象，保存数据，更新数据库
            return new DbContextFactory().GetDbContext().SaveChanges();
        }
    }
}
