using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WEB_API_HRM.Data;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<LoginResponse> SignInAsync(SignInModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.password);
            if (user == null || !passwordValid)
            {
                return null; 
            }

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id) 
    };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse
            {
                Token = tokenString,
                UserId = user.Id 
            };
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var existingUserByName = await userManager.FindByNameAsync(model.Username);
            if (existingUserByName != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists in the system." });
            }


            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(model.Email))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email format is invalid." });
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == model.RoleCode);
                if (role != null)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
                }
            }

            return result;
        }
        //public async Task<string> GetLastFirstName(string idUser)
        //{
        //    var user = await userManager.FindByIdAsync(idUser);
        //    if (user == null)
        //    {
        //        return null; 
        //    }
        //    string lastName = string.IsNullOrEmpty(user.LastName) ? "" : user.LastName;
        //    string firstName = string.IsNullOrEmpty(user.FirstName) ? "" : user.FirstName;
        //    return $"{lastName} {firstName}".Trim(); 
        //}
    }
}