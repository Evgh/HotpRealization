using Authorizer.Models;
using Authorizer.Models.Responces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authorizer.Services
{
    public interface IAccountService
    {
        event Action OnDataChanged;
        event Action OnLogin;
        event Action OnLogout;

        string Login { get; set; }
        bool IsTwoFactorAuthenticationEnabled { get; set; }

        Task<BaseResponce<User>> RegisterUser(string login, string password);
        Task<BaseResponce<User>> AuthenticateUserByPassword(string login, string password);
        void Logout();
        Task<bool> ChangeTwoFactorStatus(bool isEnabled);
    }
}
