using Phytel.API.Interface;
using System;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.DataDomain.Program
{
    public interface IProgramRepositoryFactory
    {
        IProgramRepository GetRepository(IDataDomainRequest request, RepositoryType type);
        //IProgramRepository GetContractProgramRepository(Phytel.API.Interface.IDataDomainRequest request);
        //IProgramRepository GetPatientProgramRepository(Phytel.API.Interface.IDataDomainRequest request);
        //IProgramRepository GetPatientProgramStepResponseRepository(Phytel.API.Interface.IDataDomainRequest request);
        //IProgramRepository GetProgramAttributesRepository(Phytel.API.Interface.IDataDomainRequest request);
        //IProgramRepository GetProgramRepository(Phytel.API.Interface.IDataDomainRequest request);
        //IProgramRepository GetStepResponseRepository(Phytel.API.Interface.IDataDomainRequest request);
    }
}
