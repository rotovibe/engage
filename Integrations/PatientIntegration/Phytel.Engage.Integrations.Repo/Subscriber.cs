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
    
    public partial class Subscriber
    {
        public int SUBSCRIBERID { get; set; }
        public Nullable<int> PARENTID { get; set; }
        public int CONTRACTID { get; set; }
        public string Title { get; set; }
        public string SubscriberName { get; set; }
        public string Degree { get; set; }
        public int PartTimeFlag { get; set; }
        public int PrimaryCareFlag { get; set; }
        public string PrimarySpeciality { get; set; }
        public string SecondarySpeciality { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> TermDate { get; set; }
        public int Enabled { get; set; }
        public string CategoryCode { get; set; }
        public Nullable<decimal> SubscriberFTE { get; set; }
        public Nullable<decimal> SubscriberFTE_Calc { get; set; }
        public Nullable<decimal> ProviderCnt { get; set; }
        public int MaxCalls_Mon { get; set; }
        public int MaxCalls_Tue { get; set; }
        public int MaxCalls_Wed { get; set; }
        public int MaxCalls_Thu { get; set; }
        public int MaxCalls_Fri { get; set; }
        public int MaxCalls_Sat { get; set; }
        public int MaxCalls_Sun { get; set; }
        public string RecallOption { get; set; }
        public Nullable<System.DateTime> RecallReleaseTime { get; set; }
        public Nullable<int> SubscriberIDPrev { get; set; }
        public int DeleteFlag { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int InsightPatientPanel { get; set; }
    }
}
