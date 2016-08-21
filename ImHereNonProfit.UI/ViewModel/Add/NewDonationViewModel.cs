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
    public class NewDonationViewModel : ImHereNewEntityViewModel<Donation> 
    {
        // Data
        public int? BasicUserId { get; set; }
        public DateTime? Date { get; set; }
        public int? Amount { get; set; }
        public String Currency { get; set; } = "ILS";

        // Helpers
        
        public BasicUser Donor
        {
            get { return _donor; }
            set
            {
                _donor = value;
                BasicUserId = value?.Id;
            }
        }
        private BasicUser _donor;
        public ObservableCollection<BasicUser> UsersList { get; set; }

        public NewDonationViewModel(Action<object> onSuccess) : base(onSuccess)
        {
            Task.Factory.StartNew(LoadData);
        }

        private async void LoadData()
        {
            await AppModel.BasicUsers.LoadAsync();
            UsersList = AppModel.BasicUsers.Local;
        }

        protected override bool IsFormValid()
        {
            var isValid = BasicUserId.HasValue &&
                          Date.HasValue &&
                          Amount.HasValue &&
                          !(String.IsNullOrEmpty(Currency));

            if (isValid)
            {
                NewEntity = new Donation()
                {
                    Amount = Amount.Value,
                    BasicUserId = BasicUserId.Value,
                    Currency = Currency,
                    Date = Date.Value
                }; 
               
            }
            return isValid;
        }
    }
}
