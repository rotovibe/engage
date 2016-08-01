using System;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Medication.Repo
{
    public enum RepositoryType
    {
        Medication,
        PatientMedSupp,
        MedicationMapping,
        PatientMedFrequency
    }

    public abstract class MedicationRepositoryFactory
    {

        public static IMongoMedicationRepository GetMedicationRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoMedicationRepository repo = null;

                switch (type)
                {
                    case RepositoryType.Medication:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new MongoMedicationRepository<MedicationMongoContext>(context) {UserId = request.UserId, ContractDBName = request.ContractNumber};
                        break;
                    }
                    case RepositoryType.MedicationMapping:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new MongoMedicationMappingRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                    case RepositoryType.PatientMedSupp:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new MongoPatientMedSuppRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                    case RepositoryType.PatientMedFrequency:
                    {
                        var context = new MedicationMongoContext(request.ContractNumber);
                        repo = new MongoPatientMedFrequencyRepository<MedicationMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
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
