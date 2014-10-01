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
    public class ObservationUtil : IObservationUtil
    {
        public void FindAndInsert(List<PatientObservationData> podl, string gid, ObservationValueData ovd)
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

        public bool GroupExists(List<PatientObservationData> list, string gid)
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

        public string GetPreviousValues(List<ObservationValueData> list)
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

        public List<string> GetPatientObservationIds(List<PatientObservationData> pod)
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
