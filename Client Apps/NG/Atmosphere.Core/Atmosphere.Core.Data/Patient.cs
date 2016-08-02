using Phytel.Framework.SQL.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Patient
    {
        
        public struct Columns
        {
            public const string PATIENTID = "PhytelPatientId";
            public const string CLIENTPATIENTID = "PatientId";
            public const string PATIENTNAME = "PatientName";
            public const string BIRTHDATE = "BirthDate";
            public const string PROVIDERNAME = "ProviderName";
            public const string INSURER = "Insurer";
            public const string EMAIL = "Email";
            public const string GENDER = "Gender";
            public const string PHONE = "Phone";
            public const string PROVIDERID = "ProviderId";
            public const string PAYER = "Payer";
            public const string DOS = "DOS";
            public const string MFFS = "MCFFS";
            public const string PATIENTPQRIAPPTID = "PatientPqriApptId";
            public const string PLACEOFSERVICE = "PlaceOfService";
            public const string PLACEOFSERVICENUMBER = "PlaceOfServiceNumber";
            
            //public const string PAYORNAME = "PayorName";
            //public const string AGE = "Age";
            //public const string PRIORITY = "Priority";
            //public const string FOLLOWUPDUE="FollowUpDue";
            //public const string NOTES = "Notes";

        }

        #region Public Properties

        [DataMember]
        public int PatientId { get; set; }
        [DataMember]
        public string ClientPatientId { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime DateOfService { get; set; }
        [DataMember]
        public string Payer { get; set; }
        [DataMember]
        public bool MedicareFeeForService { get; set; }
        [DataMember]
        public int PatientPqriApptId { get; set; }
        [DataMember]
        public bool PlaceOfService { get; set; }
        [DataMember]
        public string PlaceOfServiceNumber { get; set; }

        //[DataMember]
        //public string Provider { get; set; }
        //[DataMember]
        //public int Age { get; set; }
        //[DataMember]
        //public string PatientNotes { get; set; }      
        //[DataMember]
        //public int PatientPriority { get; set; }
        //[DataMember]
        //public DateTime FollowUpDue { get; set; }
        //[DataMember]
        //public List<Appointment> AppointmentData { get; set; }

        public DataTable CommunicationActivities { get; set; }
        public DataTable CommunicationAttempts { get; set; }
        public DataTable OptOutData { get; set; }

        [DataMember]
        public List<Measure> CareOpportunities { get; set; }

        #endregion

        #region Public Methods

        public string ToPqrsString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            if (builder.Length < 150)
                builder.Append(new string(' ', 150 - builder.Length));
            else
                builder.Remove(150, builder.Length - 150);
            builder.Append("|");
            builder.Append(ClientPatientId);
            if (builder.Length < 300)
                builder.Append(new string(' ', 300 - builder.Length));
            else
                builder.Remove(150, builder.Length - 300);
            builder.Append("|");
            builder.Append(Payer);
            if (builder.Length < 350)
                builder.Append(new string(' ', 350 - builder.Length));
            else
                builder.Remove(150, builder.Length - 350);
            return builder.ToString();
        }

        public static Patient Build(ITypeReader reader)
        {
            Patient patient = new Patient();

            return patient;

        }

        public static Patient BuildPQRSPatient(ITypeReader reader)
        {
            Patient patient = new Patient();
            patient.Name = reader.GetString(Columns.PATIENTNAME);
            patient.PatientId = reader.GetInt(Columns.PATIENTID);
            patient.ClientPatientId = reader.GetString(Columns.CLIENTPATIENTID);
            patient.Payer = reader.GetString(Columns.PAYER);
            patient.DateOfService = DateTime.Parse(reader.GetString(Columns.DOS));
            patient.MedicareFeeForService = reader.GetBool(Columns.MFFS);
            patient.PatientPqriApptId = reader.GetInt(Columns.PATIENTPQRIAPPTID);
            patient.PlaceOfService = reader.GetBool(Columns.PLACEOFSERVICE);
            patient.PlaceOfServiceNumber = reader.GetString(Columns.PLACEOFSERVICENUMBER);

            return patient;

        }

        public static Patient Build(DataRow row)
        {
            Patient patient = new Patient();

            patient.PatientId = Convert.ToInt32(row[Columns.PATIENTID].ToString());
            patient.ClientPatientId = row[Columns.CLIENTPATIENTID].ToString();
            patient.DateOfBirth = row[Columns.BIRTHDATE].ToString();
            patient.Gender = row[Columns.GENDER].ToString();
            patient.Phone = row[Columns.PHONE].ToString();
            patient.Email = row[Columns.EMAIL].ToString();
            patient.Name = row[Columns.PATIENTNAME].ToString();

            return patient;
        }

        ////Coordinate: 01/25/2012
        //public static Patient BuildCoordinatePatient(DataRow row)
        //{
        //    Patient patient = new Patient();

        //    patient.PatientId = Convert.ToInt32(row[Columns.PATIENTID].ToString());
        //    patient.ClientPatientId = row[Columns.CLIENTPATIENTID].ToString();
        //    patient.DateOfBirth = row[Columns.BIRTHDATE].ToString();
        //    patient.Age = Convert.ToInt32(row[Columns.AGE]);
        //    patient.Gender = row[Columns.GENDER].ToString();
        //    patient.Phone = row[Columns.PHONE].ToString();
        //    patient.Email = row[Columns.EMAIL].ToString();
        //    patient.Payer = row[Columns.PAYER].ToString();
        //    patient.Provider = row[Columns.PROVIDERNAME].ToString();
        //    patient.PatientPriority = Convert.ToInt32(row[Columns.PRIORITY]);
        //    patient.FollowUpDue=Convert.ToDateTime(row[Columns.FOLLOWUPDUE]);
        //    patient.PatientNotes = row[Columns.NOTES].ToString();
        //    return patient;
        //}

        #endregion
    }

    
}
