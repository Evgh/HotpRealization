﻿using Client.Models;
using Client.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected const string COMMON_ERROR_MESSAGE = "Sorry, something went wrong. Please, try again later";
        protected const string VERIFICATION_ERROR_MESSAGE = "Two factor verification wasn't provided";

        private string _title = string.Empty;
        private bool _isBusy = false;
        private string _errorMessage;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        protected bool SetProperty<T>(ref T backingStore,
                                       T value,
                                       [CallerMemberName] string propertyName = "",
                                       Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
