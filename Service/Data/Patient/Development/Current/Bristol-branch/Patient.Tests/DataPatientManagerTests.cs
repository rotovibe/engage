using System;
using Moq;
using System.Collections.Generic;
using NUnit.Framework;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Patient;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestFixture]
    public class DataPatientManagerTests
    {
        private const string _CONTEXT = "Bristol";
        private const string _CONTRACTNUMBER = "OrlandoHealth001";
        private const double  _VERSION = 1.0;
        private const string _USERID = "bruser";

        private InsertBatchPatientsDataRequest _insertBtchPatientRqst = null;
     //   private InsertBatchPatientsDataResponse _insertBatchPatientResp = null;
        private List<AppData> _insertBatchPatientResp = new List<AppData>();
        private PatientDataManager _PatientDataMgr = null;
        private IPatientRepository _PatientRepository;
        private IPatientRepositoryFactory _PatientRepositoryFactory;

        [SetUp]
        public void Setup()
        {
            _PatientDataMgr = new PatientDataManager();
            _insertBtchPatientRqst = new InsertBatchPatientsDataRequest()
            {
                Context = _CONTEXT,
                ContractNumber = _CONTRACTNUMBER,
                UserId = _USERID,
                PatientsData = new List<PatientData>() {
                                                                                    new PatientData() {    Background = "", ClinicalBackground = "ClnBkgrd", DataSource = "DataSrc", DeceasedId = 99, DisplayPatientSystemId = "5325db01d6a4850adc44daf7",
                                                                                                                        DOB = "", EngagePatientSystemValue = "", FirstName = "John", ExternalRecordId = "5325db01d6a4850adc44daf7", Flagged = true,
                                                                                                                        FullSSN = "123456789", Gender = "M", Id = "", LastFourSSN = "6789", LastName = "Doe",
                                                                                                                        LastUpdatedOn = new DateTime(2016, 04, 01), MaritalStatusId = "", MiddleName = "Quinton",
                                                                                                                        PreferredName = "John", PriorityData = 4, Protected = true, ReasonId = "", RecordCreatedOn = new DateTime(2016, 04,01),
                                                                                                                        StatusDataSource = "StatusDataSrc", StatusId = 4, Suffix = "JR",  UpdatedByProperty = "",   Version = _VERSION
                                                                                                                  }
                                                                                    }
            };
        }       // end Setup()
        public void Teardown()
        {
            _insertBtchPatientRqst = null;
            _insertBatchPatientResp = null;
        }   // end Teardown()

        [Test]
        public void InsertBatchPatients_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));

            // Assert
            Assert.That(ex.Message, Is.StringContaining("request param cannot be NULL"));
        }  // end InsertBatchPatients_WhenRequestIsNull_ThrowsArgumentNullException()

        [Test]
        public void InsertBatchPatients_WhenRequestContextIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.Context = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request context param cannot be NULL/EMPTY"));
        }  //  end InsertBatchPatients_WhenRequestContextIsNull_ThrowsArgumentNullException()

        [Test]
        public void InsertBatchPatients_WhenRequestContextIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.Context = String.Empty;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request context param cannot be NULL/EMPTY"));
        }  // end  InsertBatchPatients_WhenRequestContextIsEmpty_ThrowsArgumentNullException()


        [Test]
        public void InsertBatchPatients_WhenRequestContractNumber_IsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.ContractNumber = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request contract number property cannot be NULL/EMPTY"));
        }  //  end InsertBatchPatients_WhenRequestContractNumber_IsNull_ThrowsArgumentNullException()

        [Test]
        public void InsertBatchPatients_WhenRequestContractNumber_IsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.ContractNumber = String.Empty;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request contract number property cannot be NULL/EMPTY"));
        }  // end  InsertBatchPatients_WhenRequestContractNumber_IsEmpty_ThrowsArgumentNullException()



        [Test]
        public void InsertBatchPatients_WhenRequestUserId_IsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.UserId = null;
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request requested user id propery cannot be NULL/EMPTY"));
        }  //  end InsertBatchPatients_WhenRequestUserId_IsNull_ThrowsArgumentNullException()

        [Test]
        public void InsertBatchPatients_WhenRequestUserId_IsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.UserId = String.Empty;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request requested user id propery cannot be NULL/EMPTY"));
        }  // end  InsertBatchPatients_WhenRequestUserId_IsEmpty_ThrowsArgumentNullException()


        [Test]
        public void InsertBatchPatients_WhenPatientsData_IsNull_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.PatientsData = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request patients data property cannot be NULL/EMPTY"));
        }  // end  InsertBatchPatients_WhenPatientsData_IsNull_ThrowsArgumentNullException()

        [Test]
        public void InsertBatchPatients_WhenPatientsData_CountLessThanOne_ThrowsArgumentNullException()
        {
            // Arrange
            _insertBtchPatientRqst.PatientsData = new List<PatientData>();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst));
            // Assert
            Assert.That(ex.Message, Is.StringContaining("request patients data property cannot be NULL/EMPTY"));
        }  // end  InsertBatchPatients_WhenPatientsData_CountLessThanOne_ThrowsArgumentNullException()


        [Test]
        public void InsertBatchPatients_WhenRequestIsNotNull_Success()
        {
            // Arrange

            // Act
            _insertBatchPatientResp = _PatientDataMgr.InsertBatchPatients(_insertBtchPatientRqst);

            // Assert
            Assert.That(_insertBtchPatientRqst, Is.Not.Null);
        }  // end InsertBatchPatients_WhenRequestIsNull_ThrowsArgumentNullException()

    }       // end class DataPatientManagerTests
}           // end namespace
