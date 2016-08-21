using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;

namespace ImHereNonProfit.UI.ViewModel
{
    public class CommitteeActivityViewModel : ImHereBaseViewModel
    {
        #region Public Properties

        #region CurrentCommittee
        private Committee _currentCommittee;
        public Committee CurrentCommittee
        {
            get { return _currentCommittee; }
            set { Set(ref _currentCommittee, value); }
        }
        #endregion CurrentCommittee

        #region SelectedActivity
        private CommitteeActivity _selectedActivity;
        public CommitteeActivity SelectedActivity
        {
            get { return _selectedActivity; }
            set { Set(ref _selectedActivity, value); }
        }
        #endregion SelectedActivity

        #region ActivityList
        private ObservableCollection<CommitteeActivity> _activityList;
        public ObservableCollection<CommitteeActivity> ActivityList
        {
            get { return _activityList; }
            set { Set(ref _activityList, value); }
        }
        #endregion ActivityList

        public String ActivityTitle => $"{CurrentCommittee.Description()} Activities Reports";
        public String TotalActivities => CurrentCommittee.Activities?.Count.ToString() ?? EmptyNumbersString;
        public String LastActivitySubject => CurrentCommittee.Activities?.LastOrDefault()?.Subject ?? EmptyNumbersString;
        public String LastActivityDate => CurrentCommittee.Activities?.LastOrDefault()?.ReportedDate.ToLongDateString() ?? EmptyNumbersString;
        public String TotalActivitiesPastMonth => GetReportsFromPastMonth()?.ToString() ?? EmptyNumbersString;
        
        #endregion Public Properties

        #region Public Commands
        private RelayCommand _addActivityCommand;
        public RelayCommand AddActivityCommand => _addActivityCommand ??
                                                    (_addActivityCommand = new RelayCommand(DoAddActivity));

        private RelayCommand _removeActivityCommand;
        public RelayCommand RemoveActivityCommand => _removeActivityCommand ??
                                                    (_removeActivityCommand = new RelayCommand(DoRemoveActivity, () => UserHasExecutivePrivlliges));

        #endregion


        public CommitteeActivityViewModel(Committee committee)
        {
            CurrentCommittee = committee;
            Title = $"{CurrentCommittee.Name} Activities";
            AppModel = new AppModelContainer();
            
            LoadData();
        }

        private void LoadData()
        {
            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            var fetchedReportsFromServer =  CurrentCommittee.Activities.ToList();
            ActivityList = new ObservableCollection<CommitteeActivity>(fetchedReportsFromServer);
        }

        private async void DoRemoveActivity()
        {
            if (SelectedActivity == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }
            
            var confirmMessage = $"Are you sure you want to remove this activity from committee activities?";
            if (!ImHereMessageBox.ShowYesNo("Remove Activity", confirmMessage)) return;

            var success = true;
            var errorMsg = "";
            try
            {
                AppModel.CommitteeActivities.Remove(SelectedActivity);
                ActivityList.Remove(SelectedActivity);
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
        private void DoAddActivity()
        {
            var showAddNewDonationMsg = ShowPopUpMessage.ForEntity<CommitteeActivity>(OnSuccess_AddingNewActivity, CurrentCommittee );
            MessengerInstance.Send(showAddNewDonationMsg);
        }

        private void OnSuccess_AddingNewActivity(object obj)
        {
            AppModel.CommitteeActivities.Load();
            LoadData();

            RaisePropertyChanged("TotalActivities");
            RaisePropertyChanged("LastActivitySubject");
            RaisePropertyChanged("LastActivityDate");
            RaisePropertyChanged("TotalActivitiesPastMonth");
        }

        private int? GetReportsFromPastMonth()
        {
            int? totalActvitiesFromPastMonth = null;

            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            totalActvitiesFromPastMonth =
                CurrentCommittee?.Activities?.Count(activity => activity.ReportedDate >= DateTime.Today.AddDays(-30));
            return totalActvitiesFromPastMonth;
        }

    }
}

