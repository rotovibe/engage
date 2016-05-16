using System;

namespace Phytel.Engage.Integrations.Repo.DTOs
{
    public class PatientInfo
    {
        public int? SubscriberId { get; set; }
        public int? PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Ssn { get; set; }
        public string Phone { get; set; }
        public int? Priority { get; set; }
        public string PCP { get; set; }
        public Guid? CareManagerId { get; set; }
        public string CareManagerName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; }
        public DateTime? FollowupDueDate { get;set;}
    }
}
