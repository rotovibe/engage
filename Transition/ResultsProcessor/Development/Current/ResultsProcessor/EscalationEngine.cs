using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Atmosphere.Coordinate.Business;
using Atmosphere.Coordinate.Data;
using Atmosphere.Coordinate.Business.Interfaces;
using Atmosphere.Coordinate.Data.Campaign;

namespace ResultsProcessor
{
    public class EscalationEngine
    {
        public ICampaignRepository CampaignRepository { get; set; }

        public EscalationEngine(ICampaignRepository campRepository)
        {
            CampaignRepository = campRepository;
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Looks up and adds escalation information.
        /// </summary>
        /// <param name="answer">The survey answer.</param>
        /// <param name="campaignTemplateID">The campaign template ID.</param>
        /// <returns>The SurveyAnswer with escalation information</returns>
        ///////////////////////////////////////////////////////////////////////
        public CampaignSurveyAnswer LookupAndAddEscalations(CampaignSurveyAnswer answer, int campaignTemplateID)
        {
            answer = AddEscalationDataToAnswer(answer, campaignTemplateID);

            return answer;
        }

        private CampaignSurveyAnswer AddEscalationDataToAnswer(CampaignSurveyAnswer answer, int campaignTemplateID)
        {
            DataSet set = CampaignRepository.GetEscalationData(campaignTemplateID, answer.QuestionNumber, answer.ResponseCode);

            if (ContainsResults(set))
            {
                answer.Comment = set.Tables[0].Rows[0].ItemArray[0].ToString();
                answer.Identifier = int.Parse(set.Tables[0].Rows[0].ItemArray[1].ToString());
            }

            return answer;
        }

        private static bool ContainsResults(DataSet dataSet)
        {
            try
            {
                string test;
                test = dataSet.Tables[0].Rows[0].ItemArray[0].ToString();
                test = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
