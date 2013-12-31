using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program
{
    public interface IProgramRepository<T> : IRepository<T>
    {

        List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request);
        PutProgramToPatientResponse InsertPatientToProgramAssignment(PutProgramToPatientRequest request);
        GetProgramDetailsSummaryResponse GetPatientProgramDocumentDetailsById(GetProgramDetailsSummaryRequest request);
    }
}
