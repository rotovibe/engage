using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ProgramInfo
    {
        public string Id { get; set; }
        public string ProgramSourceId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }
        public int ElementState { get; set; }
        public int ProgramState { get; set; }
        public string PatientId { get; set; }
        public DateTime? AttrEndDate { get; set; }
    }
}
