using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoLibrary.Models;
using DemoLibrary.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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

        public BookApiController(DemoBooksDbContext connectionToDba,IHttpContextAccessor httpContext, IBookLibrary<Book> bookLirary, IBookLibrary<Author> Iauthor, IHostingEnvironment _hosting)
        {

            this.connectionToDba = connectionToDba;
            this.Ibook = bookLirary;
            this.Iauthor = Iauthor;
            this.hosting = _hosting;
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
                if (HttpContext.Request.Form.Files.Count > 0) {
                   // string folder = "PublishedBooks/bookImg";
                    //string fileName = bookDto.imgBook.FileName;
                    //string filePath = Path.Combine(folder, fileName);
                    //bookDto.fileUrl = filePath;
                    var fileUploaded = HttpContext.Request.Form.Files[0];
                    //await bookDto.imgBook.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    var baseUrl = httpContextAccessor.HttpContext.Request.Scheme+"://"+httpContextAccessor.HttpContext.Request.Host+ httpContextAccessor.HttpContext.Request.PathBase;
                    //    var filePath = Path.Combine(hosting.WebRootPath, "UploadsBooks", fileUploaded.FileName);
                    // var filePath = @"G:\Portfolio Projects\DemoLib\FrontEnd\FrontEnd\LibraryBooks\LibraryBooks\src\assets\imgPublished\" + fileUploaded.FileName;
                    // Retrieve the latest OBJECTID from the database
                    int latestObjectId = Ibook.GetLatestObjectId();
                    

                    // Increment the latestObjectId by 1
                    int newObjectId = latestObjectId + 1;


                    // Create the directory path using the newObjectId
                     var directoryPath = Path.Combine(@"D:\Portfolio Projects\DemoLib\FrontEnd\FrontEnd\LibraryBooks\LibraryBooks\src\assets\imgPublished", newObjectId.ToString());
                    //var directoryPath = Path.Combine(@"G:\Portfolio Projects\DemoLib\FrontEnd\FrontEnd\LibraryBooks\LibraryBooks\src\assets\imgPublished", newObjectId.ToString());

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, fileUploaded.FileName);

                    bookDto.fileUrl = filePath;


                    await bookDto.imgBook.CopyToAsync(new FileStream(filePath, FileMode.Create));


                }
                // Create a new book entity
                var book = new Book
                {
                    TitleBook = bookDto.TitleBook,
                    DescriptionBook = bookDto.DescriptionBook,
                    fileUrl = bookDto.fileUrl,
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
                if (bookDto.author!=null)
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

    }
}
