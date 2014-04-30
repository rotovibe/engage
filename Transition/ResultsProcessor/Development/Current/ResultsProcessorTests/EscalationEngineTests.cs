using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Moq;
using NUnit.Framework;
using ResultsProcessor;
using Phytel.Framework.ASE.Bus.Data;
using Atmosphere.Coordinate.Business.Interfaces;
using Atmosphere.Coordinate.Data.Campaign;

namespace ResultsProcessorTests
{
    [TestFixture]
    public class EscalationEngineTests
    {
        #region Variables

        public const int CAMPAIGN_TEMPLATE_ID = 12345;
        public const string QUESTION_NUMBER = "Question_Number_1";
        public const string ESCALATION_RESPONSE_CODE = "1";
        public const string NON_ESCALATION_RESPONSE_CODE = "2";
        public const string COMMENT = "Followup Reason";
        public const int IDENTIFIER = 3;

        Mock<ICampaignRepository> fakeCampaignRepository;
        public EscalationEngine engine;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            fakeCampaignRepository = new Mock<ICampaignRepository>();
            engine = new EscalationEngine(fakeCampaignRepository.Object);
            SetupEscalationReturnValues();
        }

        #endregion

        #region Tests

        [Test]
        public void AddEscalationData_WithEscalation_CallsRepository()
        {
            CampaignSurveyAnswer result = engine.LookupAndAddEscalations(AnswerWithEscalation(), CAMPAIGN_TEMPLATE_ID);

            fakeCampaignRepository.Verify(cr => cr.GetEscalationData(CAMPAIGN_TEMPLATE_ID, QUESTION_NUMBER, ESCALATION_RESPONSE_CODE), Times.Once());
        }

        [Test]
        public void AddEscalationData_NoEscalation_CallsRepository()
        {
            CampaignSurveyAnswer result = engine.LookupAndAddEscalations(AnswerWithoutEscalation(), CAMPAIGN_TEMPLATE_ID);

            fakeCampaignRepository.Verify(cr => cr.GetEscalationData(CAMPAIGN_TEMPLATE_ID, QUESTION_NUMBER, NON_ESCALATION_RESPONSE_CODE), Times.Once());
        }

        [Test]
        public void AddEscalationData_WithEscalation_AddsData()
        {
            CampaignSurveyAnswer result = engine.LookupAndAddEscalations(AnswerWithEscalation(), CAMPAIGN_TEMPLATE_ID);

            Assert.AreEqual(COMMENT, result.Comment);
            Assert.AreEqual(IDENTIFIER, result.Identifier);
        }

        [Test]
        public void AddEscalationData_NoEscalation_ReturnsOriginal()
        {
            CampaignSurveyAnswer result = engine.LookupAndAddEscalations(AnswerWithoutEscalation(), CAMPAIGN_TEMPLATE_ID);

            Assert.IsNull(result.Comment);
            Assert.AreEqual(-1, result.Identifier);
        }

        #endregion

        #region Private Helper Methods

        private CampaignSurveyAnswer AnswerWithEscalation()
        {
            CampaignSurveyAnswer answer = new CampaignSurveyAnswer();
            answer.QuestionNumber = QUESTION_NUMBER;
            answer.ResponseCode = ESCALATION_RESPONSE_CODE;

            return answer;
        }

        private CampaignSurveyAnswer AnswerWithoutEscalation()
        {
            CampaignSurveyAnswer answer = new CampaignSurveyAnswer();
            answer.QuestionNumber = QUESTION_NUMBER;
            answer.ResponseCode = NON_ESCALATION_RESPONSE_CODE;

            return answer;
        }

        private void SetupEscalationReturnValues()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Comment");
            table.Columns.Add("Identifier");
            table.Rows.Add(COMMENT, IDENTIFIER);
            DataSet set = new DataSet();
            set.Tables.Add(table);
            fakeCampaignRepository.Setup(cr => cr.GetEscalationData(CAMPAIGN_TEMPLATE_ID, QUESTION_NUMBER, ESCALATION_RESPONSE_CODE)).Returns(set);
        }

        #endregion
    }
}
