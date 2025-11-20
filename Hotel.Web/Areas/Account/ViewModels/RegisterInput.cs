using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.Areas.Account.ViewModels
{
    public class RegisterInput
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        [Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
