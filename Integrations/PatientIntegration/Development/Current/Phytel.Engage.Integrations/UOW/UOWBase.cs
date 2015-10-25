using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Engage.Integrations.Extensions;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Repo.DTO;

namespace Phytel.Engage.Integrations.UOW
{
    public class UowBase
    {
        public IDataDomain ServiceEndpoint { get; set; }
        public Dictionary<int, PatientInfo> PatientDict { get; set; }
        public List<HttpObjectResponse<PatientData>> PatientSaveResults = new List<HttpObjectResponse<PatientData>>();
        public List<HttpObjectResponse<PatientSystemData>> PatientSystemResults = new List<HttpObjectResponse<PatientSystemData>>();
        public List<PatientSystemData> PatientSystems { get; set; }
        public List<PatientNoteData> PatientNotes { get; set; }
        public List<PatientData> Patients { get; set; }
        public List<PCPPhone> PCPPhones { get; set; }

        internal void BulkOperation<T>(List<T> pocos, string contract, IDataDomain domain)
        {
            try
            {
                if (pocos.Count == 0) throw new Exception("There are no items to page in list.");

                if (pocos.Count > 5 && pocos.Count > ProcConstants.TakeCount)
                {
                    LoggerDomainEvent.Raise(LogStatus.Create("Handling " + pocos.Count + " records in batches.", true));
                    BatchRequest(pocos, contract, domain);
                }
                else
                    HandleResponse(domain.Save(pocos, contract));
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
                int take = ProcConstants.TakeCount;
                var count = 0;
                var pages = pocos.Pages(take);
                for (var i = 0; i <= pages; i++)
                {
                    if (count == pocos.Count) break;
                    var savePatients = pocos.Batch(take).ToList()[i];

                    var enumerable = savePatients as IList<T> ?? savePatients.ToList();
                    HandleResponse(domain.Save(enumerable, contract));

                    count = count + enumerable.Count();
                    LoggerDomainEvent.Raise(LogStatus.Create("Patients saved:" + count, true));
                }
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:BatchRequest(): " + ex.Message, false));
                throw new ArgumentException("UowBase:BatchRequest(): " + ex.Message);
            }
        }

        public void HandleResponse<T>(T list)
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
                }
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UowBase:HandleResponse(): " + ex.Message, false));
                throw new ArgumentException("UowBase:HandleResponse(): " + ex.Message);
            }
        }

        public void LoadPatientSystems(Repo.Repositories.IRepository xrepo, List<PatientSystemData> systems)
        {
            try
            {
                var xrefsDic = xrepo.SelectAll();
                systems.AddRange(from xr in (List<PatientXref>) xrefsDic select Mapper.Map<PatientSystemData>(xr));

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
                pcpPhones.AddRange(from xr in (List<PCPPhone>)phnList select xr);

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
                notes.AddRange(
                    ((List<PatientNote>) pNotesL).Select(
                        pn => new {pn, patient = pats.Find(r => r.ExternalRecordId == pn.PatientId.ToString())})
                        .Where(@t => @t.patient != null)
                        .Select(@t => ObjMapper.MapPatientNote(@t.patient.Id, @t.pn)));
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("UOWBase: LoadPatientNotes():" + ex.Message, false));
            }
        }
    }
}
