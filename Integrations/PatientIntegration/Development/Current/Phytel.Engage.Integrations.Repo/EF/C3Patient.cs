
namespace Phytel.Engage.Integrations.Repo.EF
{
    using System;
    using System.Collections.Generic;

    public class C3Patient
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string SubscriberName { get; set; }
        public string PMSID { get; set; }
        public string Payer { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> Age { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<System.DateTime> UpcomingBirthdayDate { get; set; }
        public string PatientStatus { get; set; }
        public int PatientStatusID { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.DateTime> FollowupDueDate { get; set; }
        public Nullable<System.DateTime> LastCommunicationDate { get; set; }
        public string LastCommunicationMode { get; set; }
        public Nullable<int> AlertRecommendationCount { get; set; }
        public Nullable<int> LastPatientNoteID { get; set; }
        public Nullable<System.Guid> CareManagerID { get; set; }
        public string CareManagerName { get; set; }
        public Nullable<System.DateTime> LastAppointmentDate { get; set; }
        public Nullable<System.DateTime> NextAppointmentDate { get; set; }
        public Nullable<int> ProblemListSortOrder { get; set; }
        public string MedicationAllergies { get; set; }
        public Nullable<int> MedicationAllergiesSortOrder { get; set; }
        public string Allergies { get; set; }
        public string Medications { get; set; }
        public Nullable<int> MedicationSortOrder { get; set; }
        public string CommunicationPreference { get; set; }
        public Nullable<bool> OutreachException { get; set; }
        public string Address { get; set; }
    }
}
