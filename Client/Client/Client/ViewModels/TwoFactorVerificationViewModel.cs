﻿using Client.Models;
using Client.Models.Responces;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Client.ViewModels
{
    [QueryProperty(nameof(Login), nameof(Login))]
    public class TwoFactorVerificationViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        public string Login { get; set; }

        public Command CheckIfVerifiedCommand { get; }

        public TwoFactorVerificationViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfVerifiedCommand = new Command(CheckIfVerified);

            _accountService.OnLogin += () => ErrorMessage = string.Empty;
        }

        private async void CheckIfVerified()
        {
            IsBusy = true;

            if(Utilities.VerificationHelper.TwoFactorParameter == null || string.IsNullOrEmpty(Login))
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorConfirmed(Login);

            if (!responce.IsSuccess
               || !responce.Content.HasValue)
            {
                ErrorMessage = COMMON_ERROR_MESSAGE;
            }
            else if (!responce.Content.Value)
            {
                ErrorMessage = "Verification wasn't provided. Please, execute verification in app Autorizer or repeat it you have already done in";

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(false);
            }
            else
            {
                ErrorMessage = string.Empty;
                await Shell.Current.GoToAsync("..");

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
            }

            IsBusy = false;
        }
    }
}
