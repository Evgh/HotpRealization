using Client.Models;
using Client.Models.Responces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IServiceClient _serviceClient;

        private string _login;
        private bool _isTwoFactorEnabled;

        public string Login 
        { 
            get => _login;
            set
            {
                _login = value;
                OnDataChanged?.Invoke();
            }
        }

        public bool IsTwoFactorAuthenticationEnabled
        {
            get => _isTwoFactorEnabled;
            set
            {
                _isTwoFactorEnabled = value;
                OnDataChanged?.Invoke();
            }
        }
        
        public event Action OnDataChanged;
        public event Action OnLogin;
        public event Action OnLogout;

        public AccountService()
        {
            _serviceClient = DependencyService.Get<IServiceClient>();

            Login = UserSettings.Login;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled;
        }

        #region Public Methods

        public async Task<BaseResponce<User>> RegisterUser(string login, string password)
        {
            BaseResponce<User> registrationResponce = await _serviceClient.PostRegisterUser(login, password);

            if (registrationResponce.Content != null)
                ExecuteLogin(registrationResponce.Content);

            return registrationResponce;
        }

        public void ExecuteLogin(User user)
        {
            UserSettings.Id = user.Id;
            Login = UserSettings.Login = user.Login;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled;

            OnLogin?.Invoke();
        }

        public void ExecuteLogout()
        {
            UserSettings.Id = 0;
            Login = UserSettings.Login = string.Empty;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = false;

            OnLogout?.Invoke();
        }

        public void ChangeTwoFactorStatus(bool isEnabled)
        {
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = isEnabled;
        }

        #endregion

        private static class UserSettings
        {
            public static int Id
            {
                get => Preferences.Get(nameof(Id), 0);
                set => Preferences.Set(nameof(Id), value);
            }

            public static string Login
            {
                get => Preferences.Get(nameof(Login), string.Empty);
                set => Preferences.Set(nameof(Login), value);
            }

            public static bool IsTwoFactorAuthenticationEnabled
            {
                get => Preferences.Get(nameof(IsTwoFactorAuthenticationEnabled), false);
                set => Preferences.Set(nameof(IsTwoFactorAuthenticationEnabled), value);
            }
        }
    }
}
