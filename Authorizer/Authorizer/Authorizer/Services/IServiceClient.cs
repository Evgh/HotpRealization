using Authorizer.Models;
using Authorizer.Models.Responces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authorizer.Services
{
    public interface IServiceClient
    {
        Task<BaseResponce<User>> PostRegisterUser(string login, string password);
        Task<BaseResponce<User>> PostAuthenticateUserByPassword(string login, string password);
        Task<BaseResponce<User>> PostChangeTwoFactorStatus(string login, string keyBase64, bool isEnabled);
        Task<bool> PostConfirmTwoFactorAuth(string login, string code);
    }
}
