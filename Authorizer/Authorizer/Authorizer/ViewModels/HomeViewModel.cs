using Authorizer.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Authorizer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;
        private readonly ITwoFactorVerificationService _twoFactorService;

        public string WelcomeTitle
        {
            get => $"Hello, {_accountService.Login}!";
        }

        public bool TwoFactorEnabled
        {
            get => _accountService.IsTwoFactorAuthenticationEnabled;
        }

        public Command SendPermissionCommand { get; }

        public HomeViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();
            _twoFactorService = DependencyService.Get<ITwoFactorVerificationService>();

            SendPermissionCommand = new Command(ExecuteSendPermission);
            Title = "Home";

            OnUserDataChanged();
            _accountService.OnDataChanged += OnUserDataChanged;
            _accountService.OnDataChanged += () => ErrorMessage = string.Empty;
        }

        private void OnUserDataChanged()
        {
            OnPropertyChanged(nameof(WelcomeTitle));
            OnPropertyChanged(nameof(TwoFactorEnabled));
        }

        private async void ExecuteSendPermission()
        {
            if (!_accountService.IsTwoFactorAuthenticationEnabled)
                return;

            IsBusy = true;
            var hotpCode = _twoFactorService.GenerareCode(_accountService.Login);

            if (await _serviceClient.ConfirmTwoFactorAuth(_accountService.Login, hotpCode))
            {
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = COMMON_ERROR_MESSAGE;
            }

            IsBusy = false;
        }
    }
}