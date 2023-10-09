using DemoLibrary.Models;
using DemoLibrary.Models.dto;
using DemoLibrary.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly DemoBooksDbContext connectionToDba;
        private readonly IBookLibrary<Author> Iauthor;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IBookLibrary<Book> Ibook;
      //  private  int cartItemCount = 0;

#pragma warning disable CS0618 // 'IHostingEnvironment' est obsolète : 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        public BookApiController(DemoBooksDbContext connectionToDba, IHttpContextAccessor httpContext, IBookLibrary<Book> bookLirary, IBookLibrary<Author> Iauthor, IHostingEnvironment _hosting)
#pragma warning restore CS0618 // 'IHostingEnvironment' est obsolète : 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        {

            this.connectionToDba = connectionToDba;
            this.Ibook = bookLirary;
            this.Iauthor = Iauthor;
            this.hosting = _hosting;
          //  this.cartItemCount =cartItemCount;
            this.httpContextAccessor = httpContext;

        }

        //   private readonly IBookLibrary<Author> Iauthor;
        // GET: api/<BookApiController>

        [HttpGet("GetAllBooks")]
        public IList<Book> Get()
        {
            var books = Ibook.list().ToList();
            return books;
        }

        // GET api/<BookApiController>/5
        [HttpGet("GetBookById/{objectid}")]
        public IActionResult GetBookById(int objectid)
        {

            // Assuming Ibook.list() returns a list of books
            var book = Ibook.list().FirstOrDefault(b => b.OBJECTID == objectid);

            if (book == null)
            {
                return NotFound(); // Return a 404 Not Found response if the book with the given ID is not found.
            }

            return Ok(book); // Return a 200 OK response with the book data if found.

        }

        // POST api/<BookApiController>

        [Consumes("multipart/form-data")]
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromForm] Book bookDto, [FromHeader] int auth)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string FileName = string.Empty;

                // Find the author by ID
                var authorObject = Iauthor.Find(auth);


                // Save the file to disk
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    // string folder = "PublishedBooks/bookImg";
                    //string fileName = bookDto.imgBook.FileName;
                    //string filePath = Path.Combine(folder, fileName);
                    //bookDto.fileUrl = filePath;
                    var fileUploaded = HttpContext.Request.Form.Files[0];
                    var fileUploadedImg = HttpContext.Request.Form.Files[1];
                    //await bookDto.imgBook.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    var baseUrl = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host + httpContextAccessor.HttpContext.Request.PathBase;
                    //    var filePath = Path.Combine(hosting.WebRootPath, "UploadsBooks", fileUploaded.FileName);
                    // var filePath = @"G:\Portfolio Projects\DemoLib\FrontEnd\FrontEnd\LibraryBooks\LibraryBooks\src\assets\imgPublished\" + fileUploaded.FileName;
                    // Retrieve the latest OBJECTID from the database
                    int latestObjectId = Ibook.GetLatestObjectId();


                    // Increment the latestObjectId by 1
                    int newObjectId = latestObjectId + 1;


                    // Create the directory path using the newObjectId
                    var directoryPath = Path.Combine(@"C:\Users\Geomatic PC1\Documents\GitHub\FrontEnd BooksLibrary\src\assets\imgPublished", newObjectId.ToString());
                    var directoryPathImg = Path.Combine(@"C:\Users\Geomatic PC1\Documents\GitHub\FrontEnd BooksLibrary\src\assets\BooksCover", newObjectId.ToString());

                    if (!Directory.Exists(directoryPath) && !Directory.Exists(directoryPathImg))
                    {
                        Directory.CreateDirectory(directoryPath);
                        Directory.CreateDirectory(directoryPathImg);
                    }

                    var filePath = Path.Combine(directoryPath, fileUploaded.FileName);
                    var filePathImg = Path.Combine(directoryPathImg, fileUploadedImg.FileName);

                    bookDto.fileUrl = filePath;
                    bookDto.ImgUrl = filePathImg;


                    await bookDto.imgBook.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    await bookDto.PictureBook.CopyToAsync(new FileStream(filePathImg, FileMode.Create));


                }
                // Create a new book entity
                var book = new Book
                {
                    TitleBook = bookDto.TitleBook,
                    DescriptionBook = bookDto.DescriptionBook,
                    fileUrl = bookDto.fileUrl,
                    ImgUrl = bookDto.ImgUrl,
                    author = authorObject
                };

                // Add the book to the repository
                Ibook.Add(book);

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // PUT api/<BookApiController>/5
        [Authorize]
        [HttpPut("UpdateBook")]
        public void Put([FromHeader(Name = "idBook")] int? idBook, [FromBody] Book bookDto)
        {

            // var author = JsonConvert.DeserializeObject<Author>(authorJson);
            var BookDba = Ibook.Find((int)idBook);

            if (BookDba != null)
            {
                if (!string.IsNullOrEmpty(bookDto.TitleBook))
                {
                    BookDba.TitleBook = bookDto.TitleBook;
                }
                if (!string.IsNullOrEmpty(bookDto.DescriptionBook))
                {
                    BookDba.DescriptionBook = bookDto.DescriptionBook;
                }
                if (bookDto.author != null)
                {
                    BookDba.author = bookDto.author;
                }
                // Update the entity in the database


            }


            Ibook.Edit((int)idBook, BookDba);


        }

        [Authorize]
        // DELETE api/<BookApiController>/5
        [HttpDelete("DeleteBook/{idBook}")]
        public void Delete(int idBook)
        {
            var BookSelected = Ibook.Find(idBook);
            if (BookSelected != null)
            {
                Ibook.Delete(idBook);
            }

        }

        [HttpPost("AddFavoritBookForUser")]
        public IActionResult AddBookToFavoritListUser([FromBody] FavoriteBookRequest requestfavoritBook) {

            try {
                var user = connectionToDba.users.Find(requestfavoritBook.userid) ;
                var FavoritBook = connectionToDba.books.Find(requestfavoritBook.bookid);



                if (user != null && FavoritBook != null)
                {

                    var UserFavoritBook = new favoritBooks
                    {
                        Book = FavoritBook,
                        User = user
                    };

                    connectionToDba.favoritBooks.Add(UserFavoritBook);
                    connectionToDba.SaveChanges();
                    return Ok();
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, ex.Message); // Return a 500 Internal Server Error response for other exceptions
            }


        }

        [HttpPost("AddCommentBook")]
        public IActionResult CommentUserUser([FromBody] CommentBookUserRequest commentBookUserRequest) {

            try {

                var book = connectionToDba.books.Find(commentBookUserRequest.bookid);
                var user = connectionToDba.users.Find(commentBookUserRequest.userid);

                if (book != null && user != null)
                {

                    var commentUserBook = new Comments
                    {
                        Book = book,
                        User = user,
                        Description = commentBookUserRequest.description
                    };
                    connectionToDba.comments.Add(commentUserBook);
                    connectionToDba.SaveChanges();

                    return Ok();
                }
                else {
                    return NotFound();
                }

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpGet("GetCommentBook/{idBook}")]
        public IActionResult DisplayCommentsBook(int? idBook)
        {
            try
            {
                var comments = connectionToDba.comments.Where(comment => comment.BookId == idBook).ToList();

                if (comments != null && comments.Any())
                {
                    var commentsBook = comments.Select(comment => new
                    {
                        commentid = comment.OBJECTID,
                        description = comment.Description,
                        userid = comment.UserId,
                        usernameresponse = comment.User?.UserName // Use null-conditional operator to avoid null reference
                    }).ToList();



                    return Ok(commentsBook);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[Authorize]
        //[HttpPost("AddBookToCartShopping")]
        //public IActionResult AddBookToCart()
        //{
        //    cartItemCount++;

        //    return Ok(new { cartItemCount });
        //}

    }
}
