using Microsoft.AspNetCore.Identity;

namespace DemoLibrary.Models
{
    public class User : IdentityUser
    {

        public string FirstName;
        public string LastName;
    }
}
