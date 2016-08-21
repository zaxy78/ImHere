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
    public class NewUserViewModel : ImHereNewEntityViewModel<BasicUser> 
    {
        // Data
        public UsersTypes? UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Password { get; set; }

        // Helpers

        private string _selectedUserType;
        public string SelectedUserType
        {
            get { return _selectedUserType; }
            set
            {
                _selectedUserType = value;
                UsersTypes newValue;
                Enum.TryParse(value, out newValue);
                UserType = newValue;
            }
        }

        public List<string> UsersTypesList { get; set; }
        public NewUserViewModel(Action<object> onSuccess) : base(onSuccess)
        {
            UsersTypesList = new List<string>(Enum.GetNames(typeof(UsersTypes)));
        }
        
        protected override bool IsFormValid()
        {
            var isValid = UserType.HasValue &&
                          !(String.IsNullOrEmpty(FirstName)) &&
                          !(String.IsNullOrEmpty(LastName)) &&
                          !(String.IsNullOrEmpty(Email)) &&
                          !(String.IsNullOrEmpty(Phone)) &&
                          !(String.IsNullOrEmpty(Address)) &&
                          !(String.IsNullOrEmpty(Zipcode)) &&
                          !(String.IsNullOrEmpty(Password));

            if (isValid)
            {
                NewEntity = new BasicUser()
                {
                    UserType = this.UserType.Value,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email,
                    Phone = this.Phone,
                    Address = this.Address,
                    Zipcode = this.Zipcode,
                    Password = this.Password,
                };

            }
            return isValid;
        }
    }
}

