/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ImHereNonProfit.Model"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.ViewModel;
using ImHereNonProfit.UI.ViewModel.Add;
using ImHereNonProfit.UI.Views;
using ImHereNonProfit.UI.Views.Add;
using Microsoft.Practices.ServiceLocation;
using CommitteeManageView = ImHereNonProfit.UI.Views.CommitteeManageView;

namespace ImHereNonProfit.UI
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewsLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewsLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<CommitteeView>();
            SimpleIoc.Default.Register<CommitteeActivityView>();
            SimpleIoc.Default.Register<CommitteeBudgetView>();
            SimpleIoc.Default.Register<CommitteeDocsView>();
            SimpleIoc.Default.Register<DonationsView>();
            SimpleIoc.Default.Register<ImHereMainView>();
            SimpleIoc.Default.Register<LoginView>(); 
            SimpleIoc.Default.Register<UsersView>();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        /// <summary>
        /// Returns a singelton instance of the view T, which is hopefull registered, 
        /// with the give ViewModel injected to its DataContext. or null if not a view.
        /// </summary>
        /// <typeparam name="T">View type requested</typeparam>
        /// <param name="viewModel">ViewMOdel to inject to T</param>
        /// <returns></returns>
        public static T GetViewInjectVm<T>(ImHereBaseViewModel viewModel)
        {
            var view = default(T);
            Application.Current.Dispatcher.Invoke(() =>
            {
                view = (T)Activator.CreateInstance(typeof(T));

                if (view is FrameworkElement)
                {
                    (view as FrameworkElement).DataContext = viewModel;
                }
                else
                {
                    view = default(T);
                }
            });
            return view;
        }


        /// <summary>
        /// Matches a View to the ViewModel and injects it into it.
        /// </summary>
        /// <param name="viewModel">ViewModel to which we shell match a View</param>
        /// <returns>View if found a match, otherwise Null</returns>
        public static Page MatchViewToViewModel(ImHereBaseViewModel viewModel)
        {

            if (viewModel is CommitteeViewModel) return GetViewInjectVm<CommitteeView>(viewModel);

            if (viewModel is CommitteeActivityViewModel) return GetViewInjectVm<CommitteeActivityView>(viewModel);

            if (viewModel is CommitteeBudgetViewModel) return GetViewInjectVm<CommitteeBudgetView>(viewModel);

            if (viewModel is CommitteeDocsViewModel) return GetViewInjectVm<CommitteeDocsView>(viewModel);

            if (viewModel is CommitteeManageViewModel) return GetViewInjectVm<CommitteeManageView>(viewModel);

            if (viewModel is ImHereMainViewModel) return GetViewInjectVm<ImHereMainView>(viewModel);

            if (viewModel is DonationsViewModel) return GetViewInjectVm<DonationsView>(viewModel);

            if (viewModel is LoginViewModel) return GetViewInjectVm<LoginView>(viewModel);

            if (viewModel is UsersViewModel) return GetViewInjectVm<UsersView>(viewModel);

            if (viewModel is SettingsViewModel) return GetViewInjectVm<SettingsView>(viewModel);

            return null;
        }

        /// <summary>
        /// Matches a n Entity to the relevant AddNewEntityViewModel and injects its view accordingly
        /// </summary>
        /// <param name="viewModel">ViewModel to which we shell match a View</param>
        /// <param name="onSuccess"></param>
        /// <param name="extraData"></param>
        /// <returns>View if found a match, otherwise Null</returns>
        public static Window MatchWindowAndVmToEntity<T>(Action<object> onSuccess, object extraData = null) where T : class 
        {

            if (typeof(T) == typeof(Donation)) return GetViewInjectVm<NewDonationWindow>(new NewDonationViewModel(onSuccess));

            if (typeof(T) == typeof(BasicUser)) return GetViewInjectVm<NewUserWindow>(new NewUserViewModel(onSuccess));

            if (typeof(T) == typeof(CommitteeDocument)) return GetViewInjectVm<NewDocWindow>(new NewDocViewModel(onSuccess, extraData as Committee));

            if (typeof(T) == typeof(CommitteeExpens)) return GetViewInjectVm<NewExpenssWindow>(new NewExpenssViewModel(onSuccess, extraData as Committee));

            if (typeof(T) == typeof(CommitteeActivity)) return GetViewInjectVm<NewActivityWindow>(new NewActivityViewModel(onSuccess, extraData as Committee));

            if (typeof(T) == typeof(Committee)) return GetViewInjectVm<NewCommitteeWindow>(new NewCommitteViewModel(onSuccess));

            if (typeof(T) == typeof(MemberUser)) return GetViewInjectVm<NewCommitteeMemberWindow>(new NewCommitteMemberVm(onSuccess, extraData as Committee));

            return null;
        }

    }
}