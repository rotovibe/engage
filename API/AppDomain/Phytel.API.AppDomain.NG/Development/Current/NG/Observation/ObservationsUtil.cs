using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
                            ObservationId = o.ObservationId,
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
                            Value = v.Value,
                             PreviousValue = ConvertPreviousValue(v.PreviousValue)
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

        private static PreviousValue ConvertPreviousValue(PreviousValueData pv)
        {
            PreviousValue prevValue = null;
            try
            {
                if (pv != null)
                {
                    prevValue = new PreviousValue
                    {
                        EndDate = pv.EndDate,
                        Source = pv.Source,
                        StartDate = pv.StartDate,
                        Unit = pv.Unit,
                        Value = pv.Value
                    };
                }
                return prevValue;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialGoalRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static List<ObservationLibraryItem> GetAdditionalLibraryObservations(GetAdditionalObservationLibraryRequest request, List<ObservationLibraryItemData> po)
        {
            List<ObservationLibraryItem> result = new List<ObservationLibraryItem>();
            try
            {
                if (po != null && po.Count > 0)
                {
                    po.ForEach(o =>
                    {
                        result.Add(new ObservationLibraryItem
                        {
                            Id = o.Id,
                            Name = o.Name
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

        internal static PatientObservationRecordData CreatePatientObservationRecord(AD.PatientObservation po, ObservationValue ov)
        {
            try
            {
                PatientObservationRecordData pord = new PatientObservationRecordData
                {
                    Id = ov.Id,
                    EndDate = po.EndDate,
                    GroupId = po.GroupId,
                    StartDate = po.StartDate,
                    TypeId = po.TypeId,
                    Units = po.Units,
                    Source = po.Source
                };

                double dVal = 0;
                if (double.TryParse(ov.Value, out dVal))
                {
                    pord.Value = dVal;
                }
                else
                {
                    pord.NonNumericValue = ov.Value;
                }
                return pord;
            }
            catch (Exception ex)
            {
                throw new Exception("App Domain:CreatePatientObservationRecord()" + ex.Message, ex.InnerException);
            }
        }

        internal static List<string> GetPatientObservationIds(List<AD.PatientObservation> obsl)
        {
            try
            {
                List<string> patientObservationIds = new List<string>();

                if (obsl != null && obsl.Count > 0)
                {
                    foreach (AD.PatientObservation po in obsl)
                    {
                        foreach (AD.ObservationValue v in po.Values)
                        {
                            patientObservationIds.Add(v.Id);
                        }
                    }
                }

                return patientObservationIds;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientObservationIds()" + ex.Message, ex.InnerException);
            }
        }

        internal static AD.PatientObservation GetAdditionalObservationItemForPatient(GetAdditionalObservationItemRequest request, PatientObservationData o)
        {
            AD.PatientObservation result = new DTO.Observation.PatientObservation();
            try
            {
                result = new Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation
                {
                    ObservationId = o.ObservationId,
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
                };
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialGoalRequest()" + ex.Message, ex.InnerException);
            }
        }
    }
}
