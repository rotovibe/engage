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
    
    public partial class ReportPatient
    {
        public int ReportPatientId { get; set; }
        public int PatientID { get; set; }
        public string PMSID { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public Nullable<int> SubscriberID { get; set; }
        public string ProviderName { get; set; }
        public Nullable<int> InsurerId { get; set; }
        public string Insurer { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> AttributionTypeId { get; set; }
        public Nullable<System.DateTime> FollowupDueDate { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Notes { get; set; }
        public int PatientStatusId { get; set; }
        public string Race { get; set; }
        public string LanguagePreference { get; set; }
        public bool RegistryFlag { get; set; }
        public Nullable<System.Guid> CareManagerID { get; set; }
        public string CareManagerName { get; set; }
    }
}
