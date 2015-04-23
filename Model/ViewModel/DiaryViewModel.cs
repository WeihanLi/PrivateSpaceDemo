using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class DiaryViewModel
    {
        public int DiaryId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name="Title")]
        public string DiaryTitle { get; set; }

        [Required]
        [Display(Name="Content")]
        [DataType(DataType.MultilineText)]
        public string DiaryContent { get; set; }

        
        [Display(Name="Time")]
        public DateTime DiaryTime { get; set; }
    }
}
