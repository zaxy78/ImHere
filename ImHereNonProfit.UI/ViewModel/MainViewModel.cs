using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Views;

namespace ImHereNonProfit.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ImHereBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            RegisterToMessages();
            MessengerInstance.Send(new NavigateMessage(ViewModelLocator.Login));
        }

        private void RegisterToMessages()
        {
            MessengerInstance.Register<NavigateMessage>(this, NavigateToPage);
            MessengerInstance.Register<ShowPopUpMessage>(this, ShowPopUpWindow);
            MessengerInstance.Register<ClosePopUpMessage>(this, ClosePopUpWindow);
        }

        public void Init_OnLoad()
        {
            App.MainNavigationService.Navigating += MainNavigationService_OnNavigating;
        }


        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }

        }

        private Window CurrentlyDisplayedDialog { get; set; }

        public void NavigateToPage(NavigateMessage msgNavigate)
        {
            var page = msgNavigate.ToPage;
            if (page  == null)
            {
                MessageBox.Show($"{page.ToString()} is not a Page!", "Error in Navigation!");
            }
            
            CurrentPage = (Page)page;
        }

        public void ShowPopUpWindow(ShowPopUpMessage msg)
        {
            if (msg?.AddNewEntryWindow != null)
            {
                CurrentlyDisplayedDialog = msg.AddNewEntryWindow;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CurrentlyDisplayedDialog.ShowDialog();
                }));
            }
        }

        public void ClosePopUpWindow(ClosePopUpMessage msg) 
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentlyDisplayedDialog?.Close();
                CurrentlyDisplayedDialog = null;
                
                msg.OnSuccess?.Invoke();
            }));
        }


        // App.MainNavigationService         
        private void MainNavigationService_OnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
        {
            if (navigatingCancelEventArgs.NavigationMode == NavigationMode.Back)
            {
                var frame = sender as Frame;
                var previousPage = (frame?.Content as Page);
                var viewModel = previousPage?.DataContext as ImHereBaseViewModel;

                viewModel?.Cleanup();
            }
            
        }

    }
}