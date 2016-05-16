using System;
using Microsoft.VisualBasic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Constants = Phytel.API.DataDomain.Patient.Constants;

namespace Phytel.Engage.Integrations.UOW
{
    public static class ObjMapper
    {
        private static string _dataSource = "P-Reg";

        public static PatientData MapPatientData(PatientInfo info)
        {
            var pData = new PatientData
            {
                ExternalRecordId = info.PatientId.ToString(),
                FirstName = info.FirstName,
                MiddleName = info.MiddleInitial,
                LastName = info.LastName,
                Suffix = info.Suffix,
                Gender = FormatGender(info.Gender),
                DOB = info.BirthDate.HasValue ? info.BirthDate.Value.ToShortDateString() : string.Empty,
                // LastFourSSN = info.Ssn.Substring(),
                LastFourSSN = info.Ssn != null ? Strings.Right(info.Ssn, 4) : null,
                RecordCreatedOn = info.CreateDate.GetValueOrDefault(),
                LastUpdatedOn = info.UpdateDate.GetValueOrDefault(),
                StatusId = PatientInfoUtils.GetStatus(info.Status),
                DataSource = _dataSource,
                DeceasedId = PatientInfoUtils.GetDeceased(info.Status),
                PriorityData = Convert.ToInt32(info.Priority),
                ClinicalBackground = !string.IsNullOrEmpty(info.PCP) ? "PCP: " + info.PCP : null,
                StatusDataSource = _dataSource
            };

            return pData;
        }

        public static string FormatGender(string p)
        {
            try
            {
                var gender = "N";
                if (string.IsNullOrEmpty(p)) return gender;
                gender = p;
                return gender;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetNoteType(int actionId, int categoryId)
        {
            //1	Campaign Follow-up
            //2	Left Message
            //3	Spoke to Patient
            //4	Spoke with Care Giver
            //5	Other

            const string touchpoint = "54909997d43323251c0a1dfe";
            // refactor this
            var val = actionId == 5 ? SetNoteTypeForGeneral(categoryId) : touchpoint;



            return val;
        }

        private static string SetNoteTypeForGeneral(int categoryId)
        {
            var noteTypeId = string.Empty;
            const string general = "54909992d43323251c0a1dfd";
            const string carePlanUpdate = "54ca6880d433231b90768691";
            const string assessment = "54ca6885d433231b90768692";
            const string chartReview = "5490999bd43323251c0a1dff";

            switch (categoryId)
            {
                case 1: //1	Campaigns 
                case 13: //13 Referral
                case 2: //2	Other
                case 4: //4	Appointment Scheduling
                    noteTypeId = general;
                    break;

                case 3: //3	Adherence – Meds/Treatment
                case 6: //6	Care Planning
                case 7: //7	Care/Service Coordination
                case 8: //8	Education/Self Care
                case 9: //9	Emotional Support
                case 10: //10 Medication Reconciliation
                case 12: //12 Patient Feedback
                case 14: //14 Service Feedback
                case 15: //15 TCM – Care Coordination
                case 16: //16 TCM – Care Planning
                case 17: //17 TCM – Education/Self Mgt
                    noteTypeId = carePlanUpdate;
                    break;

                case 18: //18 TCM – Health Status
                case 19: //19 TCM – Needs Assessment
                case 5: //5	Assessment – Health Status
                    noteTypeId = assessment;
                    break;

                case 20: //20 Visit Preparation
                case 11: //11 Obtain Medical Records
                    noteTypeId = chartReview;
                    break;
            }

            return noteTypeId;
        }
    }
}
