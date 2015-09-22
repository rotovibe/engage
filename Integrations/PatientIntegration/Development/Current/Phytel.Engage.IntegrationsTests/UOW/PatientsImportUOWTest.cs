using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.Engage.Integrations.UOW.Tests
{
    [TestClass()]
    public class PatientsImportUOWTest
    {
        [TestMethod()]
        public void Commit()
        {
            var uow = new PatientsImportUow<PatientData>
            {
                Pocos =
                    new List<PatientData>
                    {
                        new PatientData {FirstName = "Ted"},
                        new PatientData {FirstName = "Mammy"},
                        new PatientData {FirstName = "Lisa"},
                        new PatientData {FirstName = "George"},
                        new PatientData {FirstName = "xavier"},
                        new PatientData {FirstName = "pablo"},
                        new PatientData {FirstName = "jimmy"},
                        new PatientData {FirstName = "eric"},
                        new PatientData {FirstName = "jane"},
                        new PatientData {FirstName = "bobby"}
                    }
            };

            uow.Commit();
        }
    }
}
