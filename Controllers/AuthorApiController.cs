using DemoLibrary.Models;
using DemoLibrary.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorApiController : ControllerBase
    {
        // GET: api/<AuthorApiController>
        private readonly DemoBooksDbContext connectionToDba;

        private readonly IBookLibrary<Author> Iauthor;



        public AuthorApiController(DemoBooksDbContext connectionToDba, IBookLibrary<Author> authorLirary)
        {

            this.connectionToDba = connectionToDba;
            this.Iauthor = authorLirary;
        }

        [HttpGet("GetAllAuthors")]
        public IList<Author> GetAuthors()
        {
            return Iauthor.list().ToList();
        }

        // GET api/<AuthorApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthorApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthorApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
