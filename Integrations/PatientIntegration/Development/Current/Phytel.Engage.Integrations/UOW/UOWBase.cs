using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Engage.Integrations.Extensions;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.API.Common;

namespace Phytel.Engage.Integrations.UOW
{
    public class UowBase
    {
        public IEndpointUtil ServiceEndpoint { get; set; }
        public Dictionary<int, PatientInfo> PatientDict { get; set; }
        public List<HttpObjectResponse<PatientData>> PatientSaveResults = new List<HttpObjectResponse<PatientData>>();
        public List<HttpObjectResponse<PatientSystemData>> PatientSystemResults = new List<HttpObjectResponse<PatientSystemData>>();
        public List<PatientSystemData> PatientSystems { get; set; }
        public List<PatientData> Patients { get; set; }

        internal void BulkOperation<T>(List<T> Pocos, string contract)
        {
            try
            {
                if (Pocos.Count == 0) throw new Exception("There are no items to page in list.");

                if (Pocos.Count > 5)
                    BatchRequest(Pocos, contract);
                else
                    HandleResponse(ServiceEndpoint.SavePatientInfo(Pocos, contract));

#if DEBUG
                DBScriptGenerator.SavePatients(PatientSaveResults);
#endif
            }
            catch (Exception ex)
            {
                throw new ArgumentException("UowBase:BulkOperation(): " + ex.Message);
            }
        }

        public void BatchRequest<T>(List<T> Pocos, string contract)
        {
            var take = Convert.ToInt32(Math.Round(Pocos.Count*.25, 0, MidpointRounding.AwayFromZero));
            var count = 0;
            var pages = Pocos.Pages(take);
            for (var i = 0; i <= pages; i++)
            {
                if (count == Pocos.Count) break;
                var savePatients = Pocos.Batch(take).ToList()[i];

                HandleResponse(ServiceEndpoint.SavePatientInfo(savePatients, contract));
                
                count = count + savePatients.Count();
            }
        }

        public void HandleResponse<T>(List<T> list)
        {
            if (list.GetType().Equals(typeof(List<HttpObjectResponse<PatientSystemData>>)))
            {
                PatientSystemResults.AddRange(list as List<HttpObjectResponse<PatientSystemData>>);
            }
            else if (list.GetType().Equals(typeof(List<HttpObjectResponse<PatientData>>)))
            {
                PatientSaveResults.AddRange(list as List<HttpObjectResponse<PatientData>>);
            }
        }
    }
}
