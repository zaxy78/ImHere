//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImHereNonProfit.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class BasicUser
    {
        public BasicUser()
        {
            this.Donations = new HashSet<Donation>();
        }
    
        public int Id { get; set; }
        public UsersTypes UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Password { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
