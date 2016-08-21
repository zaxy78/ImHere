using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;

namespace ImHereNonProfit.UI.ViewModel
{
    public class TopBarViewModel : ImHereBaseViewModel
    {
        #region Public Properties
        public bool CanAppGoBack => App.MainNavigationService?.CanGoBack ?? false;

        private bool _showGoBackButton;
        public bool ShowGoBackButton
        {
            get { return _showGoBackButton; }
            set { Set(ref _showGoBackButton, value); }
        }

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                Set(ref _currentPage, value);
                ShowGoBackButton = ShouldShowGoBackButton();
            }
        }
        #endregion Public Properties

        #region Public Commands
        public RelayCommand AppGoBackCommand => _appGoBackCommand ?? (_appGoBackCommand = new RelayCommand(AppGoBack,() => CanAppGoBack));
        private RelayCommand _appGoBackCommand;

        public RelayCommand AppMockUser => _appMockUser ?? (_appMockUser = new RelayCommand(DoMockUser));

        private bool ShouldShowGoBackButton()
        {
            return /*NOT*/!(CurrentPage?.DataContext is LoginViewModel || CurrentPage?.DataContext is ImHereMainViewModel);
        }

        private void DoMockUser()
        {
            var type = App.MainLoginService.LoggedUser.UserType;
            if (type == UsersTypes.Executive)
            {
                App.MainLoginService.LoggedUser = Mocks.MemberMock;
            }
            else
            {
                App.MainLoginService.LoggedUser = Mocks.ExecutiveMock;
            }

            if (App.MainNavigationService.Content is ImHereBaseViewModel) 
            {
                ((ImHereBaseViewModel) App.MainNavigationService.Content).RaisePropertyChanged("UserHasMemberPrivlliges");
                ((ImHereBaseViewModel) App.MainNavigationService.Content).RaisePropertyChanged("UserHasExecutivePrivlliges");
            }
        }
       
        private RelayCommand _appMockUser;

        
        #endregion Public Commands

        #region Public Methods
        public void Init_OnLoad()
        {
            RegisterToNavigationMessages();
        }

        public void RegisterToNavigationMessages()
        {
            App.MainNavigationService.LoadCompleted += NewPageLoaded;
        }
        #endregion Public Methods

        #region Private Commands

        private void AppGoBack()
        {
            App.MainNavigationService?.GoBack();
        }
        private void NewPageLoaded(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var page = e.Content as Page;
            if (page != null)
            {
                CurrentPage = page;
                Title = (CurrentPage.DataContext as ImHereBaseViewModel)?.Title;
            }
        }

        #endregion Private Commands

    }
}
