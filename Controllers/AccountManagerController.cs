using DemoLibrary.Models;
using DemoLibrary.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {

        // GET: api/<AccounManagerController>

        private readonly IAccountManager accountManager;
        private readonly DemoBooksDbContext connectionToDba;

        public AccountManagerController(IAccountManager accountManager, DemoBooksDbContext connectionToDba)
        {

            this.accountManager = accountManager;
            this.connectionToDba = connectionToDba;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAccount([FromBody] SignUpModel signUpModel)
        {
            var result = await accountManager.SignUp(signUpModel);

            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        //works
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAccount([FromBody] SigninModel signinModel)
        {


            var result = await accountManager.AsyncSignIn(signinModel);


            //if (string.IsNullOrEmpty(result))
            //{
            //    return Unauthorized();
            //}

            var user = await accountManager.FindUserByEmail(signinModel.Email);

            var response = new
            {
                userlogged = user.UserName,
                tokensessionlogged = result,
                firstname = user.FirstName,
                lastname = user.LastName,
                email = user.Email
            };


            return Ok(response);
        }


        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAccount()
        {
            await accountManager.SignOutAsync();
            return Ok();
        }

        //for test
        //[HttpPost("signin")]
        //public async Task<IActionResult> SignInAccount([FromBody] SigninModel signinModel)
        //{

        //    var result = await accountManager.AsyncSignIn(signinModel);

        //    if (string.IsNullOrEmpty(result))
        //    {
        //        return Unauthorized();
        //    }

        //    // Include the token in the response header       

        //    return Ok();
        //}


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccounManagerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccounManagerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccounManagerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccounManagerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
