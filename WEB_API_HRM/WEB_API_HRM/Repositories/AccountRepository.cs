using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly HRMContext _context;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager, HRMContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this._context = context;
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
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                Token = refreshToken,
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(10)
            };

            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            return new LoginResponse
            {
                AccessToken = tokenString,
                RefreshToken = refreshToken,
                UserId = user.Id 
            };
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var existingUserByName = await userManager.FindByNameAsync(model.Username);
            if (existingUserByName != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateUser",
                    Description = "Username already exists in the system."
                });
                //return IdentityResult.Failed(new IdentityError { Description = "Username already exists in the system." });
            }


            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(model.Email))
            {
                if (!IsValidEmail(model.Email))
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "InvalidEmail",
                        Description = "Email format is invalid."
                    });
                }
                //return IdentityResult.Failed(new IdentityError { Description = "Email format is invalid." });
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
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "RoleNotFound",
                        Description = "Role not found."
                    });
                }
            }

            return result;
        }
        public async Task<LoginResponse> RenewTokenAsync(string accessToken, string refreshToken)
        {
            // Giải mã access token (không kiểm tra chữ ký, chỉ để lấy claims)
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false // Cho phép token hết hạn
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (userId == null || jti == null)
            {
                throw new SecurityTokenException("Invalid access token");
            }

            // Kiểm tra refresh token
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);

            if (storedToken == null)
            {
                throw new SecurityTokenException("Refresh token does not exist");
            }

            if (storedToken.IsUsed)
            {
                throw new SecurityTokenException("Refresh token has already been used");
            }

            if (storedToken.IsRevoked)
            {
                throw new SecurityTokenException("Refresh token has been revoked");
            }

            if (storedToken.ExpiredAt < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token has expired");
            }

            if (storedToken.JwtId != jti)
            {
                throw new SecurityTokenException("Refresh token does not match JWT");
            }

            // Đánh dấu refresh token cũ là đã sử dụng
            storedToken.IsUsed = true;
            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);

            // Tạo access token mới
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new SecurityTokenException("User not found");
            }

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var newAccessToken = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

            // Tạo refresh token mới
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = newAccessToken.Id,
                Token = newRefreshToken,
                UserId = userId,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(10)
            };

            await _context.RefreshTokens.AddAsync(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                AccessToken = newAccessTokenString,
                RefreshToken = newRefreshToken,
                UserId = userId
            };
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public async Task<InfoAccountRes> GetInfoAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return null;
            }
             
            var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == user.UserName);
            var infoAccount = new InfoAccountRes();
            if(personel == null)
            {
                infoAccount.AvatarPath = string.Empty;
            }
            else
            {
                infoAccount.AvatarPath = personel.AvatarPath ?? string.Empty;
            }
            infoAccount.UserName = user.UserName;
            return infoAccount;
        }
    }
}