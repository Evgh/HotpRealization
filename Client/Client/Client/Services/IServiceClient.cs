using Client.Models;
using Client.Models.Responces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IServiceClient
    {
        Task<BaseResponce<bool?>> GetIsTwoFactorConfirmed(string login);
        Task<BaseResponce<bool?>> GetIsTwoFactorEnabled(string login); 

        Task<BaseResponce<User>> PostRegisterUser(string login, string password);
        Task<BaseResponce<User>> PostAuthenticateUserByPassword(string login, string password);
        Task<bool> PostConfirmTwoFactorAuth(string login, string code);
    }
}
