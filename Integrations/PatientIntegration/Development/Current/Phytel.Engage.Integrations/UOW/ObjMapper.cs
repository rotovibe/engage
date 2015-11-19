﻿using System;
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

        public static PatientNoteData MapPatientNote(string patientMongoId, PatientNote n)
        {
            var pnote = new PatientNoteData
            {
                ExternalRecordId = n.NoteId.ToString(),
                Text = n.Note,
                PatientId = n.PatientId.ToString(), // look into this.
                DataSource = _dataSource,
                CreatedOn = n.CreatedDate.GetValueOrDefault(),
                CreatedById = Constants.SystemContactId,
                TypeId = GetNoteType(Convert.ToInt32(n.ActionID))// get note type
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

        private static string GetNoteType(int actionId)
        {
            const string general = "54909992d43323251c0a1dfd";
            const string touchpoint = "54909997d43323251c0a1dfe";
            var val = actionId == 5 ? general : touchpoint;

            return val;
        }
    }
}
