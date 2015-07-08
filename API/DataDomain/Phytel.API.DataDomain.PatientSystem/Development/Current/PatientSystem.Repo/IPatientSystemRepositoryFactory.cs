using System;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientSystem
{
    public interface IPatientSystemRepositoryFactory
    {
        IMongoPatientSystemRepository GetRepository(RepositoryType type);
    }
}
