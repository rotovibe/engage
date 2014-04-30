using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using Atmosphere.Coordinate.Business;
using Atmosphere.Coordinate.Business.Interfaces;
using Atmosphere.Coordinate.Data.Campaign;
using C3.Domain.Repositories.Abstract;
using C3.Domain.Repositories.Concrete.RestService;
using Phytel.Framework.ASE.Process;

namespace ResultsProcessor
{
    public class ResultsQueueProcess : QueueProcessBase
    {
        #region Variables

        public CampaignCallResult CallResult = new CampaignCallResult();
        private int MaxAttempts;
        private const int NOTIFY_SENDER_DEFAULT = 0;

        public EscalationEngine LocalEscalationEngine;
        public ICampaignRepository CampaignRepository { get; set; }
        public IDischargePatientMgmtRepository ContractRepository { get; set; }

        #endregion

        #region Constructor

        public ResultsQueueProcess()
        {
            CampaignRepository = new RestCampaignRepository();
            ContractRepository = new RestDischargePatientMgmtRepository();
            LocalEscalationEngine = new EscalationEngine(CampaignRepository);
        }

        #endregion

        #region Public Methods

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Executes the ResultsProcess using the message.
        /// </summary>
        /// <param name="message">The queue message from PDL.</param>
        ///////////////////////////////////////////////////////////////////////
        public override void Execute(QueueMessage message)
        {
            ParseMessage(message);
            MarkPatientDischargeCompleted();
            AddCampaignData();
            DetermineNotifySender();
            ProcessResult();
            //base.LogInformation("Finished Processing Successfully");
        }

        #endregion

        #region Private Methods

        private void ParseMessage(QueueMessage queueMessage)
        {
            XmlDocument MessageBody = new XmlDocument();

            try
            {
                MessageBody.LoadXml(queueMessage.Body.Trim());
                CallResult.ContractID = int.Parse(MessageBody.SelectSingleNode("/CallResult/ContractID").InnerText);
                CallResult.ContractNumber = MessageBody.SelectSingleNode("/CallResult/ContractNumber").InnerText;
                CallResult.PatientID = int.Parse(MessageBody.SelectSingleNode("/CallResult/PatientID").InnerText);
                CallResult.StatusCode = MessageBody.SelectSingleNode("/CallResult/StatusCode").InnerText;
                CallResult.FileName = MessageBody.SelectSingleNode("/CallResult/FileName").InnerText;
                CallResult.SourceBatchID = int.Parse(MessageBody.SelectSingleNode("/CallResult/SourceBatchID").InnerText);
                CallResult.CalledCount = int.Parse(MessageBody.SelectSingleNode("/CallResult/CalledCount").InnerText);
                CallResult.LastCallAttempt = DateTime.Parse(MessageBody.SelectSingleNode("/CallResult/LastCallAttempt").InnerText);
                CallResult.CallerCode = MessageBody.SelectSingleNode("/CallResult/CallerCode").InnerText;
                CallResult.PhoneNumberDialed = MessageBody.SelectSingleNode("/CallResult/PhoneNumberDialed").InnerText;
                CallResult.LengthInSeconds = int.Parse(MessageBody.SelectSingleNode("/CallResult/LengthInSeconds").InnerText);                              
                CallResult.PatientDischargeId = int.Parse(MessageBody.SelectSingleNode("/CallResult/PatientDischargeId").InnerText);
                CallResult.EntityTypeID = int.Parse(MessageBody.SelectSingleNode("/CallResult/EntityTypeID").InnerText);
                CallResult.CampaignID = int.Parse(MessageBody.SelectSingleNode("/CallResult/CampaignID").InnerText);
                CallResult.IncomingSurveyAnswers = MessageBody.SelectSingleNode("/CallResult/SurveyResults").InnerText;  
                MaxAttempts = int.Parse(MessageBody.SelectSingleNode("/CallResult/MaxAttempts").InnerText);
            }
            catch (Exception ex)
            {
                throw new InvalidMessageException(ex.Message);
            }
        }

        private void AddCampaignData()
        {
            DataSet campaignData = CampaignRepository.GetTransitionResultCampaignData(CallResult.ContractID.ToString(), CallResult.CampaignID, CallResult.PatientDischargeId, CallResult.EntityTypeID,
                CallResult.PatientID, CallResult.FileName);
            if (ContainsResults(campaignData))
            {
                CallResult.CampaignTemplateID = int.Parse(campaignData.Tables[0].Rows[0].ItemArray[0].ToString());
                CallResult.QueueStatus = campaignData.Tables[0].Rows[0].ItemArray[1].ToString();
                CallResult.QueueID = int.Parse(campaignData.Tables[0].Rows[0].ItemArray[2].ToString());
                CallResult.Attempts = int.Parse(campaignData.Tables[0].Rows[0].ItemArray[3].ToString());
            }
            else
                throw new InvalidMessageException("Error retreiving additional data");
        }

        private void DetermineNotifySender()
        {
            CallResult.NotifySender = NOTIFY_SENDER_DEFAULT;

            if (CallResult.StatusCode == "SVYCLM" || CallResult.StatusCode == "XFER")
                CallResult.NotifySender = 40;
            else if (CallResult.StatusCode == "ADC" || CallResult.StatusCode == "AFAX")
                CallResult.NotifySender = 30;
            else if (CallResult.StatusCode == "AB" || CallResult.StatusCode == "CPDB")
                CallResult.NotifySender = 21;
            else if (CallResult.StatusCode == "CPDREJ" || CallResult.StatusCode == "PM" || CallResult.StatusCode == "PU" || CallResult.StatusCode == "NI")
                CallResult.NotifySender = 23;
            else if (CallResult.StatusCode == "CPDNA" || CallResult.StatusCode == "NA")
                CallResult.NotifySender = 22;
            else if (CallResult.StatusCode == "AL" || CallResult.StatusCode == "AM" || CallResult.StatusCode == "AA")
                CallResult.NotifySender = 41;
            else if (CallResult.StatusCode == "CPDATB" || CallResult.StatusCode == "CPDSR" || CallResult.StatusCode == "CPDSV" || CallResult.StatusCode == "CPDUK" ||
                CallResult.StatusCode == "DROP" || CallResult.StatusCode == "PDROP" || CallResult.StatusCode == "LRERR")
                CallResult.NotifySender = 3;
            else if (CallResult.StatusCode == "DNC")
                CallResult.NotifySender = 162;
        }

        private void MarkPatientDischargeCompleted()
        {
            ContractRepository.MarkPatientDischargeCompleted(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.SourceBatchID);
        }

        private bool IsDuplicateRecord()
        {
            return CampaignRepository.IsDuplicateRecord(CallResult.CampaignTemplateID, CallResult.PatientID, CallResult.PatientDischargeId,
                CallResult.EntityTypeID, CallResult.CallerCode);
        }

        private void ProcessResult()
        {
            if (ValidToProcess())
            {
                SetStatusIfMaxAttemptsReached();
                CallResult.SurveyAnswers.ForEach(sa =>
                {
                    sa = LocalEscalationEngine.LookupAndAddEscalations(sa, CallResult.CampaignTemplateID);
                    if (AnswerCausesEscalation(sa))
                    {
                        InsertPatientFollowUpReason(sa);
                    }
                });
                WriteCampaignResult();
                MarkFollowupDoneIfMaxAttemptsReached();
                MarkPatientFollowUpDone();
            }
        }

        private bool ValidToProcess()
        {
            return CallResult.Attempts <= MaxAttempts && !(IsDuplicateRecord());
        }

        private void SetStatusIfMaxAttemptsReached()
        {
            if (MaxAttemptsReached())
                CallResult.QueueStatus = "COMPLETE";
        }

        private void InsertPatientFollowUpReason(CampaignSurveyAnswer answer)
        {
            ContractRepository.InsertPatientFollowUpReason(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt.ToString("yyyy-MM-dd-hh-mm-ss"), answer.Comment, answer.Identifier);
        }

        private static bool AnswerCausesEscalation(CampaignSurveyAnswer answer)
        {
            return answer.Identifier != -1;
        }

        private void WriteCampaignResult()
        {
            CampaignRepository.WriteTransitionResults(CallResult);
        }

        private void MarkFollowupDoneIfMaxAttemptsReached()
        {
            if (MaxAttemptsReached() && CallResult.StatusCode != "SVYCLM")
            {
                ContractRepository.MarkFollowupDoneMaxAttemptsReached(CallResult.ContractNumber, CallResult.PatientDischargeId, CallResult.LastCallAttempt);
            }
        }

        private void MarkPatientFollowUpDone()
        {
            if (MaxAttemptsReached() || CallResult.HasSurveyAnswers)
            {
                ContractRepository.MarkPatientFollowUpDone(CallResult.ContractNumber, CallResult.PatientDischargeId);                
            }
        }

        private bool MaxAttemptsReached()
        {
            return CallResult.CalledCount >= MaxAttempts || CallResult.Attempts >= MaxAttempts;
        }

        private static bool ContainsResults(DataSet dataSet)
        {
            return dataSet != null && dataSet.Tables != null && dataSet.Tables[0].Rows != null;
        }

        #endregion
    }
}
