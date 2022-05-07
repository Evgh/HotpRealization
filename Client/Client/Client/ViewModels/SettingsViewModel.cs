using Client.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        IAccountService _accountService;

        private bool _isTwoFactorAuthEnabled;

        public bool IsTwoFactorAuthEnabled
        {
            get => _isTwoFactorAuthEnabled;
            set => SetProperty(ref _isTwoFactorAuthEnabled, value);
        }

        public Command EnableTwoFactorAuthCommand { get; }
        public Command DisableTwoFactorAuthCommand { get; }

        public SettingsViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();

            EnableTwoFactorAuthCommand = new Command(EnableTwoFactorAuth);
            DisableTwoFactorAuthCommand = new Command(DisableTwoFactorAuth);

            OnAuthStatusChanged();
            ClearErrorMessage();

            _accountService.OnDataChanged += OnAuthStatusChanged;
            _accountService.OnLogin += OnAuthStatusChanged;
            _accountService.OnLogout += OnAuthStatusChanged;

            _accountService.OnLogin += ClearErrorMessage;
            _accountService.OnLogout += ClearErrorMessage;

            Title = "Settings";            
        }

        private async void EnableTwoFactorAuth()
        {
            await ChangeTwoFactorStatus(true);
        }

        private async void DisableTwoFactorAuth() 
        {
            await ChangeTwoFactorStatus(false);
        }

        private async Task ChangeTwoFactorStatus(bool isEnabled)
        {
            IsBusy = true;

            if (await _accountService.ChangeTwoFactorStatus(isEnabled))
                ClearErrorMessage();
            else
                ErrorMessage = COMMON_ERROR_MESSAGE;

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
        }
    }
}
