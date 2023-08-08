using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DemoLibrary.Models.Repository
{
    public interface IAccountManager
    {

        // public async Task Signup(SignUpModel signUpModel);
        Task<IdentityResult> SignUp(SignUpModel signUpModel);
        Task<string> AsyncSignIn(SigninModel signinModel);
        Task<User> FindUserByEmail(string email);
        Task SignOutAsync();
        Task<string> GetTokenAsync(string tokenAccess);
    }

}
