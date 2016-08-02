using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class TestInit
    {
        public static PatientUtilizationData PatientUtilizationData { get; set; }
        public static PatientUtilizationData NewPatientUtilizationData { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            PatientUtilizationData = new PatientUtilizationData
            {
                AdmitDate = DateTime.UtcNow,
                DischargeDate = DateTime.UtcNow,
                DispositionId = "disposition",
                LocationId = "location",
                OtherDisposition = "other disp",
                OtherLocation = "testotherlocation",
                OtherType = "12345678912ertyudjgkanh4",
                PatientId = "sjfhgry478290erhy7482911",
                ProgramIds = new List<string> { "sjfhgry478298988y7482911" },
                Reason = "test reason",
                SourceId = "783945768910123erhgy547f",
                DataSource = "Engage",
                VisitTypeId = "89rhty7857ty7745tueiop33"
            };

            var timestamp = DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() +
                            DateTime.Now.TimeOfDay.Milliseconds.ToString();

            NewPatientUtilizationData = new PatientUtilizationData
            {
                AdmitDate = DateTime.UtcNow,
                DischargeDate = DateTime.UtcNow,
                DispositionId = "disposition" + timestamp,
                LocationId = "location" + timestamp,
                OtherDisposition = "other disp" + timestamp,
                OtherLocation = "testotherlocation" + timestamp,
                OtherType = "12345678912ertyudjgkanh4",
                PatientId = "sjfhgry478290erhy7482911",
                ProgramIds = new List<string> { "sjfhgry478298988y7482911" },
                Reason = "test reason" + timestamp,
                SourceId = "783945768910123erhgy547f",
                DataSource = "Engage",
                VisitTypeId = "89rhty7857ty7745tueiop33"
            };
        }
    }
}
