//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Woooo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentNote
    {
        public long NoteId { get; set; }
        public long PaymentId { get; set; }
        public string NoteType { get; set; }
        public string NoteDetails { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
    
        public virtual Payment Payment { get; set; }
    }
}
