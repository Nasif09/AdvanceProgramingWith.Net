//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroHunger.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class DistributionRecord
    {
        public int DID { get; set; }
        public int RequestID { get; set; }
        public string Status { get; set; }
    
        public virtual CollectRequest CollectRequest { get; set; }
    }
}