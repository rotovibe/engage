using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using ResultsProcessor;
using Phytel.Framework.ASE.Process;
using Phytel.Services;
using Atmosphere.Coordinate.Business.Interfaces;
using Atmosphere.Coordinate.Data.Campaign;
using C3.Domain.Repositories.Abstract;
using Phytel.Framework.ASE.Bus.Data;

namespace ResultsProcessorTests
{
    [TestFixture]
    public class ResultsQueueProcessTests
    {
        #region Variables

        private Random rand = new Random();
        private CampaignCallResult CallResult = new CampaignCallResult();
        private const int MaxAttempts = 3;
        private DataSet TestResultsDataSet = new DataSet();
        private const int ESCALATION_IDENTIFIER = 1;
        private const string ESCALATION_COMMENT = "Followup Reason";
        private const string ANSWER_TEXT = "Answer Text";

        ResultsQueueProcess process;
        Mock<ICampaignRepository> fakeCampaignRepository;
        Mock<IDischargePatientMgmtRepository> fakeContractRepository;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            SetupTestRecord();

            fakeCampaignRepository = new Mock<Atmosphere.Coordinate.Business.Interfaces.ICampaignRepository>();
            fakeContractRepository = new Mock<IDischargePatientMgmtRepository>();

            process = new ResultsQueueProcess();
            process.CampaignRepository = fakeCampaignRepository.Object;
            process.ContractRepository = fakeContractRepository.Object;
            process.LocalEscalationEngine = new EscalationEngine(process.CampaignRepository);

            SetupEscalationReturnValues();
            SetupReturnCampaignInfoDataSet();
        }

        [TearDown]
        public void Teardown()
        {

        }

        #endregion

        #region Results Process Test

        [Test]
        public void CanCreateProcess()
        {
            Assert.AreEqual(typeof(ResultsQueueProcess), process.GetType());
        }

        [Test]
        public void Execute_TestMessage_SetsContractID()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.ContractID, process.CallResult.ContractID);
        }

        [Test]
        public void Execute_TestMessage_SetsQueueID()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.QueueID, process.CallResult.QueueID);
        }

        [Test]
        public void Execute_TestMessage_SetsStatusCode()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.StatusCode, process.CallResult.StatusCode);
        }

        [Test]
        public void Execute_TestMessage_SetsFileName()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.FileName, process.CallResult.FileName);
        }

        [Test]
        public void Execute_TestMessage_SetsSourceBatchID()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.SourceBatchID, process.CallResult.SourceBatchID);
        }

        [Test]
        public void Execute_TestMessage_SetsCalledCount()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.CalledCount, process.CallResult.CalledCount);
        }

        [Test]
        public void Execute_TestMessage_SetsLastCallAttempt()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.LastCallAttempt, process.CallResult.LastCallAttempt);
        }

        [Test]
        public void Execute_TestMessage_SetsCallerCode()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.CallerCode, process.CallResult.CallerCode);
        }

        [Test]
        public void Execute_TestMessage_SetsPhoneNumberDialed()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.PhoneNumberDialed, process.CallResult.PhoneNumberDialed);
        }

        [Test]
        public void Execute_TestMessage_SetsLengthInSeconds()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.LengthInSeconds, process.CallResult.LengthInSeconds);
        }

        [Test]
        public void Execute_TestMessage_SetsIncomingSurveyAnswers()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.IncomingSurveyAnswers, process.CallResult.IncomingSurveyAnswers);
        }

        [Test]
        public void Execute_TestMessage_SetsCampaignTemplateID()
        {
            ExecuteWithTestMessage();
            Assert.AreEqual(CallResult.CampaignTemplateID, CallResult.CampaignTemplateID);
        }

        [Test]
        public void Execute_TestMessage_SetsResultsVariable()
        {
            ExecuteWithTestMessage();

            // Test each property of the result for expected value
            PropertyDescriptorCollection resultProperties = TypeDescriptor.GetProperties(typeof(CampaignCallResult));
            for (int i = 0; i < resultProperties.Count; i++)
            {
                if (resultProperties[i].DisplayName != "QueueStatus" || process.CallResult.Attempts < MaxAttempts)
                    Assert.AreEqual(
                        resultProperties[i].GetValue(CallResult),
                        resultProperties[i].GetValue(process.CallResult));
                //else
                //    Assert.AreEqual("COMPLETE", resultProperties[i].GetValue(process.CallResult));
            }
        }

        [Test]
        public void Execute_TestMessage_AddsCampaignDataToResult()
        {
            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.GetTransitionResultCampaignData(CallResult.ContractID.ToString(), CallResult.CampaignID, CallResult.PatientDischargeId, CallResult.EntityTypeID,
                CallResult.PatientID, CallResult.FileName), Times.Once());
        }

        [Test]
        public void Execute_TestMessage_UpdatesPatientDischargeStatus()
        {
            ExecuteWithTestMessage();
            fakeContractRepository.Verify(cr => cr.MarkPatientDischargeCompleted(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.SourceBatchID), Times.Once());
        }

        [Test]
        public void Execute_TestMessage_InsertsPatientFollowup()
        {
            ExecuteWithTestMessage();
            fakeContractRepository.Verify(cr => cr.MarkPatientFollowUpDone(CallResult.ContractNumber, CallResult.PatientDischargeId), Times.Once());
        }

        [Test]
        public void Execute_EscalationAnswer_InsertsPatientFollowUpReason()
        {
            ExecuteWithTestMessage();

            CallResult.SurveyAnswers.ForEach(sa =>
            {
                if (sa.Identifier > 0)
                    fakeContractRepository.Verify(cr => cr.InsertPatientFollowUpReason(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt.ToString("yyyy-MM-dd-hh-mm-ss"), sa.Comment, sa.Identifier), Times.Exactly(1));
            });
        }

        [Test]
        public void Execute_NonEscalationAnswer_DoesNotInsertsPatientFollowUpReason()
        {
            ExecuteWithTestMessage();

            CallResult.SurveyAnswers.ForEach(sa =>
            {
                if (sa.Identifier <= 0)
                    fakeContractRepository.Verify(cr => cr.InsertPatientFollowUpReason(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt.ToString("yyyy-MM-dd-hh-mm-ss"), sa.Comment, sa.Identifier), Times.Never());
            });
        }

        [Test]
        public void Execute_TestMessage_WritesCampaignResult()
        {
            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_OverMaxAttempts_InsertsPatientFollowUp()
        {
            ExecuteWithMaxAttemptsMessage();
            fakeContractRepository.Verify(cr => cr.MarkFollowupDoneMaxAttemptsReached(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt), Times.Once());
        }

        [Test]
        public void Execute_TestMessage_DoesNotMarkPatientFollowUpDone()
        {
            ExecuteWithTestMessage();
            fakeContractRepository.Verify(cr => cr.MarkFollowupDoneMaxAttemptsReached(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt), Times.Never());
        }

        [Test]
        public void Execute_OverMaxAttempts_MarksPatientFollowUpDone()
        {
            ExecuteWithMaxAttemptsMessage();
            fakeContractRepository.Verify(cr => cr.MarkFollowupDoneMaxAttemptsReached(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt), Times.Once());
        }

        [Test]
        public void Execute_AFAX_WritesCampaignResult()
        {
            CallResult.StatusCode = "AFAX";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_CPDREJ_WritesCampaignResult()
        {
            CallResult.StatusCode = "CPDREJ";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_AB_WritesCampaignResult()
        {
            CallResult.StatusCode = "AB";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_CPDNA_WritesCampaignResult()
        {
            CallResult.StatusCode = "CPDNA";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_AA_WritesCampaignResult()
        {
            CallResult.StatusCode = "AA";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_DNC_WritesCampaignResult()
        {
            CallResult.StatusCode = "DNC";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_CPDATB_WritesCampaignResult()
        {
            CallResult.StatusCode = "CPDATB";

            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(process.CallResult), Times.Once());
        }

        [Test]
        public void Execute_DuplicateRecord_UpdatesPatientDischargeStatus()
        {
            SetupDuplicateDataSet();
            ExecuteWithTestMessage();
            fakeContractRepository.Verify(cr => cr.MarkPatientDischargeCompleted(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.SourceBatchID), Times.Once());
        }

        [Test]
        public void Execute_DuplicateRecord_DoesNotInsertPatientFollowUpReason()
        {
            SetupDuplicateDataSet();
            ExecuteWithTestMessage();

            CallResult.SurveyAnswers.ForEach(sa =>
            {
                fakeContractRepository.Verify(cr => cr.InsertPatientFollowUpReason(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt.ToString("yyyy-MM-dd-hh-mm-ss"), sa.Comment, sa.Identifier), Times.Never());
            });
        }

        [Test]
        public void Execute_DuplicateRecord_DoesNotWriteCampaignResult()
        {
            SetupDuplicateDataSet();
            ExecuteWithTestMessage();

            fakeCampaignRepository.Verify(cr => cr.WriteTransitionResults(CallResult), Times.Never());
        }

        [Test]
        public void Execute_DuplicateRecord_DoesNotInsertPatientFollowUp()
        {
            SetupDuplicateDataSet();
            ExecuteWithTestMessage();
            fakeContractRepository.Verify(cr => cr.MarkFollowupDoneMaxAttemptsReached(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt), Times.Never());
        }

        [Test]
        [ExpectedException(typeof(InvalidMessageException))]
        public void Execute_InvalidMessage_ThrowsException()
        {
            ExecuteWithInvalidMessage();
        }

        [Test]
        [ExpectedException(typeof(InvalidMessageException))]
        public void Execute_NoCampaign_ThrowsException()
        {
            fakeCampaignRepository.Setup(cr => cr.GetTransitionResultCampaignData(CallResult.ContractID.ToString(), CallResult.CampaignID, 
                CallResult.PatientDischargeId, CallResult.EntityTypeID, CallResult.PatientID, CallResult.FileName)).Returns<DataSet>(null);

            ExecuteWithTestMessage();
        }

        #endregion

        #region Private Helper Methods

        private void SetupTestRecord()
        {
            CallResult.ContractID = rand.Next(9999);
            CallResult.EntityTypeID = rand.Next(0, 9999);
            CallResult.PatientDischargeId = rand.Next(9999);
            CallResult.PatientID = rand.Next(9999);
            CallResult.CalledCount = rand.Next(4);
            CallResult.CallerCode = String.Format("V{0}", rand.Next(9999));
            CallResult.CampaignID = rand.Next(9999);
            CallResult.LastCallAttempt = new DateTime(2013, 8, 26, 11, 15, 02);
            CallResult.LengthInSeconds = rand.Next(250);
            CallResult.PhoneNumberDialed = "7125551234";
            CallResult.SourceBatchID = rand.Next(9999);
            CallResult.StatusCode = "SVYCLM";
            CallResult.IncomingSurveyAnswers = "ATTEST01_Question_1_P^ATTEST01_Question_1_P>2^ATTEST01_Question_2_P^ATTEST01_Question_2_P>1^ATTEST01_Question_4_P";
            CallResult.FileName = "FileName";
            CallResult.QueueID = rand.Next(9999);
            CallResult.Attempts = CallResult.CalledCount;
            CallResult.CampaignTemplateID = rand.Next(9999);
            CallResult.QueueStatus = "QueueStatus";
            CallResult.NotifySender = 40;
            CallResult.ContractNumber = "RHS001";

            CallResult.SurveyAnswers[1].Comment = ESCALATION_COMMENT;
            CallResult.SurveyAnswers[1].Identifier = ESCALATION_IDENTIFIER;
        }

        private void ExecuteWithTestMessage()
        {
            process.Execute(CreateTestMessage());
        }

        private void ExecuteWithMaxAttemptsMessage()
        {
            process.Execute(CreateMaxAttemptsMessage());
        }

        private void ExecuteWithInvalidMessage()
        {
            process.Execute(CreateInvalidMessage());
        }

        private QueueMessage CreateTestMessage()
        {
            QueueMessage message = new QueueMessage();
            message.Body = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<CallResult xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"> " +
                "  <PatientDischargeId>{0}</PatientDischargeId> " +
                "  <PatientID>{1}</PatientID> " +
                "  <CalledCount>{2}</CalledCount> " +
                "  <CallerCode>{3}</CallerCode> " +
                "  <FileName>{4}</FileName> " +
                "  <LastCallAttempt>{5}</LastCallAttempt> " +
                "  <LengthInSeconds>{6}</LengthInSeconds> " +
                "  <PhoneNumberDialed>{7}</PhoneNumberDialed> " +
                "  <SourceBatchID>{8}</SourceBatchID> " +
                "  <StatusCode>{9}</StatusCode> " +
                "  <SurveyResults>{10}</SurveyResults> " +
                "  <ContractID>{11}</ContractID> " +
                "  <EntityTypeID>{12}</EntityTypeID> " +
                "  <CampaignID>{13}</CampaignID> " +
                "  <MaxAttempts>{14}</MaxAttempts> " +
                "  <ContractNumber>{15}</ContractNumber>" +
                "</CallResult> ",
                CallResult.PatientDischargeId, CallResult.PatientID, CallResult.CalledCount, CallResult.CallerCode, CallResult.FileName,
                CallResult.LastCallAttempt, CallResult.LengthInSeconds, CallResult.PhoneNumberDialed, CallResult.SourceBatchID,
                CallResult.StatusCode, CallResult.IncomingSurveyAnswers, CallResult.ContractID, CallResult.EntityTypeID, CallResult.CampaignID,
                MaxAttempts, CallResult.ContractNumber
                );

            return message;
        }



        private QueueMessage CreateMaxAttemptsMessage()
        {
            CallResult.CalledCount = MaxAttempts;
            CallResult.StatusCode = "AA";

            QueueMessage message = new QueueMessage();
            message.Body = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<CallResult xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"> " +
                "  <PatientDischargeId>{0}</PatientDischargeId> " +
                "  <PatientID>{1}</PatientID> " +
                "  <CalledCount>{2}</CalledCount> " +
                "  <CallerCode>{3}</CallerCode> " +
                "  <FileName>{4}</FileName> " +
                "  <LastCallAttempt>{5}</LastCallAttempt> " +
                "  <LengthInSeconds>{6}</LengthInSeconds> " +
                "  <PhoneNumberDialed>{7}</PhoneNumberDialed> " +
                "  <SourceBatchID>{8}</SourceBatchID> " +
                "  <StatusCode>{9}</StatusCode> " +
                "  <SurveyResults>{10}</SurveyResults> " +
                "  <ContractID>{11}</ContractID> " +
                "  <EntityTypeID>{12}</EntityTypeID> " +
                "  <CampaignID>{13}</CampaignID> " +
                "  <MaxAttempts>{14}</MaxAttempts> " +
                "  <ContractNumber>{15}</ContractNumber>" +
                "</CallResult> ",
                CallResult.PatientDischargeId, CallResult.PatientID, CallResult.CalledCount, CallResult.CallerCode, CallResult.FileName,
                CallResult.LastCallAttempt, CallResult.LengthInSeconds, CallResult.PhoneNumberDialed, CallResult.SourceBatchID,
                CallResult.StatusCode, CallResult.IncomingSurveyAnswers, CallResult.ContractID, CallResult.EntityTypeID, CallResult.CampaignID,
                MaxAttempts, CallResult.ContractNumber
                );

            return message;
        }

        private QueueMessage CreateInvalidMessage()
        {
            QueueMessage message = new QueueMessage();
            message.Body = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<CallResult xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"> " +
                "  <PatientDischargeId>{0}</PatientDischargeId> " +
                "  <PatientID>{1}</PatientID> " +
                "  <CalledCount>{2}</CalledCount> " +
                "  <CallerCode>{3}</CallerCode> " +
                "  <FileName>{4}</FileName> " +
                "  <LastCallAttempt>{5}</LastCallAttempt> " +
                "  <LengthInSeconds>{6}</LengthInSeconds> " +
                "  <PhoneNumberDialed>{7}</PhoneNumberDialed> " +
                "  <SourceBatchID>{8}</SourceBatchID> " +
                //"  <StatusCode>{9}</StatusCode> " +
                "  <SurveyResults>{10}</SurveyResults> " +
                "  <ContractID>{11}</ContractID> " +
                "  <EntityTypeID>{12}</EntityTypeID> " +
                "  <CampaignID>{13}</CampaignID> " +
                "  <MaxAttempts>{14}</MaxAttempts> " +
                "  <ContractNumber>{15}</ContractNumber>" +
                "</CallResult> ",
                CallResult.PatientDischargeId, CallResult.PatientID, CallResult.CalledCount, CallResult.CallerCode, CallResult.FileName,
                CallResult.LastCallAttempt, CallResult.LengthInSeconds, CallResult.PhoneNumberDialed, CallResult.SourceBatchID,
                CallResult.StatusCode, CallResult.IncomingSurveyAnswers, CallResult.ContractID, CallResult.EntityTypeID, CallResult.CampaignID,
                MaxAttempts, CallResult.ContractNumber
                );

            return message;
        }

        private void SetupEscalationReturnValues()
        {
            foreach (CampaignSurveyAnswer answer in CallResult.SurveyAnswers)
            {
                if (answer.ResponseCode == "1")
                {
                    DataTable table = new DataTable("Table");
                    table.Columns.Add("Comment");
                    table.Columns.Add("Identifier");
                    table.Columns.Add("Text");
                    table.Rows.Add(ESCALATION_COMMENT, ESCALATION_IDENTIFIER, ANSWER_TEXT);
                    DataSet set = new DataSet();
                    set.Tables.Add(table);
                    fakeCampaignRepository.Setup(cr => cr.GetEscalationData(CallResult.CampaignTemplateID, answer.QuestionNumber, answer.ResponseCode)).Returns(set);
                }
            }
        }

        private void SetupReturnCampaignInfoDataSet()
        {
            DataTable table = new DataTable("Table");
            table.Columns.Add("CampaignTemplateID");
            table.Columns.Add("Status");
            table.Columns.Add("QueueID");
            table.Columns.Add("Attempts");
            table.Rows.Add(CallResult.CampaignTemplateID,
                           CallResult.QueueStatus,
                           CallResult.QueueID,
                           CallResult.Attempts);
            DataSet set = new DataSet();
            set.Tables.Add(table);
            fakeCampaignRepository.Setup(cr => cr.GetTransitionResultCampaignData(CallResult.ContractID.ToString(), CallResult.CampaignID, CallResult.PatientDischargeId, CallResult.EntityTypeID,
                CallResult.PatientID, CallResult.FileName)).Returns(set);
        }

        private void SetupDuplicateDataSet()
        {
            fakeCampaignRepository.Setup(cr => cr.IsDuplicateRecord(CallResult.CampaignTemplateID, CallResult.PatientID, CallResult.PatientDischargeId,
                CallResult.EntityTypeID, CallResult.CallerCode)).Returns(true);
        }

        #endregion
    }
}
