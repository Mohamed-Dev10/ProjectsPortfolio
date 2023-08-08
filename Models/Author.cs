using System.ComponentModel.DataAnnotations;

namespace DemoLibrary.Models
{
    public class Author
    {
        [Key]
        public int OBJECTID { get; set; }
        public string NameAuthor { get; set; }
    }
}
