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
        Task<BaseResponce<User>> RegisterUser(string login, string password);
        Task<BaseResponce<User>> AuthenticateUserByPassword(string login, string password);
        Task<BaseResponce<User>> ChangeTwoFactorStatus(string login, string keyBase64, bool isEnabled);
        Task<bool> ConfirmTwoFactorAuth(string login, string code);
    }
}
