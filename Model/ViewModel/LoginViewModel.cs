using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email can not be empty")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="wrong Email address")]
        [System.Web.Mvc.Remote("ExistMail","Member",ErrorMessage ="This mail does not exist")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password can not be empty")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="The length of the password must be between 6 and 20")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="RememberMe")]
        public bool RememberMe { get; set; }
    }
}
