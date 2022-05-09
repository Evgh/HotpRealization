using Client.Models;
using Client.Services;
using Client.Views;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private const string ENABLED_BUTTON_TITLE = "Disable";
        private const string ENABLED_DESCRIPTION = "Two Factor Authentication is enabled";
        private const string DISABLED_BUTTON_TITLE = "Enable";
        private const string DISABLED_DESCRIPTION = "Two Factor Authentication is disabled";

        public string ButtonTitle => IsTwoFactorAuthEnabled ? ENABLED_BUTTON_TITLE : DISABLED_BUTTON_TITLE;
        public string Description => IsTwoFactorAuthEnabled ? ENABLED_DESCRIPTION : DISABLED_DESCRIPTION;

        public Command EnableDisableTwoFactorAuthCommand { get; }

        public SettingsViewModel()
        {
            EnableDisableTwoFactorAuthCommand = new Command(EnableDisableTwoFactorAuth);

            Title = "Settings";
            OnAccountDataChanged();
        }

        protected override void OnAccountDataChanged()
        {
            base.OnAccountDataChanged();

            OnPropertyChanged(nameof(IsTwoFactorAuthEnabled));
            OnPropertyChanged(nameof(ButtonTitle));
            OnPropertyChanged(nameof(Description));
        }

        private async void EnableDisableTwoFactorAuth()
        {
            var authParameter = new TwoFactorAuthParameter();
            authParameter.OnAuthExecuted += ChangeTwoFactorStatus;
            Utilities.VerificationHelper.TwoFactorParameter = authParameter;

            await Shell.Current.GoToAsync($"//{nameof(TwoFactorConfirmChangesPage)}?{nameof(PreviousPage)}={nameof(SettingsPage)}");
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
    }
}
