using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;
using System;
namespace Phytel.API.DataDomain.PatientSystem
{
    public interface IPatientSystemRepositoryFactory
    {
        IPatientSystemRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
