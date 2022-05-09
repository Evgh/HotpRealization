using Client.Models;
using Client.Services;
using Client.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private const string ENABLED_BUTTON_TITLE = "Disable";
        private const string ENABLED_DESCRIPTION = "Two Factor Authentication is enabled";
        private const string DISABLED_BUTTON_TITLE = "Enable";
        private const string DISABLED_DESCRIPTION = "Two Factor Authentication is disabled";


        IAccountService _accountService;

        private bool _isTwoFactorAuthEnabled;

        public bool IsTwoFactorAuthEnabled
        {
            get => _isTwoFactorAuthEnabled;
            set
            {
                SetProperty(ref _isTwoFactorAuthEnabled, value);

                OnPropertyChanged(nameof(ButtonTitle));
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ButtonTitle => IsTwoFactorAuthEnabled ? ENABLED_BUTTON_TITLE : DISABLED_BUTTON_TITLE;
        public string Description => IsTwoFactorAuthEnabled ? ENABLED_DESCRIPTION : DISABLED_DESCRIPTION;

        public Command EnableDisableTwoFactorAuthCommand { get; }

        public SettingsViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();

            EnableDisableTwoFactorAuthCommand = new Command(EnableDisableTwoFactorAuth);

            OnAuthStatusChanged();
            ClearErrorMessage();

            _accountService.OnDataChanged += OnAuthStatusChanged;
            _accountService.OnLogin += OnAuthStatusChanged;
            _accountService.OnLogout += OnAuthStatusChanged;

            _accountService.OnLogin += ClearErrorMessage;
            _accountService.OnLogout += ClearErrorMessage;

            Title = "Settings";            
        }

        private async void EnableDisableTwoFactorAuth()
        {
            var authParameter = new TwoFactorAuthParameter();
            authParameter.OnAuthExecuted += ChangeTwoFactorStatus;
            Utilities.VerificationHelper.TwoFactorParameter = authParameter;

            await Shell.Current.GoToAsync($"/{nameof(TwoFactorConfirmChangesPage)}");
        }

        private void ChangeTwoFactorStatus(bool wasVerified)
        {
            IsBusy = true;

            if (wasVerified)
            {
                _accountService.ChangeTwoFactorStatus(!_accountService.IsTwoFactorAuthenticationEnabled);
                ClearErrorMessage();
            }
            else
            {
                ErrorMessage = VERIFICATION_ERROR_MESSAGE;
            }

            IsBusy = false;
        }

        private async void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }

        private async void OnAuthStatusChanged()
        {
            _isTwoFactorAuthEnabled = _accountService.IsTwoFactorAuthenticationEnabled;
            OnPropertyChanged(nameof(IsTwoFactorAuthEnabled));
            OnPropertyChanged(nameof(ButtonTitle));
            OnPropertyChanged(nameof(Description));
        }
    }
}
