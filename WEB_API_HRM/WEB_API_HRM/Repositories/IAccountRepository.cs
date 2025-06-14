﻿using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<LoginResponse> SignInAsync(SignInModel model);
        Task<LoginResponse> RenewTokenAsync(string accessToken, string refreshToken);
        Task<InfoAccountRes> GetInfoAccount(string userId);
    }
}
