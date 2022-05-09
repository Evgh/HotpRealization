using Client.Models.Responces;
using Client.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.ViewModels
{
    [QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
    public class TwoFactorConfirmChangesViewModel : BaseViewModel
    {
        private readonly IServiceClient _serviceClient;

        public string Description => _accountService.IsTwoFactorAuthenticationEnabled ? "Please, disable two factor authentication in app Authorizer" : "Please, enable two factor authentication in app Authorizer";

        public Command CheckIfChangedCommand { get; }

        public TwoFactorConfirmChangesViewModel()
        {
            _serviceClient = DependencyService.Get<IServiceClient>();

            CheckIfChangedCommand = new Command(CheckIfChanged);
        }

        protected override void OnAccountDataChanged()
        {
            base.OnAccountDataChanged();

            OnPropertyChanged(nameof(Description));
        }

        protected async override Task OnBackButtonPresed()
        {
            IsBusy = true;
            BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorEnabled(_accountService.Login);
            IsBusy = false;

            if (responce.IsSuccess && responce.Content.HasValue)
            {
                if (_accountService.IsTwoFactorAuthenticationEnabled ^ responce.Content.Value)
                {
                    _accountService.IsTwoFactorAuthenticationEnabled = responce.Content.Value;
                }

            }

            await base.OnBackButtonPresed();
            DependencyService.Get<IPullingService>().StartPulling();
        }

        private async void CheckIfChanged()
        {

            if (Utilities.VerificationHelper.TwoFactorParameter == null)
            {
                await OnBackButtonPresed();
                return;
            }

            IsBusy = true;
            BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorEnabled(_accountService.Login);
            IsBusy = false;

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
                await OnBackButtonPresed();

                Utilities.VerificationHelper.TwoFactorParameter?.SetIfAuthSucceessful(true);
            }
        }
    }
}
