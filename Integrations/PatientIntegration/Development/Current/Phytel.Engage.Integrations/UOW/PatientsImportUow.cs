using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.API.Common;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientsImportUow : UowBase, IImportUow
    {
        public void LoadPatientSystems(Repo.Repositories.IRepository xrepo)
        {
            var xrefsDic = xrepo.SelectAll();
            foreach (var xr in (List<PatientXref>)xrefsDic)
                PatientSystems.Add(Mapper.Map<PatientSystemData>(xr));
        }

        public void LoadPatients(Repo.Repositories.IRepository repo)
        {
            PatientDict = repo.SelectAll() as Dictionary<int, PatientInfo>;
            foreach (var pInfo in from pt in (Dictionary<int, PatientInfo>)PatientDict select pt.Value)
                Patients.Add(Mapper.Map<PatientData>(pInfo));
        }

        public void Commit(string contract)
        {
            BulkOperation(Patients, contract);
            // find all that succeeded

            List<PatientSystemData> pSys = GetPatientSystemsToLoad(PatientSystems, PatientSaveResults);
            BulkOperation(pSys, contract);
        }

        public List<PatientSystemData> GetPatientSystemsToLoad(List<PatientSystemData> patientSyss, List<HttpObjectResponse<PatientData>> PatientResults)
        {
            var pdList = new List<PatientSystemData>();
            var patientsIds = PatientResults.Where(r => r.Code == System.Net.HttpStatusCode.OK).Select(item => item.Body.AtmosphereId).ToList();
            patientsIds.ForEach(r =>
            {
                pdList.AddRange(patientSyss.FindAll(x => x.AtmosphereId == r).ToList());

                // add a registration for atmosphere
                var pd = PatientResults.Where(x => x.Body.AtmosphereId == r).Select(p => p.Body).FirstOrDefault();
                pdList.Add(new PatientSystemData
                {
                    AtmosphereId = pd.AtmosphereId,
                    PatientId = pd.Id, // what is this?
                    Value = pd.AtmosphereId, // this will set the actual value for the record.
                    CreatedOn = System.DateTime.UtcNow,
                    DataSource = "Integration",
                    SystemId = "55e47fb9d433232058923e87" // atmosphere
                });
            });

            return pdList;
        }


    }
}
