using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<LoginResponse> SignInAsync(SignInModel model);
       // public Task<string> GetLastFirstName(string idUser);
    }
}
