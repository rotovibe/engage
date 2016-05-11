using Moq;
using NUnit.Framework;
using Phytel.API.DataDomain.Cohort.DTO;
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
    public class DataReferralManagerReferralTest
    {
        private const string _CONTRACTNUMBER = "OrlandoHealth001";
        private const string _CONTEXT = "bristol";
        private const double _VERSION = 1.2;
        PostReferralDefinitionRequest _PostRefrDefRqst = null;
        PostReferralDefinitionResponse response = null;
        private Mock<IServiceContext> _MockContext;
        private Mock<IReferralRepository<IDataDomainRequest>> _MockRepository;
        private DataReferralManager _dataReferralMgr;
        private const string _USERID = "531f2df6072ef727c4d2a3c0";
        private const string _CONTRACT_DBName = _CONTRACTNUMBER;
        private const string ReferralId = "222aa055d4332317acc2222";
        ReferralData referralData = null;
        List<ReferralData> referralDataList = new List<ReferralData>();
        PostPatientsListReferralDefinitionRequest _PostPatientsListRefDefRqst = null;
        PostPatientsListReferralDefinitionResponse _PostPatientsListRefDefResp = null;

        [SetUp]
        public void Setup()
        {
           _PostRefrDefRqst = new PostReferralDefinitionRequest
            {
                UserId = _USERID, Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                Version = _VERSION,
                Referral = new ReferralData {
                    CohortId = "111f2df6072ef727c4d223",
                    DataSource = "ENGAGE",
                    Name = "Test Name",
                    Description = "Any descriptive commentary go be stored here",
                    Reason = "Any reason"
                }            
            };

            response = new PostReferralDefinitionResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    ErrorCode = "200",
                    Errors = new List<ResponseError>(),
                    Message = "Everything is Fine",
                    StackTrace = null
                },
                ReferralId = "222aa055d4332317acc2222",
                Status = null,
                Version = _VERSION
            };

            referralData = new ReferralData
            {
                CohortId = "528aa055d4332317acc50978",
                DataSource = "Explore",
                Name = "Test Name",
                Description = "Test desc",
                Reason = "Test Reason"
            };

            referralDataList.Add(referralData);

           _PostPatientsListRefDefRqst = new PostPatientsListReferralDefinitionRequest()
            {
                Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                UserId = _USERID,
                Name = "Patients Referral List",
                Version = _VERSION,
                 Description = "This is the patients referral listing",
                  PatientsReferralsList = new List<PatientReferralsListEntityData>() {
                      new PatientReferralsListEntityData() {
                                                                                        CreatedBy = "531f2df6072ef727c4d2a3c0", DataSource  = "Engage", ExternalId = "40001",
                                                                                        Name = "Daffy Duck", PatientId = "57153e3936512f250c5489ee", ReferralDate = new DateTime(2016,05,11) ,
                                                                                        ReferralId  = "57153e39365134250c6bdf72" },
                      new PatientReferralsListEntityData() {
                                                                                        PatientId ="57192f213651250f80f91ad3", ReferralId = "57192f213651270f80b4c08e", ReferralDate =  new DateTime(2106, 05, 11),
                                                                                        CreatedBy = "531f2df6072ef727c4d2a3c0", DataSource = "Engage",   ExternalId = "90823456", Name = "Miss Piggy"
                      }
                  }
            };

           _PostPatientsListRefDefResp = new PostPatientsListReferralDefinitionResponse() {
                ExistingPatientIds = new List<string>(),   NewPatientIds = new List<string>(),
                 ResponseStatus = new ResponseStatus() { ErrorCode = "0", Errors = new List<ResponseError>(), Message = String.Empty, StackTrace = String.Empty },
                                                                                        Version = _VERSION, Status = new ResponseStatus() {
                                                                                                                                                                                    ErrorCode = "0", Message = String.Empty,
                                                                                                                                                                                    StackTrace = String.Empty,
                                                                                                                                                                                    Errors = new List<ResponseError>() }
            };
            _MockContext = new Mock<IServiceContext>(MockBehavior.Default);
            _MockRepository = new Mock<IReferralRepository<IDataDomainRequest>>(MockBehavior.Default);
            _MockRepository.Setup(m => m.Insert(It.IsAny<PostReferralDefinitionRequest>())).Returns(() => ReferralId);
            _MockRepository.Setup(m => m.FindByID(It.IsAny<string>())).Returns(() => referralData);
            _MockRepository.Setup(m => m.SelectAll()).Returns(() => referralDataList);
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
        public void AttemptToInsertReferraltReferral_Success()
        {
            // Arrange
                
            // Act
            _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral);
           // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void AttemptToInsertReferral_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>_dataReferralMgr.InsertReferral(null));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral cannot be NULL"));
        }  

        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralCohortIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }  

        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralCohortIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.CohortId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.cohortId cannot be NULL/EMPTY"));
        }  
     
        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralNameIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = string.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.Name = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.name cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralDataSourceIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = string.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter referral.datasource cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferral_WhenRequestReferralDataSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostRefrDefRqst.Referral.DataSource = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral));
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
                ReferralID = "5717c61a36512b20b49a5a5f"
            };

            // Act
            var getResponse = _dataReferralMgr.GetReferralById(getRefrDefRqst.ReferralID);

            // Assert
            Assert.IsNotNull(getResponse);
            Assert.IsNotNull(getResponse.CohortId);
            Assert.AreEqual(getResponse.CohortId, _PostRefrDefRqst.Referral.CohortId);
            Assert.AreEqual(getResponse.Name, _PostRefrDefRqst.Referral.Name);
            Assert.AreEqual(getResponse.DataSource, _PostRefrDefRqst.Referral.DataSource);
            Assert.AreEqual(getResponse.Reason, _PostRefrDefRqst.Referral.Reason);

        }

        [Test]
        public void GetAllCohorts_Test_Success()
        {
            // Arrange
            double version = _VERSION;
            string contractNumber = _CONTRACTNUMBER;
            string context = _CONTEXT;
            GetAllReferralsDataRequest request = new GetAllReferralsDataRequest { Context = context, ContractNumber = contractNumber, Version = version };
            _dataReferralMgr.InsertReferral(_PostRefrDefRqst.Referral);

            // Act
            var getResponse = _dataReferralMgr.GetAllReferrals();
            
            // Assert
            Assert.AreNotEqual(0, getResponse.Count);
        }


        [Test]
        public void AttemptToInsertReferralAll_Success_ReturnsValidResponse()
        {
            // Arrange +  Act
            _PostPatientsListRefDefResp =_dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst);
            // Assert
            Assert.That(_PostPatientsListRefDefResp, Is.Not.Null);
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter cannot be NULL"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralContextIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.Context = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter PatientsReferralList cannot be NULL"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralContractNumberIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.ContractNumber = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter Contract Number cannot be NULL/EMPTY"));
        }
        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralContractNumberIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.ContractNumber = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter Contract Number cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralUserIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.UserId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter UserId cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralUserIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.UserId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter UserId cannot be NULL/EMPTY"));
        }
        
        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferraDataSourceIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].DataSource = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter Datasource cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferraDataSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].DataSource = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter Datasource cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralExternalIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].ExternalId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter ExternalId cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralExternalIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].ExternalId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter ExternalId cannot be NULL/EMPTY"));
        }


        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralIdIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].ReferralId = String.Empty;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter ReferralId cannot be NULL/EMPTY"));
        }

        [Test]
        public void AttemptToInsertReferralAll_WhenRequestReferralIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _PostPatientsListRefDefRqst.PatientsReferralsList[0].ReferralId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _dataReferralMgr.InsertReferralsAll(_PostPatientsListRefDefRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("Request parameter ReferralId cannot be NULL/EMPTY"));
        }
    }
}
