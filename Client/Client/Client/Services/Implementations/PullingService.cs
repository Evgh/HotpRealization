using Client.Models.Responces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.Services.Implementations
{
    public class PullingService : IPullingService
    {
        private const int PERIOD = 3;

        private readonly IAccountService _accountService;
        private readonly IServiceClient _serviceClient;

        private int _counter = -1;

        public PullingService()
        {
            _accountService = DependencyService.Get<IAccountService>();
            _serviceClient = DependencyService.Get<IServiceClient>();

            DoJob();
        }

        public void StartPulling()
        {
            _counter = PERIOD;
        }

        public void StopPulling()
        {
            _counter = -1;
        }

        private async void DoJob()
        {
            await Task.Delay(1000);

            if(_counter > 0)
            {
                _counter--;
            }
            else if(_counter == 0)
            {
                if (!string.IsNullOrEmpty(_accountService.Login))
                {
                    BaseResponce<bool?> responce = await _serviceClient.GetIsTwoFactorEnabled(_accountService.Login);

                    if (responce.IsSuccess && responce.Content.HasValue)
                    {
                        if (_accountService.IsTwoFactorAuthenticationEnabled ^ responce.Content.Value)
                        {
                            _accountService.IsTwoFactorAuthenticationEnabled = responce.Content.Value;
                        }
                    }
                }

                _counter = PERIOD;
            }

            DoJob();
        }
    }
}
