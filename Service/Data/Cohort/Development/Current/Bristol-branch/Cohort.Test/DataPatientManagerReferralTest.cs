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
        private const string _CONTRACTNUMBER = "OrlandoHealth001";
        private const string _CONTEXT = "bristol";
        private const double _VERSION = 1.2;
        PostPatientReferralDefinitionRequest _PostRefrDefRqst = null;
        PostPatientReferralDefinitionResponse _PostRefrDefResp = null;
        private Mock<IServiceContext> _MockContext;
        private Mock<IPatientReferralRepository<IDataDomainRequest>> _MockRepository;
        private DataPatientReferralManager _dataPatientMgr;
        private const string _USERID = "531f2df6072ef727c4d2a3c0";
        private const string _CONTRACT_DBName = _CONTRACTNUMBER;

        [SetUp]
        public void Setup()
        {
           _PostRefrDefRqst = new PostPatientReferralDefinitionRequest
            {
                UserId = _USERID, Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                Version = _VERSION,
                PatientReferral = new PatientReferralData {
                    CreatedBy = _USERID, 
                    PatientId = "572cc6e8ecfce9522040b6a8",
                    ReferralId = "57153625365133250c0e7dd4"
                }            
            };


            _PostRefrDefResp = new PostPatientReferralDefinitionResponse() {
                                                                                                                            PatientReferralId = "57153625365133250c0e7dd4",
                                                                                                                            Status = new ResponseStatus() { ErrorCode = "200",
                                                                                                                            Errors = new List<ResponseError>() { },
                                                                                                                           Message = String.Empty, StackTrace = null
                                                                                                                         }    };

            _MockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _MockRepository = new Mock<IPatientReferralRepository<IDataDomainRequest>>(MockBehavior.Default);
            _MockRepository.Setup(m => m.Insert(It.IsAny<PostPatientReferralDefinitionRequest>())).Returns(_PostRefrDefResp);
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
            _PostRefrDefResp = null;
            // Act
            _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst);
           // Assert
            Assert.IsNotNull(_PostRefrDefResp);
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
     
        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralReferralDateIsLessThanOrEqualMinDate_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.ReferralDate = DateTime.MinValue;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.date is INVALID"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralReferralDateIsLessThanOrEqualMinDate_ThrowsArgumentNullException

        [Test]
        public void CanInsertPatientReferral_WhenRequestPatientReferralReferralDateIsGreaterThanOrEqualMaxDate_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.PatientReferral.ReferralDate = DateTime.MaxValue;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataPatientMgr.InsertPatientReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter patientReferral.date is INVALID"));
        }   // end CanInsertPatientReferral_WhenRequestPatientReferralReferralDateIsGreaterThanOrEqualMaxDate_ThrowsArgumentNullException
    }
}
