using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;
using Phytel.Engage.Integrations.UOW;

namespace Phytel.Engage.Integrations
{
    public class MessageProcessor : IMessageProcessor
    {
        public IIsApplicableContract<RegistryCompleteMessage> IsApplicableContract { get; set; }
        public IRepositoryFactory RepositoryFactory { get; set; }
        public IImportUow PatientsUow { get; set; }

        public void Process(RegistryCompleteMessage message)
        {
            if (IsApplicableContract.IsSatisfiedBy(message))
            {
                // get patient dictionary
                var repo = RepositoryFactory.GetRepository(message.ContractDataBase, RepositoryType.PatientsContractRepository);
                PatientsUow.LoadPatients(repo);

                // get patient xrefs
                var xrepo = RepositoryFactory.GetRepository(message.ContractDataBase, RepositoryType.XrefContractRepository);
                PatientsUow.LoadPatientSystems(xrepo);

                PatientsUow.Commit(message.ContractDataBase);
            }
        }


    }
}
