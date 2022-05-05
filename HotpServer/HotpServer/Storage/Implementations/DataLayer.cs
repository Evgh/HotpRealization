using HotpServer.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace HotpServer.Storage.Implementations
{
    public class DataLayer : IDataLayer
    {
        readonly ApplicationContext _db;
        readonly object _syncObject = new object();

        public DataLayer(ApplicationContext applicationContext)
        {
            _db = applicationContext;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Login.Equals(login));
        }

        public async Task AddOrUpdateUserAsync(User user)
        {
            var existingUser = await GetUserByLoginAsync(user.Login);

            if(existingUser != null)
            {
                existingUser.Login = user.Login;
                existingUser.Password = user.Password;
                existingUser.IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled;
                existingUser.IsTwoFactorConfirmed = user.IsTwoFactorConfirmed;
                existingUser.HotpCounter = user.HotpCounter;
                existingUser.SecretKeyBase64 = user.SecretKeyBase64;
            }
            else
            {
                user.IsTwoFactorAuthenticationEnabled = false;
                user.IsTwoFactorConfirmed = false; 
                user.HotpCounter = 0;
                user.SecretKeyBase64 = string.Empty;

                _db.Users.Add(user);
            }

            await _db.SaveChangesAsync();
        }
    }
}
