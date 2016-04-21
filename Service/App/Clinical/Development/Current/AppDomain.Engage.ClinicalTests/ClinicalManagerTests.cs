using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Engage.Clinical;
using AppDomain.Engage.Clinical.DTO.Context;
using AppDomain.Engage.Clinical.DTO.Medications;
using AppDomain.Engage.Clinical.DataDomainClient;
using ServiceStack;
using AppDomain.Engage.Clinical.Service;
using AppDomain.Engage.Clinical.Service.Containers;
using AutoMapper;
using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.Common;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using MedicationData = AppDomain.Engage.Clinical.DTO.Medications.MedicationData;
using Moq;
using Phytel.Services.SQLServer.Repository;

namespace AppDomain.Engage.Clinical.Tests
{
    [TestClass()]
    public class ClinicalManagerTests
    {
        private IServiceContext _context;
        private IMedicationDataDomainClient _client;
        protected List<MedicationData> _medData;

        private Mock<IRepositorySql> _repository;


        public ClinicalManagerTests()
        {
            _repository = CreateRepository();
        }

        [TestMethod()]
        public void SavePatientMedicationsTest()
        {
            PostPatientMedicationsResponse response = new PostPatientMedicationsResponse();
            PostPatientMedicationsResponse response2 = new PostPatientMedicationsResponse();
            List<MedicationData> medsData = new List<MedicationData>();
            medsData = _medData;
            

            ClinicalManager manager = new ClinicalManager(new ServiceContextMock(), new MedicationDataDomainClientMock());

            response = manager.SavePatientMedications(medsData);
            Assert.AreEqual(response.ResponseStatus, response2.ResponseStatus);
            Assert.AreEqual(response.Status, response2.Status);
            Assert.AreEqual(response.Version, response2.Version);
        }

        private Mock<IRepositorySql> CreateRepository()
        {
            var medData = new List<MedicationData>
            {
                new MedicationData
                {
                    category = "29",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now,
                    externalRecordId = "123460",
                    dosage = "12",
                    strength = "15",
                    sig = "12",
                    route = "oral",
                    form = "thing",
                    reason = "None",
                    medType = "something",
                    medName = "somethingToo",
                    sourceType = "fun",
                    patientId = "12349",
                    externalSystem = "outside",
                    dosageFreq = "every so often",
                    notes = "none",
                    prescribedBy = "person",
                    medCodes = new string[] { "thing1", "thing2" },
                    medClasses = new string[] { "thing3", "thing4" }
                },
                new MedicationData
                {
                    category = "28",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now,
                    externalRecordId = "123459",
                    dosage = "12",
                    strength = "15",
                    sig = "12",
                    route = "oral",
                    form = "thing",
                    reason = "None",
                    medType = "something",
                    medName = "somethingToo",
                    sourceType = "fun",
                    patientId = "12348",
                    externalSystem = "outside",
                    dosageFreq = "every so often",
                    notes = "none",
                    prescribedBy = "person",
                    medCodes = new string[] { "thing1", "thing2" },
                    medClasses = new string[] { "thing3", "thing4" }
                },
                new MedicationData
                {
                    category = "27",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now,
                    externalRecordId = "123458",
                    dosage = "12",
                    strength = "15",
                    sig = "12",
                    route = "oral",
                    form = "thing",
                    reason = "None",
                    medType = "something",
                    medName = "somethingToo",
                    sourceType = "fun",
                    patientId = "12347",
                    externalSystem = "outside",
                    dosageFreq = "every so often",
                    notes = "none",
                    prescribedBy = "person",
                    medCodes = new string[] { "thing1", "thing2" },
                    medClasses = new string[] { "thing3", "thing4" }
                },
                new MedicationData
                {
                    category = "26",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now,
                    externalRecordId = "123457",
                    dosage = "12",
                    strength = "15",
                    sig = "12",
                    route = "oral",
                    form = "thing",
                    reason = "None",
                    medType = "something",
                    medName = "somethingToo",
                    sourceType = "fun",
                    patientId = "12346",
                    externalSystem = "outside",
                    dosageFreq = "every so often",
                    notes = "none",
                    prescribedBy = "person",
                    medCodes = new string[] { "thing1", "thing2" },
                    medClasses = new string[] { "thing3", "thing4" }
                }


            }.AsQueryable();
            _medData = medData.ToList();


            var repository = new Mock<IRepositorySql>();

            repository.Setup(s => s.Query<MedicationData>()).Returns(medData);
            return repository;
    }
}

    public class MedicationDataDomainClientMock : IMedicationDataDomainClient
    {
        public string PostPatientMedications(List<PatientMedSuppData> data)
        {
            return "";
        }
    }

    public class ServiceContextMock : IServiceContext
    {
        public string Contract { get; set; }
        public double Version { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public object Tag { get; set; }
    }

    
}
