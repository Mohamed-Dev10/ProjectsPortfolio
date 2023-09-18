using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary.Models.Repository
{
    public class ImplAccountManager : IAccountManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuation;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImplAccountManager(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuation, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuation = configuation;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> SignUp(SignUpModel signUpModel)
        {

            var user = new User
            {

                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                UserName = signUpModel.username,
                Email = signUpModel.email
            };
            return await _userManager.CreateAsync(user, signUpModel.password);

        }

        public async Task<string> AsyncSignIn(SigninModel signinModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signinModel.Email, signinModel.Password, false, false);


            if (!result.Succeeded)
            {

                return "";

            }

            var authClaim = new List<Claim> {
            new Claim(ClaimTypes.Name,signinModel.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };
            var authSigInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuation["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuation["JWT:ValidIssuer"],
                audience: _configuation["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigInKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GetTokenAsync(string tokenAccess)
        {
            string token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            return token;
        }

        public async Task<string> GenerateJwtTokenAsync(SigninModel signinModel)
        {
            var claims = new List<Claim>
    {
        // The username or identifier for the user
        new Claim(ClaimTypes.Email, signinModel.Email),   // The email address of the user
        // Add other claims as needed
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuation["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuation["JWT:ValidIssuer"],
                audience: _configuation["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Set the token expiration as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }
}
