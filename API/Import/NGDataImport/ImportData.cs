using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;

namespace NGDataImport
{
    public enum ImportOperation
    {
        INSERT = 1,
        UPDATE = 2,
        LOOKUP_PATIENT = 3,
        LOOKUP_USER_CONTACT =4,
        SKIPPED =5
    }

    public class ImportData : IEnumerable
    {
        public ImportOperation importOperation { get; set; }
        public PatientData patientData { get; set; }            
        public bool failed { get; set; }
        public string failedMessage { get; set; }
        public bool skipped { get; set; }
        public string skippedMessage { get; set; }
        public int rowSkipped { get; set; }
        private string _newdateofbirth;
        public Guid id = Guid.NewGuid();
        
        public string NewDateofBirth
        {
            get
            {
                DateTime newDOB;
                if (DateTime.TryParse(_newdateofbirth, out newDOB))
                {
                    _newdateofbirth = newDOB.ToString("MM/dd/yyyy");
                }
                else
                {
                    _newdateofbirth = string.Empty;
                }
                return _newdateofbirth;
            }
            set
            {
                DateTime newDob;
                if (DateTime.TryParse(value, out newDob))
                {
                    _newdateofbirth = newDob.ToString("MM/dd/yyyy");
                }
                else
                {
                    _newdateofbirth = string.Empty;
                }
            }
        }

        public static ImportData FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            ImportData idata = new ImportData();
            if (values.Length > 41)
            {
                idata.patientData = new PatientData();
                idata.patientData.FirstName = values[0];
                idata.patientData.LastName = values[1];
                idata.patientData.MiddleName = values[2];
                idata.patientData.Suffix = values[3];
                idata.patientData.PreferredName = values[4];
                idata.patientData.Gender = values[5];
                idata.NewDateofBirth = values[6];
                idata.patientData.DOB = idata.NewDateofBirth;
                idata.patientData.SysId = values[7];
                idata.patientData.Background = values[8];
                idata.patientData.TimeZ = values[9];
                idata.patientData.Ph1 = values[10];
                idata.patientData.Ph1Pref = values[11];
                idata.patientData.Ph1Type = values[12];
                idata.patientData.Ph2 = values[13];
                idata.patientData.Ph2Pref = values[14];
                idata.patientData.Ph2Type = values[15];
                idata.patientData.Em1 = values[16];
                idata.patientData.Em1Pref = values[17];
                idata.patientData.Em1Type = values[18];
                idata.patientData.Em2 = values[19];
                idata.patientData.Em2Pref = values[20];
                idata.patientData.Em2Type = values[21];
                idata.patientData.Add1L1 = values[22];
                idata.patientData.Add1L2 = values[23];
                idata.patientData.Add1L3 = values[24];
                idata.patientData.Add1City = values[25];
                idata.patientData.Add1St = values[26];
                idata.patientData.Add1Zip = values[27];
                idata.patientData.Add1Pref = values[28];
                idata.patientData.Add1Type = values[29];
                idata.patientData.Add2L1 = values[30];
                idata.patientData.Add2L2 = values[31];
                idata.patientData.Add2L3 = values[32];
                idata.patientData.Add2City = values[33];
                idata.patientData.Add2St = values[34];
                idata.patientData.Add2Zip = values[35];
                idata.patientData.Add2Pref = values[36];
                idata.patientData.Add2Type = values[37];
                idata.patientData.CMan = values[38];
                idata.patientData.sysName = values[39];
                idata.patientData.SysPri = values[40];
                idata.patientData.ActivateDeactivate = values[41];
            }
            else
            {
                idata.importOperation = ImportOperation.SKIPPED;
                idata.skipped = true;
                idata.failedMessage = "Row Skipped. Line is Empty or Column is less than required number";
            }
            return idata;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }                         
}                             
                              