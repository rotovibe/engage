using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientMedSupp
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string TypeId { get; set; }
        public int StatusId { get; set; }
        public string Dosage { get; set; }
        public string Strength { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public List<string> PharmClasses { get; set; }
        public List<string> NDCs { get; set; }
        public string FreqQuantity { get; set; }
        public string FreqHowOftenId { get; set; }
        public string FreqWhenId { get; set; }
        public string SourceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
        public string PrescribedBy { get; set; }
        public string SystemName { get; set; }
        public string SigCode { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }   
    }
}
