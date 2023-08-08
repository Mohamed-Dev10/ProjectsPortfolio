using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoLibrary.Models
{
    public class Author
    {
        [Key]
        public int OBJECTID { get; set; }
        public string NameAuthor { get; set; }
    }
}
