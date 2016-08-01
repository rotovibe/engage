using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class ToDo
    {
        public string Id { get; set; }
        public string CreatedById { get; set; }
        public string AssignedToId { get; set; }
        public string PatientId { get; set; }
        public PatientDetails PatientDetails { get; set; }
        public int StatusId { get; set; }
        public string CategoryId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartTime { get; set; }
        public int? Duration { get; set; } 
        public DateTime? ClosedDate { get; set; }
        public List<string> ProgramIds { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool DeleteFlag { get; set; }
    }

    public class PatientDetails
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
    }

}
