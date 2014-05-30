using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation
{
    public static class ObservationUtil
    {
        public static PatientObservationData MakeAdditionalObservation(GetAdditionalObservationDataItemRequest request, IPatientObservationRepository<GetAdditionalObservationDataItemResponse> repo, ObservationData od)
        {
            try
            {
                PatientObservationData pod = CreatePatientObservationData(request, od);
                ObservationValueData ovd = PatientObservationDataManager.InitializePatientObservation(request, request.PatientId, pod.Values, od, request.SetId);

                // account for composite BP observation
                if (pod.GroupId != null && pod.GroupId.Equals("530cb50dfe7a591ee4a58c51"))
                {
                    string observationId = string.Empty;
                    observationId = od.Id.Equals("530c26fcfe7a592f64473e37") ? "530c270afe7a592f64473e38" : "530c26fcfe7a592f64473e37";
                    ObservationData od2 = (ObservationData)repo.FindByID(observationId);
                    PatientObservationData pod2 = CreatePatientObservationData(request, od2);
                    ObservationValueData ovd2 = PatientObservationDataManager.InitializePatientObservation(request, request.PatientId, pod.Values, od2, request.SetId);
                }
                return pod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PatientObservationData CreatePatientObservationData(GetAdditionalObservationDataItemRequest request, ObservationData od)
        {
            PatientObservationData pod = new PatientObservationData
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ObservationId = od.Id,
                Name = od.CommonName ?? od.Description,
                Order = od.Order,
                Standard = od.Standard,
                GroupId = od.GroupId,
                Units = od.Units,
                Values = new List<ObservationValueData>(),
                TypeId = od.ObservationTypeId,
                PatientId = request.PatientId,
                Source = od.Source
            };
            return pod;
        }

        public static void FindAndInsert(List<PatientObservationData> podl, string gid, ObservationValueData ovd)
        {
            try
            {
                if (podl != null && podl.Count > 0)
                {
                    foreach (PatientObservationData p in podl)
                    {
                        if (p.GroupId != null)
                        {
                            if (p.GroupId.Equals(gid))
                            {
                                if (!p.Values.Exists(x => x.Id == ovd.Id)) p.Values.Add(ovd);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool GroupExists(List<PatientObservationData> list, string gid)
        {
            bool result = false;
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (PatientObservationData p in list)
                    {
                        if (p.GroupId != null)
                        {
                            if (p.GroupId.Equals(gid))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetPreviousValues(List<ObservationValueData> list)
        {
            string result = string.Empty;
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        result = result + x.Value;
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetPatientObservationIds(List<PatientObservationData> pod)
        {
            List<string> list = new List<string>();
            try
            {
                if (pod != null && pod.Count > 0)
                {
                    pod.ForEach(t =>
                    {
                        list.Add(t.Id);
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
