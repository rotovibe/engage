using Moq;
using NUnit.Framework;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Cohort.Test
{

    [TestFixture]
    public class DataPatientManagerReferralTest
    {
        private const string _CONTRACTNUMBER = "InHealth001";
        private const string _CONTEXT = "NG";
        private const double _VERSION = 1.2;
        PostPatientReferralDefinitionRequest _PostRefrDefRqst = null;
        PostPatientReferralDefinitionResponse response = null;
        private Mock<IServiceContext> _MockContext;
        private Mock<IPatientReferralRepository<IDataDomainRequest>> _MockRepository;
        private DataPatientReferralManager _dataPatientMgr;
        private const string _USERID = "nguser";
        private const string _CONTRACT_DBName = "InHealth001";

        [SetUp]
        public void Setup()
        {
           _PostRefrDefRqst = new PostPatientReferralDefinitionRequest
            {
                UserId = _USERID, Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                Version = _VERSION,
                PatientReferral = new PatientReferralData {
                    CreatedBy = "JeRonDavis", Id = "RF-8123",
                    PatientId = "PAT00123", ReferralId = "RF-ID 528aa055d4332317acc50978"
                }            
            };


            response = new PostPatientReferralDefinitionResponse() {
                                                                                                    ResponseStatus = new ResponseStatus()   {
                                                                                                    ErrorCode = "000",
                                                                                                    Errors = new List<ResponseError>(),
                                                                                                    Message = "Everything is Fine",
                                                                                                    StackTrace = null
                                                                                                },
                                                                                                    Status = null, Version = _VERSION 
                                                                                                };

            _MockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _MockRepository = new Mock<IPatientReferralRepository<IDataDomainRequest>>(MockBehavior.Default);
            _MockRepository.Setup(m => m.Insert(It.IsAny<PostPatientReferralDefinitionRequest>())).Returns(response);
            _dataPatientMgr = new DataPatientReferralManager(_MockContext.Object, _MockRepository.Object);
        }   // Setup()

        [TearDown]
        public void Teardown()
        {
            _MockContext = null; 
            _MockRepository = null; 
            _dataPatientMgr = null;
           _PostRefrDefRqst = null;
        }

        [Test]
        public void CanInserPatienttReferral_Success()
        {
            // Arrange
                
            // Act
            _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst);
           // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void CanInsertPatientReferral_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter cannot be NULL"));
        }   // end CanInsertPatientReferral_WhenRequestIsNull_ThrowsArgumentNullException


        [Test]
        public void CanInsertPatientReferral_WhenRequestContextIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestContextIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertPatientReferral_WhenRequestContextIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestContextIsEmpty_ThrowsArgumentNullException()

        [Test]
        public void CanInsertPatientReferral_WhenRequestContractNumberIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestContractNumberIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertPatientReferral_WhenRequestContracttNumberIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestContractNumberIsEmpty_ThrowsArgumentNullException()

/*
        [Test]
        public void CanInsertPatientReferral_WhenRequestUserIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.UserId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter user id value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestUserIdIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertPatientReferral_WhenRequestUserIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.UserId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter user id value cannot be NULL/EMPTY"));
        }   // end  CanInsertPatientReferral_WhenRequestUserIdIsEmpty_ThrowsArgumentNullException()

*/
        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral cannot be NULL"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralIsNull_ThrowsArgumentNullException


        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.Id = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.referralId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralIdIsNull_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.Id = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.referralId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralIdIsEmpty_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralPatientIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.PatientId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.patientId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralPatientIdIsNull_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralPatientIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.PatientId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.patientId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralPatientIdIsEmpty_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralReferralIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.ReferralId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.referralId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralReferralIdIsNull_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralReferraldIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.ReferralId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.referralId cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralReferralIdIsEmpty_ThrowsArgumentNullException
        /*
        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralCreatedByIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.CreatedBy = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.createdBy cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralCreatedByIsNull_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralCreatedByIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.CreatedBy = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.createdBy cannot be NULL/EMPTY"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralReferralCreatedByIsEmpty_ThrowsArgumentNullException
        */
    }
}
