using Client.Models;
using Client.Models.Responces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IAccountService
    {
        event Action OnDataChanged;
        event Action OnLogin;
        event Action OnLogout;

        string Login { get; set; }
        bool IsTwoFactorAuthenticationEnabled { get; set; }

        Task<BaseResponce<User>> RegisterUser(string login, string password);
        void ExecuteLogin(User user);
        void ExecuteLogout();
        void ChangeTwoFactorStatus(bool isEnabled);
    }
}
