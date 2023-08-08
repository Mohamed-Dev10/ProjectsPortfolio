using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoLibrary.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OBJECTID { get; set; }
        public string TitleBook { get; set; }
        public string DescriptionBook { get; set; }
        public string fileUrl { get; set; }

        [NotMapped]
        public IFormFile imgBook { get; set; }

        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile PictureBook { get; set; }

        public Author author { get; set; }
    }
}
