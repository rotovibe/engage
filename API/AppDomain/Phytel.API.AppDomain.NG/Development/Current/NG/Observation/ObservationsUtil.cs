using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using AD = Phytel.API.AppDomain.NG.DTO.Observation;

namespace Phytel.API.AppDomain.NG
{
    public static class ObservationsUtil
    {
        static readonly string DDPatientObservationsServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];

        internal static List<AD.PatientObservation> GetStandardObservationsForPatient(GetStandardObservationItemsRequest request, List<PatientObservationData> po)
        {
            List<AD.PatientObservation> result = new List<DTO.Observation.PatientObservation>();
            try
            {
                if (po != null && po.Count > 0)
                {
                    po.ForEach(o =>
                    {
                        result.Add(new Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation
                        {
                            EndDate = o.EndDate,
                            GroupId = o.GroupId,
                            Id = o.Id,
                            PatientId = o.PatientId,
                            Name = o.Name,
                            Standard = o.Standard,
                            StartDate = o.StartDate,
                            TypeId = o.TypeId,
                            Units = o.Units,
                            Values = GetValues(o.Values)
                        });
                    });
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialGoalRequest()" + ex.Message, ex.InnerException);
            }
        }

        private static List<ObservationValue> GetValues(List<ObservationValueData> list)
        {
            List<ObservationValue> ov = new List<ObservationValue>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(v =>
                    {
                        ov.Add(new ObservationValue
                        {
                            Id = v.Id,
                            Text = v.Text,
                            Value = v.Value
                        });
                    });
                }
                return ov;
            }
            catch (Exception ex)
            {
                throw new WebServiceException("App Domain:GetValues()" + ex.Message, ex.InnerException);
            }
        }
    }
}
