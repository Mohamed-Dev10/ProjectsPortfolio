using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoLibrary.Models.dto
{
    public class RatingBookRequest
    {
        public string UserId { get; set; }
        public User User { get; set; }


        public int RatingNumber { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
