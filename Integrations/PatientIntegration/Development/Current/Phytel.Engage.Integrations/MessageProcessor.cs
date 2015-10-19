using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
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
            LoggerDomainEvent.Raise(LogStatus.Create("Initializing Entity records from Atmosphere...", true));

            try
            {
                if (!IsApplicableContract.IsSatisfiedBy(message)) return;

                // load patient dictionary
                var repo = RepositoryFactory.GetRepository(message.ContractDataBase,RepositoryType.PatientsContractRepository);
                PatientsUow.LoadPatients(repo, PatientsUow.Patients = new List<PatientData>());

                // load patient xrefs
                var xrepo = RepositoryFactory.GetRepository(message.ContractDataBase,RepositoryType.XrefContractRepository);
                PatientsUow.LoadPatientSystems(xrepo, PatientsUow.PatientSystems = new List<PatientSystemData>());

                // load patient notes
                var pnRepo = RepositoryFactory.GetRepository(message.ContractDataBase,RepositoryType.PatientNotesRepository);
                PatientsUow.LoadPatientNotes(pnRepo, PatientsUow.Patients, PatientsUow.PatientNotes = new List<PatientNoteData>());

                PatientsUow.Commit(message.ContractDataBase);
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("MessageProcessor:Process(): " + ex.Message, false));
                throw;
            }
        }
    }
}
