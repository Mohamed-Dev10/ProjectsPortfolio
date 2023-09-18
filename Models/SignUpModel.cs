using System.ComponentModel.DataAnnotations;

namespace DemoLibrary.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "The First Name field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The username field is required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [Compare("ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "The password must have at least 8 characters, one uppercase letter, one lowercase letter, and one number.")]
        public string password { get; set; }

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        public string ConfirmPassword { get; set; }
    }
}
