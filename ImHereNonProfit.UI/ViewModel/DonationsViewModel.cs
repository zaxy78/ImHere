using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Services;
using ImHereNonProfit.UI.Utils;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI.ViewModel
{
    public class DonationsViewModel : ImHereBaseViewModel
    {

        #region CurrentUser
        private BasicUser _currentUser;
        public BasicUser CurrentUser
        {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }
        #endregion CurrentUser


        #region SelectedDonation
        private Donation _selectedDonation;
        public Donation SelectedDonation
        {
            get { return _selectedDonation; }
            set { Set(ref _selectedDonation, value); }
        }
        #endregion SelectedDonation

        #region UserDonationsList
        private ObservableCollection<Donation> _userDonationsList;
        public ObservableCollection<Donation> UserDonationsList
        {
            get { return _userDonationsList; }
            set { Set(ref _userDonationsList, value); }
        }
        #endregion UserDonationsList

        #region TotatDonationsAmount
        private String _totatDonationsAmount;
        public String TotatDonationsAmount
        {
            get { return _totatDonationsAmount; }
            set { Set(ref _totatDonationsAmount, value); }
        }
        #endregion TotatDonationsAmount

        #region LastDonationDate
        private String _lastDonationDate;
        public String LastDonationDate
        {
            get { return _lastDonationDate; }
            set { Set(ref _lastDonationDate, value); }
        }
        #endregion LastDonationDate

        #region Public Commands

        private RelayCommand _addDonationCommand;
        public RelayCommand AddDonationCommand => _addDonationCommand ??
                                                    (_addDonationCommand = new RelayCommand(DoAddDonation, () => UserHasMemberPrivlliges));


        private RelayCommand _removeDonationCommand;
        public RelayCommand RemoveDonationCommand => _removeDonationCommand ??
                                                    (_removeDonationCommand = new RelayCommand(DoRemoveDonation, () => UserHasExecutivePrivlliges));


        #endregion

        public String UserLabelPrefix => UserHasExecutivePrivlliges ? "All" : "My";

        public DonationsViewModel(BasicUser currentUser, bool loadDataOnStart = true)
        {
            CurrentUser = currentUser;
            UserDonationsList = new ObservableCollection<Donation>();
            Title = $"{UserLabelPrefix} Donations";
            AppModel = new AppModelContainer();
            if (loadDataOnStart) LoadData();
        }

        public async void LoadData()
        {
            if (UserHasExecutivePrivlliges)
            {
                await AppModel.Donations.LoadAsync();
                await AppModel.BasicUsers.LoadAsync();
                UserDonationsList = AppModel.Donations.Local;
            }
            else
            {
                AppModel.Entry(CurrentUser).State = EntityState.Unchanged;
                UserDonationsList = new ObservableCollection<Donation>(CurrentUser.Donations);
            }
            TotatDonationsAmount = UserDonationsList.Sum(d => d.Amount).ToString();
            LastDonationDate = GetLastDonation()?.Date.ToLongDateString() ?? EmptyNumbersString;
        }

        private async void DoRemoveDonation()
        {
            if (SelectedDonation == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }

            var confirmMessage = $"Are you sure you want to remove thisdonation from orginization donations?";
            if (!ImHereMessageBox.ShowYesNo("Remove Donation", confirmMessage)) return;

            var success = true;
            var errorMsg = "";
            try
            {
                AppModel.Donations.Remove(SelectedDonation);
                UserDonationsList.Remove(SelectedDonation);
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

        private void DoAddDonation()
        {
            var showAddNewDonationMsg = ShowPopUpMessage.ForEntity<Donation>((d) => UserDonationsList.Add(d as Donation));                
            MessengerInstance.Send(showAddNewDonationMsg);
        }

        private Donation GetLastDonation()
        {
            var sortedList = UserDonationsList?.ToList();
            sortedList?.Sort((d1, d2) => d1.Date.CompareTo(d2.Date));
            return sortedList?.LastOrDefault();
        }
    }
}
