using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The username can not be empty")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Username's length must be between 2~10")]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email can not be empty")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [System.Web.Mvc.Remote("CheckMail", "Member", ErrorMessage = "The Email has been registed")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password can not be empty")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field can not be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The 'Password' and the 'Confirm Password' does not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "The captcha can not be empty")]
        [Display(Name= "Captcha")]
        [System.Web.Mvc.Remote("ValidCode", "Member", ErrorMessage = "The captcha is wrong")]
        public string ValidCode { get; set; }
    }
}
