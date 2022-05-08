using Client.Models;
using Client.Models.Responces;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class TwoFactorVerificationViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        public Command CheckIfVerifiedCommand { get; }

        public TwoFactorVerificationViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfVerifiedCommand = new Command(CheckIfVerified);

            _accountService.OnLogin += () => ErrorMessage = string.Empty;

            ErrorMessage = "test";
        }

        private async void CheckIfVerified()
        {
            if(Utilities.VerificationHelper.TwoFactorParameter == null)
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorConfirmed(_accountService.Login);

            if (!responce.IsSuccess
               || !responce.Content.HasValue)
            {
                ErrorMessage = COMMON_ERROR_MESSAGE;
            }
            else if (!responce.Content.Value)
            {
                ErrorMessage = "Verification wasn't provided. Please, execute verification in app Autorizer or repeat it you have already done in";
            }
            else
            {
                ErrorMessage = string.Empty;

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
                Utilities.VerificationHelper.TwoFactorParameter = null;

                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
