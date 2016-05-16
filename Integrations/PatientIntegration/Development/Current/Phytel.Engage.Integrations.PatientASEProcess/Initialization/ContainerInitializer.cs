using System.Collections.Generic;
using Funq;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Commands;
using Phytel.Engage.Integrations.Configurations;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.QueueProcess;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;
using Phytel.Engage.Integrations.UOW;
using Phytel.Engage.Integrations.Repo.Connections;

namespace Phytel.Engage.Integrations.Process.Initialization
{
    public class ContainerInitializer : IInitializer<Container>
    {
        public Container Build()
        {
            var container = new Container();

            container.RegisterAutoWiredAs<SQLConnectionProvider, ISQLConnectionProvider>();
            container.RegisterAutoWiredAs<GetSendingApplicationId, IIntegrationCommand<string, string>>();
            //container.RegisterAutoWiredAs<RepositoryFactory, IRepositoryFactory>();

            container.Register<IImportUow>(
                c =>
                    new PatientsImportUow
                    {
                        ServiceEndpoint = new PatientDataDomain(),
                        Patients = new List<PatientData>(),
                        PatientSystems = new List<PatientSystemData>(),
                        RepositoryFactory = new RepositoryFactory(),
                        ParseToDosSpec = new ParseToDosSpecification<string>()
                    });

            container.RegisterAutoWiredAs<ApplicableContractProvider, IApplicableContractProvider>();

            container.Register<IIsApplicableContract<RegistryCompleteMessage>>(
                c => new IsApplicableContractSpecification<RegistryCompleteMessage> { ContractProvider = new ApplicableContractProvider()});

            container.RegisterAutoWiredAs<MessageProcessor, IMessageProcessor>();
            
            return container;
        }
    }
}
