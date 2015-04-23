namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PrivateEntity : DbContext
    {
        public PrivateEntity()
            : base("name=PrivateEntity")
        {
        }

        public virtual DbSet<Diary> Diary { get; set; }
        public virtual DbSet<Mood> Mood { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
