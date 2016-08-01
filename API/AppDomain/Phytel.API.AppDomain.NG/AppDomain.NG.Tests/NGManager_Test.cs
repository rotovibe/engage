using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG;
using Phytel.API.AppDomain.NG.Test.Stubs;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.Service.Mappers;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class NGManager_Test
    {

        [TestInitialize]
        public void SetUp()
        {
            ContactMapper.Build();
        }

        [TestClass()]
        public class GetPatientProgramDetailsSummary_Method
        {
            [TestMethod()]
            [TestCategory("NIGHT-831")]
            [TestProperty("TFS", "11172")]
            [TestProperty("Layer", "NGManager")]
            public void Get_Response_AssignOn_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000001";
                DateTime now = System.DateTime.UtcNow.Date;

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                DateTime assignbyDate = ((DateTime)response.Program.AssignDate).Date;
                Assert.AreEqual(now, assignbyDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-832")]
            [TestProperty("TFS", "11159")]
            [TestProperty("Layer", "NGManager")]
            public void Get_Response_AssignById_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                string assignbyid = response.Program.AssignById;
                Assert.AreEqual(userId, assignbyid);
            }

            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "NGManager")]
            public void Get_Response_StateChange_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                DateTime statechangeDate = ((DateTime)response.Program.StateUpdatedOn).Date;
                Assert.AreEqual(DateTime.UtcNow.Date, statechangeDate);
            }

            [TestMethod()]
            public void Get_Response_Not_Null_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response);
            }

            [TestMethod()]
            public void Get_With_Description_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Description);
            }

            [TestMethod()]
            public void Get_With_Objectives_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Objectives);
            }

            [TestMethod()]
            public void Get_With_No_Steps_Test()
            {
                string patientId = "5325dab8d6a4850adcbba71a";
                string programId = "534d9217d6a48504b0586f68";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Assert.AreEqual(0, response.Program.Modules[0].Actions[0].Steps.Count);
            }

            [TestMethod()]
            public void Get_With_Attributes_Test()
            {
                string patientId = "5325dab8d6a4850adcbba71a";
                string programId = "534d9217d6a48504b0586f68";
                string userId = "0000000000000000000000000";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Attributes);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1886")]
            public void AD_Get_With_Module_Description_Test()
            {
                string patientId = "5325dad4d6a4850adcbba776";
                string programId = "534d9bffd6a48504b058a2cf";
                string userId = "0000000000000000000000000";
                string desc = "BSHSI - Outreach & Enrollment";
                string ModuleSourceId = "532b5585a381168abe00042c";

                INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PatientProgramId = programId,
                    UserId = userId
                };

                GetPatientProgramDetailsSummaryResponse response = ngm.GetPatientProgramDetailsSummary(request);
                Module module = response.Program.Modules.Find(m => m.SourceId == ModuleSourceId);
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }
        }

        [TestMethod]
        public void GetPatientByID_Test()
        {

            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager() { PlanElementUtils = new PlanElementUtils() };
            GetPatientRequest request = new GetPatientRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                PatientID = "5325db50d6a4850adcbba8e6",
                UserId = "5325c821072ef705080d3488"
            };
            // Act
            GetPatientResponse response = ngManager.GetPatient(request);

            //Assert
            Assert.IsTrue(response.Patient != null);
        }

        [TestMethod]
        public void DeletePatient_Tests()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            INGManager ngm = new NGManager
            {
                EndpointUtils = new StubPlanElementEndpointUtils(),
                PlanElementUtils = new PlanElementUtils()
            };
            PostDeletePatientRequest request = new PostDeletePatientRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                Id = "5325db68d6a4850adcbba92e",
                UserId = "5325c821072ef705080d3488"
            };
            // Act
            PostDeletePatientResponse response = ngm.DeletePatient(request);

            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllCohortsRequest request = new GetAllCohortsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            List<Cohort> response = ngManager.GetCohorts(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }


        [TestMethod]
        public void GetCohortPatients_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetCohortPatientsRequest request = new GetCohortPatientsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                CohortID = "530f9cff072ef715f4b411cf",
                SearchFilter = "",
                Skip = "0",
                Take = "100",
                UserId = "EC0849A4-D0A1-44BF-A482-7A97103E96CD"
            };
            // Act
            GetCohortPatientsResponse response = ngManager.GetCohortPatients(request);

            //Assert
            Assert.IsTrue(response.Patients.Count > 0);
        }

        [TestMethod]
        public void GetAllSettings_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "52936c88d6a48509e8d30632";
            NGManager ngManager = new NGManager();
            GetAllSettingsRequest request = new GetAllSettingsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            GetAllSettingsResponse response = ngManager.GetAllSettings(request);

            //Assert
            Assert.IsTrue(response.Settings.Count > 0);
        }

        #region Contact

        [TestMethod]
        public void GetContactByPatientId_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetContactByPatientIdRequest request = new GetContactByPatientIdRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                PatientID = "5336d17bd6a4850d346e4351",
                UserId = "5325c821072ef705080d3488"

            };
            // Act
            Contact response = ngManager.GetContactByPatientId(request);

            //Assert
            Assert.IsTrue(response != null);
        }

        [TestMethod]
        public void UpdatePatient_Test()
        {
            DTO.UpdateContactRequest request = new DTO.UpdateContactRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            Contact contact = new Contact();
            contact.Modes = new List<CommMode>();

            List<CommMode> modes = new List<CommMode>();
            modes.Add(new CommMode { LookUpModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false });
            modes.Add(new CommMode { LookUpModeId = "52e17ce6d433232028e9e390", OptOut = true, Preferred = false });
            modes.Add(new CommMode { LookUpModeId = "52e17d08d433232028e9e391", OptOut = false, Preferred = true });
            modes.Add(new CommMode { LookUpModeId = "52e17d10d433232028e9e392", OptOut = false, Preferred = false });
            contact.Modes = modes;

            List<Address> addresses = new List<Address>();
            addresses.Add(new Address { Id = "52e75858d43323149870c22a", Line1 = "phytel", Line2 = "11511 luna road", Line3 = "suite 600", City = "Dallas", PostalCode = "75234", StateId = "52e195b8d433232028e9e3e4", Preferred = false, OptOut = false, TypeId = "52e18c45d433232028e9e3ab" });
            contact.Addresses = addresses;

            List<Phone> phones = new List<Phone>();
            phones.Add(new Phone { Id = "52e7583dd43323149870c225", IsText = false, Number = 2142142147, OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c2ed433232028e9e3a6" });
            phones.Add(new Phone { Id = "52e75847d43323149870c226", IsText = true, Number = 8179035768, OptOut = false, PhonePreferred = false, TextPreferred = true, TypeId = "52e18c38d433232028e9e3a8" });
            //  phones.Add(new Phone { Id = "52e7584bd43323149870c227", IsText = false, Number = "9729729723", OptOut = false, PhonePreferred = false, TextPreferred = false, TypeId = "52e18c32d433232028e9e3a7" });
            // phones.Add(new Phone { Id = "-1", IsText = false, Number = "0000000", OptOut = false, PhonePreferred = false, TextPreferred = false, TypeId = "52e18c32d433232028e9e3a7" });
            contact.Phones = phones;

            //List<Email> emails = new List<Email>();
            //emails.Add(new Email { Id = "52e75852d43323149870c228", OptOut = false, Preferred = true, TypeId = "52e18c32d433232028e9e3a7", Text = "test1@gmail.com" });
            //emails.Add(new Email { Id = "52e75855d43323149870c229", OptOut = false, Preferred = false, TypeId = "52e18c41d433232028e9e3aa", Text = "test2@gmail.com" });
            //contact.Emails = emails;

            List<Language> languages = new List<Language>();
            languages.Add(new Language { LookUpLanguageId = "52e199dfd433232028e9e3f3", Preferred = true });
            languages.Add(new Language { LookUpLanguageId = "52e199d5d433232028e9e3f2", Preferred = false });
            languages.Add(new Language { LookUpLanguageId = "52e199d1d433232028e9e3f1", Preferred = false });
            languages.Add(new Language { LookUpLanguageId = "52e199cdd433232028e9e3f0", Preferred = false });
            contact.Languages = languages;

            List<string> times = new List<string>();
            times.Add("52e17de8d433232028e9e394");
            times.Add("52e17dedd433232028e9e395");
            contact.TimesOfDaysId = times;

            List<int> days = new List<int>();
            days.Add(1);
            days.Add(2);
            days.Add(3);
            days.Add(4);
            contact.WeekDays = days;

            contact.PatientId = "52e26f3b072ef7191c1179c3";
            contact.Id = "52ec1b77d6a4850b78581986";
            contact.TimeZoneId = "52e1817ad433232028e9e39d";
            request.Token = "52ebf7d3d6a4850b78619119";


            request.Contact = contact;
            NGManager ngManager = new NGManager();
            UpdateContactResponse response = ngManager.PutUpdateContact(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetRecentPatients_Test()
        {

            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager() { PlanElementUtils = new PlanElementUtils() };
            GetRecentPatientsRequest request = new GetRecentPatientsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                ContactId = "5325c821072ef705080d3488",
                UserId = "5325c821072ef705080d3488"
            };
            // Act
            GetRecentPatientsResponse response = ngManager.GetRecentPatients(request);

            //Assert
            Assert.IsTrue(response != null);
        }

        #endregion

        [TestClass()]
        public class PostPatientToProgram
        {
            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11199")]
            [TestProperty("Layer", "AD.NGManager")]
            public void Assign_With_CarememberId()
            {
                string progId = "111111111111111111111111";
                INGManager ngm = new NGManager { EndpointUtils = new StubPlanElementEndpointUtils(), PlanElementUtils = new StubPlanElementUtils() };
                PostPatientToProgramsRequest request = new PostPatientToProgramsRequest
                {
                    PatientId = "123456789012345678901234",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    ContractProgramId = "111111111111111111112222",
                    Token = "123456789012345678909999",
                    UserId = "123451234512345123455555",
                    Version = 1.0
                };
                PostPatientToProgramsResponse response = ngm.PostPatientToProgram(request);

                Assert.AreEqual(progId, response.Program.Id);
            }
        }

        [TestClass()]
        public class PostProgramAttributeChanges_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-937")]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.NGManager")]
            public void Post_Program_Change_AssignTo_Action()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                string assignTo = ObjectId.GenerateNewId().ToString();
                string patientId = ObjectId.GenerateNewId().ToString();

                PostProgramAttributesChangeRequest request = new PostProgramAttributesChangeRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PlanElement = new PlanElement
                    {
                        Id = "538ca77dfe7a59112c3649e4",
                        AssignToId = assignTo
                    },
                    ProgramId = "000000000000000000000000",
                    UserId = "userId",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostProgramAttributesChangeResponse response = ngm.PostProgramAttributeChanges(request);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.PlanElems.Actions[0]);
            }

            [TestMethod()]
            [TestCategory("NIGHT-967")]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.NGManager")]
            public void Post_Program_Change_AssignTo_Module()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                string assignTo = ObjectId.GenerateNewId().ToString();
                string patientId = ObjectId.GenerateNewId().ToString();

                PostProgramAttributesChangeRequest request = new PostProgramAttributesChangeRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PlanElement = new PlanElement
                    {
                        Id = "123450000000000000000000",
                        AssignToId = assignTo
                    },
                    ProgramId = "111100000000000000000000",
                    UserId = "userId",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostProgramAttributesChangeResponse response = ngm.PostProgramAttributeChanges(request);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.PlanElems.Modules[0]);
            }

            [TestMethod()]
            [TestCategory("NIGHT-968")]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.NGManager")]
            public void Post_Program_Change_AssignTo_Program()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                string assignTo = ObjectId.GenerateNewId().ToString();
                string patientId = ObjectId.GenerateNewId().ToString();

                PostProgramAttributesChangeRequest request = new PostProgramAttributesChangeRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PlanElement = new PlanElement
                    {
                        Id = "111100000000000000000000",
                        AssignToId = assignTo
                    },
                    ProgramId = "111100000000000000000000",
                    UserId = "userId",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostProgramAttributesChangeResponse response = ngm.PostProgramAttributeChanges(request);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.PlanElems.Programs[0]);
            }

            [TestMethod()]
            [TestCategory("NIGHT-968")]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.NGManager")]
            public void Post_Program_Change_AssignTo_Null_Program()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                string assignTo = null;
                string patientId = ObjectId.GenerateNewId().ToString();

                PostProgramAttributesChangeRequest request = new PostProgramAttributesChangeRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = patientId,
                    PlanElement = new PlanElement
                    {
                        Id = "111100000000000000000000",
                        AssignToId = assignTo
                    },
                    ProgramId = "111100000000000000000000",
                    UserId = "userId",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostProgramAttributesChangeResponse response = ngm.PostProgramAttributeChanges(request);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.PlanElems.Programs[0]);
            }

            [TestMethod()]
            [TestProperty("Layer", "AD.NGManager")]
            public void DeletePatient_Test()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                PostDeletePatientRequest request = new PostDeletePatientRequest
                {
                    ContractNumber = "InHealth001",
                    Id = "5325db70d6a4850adcbba946",
                    UserId = "5325c821072ef705080d3488",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostDeletePatientResponse response = ngm.DeletePatient(request);
                Assert.IsNotNull(response);
            }

            [TestMethod()]
            [TestProperty("Layer", "AD.NGManager")]
            public void RemoveProgram_Test()
            {
                INGManager ngm = new NGManager
                {
                    EndpointUtils = new StubPlanElementEndpointUtils(),
                    PlanElementUtils = new PlanElementUtils()
                };

                PostRemovePatientProgramRequest request = new PostRemovePatientProgramRequest
                {
                    ContractNumber = "InHealth001",
                    Id = "53d138d7d6a4850d58008e85",
                    PatientId = "5325dae5d6a4850adcbba7aa",
                    Reason = "Just liked that.",
                    ProgramName = "BSHSI - Healthy Weight",
                    UserId = "5325c821072ef705080d3488",
                    Token = ObjectId.GenerateNewId().ToString(),
                    Version = 1.0
                };

                PostRemovePatientProgramResponse response = ngm.RemovePatientProgram(request);
                Assert.IsNotNull(response);
            }
        }

        [TestMethod()]
        public void GetPhonesDataTest()
        {
            Mapper.CreateMap<Phone, PhoneData>();
            var ngMgr = new NGManager
            {
                EndpointUtils = new StubEndpointUtils(),
                PlanElementUtils = new StubPlanElementUtils()
            };

            var list = new List<Phone>
            {
                new Phone{ Id = "000000000000000000000000", Number=2146658790, OptOut = false, TextPreferred = true, DataSource = "Engage", IsText=true},
                new Phone{ Id = "111111111111111111111111", Number=2145553333, OptOut = false, TextPreferred = true, DataSource = "Engage", IsText=true}
            };

            var lP = ngMgr.GetPhonesData(list);
            Assert.AreEqual(typeof (Phone).Name, list[0].GetType().Name);
            Assert.AreEqual("Engage", list[0].DataSource);
        }

        [TestMethod()]
        public void InsertContact_ShouldNotReturnNull_Test()
        {
            InsertContactRequest request = new InsertContactRequest()
            {
                ContractNumber = "InHealth001",               
                UserId = "5325c821072ef705080d3488",
                Token = ObjectId.GenerateNewId().ToString(),
                Version = 1.0,
                Contact = new Contact()
                {
                   FirstName   = "Carl",
                   LastName = "Lewis",
                   
                }
            };
            INGManager ngManager = new StubNGManager();
            var response = ngManager.InsertContact(request);
            Assert.IsNotNull(response);
        }
    }
}