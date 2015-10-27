using System;

namespace Phytel.Engage.Integrations.Repo.DTO
{
    public class PatientXref
    {
        public int ID { get; set; }
        public int? PhytelPatientID { get; set; }
        public string ExternalDisplayPatientId { get; set; }
        public string ExternalPatientID { get; set; }
        public string SendingApplication { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
