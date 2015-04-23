using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class MoodViewModel
    {
        public int MoodId { get; set; }

        [Required]
        [Display(Name= "MoodContent")]
        public string MoodContent { get; set; }

        [Display(Name="MoodTime")]
        public DateTime MoodTime { get; set; }        
    }
}
