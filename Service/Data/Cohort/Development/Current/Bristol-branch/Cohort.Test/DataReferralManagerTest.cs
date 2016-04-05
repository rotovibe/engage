using Moq;
using NUnit.Framework;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Cohort.DTO;
using ServiceStack.Common.Utils;

namespace Phytel.API.DataDomain.Cohort.Test
{

    [TestFixture]
    public class DataReferralManagerReferralTest
    {
        private const string _CONTRACTNUMBER = "InHealth001";
        private const string _CONTEXT = "NG";
        private const double _VERSION = 1.2;
        PostReferralDefinitionRequest _PostRefrDefRqst = null;
        private GetReferralDataResponse getresponse = null;
        PostReferralDefinitionResponse response = null;
        private Mock<IServiceContext> _MockContext;
        private Mock<IReferralRepository<IDataDomainRequest>> _MockRepository;
        private DataReferralManager _dataReferralMgr;
        private const string _USERID = "nguser";
        private const string _CONTRACT_DBName = "InHealth001";

        [SetUp]
        public void Setup()
        {
           _PostRefrDefRqst = new PostReferralDefinitionRequest
            {
                UserId = _USERID, Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                Version = _VERSION,
                Referral = new ReferralData {
                    CohortId = "528aa055d4332317acc50978",
                    DataSource = "Explore",
                    Name = "Test Name",
                    Description = "Test desc",
                    Reason = "Test Reason"
                }            
            };

            response = new PostReferralDefinitionResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    ErrorCode = "000",
                    Errors = new List<ResponseError>(),
                    Message = "Everything is Fine",
                    StackTrace = null
                },
                ReferralId = "222aa055d4332317acc2222",
                Status = null,
                Version = _VERSION
            };

           getresponse = new GetReferralDataResponse()
            {
                Referral = new ReferralData
                {
                    CohortId = "528aa055d4332317acc50978",
                    DataSource = "Explore",
                    Name = "Test Name",
                    Description = "Test desc",
                    Reason = "Test Reason"
                },
                Status = null,
                Version = _VERSION
            };

            _MockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _MockRepository = new Mock<IReferralRepository<IDataDomainRequest>>(MockBehavior.Default);
            _MockRepository.Setup(m => m.Insert(It.IsAny<PostReferralDefinitionRequest>())).Returns(response);
            _MockRepository.Setup(m => m.FindByID(It.IsAny<string>())).Returns(getresponse);
            _dataReferralMgr = new DataReferralManager(_MockContext.Object, _MockRepository.Object);
        }   // Setup()

        [TearDown]
        public void Teardown()
        {
            _MockContext = null; 
            _MockRepository = null; 
            _dataReferralMgr = null;
           _PostRefrDefRqst = null;
        }

        [Test]
        public void CanInserReferraltReferral_Success()
        {
            // Arrange
                
            // Act
            _dataReferralMgr.InsertReferral(_PostRefrDefRqst);
           // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void CanInsertReferral_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter cannot be NULL"));
        }  


        [Test]
        public void CanInsertReferral_WhenRequestContextIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }   

        [Test]
        public void CanInsertReferral_WhenRequestContextIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }  

        [Test]
        public void CanInsertReferral_WhenRequestContractNumberIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   

        [Test]
        public void CanInsertReferral_WhenRequestContracttNumberIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   

        [Test]
        public void CanInsertReferral_WhenRequestReferralIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral cannot be NULL"));
        }  

        [Test]
        public void CanInsertReferral_WhenRequestReferralCohortIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }  

        [Test]
        public void CanInsertReferral_WhenRequestReferralCohortIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }  
     
        [Test]
        public void CanInsertReferral_WhenRequestReferralNameIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = string.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }

        [Test]
        public void CanInsertReferral_WhenRequestReferralNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }

        [Test]
        public void CanInsertReferral_WhenRequestReferralDataSourceIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = string.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.datasource cannot be NULL/EMPTY"));
        }

        [Test]
        public void CanInsertReferral_WhenRequestReferralDataSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.datasource cannot be NULL/EMPTY"));
        }

        [Test]
        public void GetCohortByID_Test_Success()
        {
            // Arrange
            var getRefrDefRqst = new GetReferralDataRequest()
            {
                UserId = _USERID,
                Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                Version = _VERSION,
                ReferralID = "Some Id"
            };

            // Act
            GetReferralDataResponse getResponse = _dataReferralMgr.GetReferralByID(getRefrDefRqst);

            // Assert
            Assert.IsNotNull(getResponse);
            Assert.IsNotNull(getResponse.Referral);
            Assert.IsNotNull(getResponse.Referral.CohortId);
            Assert.AreEqual(getResponse.Referral.CohortId, _PostRefrDefRqst.Referral.CohortId);
            Assert.AreEqual(getResponse.Referral.Name, _PostRefrDefRqst.Referral.Name);
            Assert.AreEqual(getResponse.Referral.DataSource, _PostRefrDefRqst.Referral.DataSource);
            Assert.AreEqual(getResponse.Referral.Reason, _PostRefrDefRqst.Referral.Reason);

        }

        [Test]
        public void GetAllCohorts_Test_Success()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllReferralsDataRequest request = new GetAllReferralsDataRequest { Context = context, ContractNumber = contractNumber, Version = version };
            _dataReferralMgr.InsertReferral(_PostRefrDefRqst);

            // Act
            GetAllReferralsDataResponse getResponse = _dataReferralMgr.GetReferrals(request);
            
            // Assert
            Assert.AreNotEqual(0, getResponse.Referrals.Count);
        }
    }
}
