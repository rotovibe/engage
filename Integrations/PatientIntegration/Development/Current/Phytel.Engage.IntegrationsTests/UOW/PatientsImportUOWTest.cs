using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.Engage.Integrations.UOW.Tests
{
    [TestClass()]
    public class PatientsImportUOWTest
    {
        [TestMethod()]
        public void Commit()
        {
            var Patients =
                new List<PatientData>
                {
                    new PatientData {FirstName = "Ted", ExternalRecordId = "111111", DataSource = "P-Reg"},
                    new PatientData {FirstName = "Mammy", ExternalRecordId = "2222", DataSource = "P-Reg"},
                    new PatientData {FirstName = "Lisa", ExternalRecordId = "3333", DataSource = "P-Reg"},
                    new PatientData {FirstName = "George", ExternalRecordId = "44444", DataSource = "P-Reg"},
                    new PatientData {FirstName = "xavier", ExternalRecordId = "555555", DataSource = "P-Reg"},
                    new PatientData {FirstName = "pablo", ExternalRecordId = "666666", DataSource = "P-Reg"},
                    new PatientData {FirstName = "jimmy", ExternalRecordId = "7777777", DataSource = "P-Reg"},
                    new PatientData {FirstName = "eric", ExternalRecordId = "8888888", DataSource = "P-Reg"},
                    new PatientData {FirstName = "jane", ExternalRecordId = "9999999", DataSource = "P-Reg"},
                    new PatientData {FirstName = "bobby", ExternalRecordId = "100000", DataSource = "P-Reg"}
                };
            var uow = new PatientsImportUow{ Patients = Patients};

            uow.Commit("HILLCREST001");
        }
    }
}
