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
    public class NewActivityViewModel : ImHereNewEntityViewModel<CommitteeActivity> 
    {
        // Data
        public int CommiteeId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public DateTime? ReportedDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        // Helpers

        public NewActivityViewModel(Action<object> onSuccess, Committee committee) : base(onSuccess)
        {
            CommiteeId = committee.Id;
        }

        protected override bool IsFormValid()
        {
            var isValid = ActivityDate.HasValue &&
                          ReportedDate.HasValue &&
                          !(String.IsNullOrEmpty(Subject)) &&
                          !(String.IsNullOrEmpty(Description)) &&
                          !(String.IsNullOrEmpty(Tags));

            if (isValid)
            {
                NewEntity = new CommitteeActivity()
                {
                    CommiteeId = this.CommiteeId,
                    ActivityDate= ActivityDate.Value,
                    ReportedDate = ReportedDate.Value,
                    Subject = Subject,
                    Description = Description,
                    Tags = Tags
                };

            }
            return isValid;
        }
    }
}
