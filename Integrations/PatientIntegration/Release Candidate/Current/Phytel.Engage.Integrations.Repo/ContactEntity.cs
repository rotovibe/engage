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
    
    public partial class ContactEntity
    {
        public ContactEntity()
        {
            this.KeyDatas = new HashSet<KeyData>();
            this.Phones = new HashSet<Phone>();
            this.PatientPersonalPhysicians = new HashSet<PatientPersonalPhysician>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> LockedByCaregiverID { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string DispenserType { get; set; }
        public string CategoryCode { get; set; }
        public string RaceCode { get; set; }
        public string LanguagePreferenceCode { get; set; }
        public string Open24HoursFlag { get; set; }
        public byte SystemFlag { get; set; }
        public Nullable<byte> AllergyFlag { get; set; }
        public byte DeleteFlag { get; set; }
        public Nullable<int> MasterID { get; set; }
        public Nullable<int> DataSourceID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string HL7GUID { get; set; }
        public string CPTGuid { get; set; }
    
        public virtual ICollection<KeyData> KeyDatas { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<PatientPersonalPhysician> PatientPersonalPhysicians { get; set; }
    }
}
