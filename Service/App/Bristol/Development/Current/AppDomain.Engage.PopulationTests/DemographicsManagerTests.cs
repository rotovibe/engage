using System;
using System.Collections.Generic;
using Moq;
using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Demographics;
using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;

using NUnit.Framework;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;


namespace AppDomain.Engage.Population.Tests
{
    [TestFixture()]
    public class DemographicsManagerTests
    {
        private const string _CONTEXT = "Bristol";
        private const string _CONTRACTNUMBER = "OrlandoHealth001";
        private const double _VERSION = 1.0;
        private const string _USERID = "nguser";
        private ProcessedPatientsList testProcessedPatientsList = null;
        private List<Patient> testPatientsList ;
        private IDemographicsManager manager;
        private Mock<IPatientDataDomainClient> _mockDataDomainClient;
        private Mock<IServiceContext> _mockContext;
        private Mock<IHelpers> _mockHelpers;
        private Mock<JsonServiceClient> _mockJsonServiceClient;
        private Mock<IMappingEngine> _mockMappingengine;
        private Mock<UserContext> _mockUserContext;
        private List<Patient> testData;
        private PostReferralWithPatientsListResponse testProcessedresponse;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _mockUserContext = new Mock<UserContext>(MockBehavior.Default);
            //_mockHelpers = new Mock<IHelpers>(MockBehavior.Default);
            //_mockMappingengine = new Mock<IMappingEngine>(MockBehavior.Default);
            _mockDataDomainClient = new Mock<IPatientDataDomainClient>(MockBehavior.Default);
           
            manager = new DemographicsManager(_mockContext.Object, _mockDataDomainClient.Object,_mockUserContext.Object
                
                );
            testData = new List<Patient>
            {
                new Patient() { FirstName = "testfirstname", LastName = "testlastname" , DOB = "20070909" , DataSource = "testdatasource" , ExternalRecordId = "92ufieekekememeem" },
                new Patient() { FirstName = "testfirstname", LastName = "testlastname",DOB = "20070909",DataSource = "testdatasource",ExternalRecordId = "92ufieekekememeeo"}
                
            };

            testProcessedresponse = new PostReferralWithPatientsListResponse();
            testProcessedresponse.ProcessedPatients = new ProcessedPatientsList();
            testProcessedresponse.ProcessedPatients.InsertedPatients = new List<ProcessedData>
            {
                new ProcessedData() {EngagePatientSystemValue = "4565", ExternalRecordId = "7888",Id="jgkgk455"},
                new ProcessedData() { EngagePatientSystemValue = "4566", ExternalRecordId = "7889", Id = "jgkgk455"}
            };

            _mockDataDomainClient.Setup(m => m.PostPatientsListDetails(It.IsAny<List<Patient>>(), _mockUserContext.Object)).Returns(() => testProcessedresponse);


        }



        [Test]
        public void InsertBulkPatients_Success()
        {
            testPatientsList = testData;
            var response = manager.InsertBulkPatients(testPatientsList);
            Assert.IsNotNull(response);
        }

        [Test]
        public void InsertBulkPatients_WhenRequestContextIsNull_ThrowsArgumentNullException()
        {
            testPatientsList = null;
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            Assert.That(ex.Message, Is.StringContaining("Request cannot be null/empty"));
        }

        [Test]
        public void InsertBulkPatients_WhenFirstNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            testPatientsList = testData;
            testPatientsList[0].FirstName = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter FirstName value cannot be NULL/EMPTY for patient"));
        }

        [Test]
        public void InsertBulkPatients_WhenLastNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            testPatientsList = testData;
            testPatientsList[0].LastName = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter LastName value cannot be NULL/EMPTY for patient "));
        }
        [Test]
        public void InsertBulkPatients_WhenDOBIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            testPatientsList = testData;
            testPatientsList[0].DOB = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter DOB value cannot be NULL/EMPTY for patient"));
        }

        [Test]
        public void InsertBulkPatients_WhenExternalRecordIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            testPatientsList = testData;
            testPatientsList[0].ExternalRecordId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter ExternalRecordId value  cannot be NULL/EMPTY for patient"));
        }

        [Test]
        public void InsertBulkPatients_WhenDataSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            testPatientsList = testData;
            testPatientsList[0].DataSource = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => manager.InsertBulkPatients(testPatientsList));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter Datasource value  cannot be NULL/EMPTY for patient"));
        }



    }
}
