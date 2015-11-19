using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Extensions;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Engage.Integrations.Repo.DTOs.SQL;
using Phytel.Engage.Integrations.Repo.Repositories;
using RepositoryType = Phytel.Engage.Integrations.Repo.Repositories.RepositoryType;

namespace Phytel.Engage.Integrations.UOW
{
    public class UowBase
    {
        public IDataDomain ServiceEndpoint { get; set; }
        public Dictionary<int, PatientInfo> PatientDict { get; set; }
        public List<HttpObjectResponse<PatientData>> PatientSaveResults;
        public List<HttpObjectResponse<PatientSystemData>> PatientSystemResults;
        public List<PatientSystemData> PatientSystems { get; set; }
        public List<PatientNoteData> PatientNotes { get; set; }
        public List<PatientData> Patients { get; set; }
        public List<PCPPhone> PCPPhones { get; set; }
        public List<ToDoData> ToDos { get; set; }

        internal void BulkOperation<T>(List<T> pocos, string contract, IDataDomain domain)
        {
            try
            {
                if (pocos.Count == 0) throw new Exception("There are no items to page in list.");

                if (pocos.Count > 5 && pocos.Count > ProcConstants.TakeCount)
                {
                    LoggerDomainEvent.Raise(LogStatus.Create("[Batch Process]: Handling " + pocos.Count + " records in batches.", true));
                    BatchRequest(pocos, contract, domain);
                }
                else
                    HandleResponse(domain.Save(pocos, contract), contract);
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:BulkOperation(): " + ex.Message, false));
                throw new ArgumentException("UowBase:BulkOperation(): " + ex.Message);
            }
        }

        public void BatchRequest<T>(List<T> pocos, string contract, IDataDomain domain)
        {
            try
            {
                var take = ProcConstants.TakeCount;
                var count = 0;
                var pages = pocos.Pages(take);
                for (var i = 0; i <= pages; i++)
                {
                    try
                    {
                        if (count == pocos.Count) break;
                        var savePatients = pocos.Batch(take).ToList()[i];

                        var enumerable = savePatients as IList<T> ?? savePatients.ToList();
                        FormatStatusResponse(savePatients.ToList(), "saving");
                        HandleResponse(domain.Save(enumerable, contract), contract);

                        count = count + enumerable.Count();
                        LoggerDomainEvent.Raise(LogStatus.Create("Patients saved:" + count, true));
                    }
                    catch (Exception ex)
                    {
                        LoggerDomainEvent.Raise(LogStatus.Create("Failure to save batch ["+ i +"]:" + count + " " + ex.Message, false));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:BatchRequest(): " + ex.Message, false));
                throw new ArgumentException("UowBase:BatchRequest(): " + ex.Message);
            }
        }

        public void HandleResponse<T>(T list, string contract)
        {
            try
            {
                if (list == null) throw new Exception("response list is null.");
                if (list.GetType() == typeof (List<HttpObjectResponse<PatientSystemData>>))
                {
                    PatientSystemResults.AddRange(list as List<HttpObjectResponse<PatientSystemData>>);
                }
                else if (list.GetType() == typeof (List<HttpObjectResponse<PatientData>>))
                {
                    PatientSaveResults.AddRange(list as List<HttpObjectResponse<PatientData>>);
                    SaveIntegrationXref(list, contract);
                    FormatStatusResponse(list, "saved");
                }
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:HandleResponse(): " + ex.Message, false));
                throw new ArgumentException("UowBase:HandleResponse(): " + ex.Message);
            }
        }

        public void FormatStatusResponse<T>(T list, string action)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var l = list as List<HttpObjectResponse<PatientData>>;
                l.ForEach(r => sb.Append(r.Body.ExternalRecordId + ", "));
                LoggerDomainEvent.Raise(LogStatus.Create("[Batch Process]: " + l.Count + " Patients "+ action +"  : (" + sb.ToString() +")", true));
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:FormatStatusResponse(): " + ex.Message, false));
                throw new ArgumentException("UowBase:FormatStatusResponse(): " + ex.Message);                
            }
        }

        private void SaveIntegrationXref<T>(T list, string contract)
        {
            // save integrationpatientxref
            var atmoXrefList = GetIntegrationXrefToSave(list as List<HttpObjectResponse<PatientData>>);
            var repo = new RepositoryFactory().GetRepository(contract, RepositoryType.XrefContractRepository);
            if (atmoXrefList != null && atmoXrefList.Count > 0) repo.Insert(atmoXrefList.ToList());
            LoggerDomainEvent.Raise(new LogStatus{ Message = "Register patients in IntegrationPatientXref - success", Type = LogType.Debug });
        }

        public ConcurrentBag<EIntegrationPatientXref> GetIntegrationXrefToSave(List<HttpObjectResponse<PatientData>> pRes)
        {
            var cb = new ConcurrentBag<EIntegrationPatientXref>();

            //var psl = pSystemRes.Where(r => r.Code == HttpStatusCode.Created).ToList();
            var pIds = pRes.Where(r => r.Code == HttpStatusCode.Created).Select(item => item.Body.Id).Distinct().ToList();

            Parallel.ForEach(pIds, r =>
            //foreach(var r in pIds)
            {
                var phytelId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.ExternalRecordId).FirstOrDefault();
                var mongoId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.Id).FirstOrDefault();
                var engageId = pRes.Where(x => x.Body.Id == r).Select(y => y.Body.EngagePatientSystemValue).FirstOrDefault();
                //var httpObjectResponse = psl.FirstOrDefault(x => x.Body.PatientId == r);

               // if (httpObjectResponse == null) return;

                cb.Add(new EIntegrationPatientXref
                {
                    CreateDate = PatientInfoUtils.CstConvert(DateTime.UtcNow),
                    ExternalPatientID = mongoId,
                    ExternalDisplayPatientId = engageId, //"engageid"
                    PhytelPatientID = Convert.ToInt32(phytelId), //"phytelid"
                    SendingApplication = "Engage"
                });
            });

            return cb;
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

        public void LoadPatientSystems(Repo.Repositories.IRepository xrepo, List<PatientSystemData> systems)
        {
            try
            {
                var xrefsDic = xrepo.SelectAll();
                systems.AddRange(from xr in (List<PatientXref>) xrefsDic select Mapper.Map<PatientSystemData>(xr));
                foreach (var t in systems)
                {
                    t.CreatedOn = t.CreatedOn.ToUniversalTime();
                    if (t.UpdatedOn.HasValue)
                    {
                        t.UpdatedById = ProcConstants.UserId; 
                        t.UpdatedOn = t.UpdatedOn.Value.ToUniversalTime();
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: LoadPatientSystems():" + ex.Message, false));
            }
        }

        public void LoadPcpPhones(Repo.Repositories.IRepository xrepo, List<PCPPhone> pcpPhones)
        {
            try
            {
                var phnList = xrepo.SelectAll();
                var final = ((List<PCPPhone>) phnList).Where(x => x.Phone.Length == 10);
                pcpPhones.AddRange(from xr in final select xr);
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: LoadPcpPhones():" + ex.Message, false));
            }
        }

        public void LoadPatients(Repo.Repositories.IRepository repo, List<PatientData> pats)
        {
            try
            {
                PatientDict = repo.SelectAll() as Dictionary<int, PatientInfo>;
                pats.AddRange((from pt in PatientDict select pt.Value).Select(ObjMapper.MapPatientData));

                foreach (var t in pats)
                {
                    t.RecordCreatedOn = t.RecordCreatedOn.ToUniversalTime();
                    if (!t.LastUpdatedOn.HasValue) continue;
                    t.UpdatedByProperty = ProcConstants.UserId;
                    t.LastUpdatedOn = t.LastUpdatedOn.Value.ToUniversalTime();
                }
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: LoadPatients():" + ex.Message, false));
            }
        }

        public void LoadPatientNotes(Repo.Repositories.IRepository repo, List<PatientData> pats, List<PatientNoteData> notes)
        {
            try
            {
                var pNotesL = repo.SelectAll();
                var valid =
                    ((List<PatientNote>) pNotesL).Select(
                        pn => new {pn, patient = pats.Find(r => r.ExternalRecordId == pn.PatientId.ToString())})
                        .Where(@t => @t.patient != null)
                        .Select(@t => ObjMapper.MapPatientNote(@t.patient.Id, @t.pn));

                notes.AddRange(valid);
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: LoadPatientNotes():" + ex.Message, false));
            }
        }

        public List<ToDoData> ParseToDos(List<HttpObjectResponse<PatientData>> pRes)
        {
            try
            {
                var list = new List<ToDoData>();

                pRes.ForEach(x =>
                {
                    var ptid = Convert.ToInt32(x.Body.ExternalRecordId);
                    var pt = PatientDict[ptid];
                    var followUpDate = pt.FollowupDueDate.HasValue ? pt.FollowupDueDate.Value.ToShortDateString() : string.Empty;

                    if (String.IsNullOrEmpty(followUpDate)) return;
                    var fDate = pt.FollowupDueDate.Value;
                    var val =  fDate > DateTime.UtcNow;
                    if (val)
                    {
                        list.Add(new ToDoData
                        {
                            DueDate = fDate.AddHours(12) , // add 12hrs to make it land of the middle of the day.
                            PatientId = x.Body.Id,
                            Description = "Follow-up initiated in Coordinate",
                            Title = "Follow-up Date",
                            CategoryId = "562e8f8ad4332315e0a4fffa", //follow up category
                            PriorityId = 0, // notset
                            StatusId = 1, // Open
                            CreatedById = ProcConstants.UserId,
                            CreatedOn = DateTime.UtcNow
                        });
                    }
                });
                return list;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: ParseToDos():" + ex.Message, false));
                throw;
            }
        }
    }
}
