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
            DependencyService.Register<IAccountService, Services.Implementations.AccountService>();
            DependencyService.Register<ITwoFactorVerificationService, Services.Implementations.TwoFactorVerificationService>();

            MainPage = new AppShell();
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
    }
}
