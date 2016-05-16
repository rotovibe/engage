using System;
using Microsoft.VisualBasic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Constants = Phytel.API.DataDomain.Patient.Constants;

namespace Phytel.Engage.Integrations.UOW.Notes
{
    public class OrlandoHealth001_NoteMapper : INoteMapper
    {
        private static string _dataSource = "P-Reg";

        public PatientNoteData MapPatientNote(string patientMongoId, PatientNote n)
        {
            var pnote = new PatientNoteData
            {
                ExternalRecordId = n.NoteId.ToString(),
                Text = n.Note,
                PatientId = n.PatientId.ToString(), // look into this.
                DataSource = _dataSource,
                CreatedOn = n.CreatedDate.GetValueOrDefault(),
                CreatedById = Constants.SystemContactId,
                TypeId = GetNoteType(Convert.ToInt32(n.ActionID), n.CategoryId)// get note type
            };

            if (!pnote.TypeId.Equals("54909997d43323251c0a1dfe")) return pnote;

            if (pnote.TypeId.Equals("54909997d43323251c0a1dfe"))
            {
                pnote.SourceId = "540f2091d4332319883f3e9c";//outbound
                //pnote.DurationId = "540f216cd4332319883f3e9e"; //not applicable
                pnote.ContactedOn = n.CreatedDate.GetValueOrDefault();
            }

            switch (n.ActionID)
            {
                case 1:
                    pnote.MethodId = "540f1da4d4332319883f3e8b";  //mail
                    pnote.OutcomeId = "540f1f10d4332319883f3e92"; // successful
                    pnote.WhoId = "540f1fbcd4332319883f3e95";
                    break;
                case 2:
                    pnote.MethodId = "540f1d9dd4332319883f3e89";  //phone
                    pnote.OutcomeId = "540f1f14d4332319883f3e93"; // unsuccessful
                    pnote.WhoId = "540f1fbcd4332319883f3e95";
                    break;
                case 3:
                    pnote.MethodId = "540f1d9dd4332319883f3e89";  //phone
                    pnote.OutcomeId = "540f1f10d4332319883f3e92"; // successful
                    pnote.WhoId = "540f1fbcd4332319883f3e95";
                    break;
                case 4:
                    pnote.MethodId = "540f1d9dd4332319883f3e89";  //phone
                    pnote.OutcomeId = "540f1f10d4332319883f3e92"; // successful
                    pnote.WhoId = "540f1fc0d4332319883f3e96"; //caremanager
                    break;
            }
            return pnote;
        }

        public string GetNoteType(int actionId, int categoryId)
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

        public string SetNoteTypeForGeneral(int categoryId)
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
