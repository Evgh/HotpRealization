namespace Client.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string WelcomeTitle => $"Hello, {_accountService.Login}!";

        public string Description => _accountService.IsTwoFactorAuthenticationEnabled ?
                                        "Two-factor authentication is enabled. Go to Settings to disable it." :
                                        "Two-factor authentication is disabled. Go to Settings to enable it.";

        public HomeViewModel()
        {
            Title = "Home";

            OnAccountDataChanged();
        }

        protected override void OnAccountDataChanged()
        {
            base.OnAccountDataChanged();

            OnPropertyChanged(nameof(WelcomeTitle));
            OnPropertyChanged(nameof(Description));
        }
    }
}