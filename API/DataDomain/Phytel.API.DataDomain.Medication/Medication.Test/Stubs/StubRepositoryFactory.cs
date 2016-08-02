using System;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Medication.Test
{
    public abstract class StubRepositoryFactory
    {

        public static IMongoMedicationRepository GetMedicationRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoMedicationRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientMedSupp:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new StubPatientMedSuppRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                    case RepositoryType.Medication:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new StubMedicationRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                    case RepositoryType.MedicationMapping:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new StubMedicationMappingRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                }
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
