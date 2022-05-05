using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Authorizer.Services.Implementations
{
    public class TwoFactorVerificationService : ITwoFactorVerificationService
    {
        #region Public Methods

        public bool IsTwoFactorAuthInitialized(string login)
        {
            string userKey = TwoFactorAuthSettings.GetUserKey(login);
            return !userKey.Equals(string.Empty);
        }

        public string InitializeTwoFactorAuth(string login)
        {
            if (!IsTwoFactorAuthInitialized(login))
            {
                byte[] key = KeyGeneration.GenerateRandomKey();
                string keyBase64 = Convert.ToBase64String(key);

                TwoFactorAuthSettings.SetUserKey(login, keyBase64);
                TwoFactorAuthSettings.SetUserCounter(login, 0);

                return keyBase64;
            }
            return null;
        }

        public string GenerareCode(string login)
        {
            string keyBase64 = TwoFactorAuthSettings.GetUserKey(login);
            
            if(keyBase64 != null && !string.IsNullOrEmpty(keyBase64))
            {
                byte[] key = Convert.FromBase64String(keyBase64);
                int counter = TwoFactorAuthSettings.GetUserCounter(login);

                Hotp hotp = new Hotp(key);
                string code = hotp.ComputeHOTP(counter);

                counter++;
                TwoFactorAuthSettings.SetUserCounter(login, counter);

                return code;
            }

            return string.Empty;
        }

        #endregion

        private static class TwoFactorAuthSettings
        {
            public static string GetUserKey(string login)
            {
                return Preferences.Get($"{login}-KeyBase64", string.Empty);
            }

            public static void SetUserKey(string login, string value)
            {
                Preferences.Set($"{login}-KeyBase64", value);
            }


            public static int GetUserCounter(string login)
            {
                return Preferences.Get($"{login}-Counter", 0);
            }

            public static void SetUserCounter(string login, int value)
            {
                Preferences.Set($"{login}-Counter", value);
            }
        }
    }
}
