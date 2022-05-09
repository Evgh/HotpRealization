using Client.Models.Responces;
using Client.Services;
using Xamarin.Forms;

namespace Client.ViewModels
{
    [QueryProperty(nameof(Login), nameof(Login))]
    [QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
    public class TwoFactorVerificationViewModel : BaseViewModel
    {
        private readonly IServiceClient _serviceClient;

        public string Login { get; set; }

        public Command CheckIfVerifiedCommand { get; }

        public TwoFactorVerificationViewModel()
        {
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfVerifiedCommand = new Command(CheckIfVerified);
        }

        private async void CheckIfVerified()
        {
            IsBusy = true;

            if(Utilities.VerificationHelper.TwoFactorParameter == null || string.IsNullOrEmpty(Login))
            {
                await OnBackButtonPresed();
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
                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
            }

            IsBusy = false;
        }
    }
}
