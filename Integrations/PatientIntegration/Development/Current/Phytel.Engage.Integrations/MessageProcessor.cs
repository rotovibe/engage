using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;
using Phytel.Engage.Integrations.UOW;


namespace Phytel.Engage.Integrations
{
    public class MessageProcessor : IMessageProcessor
    {
        public IIsApplicableContract<RegistryCompleteMessage> IsApplicableContract { get; set; }
        public IRepositoryFactory RepositoryFactory { get; set; }
        public IImportUOW<PatientData> UOW { get; set; }

        public void Process(RegistryCompleteMessage message)
        {
            if (IsApplicableContract.IsSatisfiedBy(message))
            {
                // get patient dictionary
                // create a command to abstract this logic
                var repo = RepositoryFactory.GetRepository(message.ContractDataBase, RepositoryType.PatientsContractRepository);
                var patientsDic = repo.SelectAll();

                // 1) hydrate patientdata command
                foreach (var pt in (Dictionary<int, PatientInfo>) patientsDic)
                {
                    UOW.Add(new PatientData
                    {
                        FirstName = pt.Value.FirstName,
                        MiddleName = pt.Value.MiddleInitial,
                        LastName = pt.Value.LastName,
                        Suffix = pt.Value.Suffix,
                        Gender = pt.Value.Gender,
                        DOB = pt.Value.BirthDate.ToString(),
                        FullSSN = pt.Value.Ssn,
                        RecordCreatedOn = pt.Value.CreateDate.GetValueOrDefault(),
                        LastUpdatedOn = pt.Value.UpdateDate.GetValueOrDefault(),
                        StatusId = GetStatus(pt.Value.Status),
                        DataSource = "P-Reg",
                        DeceasedId = GetDeceased(pt.Value.Status)
                        //PriorityData = get color value from source.
                    });
                }

                UOW.Commit();

                // 2) put formatted patientdata objects in UOW


                // get patient xrefs
                var xrepo = RepositoryFactory.GetRepository(message.ContractDataBase, RepositoryType.XrefContractRepository);
                var xrefsDic = xrepo.SelectAll();
            }
        }

        private int GetDeceased(string p)
        {
            var val = 0;
            //None = 0,
            //Yes = 1,
            //No = 2
            if (!p.Equals("Deceased")) return val;

            switch (p)
            {
                case "Deceased":
                {
                    val = 1;
                    break;
                }
                default:
                {
                    val = 0;
                    break;
                }
            }
            return val;
        }

        private int GetStatus(string p)
        {
            int val;
            //Active = 1,
            //Inactive = 2,
            //Archived = 3

            switch (p)
            {
                case "Active":
                {
                    val = 1;
                    break;
                }
                case "Inactive":
                {
                    val = 2;
                    break;
                }
                case "Deceased":
                {
                    val = 1;
                    break;
                }
                case "Bad Debt":
                {
                    val = 1;
                    break;
                }
                default:
                {
                    val = 1;
                    break;
                }
            }
            return val;
        }

        public MessageProcessor()
        {
            RepositoryFactory = new RepositoryFactory();
            UOW = new PatientsImportUow<PatientData>();
        }
    }
}
