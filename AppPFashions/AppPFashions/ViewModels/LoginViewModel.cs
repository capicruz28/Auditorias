using AppPFashions.Models;
using AppPFashions.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace AppPFashions.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        private DataService dataService;
        private bool isRunning;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged; 
        #endregion

   

        #region Properties
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            dataService = new DataService();
            IsRemembered = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(User))
            {
                await dialogService.ShowMessage("Error", "Ingrese un usuario.");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Error", "Ingrese una contraseña.");
                return;
            }

            IsRunning = true;
            var response = await apiService.Login(User, Password);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var user = (Usuario)response.Result;
            user.IsRemembered = IsRemembered;
            user.Password = Password;
            VariableGlobal.ctraba = user.ctraba;

            dataService.InsertUser(user);

            navigationService.SetMainPage(user);
        }
        #endregion
    }
}
