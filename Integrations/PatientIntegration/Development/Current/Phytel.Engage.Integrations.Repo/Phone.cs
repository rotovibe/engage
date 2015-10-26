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
    
    public partial class Phone
    {
        public int PhoneID { get; set; }
        public int OwnerID { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumberString { get; set; }
        public string DialString { get; set; }
        public string Extension { get; set; }
        public Nullable<int> ExtDialDelay { get; set; }
        public string CategoryCode { get; set; }
        public string PagerPIN { get; set; }
        public string PagerTypeCode { get; set; }
        public byte Preferred { get; set; }
        public byte CallerIDFlag { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> DataSourceID { get; set; }
        public Nullable<byte> DeleteFlag { get; set; }
        public Nullable<int> PreferenceSourceID { get; set; }
    
        public virtual ContactEntity ContactEntity { get; set; }
    }
}