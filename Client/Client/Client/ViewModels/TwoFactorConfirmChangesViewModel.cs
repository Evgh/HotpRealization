using Client.Models.Responces;
using Client.Services;
using Xamarin.Forms;

namespace Client.ViewModels
{
    [QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
    public class TwoFactorConfirmChangesViewModel : BaseViewModel
    {
        private readonly IServiceClient _serviceClient;

        public bool IsTwoFactorEnabled => _accountService.IsTwoFactorAuthenticationEnabled;

        public Command CheckIfChangedCommand { get; }

        public TwoFactorConfirmChangesViewModel()
        {
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfChangedCommand = new Command(CheckIfChanged);
        }

        private async void CheckIfChanged()
        {
            IsBusy = true;

            if(Utilities.VerificationHelper.TwoFactorParameter == null)
            {
                OnBackButtonPresed();
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
                OnBackButtonPresed();

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
                Utilities.VerificationHelper.TwoFactorParameter = null;                
            }

            IsBusy = false;
        }
    }
}
