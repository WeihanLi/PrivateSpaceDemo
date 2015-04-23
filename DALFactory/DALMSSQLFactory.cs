using System;
using IDAL;

namespace DALFactory
{
    /// <summary>
    /// DALMSSQLFactory 实体工厂
    /// </summary>
    public class DALMSSQLFactory :DALAbsFactory
    {

        public override IDiary GetDiary()
        {
            return new DALMSSQL.Diary();
        }

        public override IMood GetMood()
        {
            return new DALMSSQL.Mood();
        }

        public override IType GetType()
        {
            return new DALMSSQL.Type();
        }

        public override IUser GetUser()
        {
            return new DALMSSQL.User();
        }

        public override int SaveChanges()
        {
           return DbContext.SaveChanges();
        }
    }
}
