using APExittest.Bussiness.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace APExittest.Business.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpModel userModel);
        Task<string> PasswordSignInAsync(SignInModel signInModel);
        Task SignOutAsync();
    }
}