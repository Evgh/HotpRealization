using Authorizer.Services;
using Authorizer.ViewModels;
using Authorizer.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Authorizer
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAccountService>().Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
