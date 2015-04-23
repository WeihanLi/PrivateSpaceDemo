using IBLL;

namespace BLL
{
    public class BLLSession : IBLL.IBLLSession
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
                if (iMood==null)
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
                if (iUser==null)
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
            //1.根据配置文件内容 创建 DBSessionFactory 工厂对象
            IDAL.IDbSessionFactory sessionFactory = null;

            //2.通过 工厂 创建 DBSession对象
            IDAL.IDbSession iDbSession = sessionFactory.GetDbSession();

            return iDbSession.SaveChanges();
        }
    }
}
