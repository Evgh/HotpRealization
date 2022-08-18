using HotpServer.Exceptions;
using HotpServer.Storage;
using HotpServer.Storage.Models;
using OtpNet;

namespace HotpServer.Services.Implementations
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private readonly IDataLayer _dataLayer;

        public TwoFactorAuthService(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        #region Public methods
        public async Task<bool> ConfirmTwoFactorAuth(string login, string hotpCode)
        {
            User user = await GetUserByLoginFromDdAsync(login);
            Hotp hotp = new Hotp(Convert.FromBase64String(user.SecretKeyBase64));

            var verificationResult = true; hotp.VerifyHotp(hotpCode, user.HotpCounter);
            if (verificationResult)
                user.HotpCounter++;

            user.IsTwoFactorConfirmed = verificationResult; // became true when confirmed, became false after next request to the field
            await _dataLayer.AddOrUpdateUserAsync(user);

            return verificationResult;
        }

        public async Task<bool> IsTwoFactorConfirmed(string login)
        {
            User user = await GetUserByLoginFromDdAsync(login);

            if (user.IsTwoFactorAuthenticationEnabled)
            {
                if (user.IsTwoFactorConfirmed)
                {
                    user.IsTwoFactorConfirmed = false; // became true when confirmed, became false after next request to the field
                    await _dataLayer.AddOrUpdateUserAsync(user);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<User> ChangeTwoFactorStatus(string login, string password, bool isEnabled, string secretKey = "")
        {
            User user = await GetUserByLoginFromDdAsync(login);

            if (!user.Password.Equals(password))
                throw new InvalidCredentialsException();

            if (user.IsTwoFactorAuthenticationEnabled ^ isEnabled)
            {
                if (!string.IsNullOrEmpty(secretKey))
                {
                    user.SecretKeyBase64 = secretKey;
                    user.HotpCounter = 0;
                    user.IsTwoFactorConfirmed = false;
                }

                if (user.IsTwoFactorAuthenticationEnabled || !string.IsNullOrEmpty(user.SecretKeyBase64)) // 1) enabled or 2) disabled and has secret key
                {
                    user.IsTwoFactorAuthenticationEnabled = isEnabled;
                }

                if (!user.IsTwoFactorAuthenticationEnabled)
                    user.IsTwoFactorConfirmed = false;

                await _dataLayer.AddOrUpdateUserAsync(user);
            }

            return user;
        }

        public async Task<bool> IsTwoFactorAuthEnabled(string login)
        {
            User user = await GetUserByLoginFromDdAsync(login);
            return user.IsTwoFactorAuthenticationEnabled;
        }

        #endregion

        #region Private
        private async Task<User> GetUserByLoginFromDdAsync(string login)
        {
            User user = await _dataLayer.GetUserByLoginAsync(login);

            if (user == null)
                throw new NoSuchUserException();

            return user;
        }
        #endregion
    }
}
