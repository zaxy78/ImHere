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
using ImHereNonProfit.Model;

namespace ImHereNonProfit.UI.ViewModel.Add
{
    public class NewCommitteViewModel : ImHereNewEntityViewModel<Committee> 
    {
        // Data
        public string Name { get; set; }
        public int? CurrentYearBudget { get; set; }
        public int? ChairId { get; set; }

        // Helpers

        public ExecutiveUser Chair
        {
            get { return _chair; }
            set
            {
                _chair = value;
                ChairId = value?.Id;
            }
        }
        private ExecutiveUser _chair;
        
        #region UserList
        public ObservableCollection<ExecutiveUser> _usersList;
        public ObservableCollection<ExecutiveUser> UsersList
        {
            get { return _usersList; }
            set { Set(ref _usersList, value); }
        }
        #endregion UserList

        public NewCommitteViewModel(Action<object> onSuccess) : base(onSuccess)
        {
            Task.Factory.StartNew(LoadData);
        }

        private async void LoadData()
        {
            await AppModel.BasicUsers.LoadAsync();
            var executives = AppModel.BasicUsers.Where(user => user.UserType == UsersTypes.Executive).ToList();
            UsersList = new ObservableCollection<ExecutiveUser>();
            foreach (var executive in executives)
            {
                UsersList.Add(executive as ExecutiveUser);
            }
            
        }

        protected override bool IsFormValid()
        {
            var isValid = ChairId.HasValue &&
                          !(String.IsNullOrEmpty(Name));

            if (isValid)
            {
                NewEntity = new Committee()
                {
                    Name = Name,
                    ChairId = ChairId.Value,
                    CurrentYearBudget = CurrentYearBudget,
                };
            }
            return isValid;
        }
    }
}
