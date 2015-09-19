using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;


namespace Phytel.Engage.Integrations
{
    public class MessageProcessor : IMessageProcessor
    {
        public IIsApplicableContract<RegistryCompleteMessage> IsApplicableContract { get; set; }
        public IRepositoryFactory RepositoryFactory { get; set; }

        public void Process(RegistryCompleteMessage message)
        {
            if (IsApplicableContract.IsSatisfiedBy(message))
            {
                // get patient dictionary
                var repo = RepositoryFactory.GetRepository(message.ContractDataBase, RepositoryType.PatientsContractRepository);
                var patientsDic = repo.SelectAll();
            }
        }

        public MessageProcessor()
        {
            RepositoryFactory = new RepositoryFactory();
        }
    }
}
