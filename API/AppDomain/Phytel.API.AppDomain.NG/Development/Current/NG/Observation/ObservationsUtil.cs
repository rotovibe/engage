using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public static class ObservationsUtil
    {
        static readonly string DDPatientObservationsServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];

        internal static List<PatientObservation> GetStandardObservationsForPatient(GetStandardObservationItemsRequest request, List<PatientObservationData> po)
        {
            List<PatientObservation> result = new List<PatientObservation>();
            try
            {
                if (po != null && po.Count > 0)
                {
                    po.ForEach(o =>
                    {
                        result.Add(new PatientObservation
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
                            Values = GetValues(o.Values),
                            StateId = o.StateId,
                            DisplayId = o.DisplayId,
                            DataSource = o.DataSource,
                            ExternalRecordId = o.ExternalRecordId
                        });
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetStandardObservationsForPatient()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetValues()::" + ex.Message, ex.InnerException);
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
            catch (Exception ex)
            {
                throw new Exception("AD:ConvertPreviousValue()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<Phytel.API.AppDomain.NG.DTO.Observation> GetObservations(GetObservationsRequest request, List<ObservationData> po)
        {
            List<Phytel.API.AppDomain.NG.DTO.Observation> result = new List<Phytel.API.AppDomain.NG.DTO.Observation>();
            try
            {
                if (po != null && po.Count > 0)
                {
                    po.ForEach(o =>
                    {
                        result.Add(new Phytel.API.AppDomain.NG.DTO.Observation
                        {
                            Id = o.Id,
                            Name = o.CommonName != null ? o.CommonName : o.Description,
                            Standard = o.Standard,
                            TypeId = o.ObservationTypeId
                        });
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetObservations()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservationRecordData CreatePatientObservationRecord(PatientObservation po, ObservationValue ov)
        {
            try
            {
                PatientObservationRecordData pord = new PatientObservationRecordData
                {
                    GroupId = po.GroupId,
                    StartDate = po.StartDate,
                    TypeId = po.TypeId,
                    Units = po.Units,
                    DisplayId = po.DisplayId,
                    StateId = po.StateId,
                    DeleteFlag = po.DeleteFlag,
                    DataSource = po.DataSource,
                    ExternalRecordId = po.ExternalRecordId
                };

                // Populate Values for Labs and Vitals
                if (ov != null)
                {
                    pord.Id = ov.Id;
                    double dVal = 0;
                    if (double.TryParse(ov.Value, out dVal))
                    {
                        pord.Value = dVal;
                    }
                    else
                    {
                        pord.NonNumericValue = ov.Value;
                    }
                    // Set the End date to start date for Labs and Vitals
                    pord.EndDate = po.StartDate;
                }
                else //  Populate Values for Problems.
                {
                    pord.Id = po.Id;
                    // If the status for PatientObservation(problem) is changed to Resolved or Inactive, then set EndDate to Today.
                    if (IsResolvedOrInactivated(po.StateId))
                    {
                        pord.EndDate = DateTime.UtcNow;
                    }
                    else
                    {
                        pord.EndDate = po.EndDate;
                    }
                }

                return pord;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:CreatePatientObservationRecord()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool IsResolvedOrInactivated(int p)
        {
            bool result = false;
            if (p == (int)ObservationState.Inactive || p == (int)ObservationState.Resolved)
            {
                result = true;
            }
            return result;
        }

        internal static List<string> GetPatientObservationIds(List<PatientObservation> obsl)
        {
            try
            {
                List<string> patientObservationIds = new List<string>();

                if (obsl != null && obsl.Count > 0)
                {
                    foreach (PatientObservation po in obsl)
                    {
                        foreach (ObservationValue v in po.Values)
                        {
                            patientObservationIds.Add(v.Id);
                        }
                    }
                }

                return patientObservationIds;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPatientObservationIds()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservation GetAdditionalObservationItemForPatient(GetAdditionalObservationItemRequest request, PatientObservationData o)
        {
            PatientObservation result = new PatientObservation();
            try
            {
                result = new PatientObservation
                {
                    ObservationId = o.ObservationId,
                    EndDate = o.EndDate,
                    GroupId = o.GroupId,
                    Id = o.Id,
                    PatientId = o.PatientId,
                    Name = o.Name,
                    Standard = o.Standard,
                    TypeId = o.TypeId,
                    Units = o.Units,
                    Values = GetValues(o.Values),
                    StartDate = o.StartDate,
                    DisplayId = o.DisplayId,
                    StateId = o.StateId,
                    DataSource = o.DataSource,
                    ExternalRecordId = o.ExternalRecordId
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAdditionalObservationItemForPatient()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservation GetInitializeProblem(GetInitializeProblemRequest request, PatientObservationData o)
        {
            PatientObservation result = new PatientObservation();
            try
            {
                result = new PatientObservation
                {
                    Id = o.Id,
                    PatientId = o.PatientId,
                    ObservationId = o.ObservationId,
                    TypeId = o.TypeId,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    Name = o.Name,
                    DisplayId = o.DisplayId,
                    StateId = o.StateId,
                    Source = o.Source,
                    DeleteFlag  = o.DeleteFlag,
                    DataSource = o.DataSource,
                    ExternalRecordId = o.ExternalRecordId
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetInitializeProblem()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<PatientObservation> GetPatientObservations(List<PatientObservationData> po)
        {
            List<PatientObservation> result = null;
            try
            {
                if (po != null && po.Count > 0)
                {
                    result = new List<PatientObservation>();
                    po.ForEach(o => result.Add(GetPatientObservation(o)));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPatientObservations()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservation GetPatientObservation(PatientObservationData po)
        {
            PatientObservation result = null;
            try
            {
                if (po != null)
                {
                    result = new PatientObservation
                    {
                        ObservationId = po.ObservationId,
                        Id = po.Id,
                        PatientId = po.PatientId,
                        StartDate = po.StartDate,
                        EndDate = po.EndDate,
                        Units = po.Units,
                        Values = GetValues(po.Values),
                        StateId = po.StateId,
                        DisplayId = po.DisplayId,
                        Source = po.Source,
                        GroupId = po.GroupId,
                        Name = po.Name,
                        Standard = po.Standard,
                        TypeId = po.TypeId,
                        DeleteFlag = po.DeleteFlag,
                        DataSource = po.DataSource,
                        ExternalRecordId = po.ExternalRecordId
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPatientObservation()::" + ex.Message, ex.InnerException);
            } 
            return result;
        }
    }
}
