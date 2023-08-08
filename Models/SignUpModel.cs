using System.ComponentModel.DataAnnotations;

namespace DemoLibrary.Models
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        public string username { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [Compare("confirmpassword")]
        public string password { get; set; }

        [Required]
        public string confirmpassword { get; set; }
    }
}
