using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoLibrary.Models
{
    [Table("RatingUserBook")]
    public class RatingUserBook
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OBJECTID { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }


        public int RatingNumber { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}
