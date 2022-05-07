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
        private readonly ITwoFactorVerificationService _twoFactorService;

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
            _twoFactorService = DependencyService.Get<ITwoFactorVerificationService>();

            Login = UserSettings.Login;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled;
        }

        #region Public Methods

        public async Task<BaseResponce<User>> RegisterUser(string login, string password)
        {
            BaseResponce<User> registrationResponce = await _serviceClient.RegisterUser(login, password);

            if (registrationResponce.Content != null)
                ExecuteLogin(registrationResponce.Content);

            return registrationResponce;
        }

        public async Task<BaseResponce<User>> AuthenticateUserByPassword(string login, string password)
        {
            BaseResponce<User> authResponce = await _serviceClient.AuthenticateUserByPassword(login, password);

            if (authResponce.Content != null)
                ExecuteLogin(authResponce.Content);

            return authResponce;
        }

        public void Logout()
        {
            UserSettings.Id = 0;
            Login = UserSettings.Login = string.Empty;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = false;

            OnLogout?.Invoke();
        }

        public async Task<bool> ChangeTwoFactorStatus(bool isEnabled)
        {
            string code = string.Empty;
            if (isEnabled && !_twoFactorService.IsTwoFactorAuthInitialized(Login))
            {
                code = _twoFactorService.InitializeTwoFactorAuth(Login);
            }

            var changeStatusResponce = await _serviceClient.ChangeTwoFactorStatus(Login, code, isEnabled);

            if (changeStatusResponce.StatusCode.Equals(HttpStatusCode.NoContent) ||
                !changeStatusResponce.IsSuccess)
            {
                return false;
            }
            else
            {
                var userUpdated = changeStatusResponce.Content;
                IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = userUpdated.IsTwoFactorAuthenticationEnabled;
                return true;
            }
        }

        #endregion

        #region Private Methods

        private async void ExecuteLogin(User user)
        {
            UserSettings.Id = user.Id;
            Login = UserSettings.Login = user.Login;
            IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled;


            if(IsTwoFactorAuthenticationEnabled && !_twoFactorService.IsTwoFactorAuthInitialized(Login))
            {
                if(!await ChangeTwoFactorStatus(IsTwoFactorAuthenticationEnabled))
                {
                    IsTwoFactorAuthenticationEnabled = UserSettings.IsTwoFactorAuthenticationEnabled = false;
                }
            }

            OnLogin?.Invoke();
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
