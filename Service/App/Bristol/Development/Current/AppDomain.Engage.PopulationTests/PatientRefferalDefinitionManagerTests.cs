using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Engage.Population.DTO.Context;
using Moq;
using NUnit.Framework;
using AppDomain.Engage.Population.DTO.Referrals;
using AppDomain.Engage.Population.DataDomainClient;

namespace AppDomain.Engage.Population.Tests
{
    class PatientRefferalDefinitionManagerTests
    {
        private const string _CONTEXT = "Bristol";
        private const string _CONTRACTNUMBER = "OrlandoHealth001";
        private const double _VERSION = 1.0;
        private const string _USERID = "nguser";
        private const string _RETURNVALUE = "567676767udydj";
        private Mock<ICohortDataDomainClient> _mockDataDomainClient;
        private Mock<UserContext> _mockUserContext;
        private Mock<IServiceContext> _mockContext;
        private CohortManager manager;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _mockUserContext = new Mock<UserContext>(MockBehavior.Default);
            _mockDataDomainClient = new Mock<ICohortDataDomainClient>(MockBehavior.Default);
            _mockDataDomainClient.Setup(
                m => m.PostPatientReferralDefinition(It.IsAny<string>(), It.IsAny<string>(), _mockUserContext.Object))
                .Returns(() => _RETURNVALUE);
            manager = new CohortManager(_mockContext.Object, _mockDataDomainClient.Object);
        }

        [Test]
        public void InsertPatientReferral_Success()
        {
            string testPatientId = "234567testpatient";
            string testReferralId = "9485testreferral";
            var response = manager.CreatePatientReferral(testPatientId, testReferralId);
            Assert.IsNotNull(response);
        }

        [Test]
        public void InsertPatientReferral_WhenArgumentsIsNull_ThrowsArgumentNullException()
        {
            
            var ex = Assert.Throws<ArgumentNullException>(() => manager.CreatePatientReferral(null,null));
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientId value cannot be NULL/EMPTY"));

        }
    }
}
