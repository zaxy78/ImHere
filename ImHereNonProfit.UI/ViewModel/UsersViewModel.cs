using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI.ViewModel
{
    public class UsersViewModel : ImHereBaseViewModel
    {

        #region CurrentUser
        private BasicUser _currentUser;
        public BasicUser CurrentUser
        {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }
        #endregion CurrentUser

        #region SelectedUser
        private BasicUser _selectedUser;
        public BasicUser SelectedUser
        {
            get { return _selectedUser; }
            set { Set(ref _selectedUser, value); }
        }
        #endregion SelectedUser


        #region UsersList
        private ObservableCollection<BasicUser> _fullUsersList;
        private ObservableCollection<BasicUser> _filteredUsersList;
        public ObservableCollection<BasicUser> FilteredUsersList
        {
            get { return _filteredUsersList; }
            set { Set(ref _filteredUsersList, value); }
        }
        #endregion UsersList



        #region UsersTypesList
        private List<String> _usersTypesList;
        public List<String> UsersTypesList
        {
            get { return _usersTypesList; }
            set { Set(ref _usersTypesList, value); }
        }
        #endregion UsersTypesList


        #region Public Commands
        public RelayCommand<String> FilterUsersByTypeCommand => _filterUsersByTypeCommand ?? (_filterUsersByTypeCommand = new RelayCommand<String>(DoFilterUsersByType));
        private RelayCommand<String> _filterUsersByTypeCommand;

        private RelayCommand _addUserCommand;
        public RelayCommand AddUserCommand => _addUserCommand ??
                                                    (_addUserCommand = new RelayCommand(DoAddUser, () => UserHasExecutivePrivlliges));

     
        private RelayCommand _removeUserCommand;
        private UsersTypes _currentFilter;

        public RelayCommand RemoveUserCommand => _removeUserCommand ??
                                                    (_removeUserCommand = new RelayCommand(DoRemoveUser, () => UserHasExecutivePrivlliges));


        #endregion


        public String UserLabelPrefix => UserHasExecutivePrivlliges ? "Manage" : "View";
        public String CommitteesLabel => $"{UserLabelPrefix} Committees";
        public String DonationsLabel => $"{UserLabelPrefix} Donations";


        public UsersViewModel()
        {
            Title = $"{UserLabelPrefix} Users";
            CurrentUser = App.MainLoginService?.LoggedUser;
            UsersTypesList = Enum.GetNames(typeof(UsersTypes)).ToList();
            UsersTypesList.Insert(0,"No Filter");
            AppModel = new AppModelContainer();

            LoadData();
        }

        public async void LoadData()
        {
            await AppModel.BasicUsers.LoadAsync();
            _fullUsersList = AppModel.BasicUsers.Local;
            FilterUsersByType(null);
        }


        private void FilterUsersByType(UsersTypes? type)
        {
            if (type.HasValue)
            {
                var filterList = _fullUsersList.AsQueryable().Where(user => user.UserType.Equals(type));
                FilteredUsersList = new ObservableCollection<BasicUser>(filterList);
            }
            else
            {
                FilteredUsersList = _fullUsersList;
            }
        }

        private void DoFilterUsersByType(String typeName)
        {
            UsersTypes newFilter;
            if (Enum.TryParse(typeName, out newFilter))
            {
                _currentFilter = newFilter;
                FilterUsersByType(newFilter);
            }
            else
            {
                FilterUsersByType(null);
            }
        }

        private async void DoRemoveUser()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }

            var confirmMessage = $"Are you sure you want to remove {SelectedUser.FullName()} from the system?" +
                                 $"Warning! this action will remove any related data to this user.";
            if (!ImHereMessageBox.ShowYesNo("Remove User", confirmMessage)) return;

            var success = true;
            var errorMsg = "";
            try
            {
                await SelectedUser.RemoveFromAllCommitteesAsync(AppModel);
                AppModel.BasicUsers.Remove(SelectedUser);
                _fullUsersList.Remove(SelectedUser);

                await AppModel.SaveChangesAsync();
            }
            catch (Exception e)
            {
                success = false;
                errorMsg = e.Message;
            }

            if (success) return;
            ImHereMessageBox.ShowError("Error deleting row!", errorMsg);
        }

        private void DoAddUser()
        {
            var showAddNewDonationMsg = ShowPopUpMessage.ForEntity<BasicUser>((u) =>
            {
                var user = u as BasicUser;
                _fullUsersList.Add(user);
                FilterUsersByType(_currentFilter);
            });
            MessengerInstance.Send(showAddNewDonationMsg);
        }


    }
}
