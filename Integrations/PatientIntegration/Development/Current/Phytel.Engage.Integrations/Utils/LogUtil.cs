﻿using System.Collections.Generic;
using System.Text;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using Phytel.Engage.Integrations.DomainEvents;

namespace Phytel.Engage.Integrations.Utils
{
    public static class LogUtil
    {
        public static void FormatOutputDebug<T>(IDomainResponse response)
        {
            //InsertBatchPatientsDataResponse
            //Helpers.SerializeObject<List<PatientData>>(patients as List<PatientData>, System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsExample.txt");
            //var lPsd = Helpers.DeserializeObject<List<PatientSystemData>>(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");
            StringBuilder sb = new StringBuilder();
            ((InsertBatchPatientsDataResponse)response).Responses.ForEach(r =>
            {
                sb.Append("Id: " + r.Body.Id + " ,");
                sb.Append("ExternalRecordId: " + r.Body.ExternalRecordId + " ,");
                sb.Append("EngageId: " + r.Body.EngagePatientSystemValue + " |");
            });

            LoggerDomainEvent.Raise(new LogStatus { Message = "patient save result: " + sb.ToString(), Type = LogType.Debug });
        }
    }
}
