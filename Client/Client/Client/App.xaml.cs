using Client.Services;
using Client.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IServiceClient, Services.Implementations.ServiceClient>();
            DependencyService.RegisterSingleton<IAccountService>(new Services.Implementations.AccountService());
            DependencyService.RegisterSingleton<IPullingService>(new Services.Implementations.PullingService());

            MainPage = new AppShell();
            RegisterPullingService();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected void RegisterPullingService()
        {
            var pullingService = DependencyService.Get<IPullingService>();
            var accountService = DependencyService.Get<IAccountService>();
            accountService.OnLogin += pullingService.StartPulling;
            accountService.OnLogout += pullingService.StopPulling;
        }
    }
}
