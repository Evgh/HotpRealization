﻿using Client.ViewModels;
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

            Routing.RegisterRoute(nameof(TwoFactorVerificationPage), typeof(TwoFactorVerificationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
