//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Phytel.Engage.Integrations.Repo
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatientPersonalPhysician
    {
        public int PatientPersonalPhysicianID { get; set; }
        public int PatientRegistryHdrID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<int> SubscriberID { get; set; }
        public bool ClientSpecified { get; set; }
        public string SubscriberStatus { get; set; }
        public Nullable<System.DateTime> SubscriberStatusDate { get; set; }
        public bool DeleteFlag { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual ContactEntity ContactEntity { get; set; }
    }
}
