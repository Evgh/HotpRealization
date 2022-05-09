using Client.Models;
using Client.Models.Responces;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class TwoFactorConfirmChangesViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        public bool IsTwoFactorEnabled => _accountService.IsTwoFactorAuthenticationEnabled;

        public Command CheckIfVerifiedCommand { get; }

        public TwoFactorConfirmChangesViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfVerifiedCommand = new Command(CheckIfVerified);

            _accountService.OnLogin += () => ErrorMessage = string.Empty;
        }

        private async void CheckIfVerified()
        {
            IsBusy = true;

            if(Utilities.VerificationHelper.TwoFactorParameter == null)
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorEnabled(_accountService.Login);

            if (!responce.IsSuccess
               || !responce.Content.HasValue)
            {
                ErrorMessage = COMMON_ERROR_MESSAGE;
            }
            else if (responce.Content.Value.Equals(_accountService.IsTwoFactorAuthenticationEnabled))
            {
                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(false);
                ErrorMessage = "Two factor authentication status in authorizer wasn't changed. Please, try again";
            }
            else
            {
                ErrorMessage = string.Empty;
                await Shell.Current.GoToAsync("..");

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
                Utilities.VerificationHelper.TwoFactorParameter = null;                
            }

            IsBusy = false;
        }
    }
}
