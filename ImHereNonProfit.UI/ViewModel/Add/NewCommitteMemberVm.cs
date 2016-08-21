using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;

namespace ImHereNonProfit.UI.ViewModel.Add
{
    public class NewCommitteMemberVm : ImHereNewEntityViewModel<MemberUser> 
    {
        // Data
        public int? BasicUserId { get; set; }

        // Helpers
        public BasicUser SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                BasicUserId = value?.Id;
            }
        }
        private BasicUser _selectedUser;

        public Committee CurrentCommittee { get; set; }

        #region UserList
        public ObservableCollection<BasicUser> _usersList;
        public ObservableCollection<BasicUser> UsersList
        {
            get { return _usersList; }
            set { Set(ref _usersList, value); }
        }
        #endregion UserList

        private RelayCommand _addMemberToCommitteeCommand;
        public override RelayCommand AddEntityCommand => _addMemberToCommitteeCommand ??
            (_addMemberToCommitteeCommand = new RelayCommand(AddMemberToCommittee, IsFormValid));

        public NewCommitteMemberVm(Action<object> onSuccess, Committee committee) : base(onSuccess)
        {
            CurrentCommittee = new Committee {Id = committee.Id, Name = committee.Name};
            Task.Factory.StartNew(LoadData);

            Title = "Add Member to Committee";
        }

        private async void LoadData()
        {
            await AppModel.BasicUsers.LoadAsync();
            var membersOrAbove = AppModel.BasicUsers.Where(user => user.UserType >= UsersTypes.Member).ToList();
            UsersList = new ObservableCollection<BasicUser>(membersOrAbove);
        }

        protected override bool IsFormValid()
        {
            var isValid = BasicUserId.HasValue;
                          
            return isValid;
        }

        private async void AddMemberToCommittee()
        {
            try
            {
                var member = SelectedUser as MemberUser;
                member.MemberOf.Add(CurrentCommittee);
                AppModel.Entry(SelectedUser).State = EntityState.Modified;
                AppModel.SaveChanges();

                OnSuccess?.Invoke(member);
                MessengerInstance.Send(new ClosePopUpMessage(null));
            }
            catch (Exception e)
            {
                ImHereMessageBox.ShowError("Error Adding New Entry", e.Message);
            }
        }
    }
}
