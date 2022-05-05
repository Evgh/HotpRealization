using Authorizer.Models;
using Authorizer.Models.Responces;
using Authorizer.Services;
using Authorizer.Views;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace Authorizer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

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
                var loginResponce = await _accountService.AuthenticateUserByPassword(Login.Trim(), Password.Trim());
                IsBusy = false;

                if (ValidateLoginResponce(loginResponce))
                {
                    ClearFields();
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }
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
                ErrorMessage = "Something went wrong. Pleace, try later";
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
