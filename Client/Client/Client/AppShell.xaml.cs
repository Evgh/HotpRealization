using Client.Services;
using Client.ViewModels;
using Client.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Client
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAccountService>().ExecuteLogout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
