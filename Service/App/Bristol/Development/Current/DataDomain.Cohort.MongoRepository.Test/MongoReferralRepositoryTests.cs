using Moq;
using NUnit.Framework;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using System;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace DataDomain.Cohort.MongoReferralRepository.Test
{
    [TestFixture]
    public class MongoReferralRepositoryTests
    {
        private DataReferralManager _dataReferralMgr;
        private Mock<IServiceContext> _mockSvcContext;
        private Mock<IReferralRepository<IDataDomainRequest>> _mockReferralRepository;
        private const string _CONTEXT = "NG";
        private const string _CONTRACT = null;
        private const string _USERID = "nguser";
        private const string _CONTRACT_DBName = "InHealth001";
        private PostReferralDefinitionResponse _PostRefrDefResp;
        private PostReferralDefinitionRequest _PostRefrDefRqst;

        [SetUp]
        public void SetUp()
        {
            _PostRefrDefRqst = new PostReferralDefinitionRequest() { Context = _CONTEXT,  ContractNumber = _CONTRACT_DBName,  UserId = _USERID,    Version = 1.0,
                                                                                                            Referral = new ReferralData() { Description = "Test Description", CohortId = "528aa055d4332317acc50978",
                                                                                                            CreatedBy = "531f2df6072ef727c4d2a3c0", DataSource = "Test DataSource", Name= "Test Name", Reason = "Any Reason"}  };
            _PostRefrDefResp = new PostReferralDefinitionResponse() {  ResponseStatus =  new ResponseStatus() { ErrorCode = "000", Errors = new List<ResponseError>() , Message = "Everything is Fine", StackTrace = null }, Version = 1.0 };
            _mockReferralRepository = new Mock<IReferralRepository<IDataDomainRequest>>(MockBehavior.Default);
            _mockSvcContext = new Mock<IServiceContext>(MockBehavior.Default);
            _mockReferralRepository.Setup(m => m.Insert(It.IsAny<PostReferralDefinitionRequest>())).Returns(_PostRefrDefResp);
            _dataReferralMgr = new DataReferralManager(_mockSvcContext.Object, _mockReferralRepository.Object);
        }

       [TearDown]
       public void TearDown()
        {
            _mockReferralRepository  = null;
            _mockSvcContext = null;
            _dataReferralMgr = null;
        }

        [Test]
        public void CanInsertReferral_Success()
        {
            // Arrange
          // Act
          _PostRefrDefResp = _dataReferralMgr.InsertReferral(_PostRefrDefRqst);
            // Assert

            Assert.That(_PostRefrDefResp, Is.Not.Null);
        }   // end CanInsertReferral_Success()


        [Test]
        public void CanInsertReferral_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter cannot be NULL"));
        }   // end CanInsertReferral_WhenRequestIsNull_ThrowsArgumentNullException


        [Test]
        public void CanInsertReferral_WhenRequestContextIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestContextIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestContextIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Context = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter context value cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestContextIsEmpty_ThrowsArgumentNullException()


        [Test]
        public void CanInsertReferral_WhenRequestContractNumberIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   // end   CanInsertReferral_WhenRequestContractNumberIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestContractNumberIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.ContractNumber = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter contract number value cannot be NULL/EMPTY"));
        }   // end   CanInsertReferral_WhenRequestContractNumberIsEmpty_ThrowsArgumentNullException()


        [Test]
        public void CanInsertReferral_WhenRequestUserIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.UserId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter user id value cannot be NULL/EMPTY"));
        }   // end   CanInsertReferral_WhenRequestUserIdIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequesUserIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.UserId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter user id value cannot be NULL/EMPTY"));
        }   // end   CanInsertReferral_WhenRequestUserIdIsEmpty_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral cannot be NULL"));
        }   // end  CanInsertReferral_WhenRequestReferralIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralNameIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralNameIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralNameIsEmpty_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralCohortIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralCohortIdIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralCohortIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralCohortIdIsEmpty_ThrowsArgumentNullException()


        [Test]
        public void CanInsertReferral_WhenRequestReferralDataSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.datasource cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralDataSourceIsNull_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralDataSourceIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.datasource cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralDataSourceIsEmpty_ThrowsArgumentNullException()

        [Test]
        public void CanInsertReferral_WhenRequestReferralCreatedByIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CreatedBy = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.createdBy cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralCreatedBysNull_ThrowsArgumentNullException()


        [Test]
        public void CanInsertReferral_WhenRequestReferralCreatedBysEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CreatedBy = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.createdBy cannot be NULL/EMPTY"));
        }   // end  CanInsertReferral_WhenRequestReferralCreatedByIsEmpty_ThrowsArgumentNullException()

        /*

        [Test]
        public void CanFindByID()
        {
            // Arrange

            // Act
            // Assert
        }

        [Test]
        public void CanInsert()
        {
            // Arrange
       //     _repository.Setup(m => m.Insert(It.IsAny<object>())).Returns(It.IsAny<object>());
            // Act

            // Assert
        }

        [Test]
        public void CanInsertAll()
        {
            // Arrange

            // Act
            // Assert
        }

        [Test]
        public void CanUpdate()
        {
            // Arrange

            // Act
            // Assert
        }

        [Test]
        public void CanDelete()
        {
            // Arrange

            // Act
            // Assert
        }

        [Test]
        public void CanDeleteAll()
        {
            // Arrange

            // Act
            // Assert
        }

        [Test]
        public void CanUndoDelete()
        {
            // Arrange

            // Act
            // Assert
        }
        */
    }
}
