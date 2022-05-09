using Client.Models;
using Client.Models.Responces;
using Client.Services;
using Client.Views;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        private string _login;
        private string _password;

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public Command LoginCommand { get; }
        public Command RegistrationCommand { get; }

        public LoginViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

            LoginCommand = new Command(ExecuteLogin);
            RegistrationCommand = new Command(ExecuteRegistration);

            this.PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        private async void ExecuteLogin()
        {
            if (ValidateForLogin())
            {
                IsBusy = true;
                var loginResponce = await _serviceClient.PostAuthenticateUserByPassword(Login.Trim(), Password.Trim());
                IsBusy = false;

                if (ValidateLoginResponce(loginResponce))
                {
                    ClearFields();

                    if (loginResponce.Content.IsTwoFactorAuthenticationEnabled)
                    {
                        var authParameter = new TwoFactorAuthParameter();
                        authParameter.OnAuthExecuted += (isVerified) => TwoFactorAuthCallback(isVerified, loginResponce.Content);

                        Utilities.VerificationHelper.TwoFactorParameter = authParameter;

                        await Shell.Current.GoToAsync($"/{nameof(TwoFactorVerificationPage)}?{nameof(TwoFactorVerificationViewModel.Login)}={loginResponce.Content.Login}");
                    }
                    else
                    {
                        await ExecuteSuccessfulLogin(loginResponce.Content);
                    }
                }
            }
        }

        private async void TwoFactorAuthCallback(bool isVerified, User user)
        {
            if (isVerified)
            {
                var isTwoFactorStillEnabled = await _serviceClient.GetIsTwoFactorEnabled(_accountService.Login);
                user.IsTwoFactorAuthenticationEnabled = (isTwoFactorStillEnabled?.Content.HasValue ?? false) ? (bool)isTwoFactorStillEnabled?.Content.Value : user.IsTwoFactorAuthenticationEnabled;

                Utilities.VerificationHelper.TwoFactorParameter = null;
                ExecuteSuccessfulLogin(user).GetAwaiter(); 
            }
            else
            {
                _accountService.ExecuteLogout();
            }
        }

        private async Task ExecuteSuccessfulLogin(User user)
        {
            _accountService.ExecuteLogin(user);

            await Shell.Current.GoToAsync($"..");
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private async void ExecuteRegistration()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
        }

        private bool ValidateForLogin()
        {
            bool isValid = !string.IsNullOrEmpty(Login) &&
                           !string.IsNullOrEmpty(Password);

            ErrorMessage = !isValid ? "Login and password could not be empty" : string.Empty;

            return isValid;
        }

        private void ClearFields()
        {
            Login = string.Empty;
            Password = string.Empty;

            ErrorMessage = string.Empty;
        }

        private bool ValidateLoginResponce(BaseResponce<User> registrationResponce)
        {
            if (registrationResponce.StatusCode.Equals(HttpStatusCode.NoContent))
            {
                ErrorMessage = "Sorry, login or password are incorrect";
                return false;
            }
            else if (!registrationResponce.IsSuccess)
            {
                ErrorMessage = COMMON_ERROR_MESSAGE;
                return false;
            }
            else
            {
                ErrorMessage = string.Empty;
                return true;
            }
        }
    }
}
