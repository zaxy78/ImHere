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
    public class NewDocViewModel : ImHereNewEntityViewModel<CommitteeDocument> 
    {
        // Data
        public int CommitteeId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string URL { get; set; }
        public System.DateTime? LastModified { get; set; }
        public System.DateTime? CreatedOn { get; set; }

       public NewDocViewModel(Action<object> onSuccess, Committee committee) : base(onSuccess)
       {
           CommitteeId = committee.Id;
       }
        
        protected override bool IsFormValid()
        {
            var isValid = LastModified.HasValue &&
                          CreatedOn.HasValue &&
                          !(String.IsNullOrEmpty(Name)) &&
                          !(String.IsNullOrEmpty(Tags));
            
            
            if (isValid)
            {
                NewEntity = new CommitteeDocument()
                {
                    CommitteeId = CommitteeId,
                    CreatedOn = CreatedOn.Value,
                    LastModified = LastModified.Value,
                    Name = Name,
                    Tags = Tags,
                    Description = URL,
                };

            }
            return isValid;
        }
    }
}
