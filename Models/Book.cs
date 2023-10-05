using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
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
        public int NumberPage { get; set; }
        public int NumberDownlaods { get; set; }
        public int NumberViews { get; set; }
        public string language { get; set; }


        public string fileUrl { get; set; }

        [NotMapped]
        public IFormFile imgBook { get; set; }

        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile PictureBook { get; set; }

        public Author author { get; set; }

        public ICollection<Comments> Comments { get; set; } = new List<Comments>();
        public ICollection<favoritBooks> UserBooks { get; set; }
        public ICollection<DownloadBook> DownBooks { get; set; }
        public ICollection<RatingUserBook> RatingBooks { get; set; }
    }
}
