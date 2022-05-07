using Client.Models;
using Client.Models.Responces;
using Client.Services;
using Client.Views;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private const int MIN_PASSWORD_LENGTH = 1;

        IAccountService _accountService;

        private string _login;
        private string _password;
        private string _passwordRepeated;
        private string _loginErrorMessage;
        private string _passwordErrorMessage;
        private string _passwordRepeatedErrorMessage;

        public string Login
        {
            get => _login;
            set
            {
                SetProperty(ref _login, value);
                ValidateLogin();
                CreateAccountCommand.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ValidatePassword();
                ValidatePasswordRepeated();
                CreateAccountCommand.ChangeCanExecute();
            }
        }

        public string PasswordRepeated
        {
            get => _passwordRepeated;
            set
            {
                SetProperty(ref _passwordRepeated, value);
                ValidatePasswordRepeated();
                CreateAccountCommand.ChangeCanExecute();
            }
        }

        public string LoginErrorMessage
        {
            get => _loginErrorMessage;
            set => SetProperty(ref _loginErrorMessage, value);
        }

        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }

        public string PasswordRepeatedErrorMessage
        {
            get => _passwordRepeatedErrorMessage;
            set => SetProperty(ref _passwordRepeatedErrorMessage, value);
        }

        public Command CreateAccountCommand { get; }
        public Command GoBackCommand { get; }

        public RegistrationViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();

            CreateAccountCommand = new Command(ExecuteCreateAccount);
            GoBackCommand = new Command(ExecuteGoBack);
        }

        private async void ExecuteCreateAccount()
        {
            if (ValidateUserInput())
            {
                IsBusy = true;
                var responce = await _accountService.RegisterUser(Login.Trim(), Password.Trim());
                IsBusy = false;

                if (ValidateRegistrationResponce(responce))
                {
                    ClearFields();
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }
        }
        private async void ExecuteGoBack()
        {
            ClearFields();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void ClearFields()
        {
            Login = string.Empty;
            Password = string.Empty;
            PasswordRepeated = string.Empty;

            LoginErrorMessage = string.Empty;
            PasswordErrorMessage = string.Empty;
            PasswordRepeatedErrorMessage = string.Empty;

            ErrorMessage = string.Empty;
        }

        private bool ValidateUserInput()
        {
            return ValidateLogin() && ValidatePassword() && ValidatePasswordRepeated();
        }

        private bool ValidateLogin()
        {
            bool valid = !string.IsNullOrEmpty(Login); 
            LoginErrorMessage = !valid ? "Login can't be empty" : string.Empty;

            return valid;
        }

        private bool ValidatePassword()
        {
            bool valid = Password != null && !(Password.Length < MIN_PASSWORD_LENGTH);
            PasswordErrorMessage = !valid ? $"Password should contain minimum {MIN_PASSWORD_LENGTH} digits" : string.Empty;

            return valid;
        }
        private bool ValidatePasswordRepeated()
        {
            bool valid = string.Compare(Password, PasswordRepeated).Equals(0);
            PasswordRepeatedErrorMessage = !valid ? $"Passwords are different" : string.Empty;

            return valid;
        }

        private bool ValidateRegistrationResponce(BaseResponce<User> registrationResponce)
        {
            if (registrationResponce.StatusCode.Equals(HttpStatusCode.NoContent))
            {
                ErrorMessage = "Login is already taken. Pleace, create another one";
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
