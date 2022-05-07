using Client.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        public string WelcomeTitle
        {
            get => $"Hello, {_accountService.Login}!";
        }

        public bool TwoFactorEnabled
        {
            get => _accountService.IsTwoFactorAuthenticationEnabled;
        }

        public HomeViewModel()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

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
    }
}