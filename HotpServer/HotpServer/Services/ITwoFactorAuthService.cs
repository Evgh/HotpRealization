using HotpServer.Storage.Models;

namespace HotpServer.Services
{
    public interface ITwoFactorAuthService
    {
        Task<bool> ConfirmTwoFactorAuth(string login, string hotpCode);
        Task<bool> IsTwoFactorConfirmed(string login);
        Task<User> ChangeTwoFactorStatus(string login, string password, bool isEnabled, string secretKey = "");
        Task<bool> IsTwoFactorAuthEnabled(string login);
    }
}
