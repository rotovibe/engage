using System;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Constants = Phytel.API.DataDomain.Patient.Constants;

namespace Phytel.Engage.Integrations.UOW.Notes
{
    public class Hillcrest001_NoteMapper : INoteMapper
    {
        private static string _dataSource = "P-Reg";

        public PatientNoteData MapPatientNote(string patientMongoId, PatientNote n)
        {
            const string touchPoint = "54909997d43323251c0a1dfe";

            var pnote = new PatientNoteData
            {
                ExternalRecordId = n.NoteId.ToString(),
                Text = n.Note,
                PatientId = n.PatientId.ToString(), // look into this.
                DataSource = _dataSource,
                CreatedOn = n.CreatedDate.GetValueOrDefault(),
                CreatedById = Constants.SystemContactId,
                TypeId = GetNoteType(Convert.ToInt32(n.ActionID), n.CategoryId) // get note type
            };

            if (pnote.TypeId.Equals(touchPoint))
                SetTouchPointNoteTypeProperties(n, pnote, touchPoint);

            return pnote;
        }

        public void SetTouchPointNoteTypeProperties(PatientNote n, PatientNoteData pnote, string touchPoint)
        {
            const string mail = "540f1da4d4332319883f3e8b";
            const string successful = "540f1f10d4332319883f3e92";
            const string unsuccessful = "540f1f14d4332319883f3e93";
            const string phone = "540f1d9dd4332319883f3e89";
            const string facetofaceother = "540f1da9d4332319883f3e8d";
            const string careManager = "540f1fc0d4332319883f3e96";
            const string individual = "540f1fbcd4332319883f3e95";

            if (pnote.TypeId.Equals(touchPoint))
            {
                pnote.SourceId = "540f2091d4332319883f3e9c"; //outbound
                //pnote.DurationId = "540f216cd4332319883f3e9e"; //not applicable
                pnote.ContactedOn = n.CreatedDate.GetValueOrDefault();
            }

            switch (n.ActionID)
            {
                //5	Other
                //6	PreVIsit Planning
                //7	Care Plan Management
                //8	Campaign
                //11	Medication Management
                //12	Escalations managed in NextGen

                case 1: //1	Campaign Follow-up
                    pnote.MethodId = mail; //mail
                    pnote.OutcomeId = successful; // successful
                    pnote.WhoId = individual;
                    break;
                case 2: //2	Left Message
                    pnote.MethodId = phone; //phone
                    pnote.OutcomeId = unsuccessful; // unsuccessful
                    pnote.WhoId = individual;
                    break;
                case 3: //3	Spoke to Patient
                    pnote.MethodId = phone; //phone
                    pnote.OutcomeId = successful; // successful
                    pnote.WhoId = individual;
                    break;
                case 4: //4	Spoke with Care Giver
                    pnote.MethodId = phone; //phone
                    pnote.OutcomeId = successful; // successful
                    pnote.WhoId = careManager; //caremanager
                    break;
                case 9: //9	Face-to-Face
                    pnote.MethodId = facetofaceother; //face to face other
                    pnote.OutcomeId = successful; // successful
                    pnote.WhoId = careManager; //caremanager
                    break;
                case 10: //10	Left Message
                    pnote.MethodId = phone; //phone
                    pnote.OutcomeId = unsuccessful; // unsuccessful
                    pnote.WhoId = individual; //individual
                    break;
            }
        }

        public string GetNoteType(int actionId, int categoryId)
        {
            //1	Campaign Follow-up
            //2	Left Message
            //3	Spoke to Patient
            //4	Spoke with Care Giver
            //5	Other
            //10	Left Message

            const string touchpoint = "54909997d43323251c0a1dfe";
            const string facetoface = "571fb85e8e7b7d5679fb2db8";
            const string medicationManagement = "571fb9cd8e7b7d5679fb2db9";
            const string careManagement = "57227c19f0474f4acc949dcb";
            const string accessAndContinuity = "571fb85e8e7b7d5679fb2db8";
            const string comprehensivenessAndCoordination = "571fb9cd8e7b7d5679fb2db9";
            const string populationHealth = "571fb7ee8e7b7d5679fb2db7";

            //var val = actionId == 5 ? SetNoteTypeForGeneral(categoryId) : touchpoint;
            string val;
            switch (actionId)
            {
                case 5: 
                    val = SetNoteTypeForGeneral(categoryId);
                    break;
                case 6: //6	PreVIsit Planning
                    val = careManagement;
                    break;
                case 7: //7	Care Plan Management
                    val = careManagement;
                    break;
                case 8: //8	Campaign
                    val = populationHealth;
                    break;
                case 12: //12	Escalations managed in NextGen
                    val = comprehensivenessAndCoordination;
                    break;
                case 9: //9	Face-to-Face
                    val = accessAndContinuity;
                    break;
                case 11: //11	Medication Management
                    val = comprehensivenessAndCoordination;
                    break;
                default:
                    val = touchpoint;
                    break;
            }
            return val;
        }

        public string SetNoteTypeForGeneral(int categoryId)
        {
            var noteTypeId = string.Empty;
            const string general = "54909992d43323251c0a1dfd";
            const string carePlanUpdate = "54ca6880d433231b90768691";
            const string assessment = "54ca6885d433231b90768692";
            const string chartReview = "5490999bd43323251c0a1dff";
            const string careManagement = "57227c19f0474f4acc949dcb";
            const string populationHealth = "571fb7ee8e7b7d5679fb2db7";

            switch (categoryId)
            {
                case 1: //1	Campaigns 
                    noteTypeId = populationHealth;
                    break;
                case 2: //2	Other  
                    noteTypeId = general;
                    break;
                case 3: //3	Adherence – Meds/Treatment
                    noteTypeId = carePlanUpdate;
                    break;
                case 4: //4	Appointment Scheduling
                    noteTypeId = general;
                    break;
                case 5: //5	Assessment – Health Status
                    noteTypeId = assessment;
                    break;
                case 6: //6	Care Planning
                    noteTypeId = carePlanUpdate;
                    break;
                case 7: //7	Care/Service Coordination
                    noteTypeId = carePlanUpdate;
                    break;
                case 8: //8	Education/Self Care
                    noteTypeId = carePlanUpdate;
                    break;
                case 9: //9	Emotional Support
                    noteTypeId = carePlanUpdate;
                    break;
                case 10: //10 Medication Reconciliation
                    noteTypeId = carePlanUpdate;
                    break;
                case 11: //11 Obtain Medical Records
                    noteTypeId = chartReview;
                    break;
                case 12: //12 Patient Feedback
                    noteTypeId = carePlanUpdate;
                    break;
                case 13: //13 Referral
                    noteTypeId = general;
                    break;
                case 14: //14 Service Feedback
                    noteTypeId = carePlanUpdate;
                    break;
                case 15: //15 TCM – Care Coordination
                    noteTypeId = carePlanUpdate;
                    break;
                case 16: //16 TCM – Care Planning
                    noteTypeId = carePlanUpdate;
                    break;
                case 17: //17 TCM – Education/Self Mgt
                    noteTypeId = carePlanUpdate;
                    break;
                case 18: //18 TCM – Health Status
                    noteTypeId = assessment;
                    break;
                case 19: //19 TCM – Needs Assessment
                    noteTypeId = assessment;
                    break;
                case 20: //20 Visit Preparation
                    noteTypeId = chartReview;
                    break;
                case 21: //21	Care Coordination
                    noteTypeId = carePlanUpdate;
                    break;
                case 22: //22	Transiton care
                    noteTypeId = careManagement;
                    break;
                case 23: //23	PreVisit Planning
                    noteTypeId = careManagement;
                    break;
            }

            return noteTypeId;
        }
    }
}
