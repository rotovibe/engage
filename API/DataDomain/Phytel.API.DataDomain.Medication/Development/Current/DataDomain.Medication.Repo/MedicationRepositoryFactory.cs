using System;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Medication.Repo
{
    public enum RepositoryType
    {
        Medication
    }

    public abstract class MedicationRepositoryFactory
    {

        public static IMongoMedicationRepository GetMedicationRepository(string userId, string contract, RepositoryType type)
        {
            try
            {
                IMongoMedicationRepository repo = null;

                switch (type)
                {
                    case RepositoryType.Medication:
                    {
                        var context = new MedicationMongoContext(contract);
                        repo = new MongoMedicationRepository<MedicationMongoContext>(context) {UserId = userId};
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
