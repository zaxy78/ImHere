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
    
    public partial class ExecutiveUser : MemberUser
    {
        public ExecutiveUser()
        {
            this.ChairOf = new HashSet<Committee>();
        }
    
    
        public virtual ICollection<Committee> ChairOf { get; set; }
    }
}
