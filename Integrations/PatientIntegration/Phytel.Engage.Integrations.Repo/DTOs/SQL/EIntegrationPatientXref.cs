using System;

namespace Phytel.Engage.Integrations.Repo.DTOs.SQL
{
    public class EIntegrationPatientXref
    {
        public string SendingApplication { get; set; }
        public string ExternalPatientID { get; set; }
        public int PhytelPatientID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public string ExternalDisplayPatientId { get; set; }
    }
}
