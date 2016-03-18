using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit;
using NUnit.Framework;
using NUnit.Core;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Cohort.Test
{
    [TestFixture]
   public  class DataCohortManagerTest
    {
        private const string _CONTEXT = "NG";
        private const string _USERID = "nguser";
        private const string _CONTRACT_DBName = "InHealth001";
        private const string _CONTRACT = _CONTRACT_DBName;
        private const double _VERSION = 1.1;
        private Object _repository = new object();
        private Mock<ICohortRepository<GetCohortDataResponse>> _mockedCohortRepository;
        private GetCohortDataRequest _getCohortDataRequest;
        private GetCohortDataResponse _getCohortDataResponse;
        private GetAllCohortsDataRequest _getAllCohortsDataRequest;
        private GetAllCohortsDataResponse _getAllCohortsDataResponse;
        private const string _COHORTID = "528aa055d4332317acc50978";
        private Mock<MongoCohortRepository<GetCohortDataResponse>> _mockedMongoCohortRepository;

        //   private static DataCohortManager _dataCohortMgr;

        [SetUp]
        public void Setup()
        {
            _getCohortDataRequest = new GetCohortDataRequest() {       CohortID = _COHORTID, Context = _CONTEXT,
                                                                                                                ContractNumber = _CONTRACT_DBName,
                                                                                                                UserId = _USERID, Version = _VERSION
                                                                                                        };

            _getCohortDataResponse = new GetCohortDataResponse() { Cohort = new CohortData() { Id = _COHORTID, Query = "", QueryWithFilter = "", SName = "SName", Sort = "DESC" },
                                                                                                                 Status = new ResponseStatus() { ErrorCode = "00", Errors = new List<ResponseError>() }, Version = _VERSION };

            _getAllCohortsDataResponse = new GetAllCohortsDataResponse() { Cohorts = new List<CohortData>(),
                                                                                                                            Status = null, Version = _VERSION };

            _getAllCohortsDataRequest = new GetAllCohortsDataRequest() { Context = _CONTEXT, ContractNumber = _CONTRACT_DBName,
                                                                                                                        UserId = _USERID, Version = _VERSION };

            _mockedCohortRepository = new Mock<ICohortRepository<GetCohortDataResponse>>(MockBehavior.Default);
            _mockedCohortRepository.Setup(m => m.FindByID(It.IsAny<string>())).Returns(_getAllCohortsDataResponse);
            _mockedMongoCohortRepository = new Mock<MongoCohortRepository<GetCohortDataResponse>>(_CONTRACT_DBName);
            _mockedMongoCohortRepository.Setup(m => m.FindByID("")).Returns("");

            
        }

        [Test]
         public void CanGetCohortById_Success()
        {
            // Arrange
            // Act
            _getCohortDataResponse = DataCohortManager.GetCohortByID(_getCohortDataRequest);

            // Assert
            Assert.That(_getCohortDataResponse, Is.Not.Null);
        }
        [Test]
        public void CanGelCohorts_Success()
        {

        }

        [TearDown]
        public void Teardown()
        {

        }
    }   // end class
}
