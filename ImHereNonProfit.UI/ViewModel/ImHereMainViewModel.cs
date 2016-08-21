using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;
using ImHereNonProfit.UI.ViewModel;
using ImHereNonProfit.UI.Views;

namespace ImHereNonProfit.UI.ViewModel
{
    public class ImHereMainViewModel : ImHereBaseViewModel
    {

        #region CurrentUser
        private BasicUser _currentUser;
        public BasicUser CurrentUser
        {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }
        #endregion CurrentUser

        #region SelectedCommittee
        private Committee _selectedCommittee;
        public Committee SelectedCommittee
        {
            get { return _selectedCommittee; }
            set { Set(ref _selectedCommittee, value); }
        }
        #endregion SelectedCommittee

        #region CommitteeList
        private ObservableCollection<Committee> _userCommitteeList;
        public ObservableCollection<Committee> UserCommitteeList
        {
            get { return _userCommitteeList; }
            set { Set(ref _userCommitteeList, value); }
        }
        #endregion MembersList

        #region Public Commands
        public RelayCommand NavigateToCommitteesCommand => _navigateToCommitteesCommand ?? (_navigateToCommitteesCommand = new RelayCommand(NavigateToCommittees, () => IsCommitteeSelected));
        private RelayCommand _navigateToCommitteesCommand;

        public RelayCommand NavigateToDonationsCommand => _navigateToDonationsCommand ?? (_navigateToDonationsCommand = new RelayCommand(NavigateToDonations));
        private RelayCommand _navigateToDonationsCommand;

        public RelayCommand NavigateToManageCommand => _navigateToManageCommand ?? (_navigateToManageCommand = new RelayCommand(NavigateToManage));
        private RelayCommand _navigateToManageCommand;

        public RelayCommand NavigateToSettingsCommand => _navigateToSettingsCommand ?? (_navigateToSettingsCommand = new RelayCommand(NavigateToSettings));
        private RelayCommand _navigateToSettingsCommand;

        public RelayCommand AddCommitteeCommand => _addCommitteeCommand ?? (_addCommitteeCommand = new RelayCommand(DoAddCommittee));
        private RelayCommand _addCommitteeCommand;        
        #endregion

        public String UserLabelPrefix => UserHasExecutivePrivlliges ? "All" : "My";
        public String CommitteesLabel => $"{UserLabelPrefix} Committees";
        public String DonationsLabel => $"{UserLabelPrefix} Donations";
        public String UsersLabel => $"{(UserHasExecutivePrivlliges? "Manage" : "View")} Users";

        public bool IsCommitteeSelected => SelectedCommittee != null;
        public ImHereMainViewModel(bool shouldLoadDataOnCreate = true)
        {
            CurrentUser = App.MainLoginService?.LoggedUser;
            UserCommitteeList = new ObservableCollection<Committee>();
            Title = $"Welcome {CurrentUser?.FirstName ?? "[User]"}, glad you are Here!";

            if (shouldLoadDataOnCreate) LoadData();
        }

        public async void LoadData()
        {
            if (CurrentUser == null || CurrentUser?.UserType < UsersTypes.Member)
                return;

            using (AppModel = new AppModelContainer())
            {
                AppModel.Entry(CurrentUser).State = EntityState.Unchanged;
                if (CurrentUser.UserType == UsersTypes.Member)
                {
                    var member = CurrentUser as MemberUser;
                    var fetchedCommitteesFromServer = member?.MemberOf.ToList() ?? new List<Committee>();
                    UserCommitteeList = new ObservableCollection<Committee>(fetchedCommitteesFromServer);
                }
                else //Executibe
                {
                    await AppModel.Committees.LoadAsync();
                    UserCommitteeList = AppModel.Committees.Local;
                }
            }
        }

        private void NavigateToCommittees()
        {
            var viewModel = new CommitteeViewModel(SelectedCommittee);
            MessengerInstance.Send(new NavigateMessage(viewModel));
        }

        private void NavigateToDonations()
        {
            var viewModel = new DonationsViewModel(CurrentUser);
            MessengerInstance.Send(new NavigateMessage(viewModel));
        }
        
        private void NavigateToManage()
        {
            var viewModel = new UsersViewModel();
            MessengerInstance.Send(new NavigateMessage(viewModel));
        }
        private void NavigateToSettings()
        {
            var viewModel = new SettingsViewModel();
            MessengerInstance.Send(new NavigateMessage(viewModel));
        }

        private void DoAddCommittee()
        {
            var showAddNewCommitteeMsg = ShowPopUpMessage.ForEntity<Committee>(OnSuccess_AddingNewCommittee);
            MessengerInstance.Send(showAddNewCommitteeMsg);
        }

        private void OnSuccess_AddingNewCommittee(object obj)
        {
            LoadData();
        }


        public override void Cleanup()
        {
            base.Cleanup();
            
        }

    }
}
