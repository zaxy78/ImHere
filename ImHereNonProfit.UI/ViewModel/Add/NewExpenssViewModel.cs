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
    public class NewExpenssViewModel : ImHereNewEntityViewModel<CommitteeExpens> 
    {
        // Data
        public int CommitteeId { get; set; }
        public int? Amount { get; set; }
        public string Currency { get; set; } = "ILS";
        public System.DateTime? Date { get; set; }
        public string Description { get; set; }

        // Helpers

        public NewExpenssViewModel(Action<object> onSuccess, Committee committee) : base(onSuccess)
        {
            CommitteeId = committee.Id;
        }

        protected override bool IsFormValid()
        {
            var isValid = Date.HasValue &&
                          Amount.HasValue &&
                          !(String.IsNullOrEmpty(Currency)) &&
                          !(String.IsNullOrEmpty(Description));

            if (isValid)
            {
                NewEntity = new CommitteeExpens()
                {
                    CommitteeId = CommitteeId,
                    Amount = Amount.Value,
                    Date = Date.Value,
                    Currency = Currency,
                    Description = Description,
                };
            }
            return isValid;
        }
    }
}
