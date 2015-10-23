using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.DTOs.SQL;
using Phytel.Engage.Integrations.Repo.Repositories;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientsImportUow : UowBase, IImportUow
    {
        public void Commit(string contract)
        {
            try 
            { 
                LoggerDomainEvent.Raise(new LogStatus { Message = "Commit operation started.", Type = LogType.Debug });

                //1) save patients
                BulkOperation(Patients, contract, new PatientDataDomain());
                LoggerDomainEvent.Raise(new LogStatus { Message = "1) Saved patients - success", Type = LogType.Debug });

                //2) save patientsystems
                List<PatientSystemData> pSys = GetPatientSystemsToLoad(PatientSystems, PatientSaveResults);
                if (pSys != null && pSys.Count > 0) BulkOperation(pSys, contract, new PatientSystemDataDomain());
                LoggerDomainEvent.Raise(new LogStatus { Message = "2) Saved patientsystems - success", Type = LogType.Debug });

                //3) save integrationpatientxref
                var atmoXrefList = GetPatientSystemsToRegister(PatientSaveResults, PatientSystemResults);
                var repo = new RepositoryFactory().GetRepository(contract, RepositoryType.XrefContractRepository);
                if (atmoXrefList != null && atmoXrefList.Count > 0) repo.Insert(atmoXrefList.ToList());
                LoggerDomainEvent.Raise(new LogStatus { Message = "3) Register patients in IntegrationPatientXref - success", Type = LogType.Debug });

                //4) save contact info
                var contactList = GetPatientContactsToRegister(PatientSaveResults);
                if(contactList != null && contactList.Count > 0) BulkOperation(contactList, contract, new ContactDataDomain());
                LoggerDomainEvent.Raise(new LogStatus { Message = "4) Saved contact info - success", Type = LogType.Debug });

                //5) save patient notes
                var patientNotesList = GetPatientNotesToRegister(PatientSaveResults, PatientNotes);
                if (patientNotesList != null && patientNotesList.Count > 0) BulkOperation(patientNotesList, contract, new PatientNoteDataDomain());
                LoggerDomainEvent.Raise(new LogStatus { Message = "5) Saved Patient notes - success", Type = LogType.Debug });
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientsImportUow:Commit(): " + ex.Message, Type = LogType.Error });
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
                    
                    if (string.IsNullOrEmpty(ptInfo.Phone)) return;

                    list.Add( new ContactData
                                {
                                    PatientId = mongoPtId,
                                    CreatedOn = ptInfo.CreateDate != null ? Convert.ToDateTime(ptInfo.CreateDate) : default(DateTime),
                                    FirstName = ptInfo.FirstName,
                                    Gender = ptInfo.Gender,
                                    LastName = ptInfo.LastName,
                                    MiddleName = ptInfo.MiddleInitial,
                                    UserId = API.DataDomain.Patient.Constants.SystemContactId,
                                    PreferredName = ptInfo.FirstName + " " + ptInfo.LastName,
                                    Phones = new List<PhoneData>{new PhoneData{DataSource = "P-Reg", Number = Convert.ToInt64(ptInfo.Phone), TypeId = "52e18c2ed433232028e9e3a6"}}
                                });
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

        public ConcurrentBag<EIntegrationPatientXref> GetPatientSystemsToRegister(List<HttpObjectResponse<PatientData>> pRes, List<HttpObjectResponse<PatientSystemData>> pSystemRes)
        {
            var cb = new ConcurrentBag<EIntegrationPatientXref>();

            var psl = pSystemRes.Where(r => r.Code == HttpStatusCode.Created).ToList();
            var psIds = pSystemRes.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.PatientId).Distinct().ToList();

            Parallel.ForEach(psIds, r =>
            //foreach(var r in psIds)
            {
                var phytelId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.ExternalRecordId).FirstOrDefault();
                var mongoId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.Id).FirstOrDefault();
                var engageId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.EngagePatientSystemValue).FirstOrDefault();
                var httpObjectResponse = psl.FirstOrDefault(x => x.Body.PatientId == r);
                
                if (httpObjectResponse == null) return;

                cb.Add(new EIntegrationPatientXref
                {
                    CreateDate = DateTime.Now,
                    ExternalPatientID = mongoId,
                    ExternalDisplayPatientId = engageId, //"engageid"
                    PhytelPatientID = Convert.ToInt32(phytelId), //"phytelid"
                    SendingApplication = "Engage"
                });
            });

            return cb;
        }

        public List<PatientSystemData> GetPatientSystemsToLoad(List<PatientSystemData> patientSyss, List<HttpObjectResponse<PatientData>> patientResults)
        {
            try
            {
                var pdList = new ConcurrentBag<PatientSystemData>();
                var patientsIds = patientResults.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.ExternalRecordId).ToList();

                Parallel.ForEach(patientsIds, r =>
                {
                    var pslist = patientSyss.FindAll(x => x.PatientId == r).ToList();
                    var pd = patientResults.Where(x => x.Body.ExternalRecordId == r).Select(p => p.Body).FirstOrDefault();

                    // set the mongoid patient on patientsystem.
                    var fPsList = pslist.Select(c => {if (pd != null) c.PatientId = pd.Id; c.Id = null; return c;}).ToList();
                    fPsList.ForEach(x => pdList.Add(x));

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
    }
}
