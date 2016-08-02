using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Extensions;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;
using ServiceStack.Common;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientsImportUow : UowBase, IImportUow
    {
        public IRepositoryFactory RepositoryFactory { get; set; }

        public void Initialize(string contractDb)
        {
            LoggerDomainEvent.Raise(LogStatus.Create("Initializing Integration process for : [" + contractDb + "] contract", true));
            InitPCPPhones(contractDb, RepositoryFactory);
            InitPatients(contractDb, RepositoryFactory);
            InitPatientSystems(contractDb, RepositoryFactory);
            InitPatientNotes(contractDb, RepositoryFactory);
        }

        public void Commit(string contract)
        {
            try
            {
                LoggerDomainEvent.Raise(new LogStatus {Message = "Commit operation started.", Type = LogType.Debug});

                // initialize save results
                if (PatientSaveResults != null) PatientSaveResults.Clear();
                PatientSaveResults = new List<HttpObjectResponse<PatientData>>();

                if (PatientSystemResults != null) PatientSystemResults.Clear();
                PatientSystemResults = new List<HttpObjectResponse<PatientSystemData>>();

                //1) save patients
                AssignPatientPCPContactInfo(Patients, PCPPhones);
                BulkOperation(Patients, contract, new PatientDataDomain(), "patients");

                //2) save patientsystems
                List<PatientSystemData> pSys = GetPatientSystemsToLoad(PatientSystems, PatientSaveResults);
                if (pSys != null && pSys.Count > 0)
                    BulkOperation(pSys, contract, new PatientSystemDataDomain(), "patient systems");

                //3) save contact info
                var contactList = GetPatientContactsToRegister(PatientSaveResults);
                if (contactList != null && contactList.Count > 0)
                    BulkOperation(contactList, contract, new ContactDataDomain(), "contact info");

                //4) save patient notes
                var patientNotesList = GetPatientNotesToRegister(PatientSaveResults, PatientNotes);
                if (patientNotesList != null && patientNotesList.Count > 0)
                    BulkOperation(patientNotesList, contract, new PatientNoteDataDomain(), "patient notes");

                // 5) save todos from patientnoteslist
                if (!ParseToDosSpec.IsSatisfiedBy(contract)) return;
                //refactor to include command pattern!!!!!!
                var toDoList = ParseToDos(PatientSaveResults);
                if (toDoList != null && toDoList.Count > 0)
                    BulkOperation(toDoList, contract, new ToDoDataDomain(), "to dos");
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:Commit(): " + ex.Message, Type = LogType.Error });
                throw;
            }
        }

        private void AssignPatientPCPContactInfo(List<PatientData> Patients, List<PCPPhone> PCPPhones)
        {
            try
            {
                Patients.ForEach(p =>
                {
                    var phones = PCPPhones.FindAll(f => f.PatientID == Convert.ToInt64(p.ExternalRecordId));
                    StringBuilder sb = new StringBuilder();
                    if (phones != null && phones.Count > 0)
                    {
                        phones.ForEach(
                            pd =>
                            {
                                if (!pd.PCP_Name.IsNullOrEmpty())
                                    sb.Append(pd.PCP_Name + " | " + pd.Phone + " | " + pd.Facility + "; ");
                            });
                    }

                    p.ClinicalBackground = sb.ToString();
                    if (p.ClinicalBackground.Equals(String.Empty)) p.ClinicalBackground = null;
                });
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:AssignPatientPCPContactInfo(): " + ex.Message, Type = LogType.Error });
                throw;
            }
        }

        public List<ContactData> GetPatientContactsToRegister(List<HttpObjectResponse<PatientData>> saveResults)
        {
            try
            {
                var list = new ConcurrentBag<ContactData>();
                if (PatientDict == null) return list.ToList();
                var psIds = saveResults.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.ExternalRecordId).Distinct().ToList();
                Parallel.ForEach(psIds, id =>
                {
                    var ptInfo = PatientDict[Convert.ToInt32(id)];
                    var mongoPtId = saveResults.Find(pt => pt.Body.ExternalRecordId == id).Body.Id;

                    var contactData = new ContactData
                    {
                        PatientId = mongoPtId,
                        CreatedOn = ptInfo.CreateDate != null ? Convert.ToDateTime(ptInfo.CreateDate) : default(DateTime),
                        RecentsList = null,
                        ExternalRecordId = id,
                        FirstName = ptInfo.FirstName,
                        LastName = ptInfo.LastName,
                        MiddleName = ptInfo.MiddleInitial,
                        Gender = PatientInfoUtils.FormatGender(ptInfo.Gender),
                        Suffix = ptInfo.Suffix,
                        StatusId = PatientInfoUtils.GetStatus(ptInfo.Status),
                        DeceasedId = PatientInfoUtils.GetDeceased(ptInfo.Status),
                        DataSource = "P-Reg",
                        ContactTypeId = Phytel.API.DataDomain.Contact.DTO.Constants.PersonContactTypeId
                    };
                    if (!string.IsNullOrEmpty(ptInfo.Phone) && ptInfo.Phone.Length == 10)
                    {
                        contactData.Phones = new List<PhoneData>
                        {
                            new PhoneData
                            {
                                DataSource = "P-Reg",
                                Number = Convert.ToInt64(ptInfo.Phone),
                                TypeId = "52e18c2ed433232028e9e3a6"
                            }
                        };
                    }
                    list.Add(contactData);
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:GetPatientContactsToRegister(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientsImportUow:GetPatientContactsToRegister(): " + ex.Message);
            }
        }

        private List<PatientNoteData> GetPatientNotesToRegister(List<HttpObjectResponse<PatientData>> patientSaveResults, List<PatientNoteData> patientNotes)
        {
            try
            {
                var cb = new ConcurrentBag<PatientNoteData>();
                var psl = patientSaveResults.Where(r => r.Code == HttpStatusCode.Created).ToList();
                var psIds = patientSaveResults.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.ExternalRecordId).Distinct().ToList();

                Parallel.ForEach(psIds, id =>
                    //foreach (var id in psIds)
                {
                    var pMId = psl.Find(x => x.Body.ExternalRecordId == id).Body.Id;
                    var pn = patientNotes.FindAll(r => r.PatientId == id);
                    var pnF = pn.Select(c => { c.PatientId = pMId; return c; }).ToList();
                    if (pnF.Count > 0)
                        pnF.ForEach(n => cb.Add(n));
                });

                return cb.ToList();
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:GetPatientNotesToRegister(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientsImportUow:GetPatientNotesToRegister(): " + ex.Message);
            }
        }

        public List<PatientSystemData> GetPatientSystemsToLoad(List<PatientSystemData> patientSyss, List<HttpObjectResponse<PatientData>> patientResults)
        {
            try
            {
                var pdList = new ConcurrentBag<PatientSystemData>();
                var patientsIds = patientResults.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.ExternalRecordId).ToList();

                //Parallel.ForEach(patientsIds, r =>
                patientsIds.ForEach( r =>
                {
                    // find all relevant patient system entries.
                    var pslist = patientSyss.FindAll(x => x.PatientId == r).ToList();
                    var pd = patientResults.Where(x => x.Body.ExternalRecordId == r).Select(p => p.Body).FirstOrDefault();


                    // set the mongoid patient on patientsystem.
                    var fPsList = pslist.Select(c => {if (pd != null) c.PatientId = pd.Id; c.Id = null; return c;}).ToList();

                    var list = fPsList.OrderByDescending(x => x.CreatedOn).OrderedDistinct(new PsDMyEqualityComparer()).ToList();
                    list.ForEach(x => pdList.Add(x));

                    // add a registration for atmosphere
                    pdList.Add(new PatientSystemData
                    {
                        ExternalRecordId = Guid.NewGuid().ToString(),
                        PatientId = pd.Id, // what is this?
                        Value = pd.ExternalRecordId, // this will set the actual value for the record.
                        CreatedOn = DateTime.UtcNow,
                        CreatedById = API.DataDomain.Patient.Constants.SystemContactId,
                        DataSource = "P-Reg",//"Integration",
                        SystemId = "55e47fb9d433232058923e87", // atmosphere
                        StatusId = 1
                    });
                });

                return pdList.ToList();
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:GetPatientSystemsToLoad(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientsImportUow:GetPatientSystemsToLoad(): " + ex.Message);
            }
        }

        public void PatientBackgroundModification(string contract, string ddlPath)
        {
            var _patientDd = new PatientDataDomain();
            var _phones = InitPatientPhonesGeneral(contract, RepositoryFactory); // get a different set that is not filtered
            List<string> _patientIds = _patientDd.GetAllPatientIds(contract, ddlPath).Select(p => p.Id).ToList(); // web call to get all patients in contract
            List<PatientData> _patients = _patientDd.getAllPatientsById(_patientIds, ddlPath, contract);

            _patients.ForEach(pt =>
            {
                var pphones = _phones.FindAll(f => f.PatientID == Convert.ToInt64(pt.ExternalRecordId));
                StringBuilder sb = new StringBuilder();
                if (pphones != null && pphones.Count > 0)
                {
                    pphones.ForEach(
                        pd =>
                        {
                            if (!pd.PCP_Name.IsNullOrEmpty())
                                sb.Append(pd.PCP_Name + " | " + pd.Phone + " | " + pd.Facility + "; ");
                        });
                }

                pt.ClinicalBackground = sb.ToString();
                if (pt.ClinicalBackground.Equals(String.Empty)) pt.ClinicalBackground = null;
            });

            // save back to patient DD as updates
            _patientDd.Update(_patients, contract, ddlPath);
        }
    }
}
