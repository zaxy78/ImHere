using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;

namespace ImHereNonProfit.UI.ViewModel
{
    public class CommitteeViewModel : ImHereBaseViewModel
    {
        
        #region Public Commands
        public RelayCommand NavigateToDocsCommand => _navigateToDocsCommand ?? (_navigateToDocsCommand = new RelayCommand(NavigateToDocs));
        private RelayCommand _navigateToDocsCommand;
        public RelayCommand NavigateToActivityCommand => _navigateToActivityCommand ?? (_navigateToActivityCommand = new RelayCommand(NavigateToActivity));
        private RelayCommand _navigateToActivityCommand;
        public RelayCommand NavigateToBudgetCommand => _navigateToBudgetCommand ?? (_navigateToBudgetCommand = new RelayCommand(NavigateToBudget));
        private RelayCommand _navigateToBudgetCommand;
        public RelayCommand NavigateToManageCommand => _navigateToManageCommand ?? (_navigateToManageCommand = new RelayCommand(NavigateToManage));
        private RelayCommand _navigateToManageCommand;
        #endregion

        #region CurrentCommittee
        private Committee _currentCommittee;
        public Committee CurrentCommittee
        {
            get { return _currentCommittee; }
            set { Set(ref _currentCommittee, value); }
        }
        #endregion


        public CommitteeViewModel(Committee currentCommitee)
        {
            CurrentCommittee = currentCommitee;
            Title = currentCommitee.Description();

            LoadData();
        }

        private async void LoadData()
        {
            using (AppModel = new AppModelContainer())
            {
                AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            }
        }

        private void NavigateToDocs()
        {
            var viewModel = new CommitteeDocsViewModel(CurrentCommittee);
            MessengerInstance.Send<NavigateMessage>(new NavigateMessage(viewModel));
        }

        private void NavigateToActivity()
        {
            var viewModel = new CommitteeActivityViewModel(CurrentCommittee);
            MessengerInstance.Send<NavigateMessage>(new NavigateMessage(viewModel));
        }

        private void NavigateToBudget()
        {
            var viewModel = new CommitteeBudgetViewModel(CurrentCommittee);
            MessengerInstance.Send<NavigateMessage>(new NavigateMessage(viewModel));
        }
        private void NavigateToManage()
        {
            var viewModel = new CommitteeManageViewModel(CurrentCommittee);
            MessengerInstance.Send<NavigateMessage>(new NavigateMessage(viewModel));
        }
    }
}
