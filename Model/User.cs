using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("User")]
    public partial class User
    {
        [Key]
        public int userId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime RegTime { get; set; }

        public string Mail { get; set; }

        public string AuthCode { get; set; }

        public virtual ICollection<Diary> Diary { get; set; }

        public virtual ICollection<Mood> Mood { get; set; }
    }
}
