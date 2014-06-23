using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Patient
{
    public interface IPatientRepositoryFactory
    {
        IPatientRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
