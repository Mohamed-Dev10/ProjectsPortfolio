using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DemoLibrary.Models
{
    public class User : IdentityUser
    {

        public string FirstName;
        public string LastName;
        public List<Comments> Comments { get; set; } = new List<Comments>();
        public ICollection<favoritBooks> UserFavoritBooks { get; set; }
        public ICollection<DownloadBook> UserDowloadBooks { get; set; }
        public ICollection<RatingUserBook> UserRatingBooks { get; set; }


    }
}
