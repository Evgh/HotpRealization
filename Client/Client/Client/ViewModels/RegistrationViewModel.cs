using Client.Models;
using Client.Models.Responces;
using Client.Views;
using System.Net;
using Xamarin.Forms;

namespace Client.ViewModels
{ 
    [QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
    public class RegistrationViewModel : BaseViewModel
    {
        private const int MIN_PASSWORD_LENGTH = 6;

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
            }
        }

        public string PasswordRepeated
        {
            get => _passwordRepeated;
            set
            {
                SetProperty(ref _passwordRepeated, value);
                ValidatePasswordRepeated();
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

        public RegistrationViewModel()
        {
            CreateAccountCommand = new Command(ExecuteCreateAccount);
        }

        protected override void OnAccountDataChanged()
        {
            base.OnAccountDataChanged();

            ClearFields();
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

        //private async void ExecuteGoBack()
        //{
        //    ClearFields();
        //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        //}

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
            PasswordRepeatedErrorMessage = !valid ? $"Passwords aren't match" : string.Empty;

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
