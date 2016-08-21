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
    public class CommitteeManageViewModel : ImHereBaseViewModel
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

        #region SelectedMember
        private MemberUser _selectedMember;
        public MemberUser SelectedMember
        {
            get { return _selectedMember; }
            set { Set(ref _selectedMember, value); }
        }
        #endregion SelectedMember

        #region MembersList
        private ObservableCollection<MemberUser> _membersList;
        public ObservableCollection<MemberUser> MembersList
        {
            get { return _membersList; }
            set { Set(ref _membersList, value); }
        }
        #endregion MembersList

        #region CommitteeBudgetText
        private String _committeeBudgetText;
        public String CommitteeBudgetText 
        {
            get { return _committeeBudgetText; }
            set { Set(ref _committeeBudgetText, value); }
        }
        #endregion CommitteeBudgetText

        public String CurrentUserAction => UserHasExecutivePrivlliges ? "Manage" : "View";
        public String ManageSubTitle => $"{CurrentUserAction} {CurrentCommittee.Description()} Properties";
        public String ChairDetails => GetCommitteeChairDetails();
        public String TotalMembers => CurrentCommittee.Members?.Count.ToString() ?? EmptyNumbersString;


        #endregion Public Properties

        #region Public Commands
        private RelayCommand _addUserCommand;
        public RelayCommand AddUserCommand => _addUserCommand ??
                                                    (_addUserCommand = new RelayCommand(DoAddUser, () => UserHasExecutivePrivlliges));

        private RelayCommand _removeUserCommand;
        public RelayCommand RemoveUserCommand => _removeUserCommand ??
                                                    (_removeUserCommand = new RelayCommand(DoRemoveUser, () => UserHasExecutivePrivlliges));

        private RelayCommand _setBudgetCommand;
        public RelayCommand SetBudgetCommand => _setBudgetCommand ??
                                                    (_setBudgetCommand = new RelayCommand(DoSetBudget, () => UserHasExecutivePrivlliges));

        #endregion


        public CommitteeManageViewModel(Committee committee)
        {
            CurrentCommittee = committee;
            Title = $"{CurrentCommittee.Name} Members";
            AppModel = new AppModelContainer();

            LoadData();
        }

        private void LoadData()
        {
            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            var fetchedMembersFromServer = CurrentCommittee.Members.ToList();
            MembersList = new ObservableCollection<MemberUser>(fetchedMembersFromServer);
            CommitteeBudgetText = GetBudgetText;
        }

        private string GetCommitteeChairDetails()
        {
            var chair = CurrentCommittee.Chair;
            return chair == null ? EmptyNumbersString : $"{chair.FullName()} <{chair.Email}>";
        }

        
        private async void DoRemoveUser()
        {
            if (SelectedMember == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }

            var confirmMessage = $"Are you sure you want to remove {SelectedMember.FullName()} from committee?";
            if (!ImHereMessageBox.ShowYesNo("Remove Member", confirmMessage)) return;
            
            var success = true;
            var errorMsg = "";
            try
            {
                MembersList.Remove(SelectedMember);
                CurrentCommittee.Members = MembersList;
                AppModel.Entry(CurrentCommittee).State = EntityState.Modified;
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
            var showAddNewMemberMsg = ShowPopUpMessage.ForEntity<MemberUser>(OnSuccess_AddingNewMember, CurrentCommittee);
            MessengerInstance.Send(showAddNewMemberMsg);
        }

        private void OnSuccess_AddingNewMember(object obj)
        {
            AppModel.Committees.Load();
            LoadData();

            RaisePropertyChanged("TotalMembers");
        }



        private void DoSetBudget()
        {
            int newBudget;
            if (Int32.TryParse(CommitteeBudgetText, out newBudget))
            {
                SetNewBuget(newBudget);    
            }
            else //no new budget. revert.
            {
                CommitteeBudgetText = GetBudgetText;
            }
        }

        private async void SetNewBuget(int newBudget)
        {
            CurrentCommittee.CurrentYearBudget = newBudget;
            AppModel.Entry(CurrentCommittee).State = EntityState.Modified;
            await AppModel.SaveChangesAsync();
        }

        private String GetBudgetText => CurrentCommittee.CurrentYearBudget?.ToString() ?? "Not Set";

    }
}
