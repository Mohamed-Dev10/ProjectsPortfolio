using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoLibrary.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int OBJECTID { get; set; }
        public string Description { get; set; }

        public int BookId { get; set; } // Foreign key for Book
        public Book Book { get; set; }   // Navigation property to Book entity

        public string UserId { get; set; } // Foreign key for User
        public User User { get; set; }

    }
}
