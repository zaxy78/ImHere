using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI.ViewModel
{
    public class LoginViewModel : ImHereBaseViewModel
    {

        #region IsWaiting
        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { Set(ref _isWaiting, value); }
        }
        #endregion IsWaiting

        #region EmailInput
        private String _emailInput;
        public String EmailInput
        {
            get { return _emailInput; }
            set { Set(ref _emailInput, value); }
        }
        #endregion EmailInput

        #region PasswordInput
        private String _passwordInput;
        public String PasswordInput
        {
            get { return _passwordInput; }
            set { Set(ref _passwordInput, value); }
        }
        #endregion PasswordInput


        #region Public Commands
        public RelayCommand SignInCommand => _signInCommand ?? (_signInCommand = new RelayCommand(DoSignIn));
        private RelayCommand _signInCommand;
        #endregion 

        public LoginViewModel()
        {
            Title = $"Welcome to ImHereForFree";
            var subTitle = "We are. Are you?";

        }

        private async void DoSignIn()
        {
            IsWaiting = true;
            var success = await App.MainLoginService.TryLogin(EmailInput, PasswordInput);
            if (success)
            {
                await Task.Factory.StartNew(() => { 
                var viewModel = new ImHereMainViewModel();
                MessengerInstance.Send(new NavigateMessage(viewModel));

                    Cleanup();
                });
            }
            else
            {
                MessageBox.Show("Loging Failed. Unknown error!", "Login failed!");
            }

            IsWaiting = false;
        }

        public override void Cleanup()
        {
            base.Cleanup();
            EmailInput = "";
            PasswordInput = "";
        }
    }
}
