using System;
using System.Collections.Generic;
using System.Web;
using Phytel.Framework.ErrorHandler;
using Phytel.Framework.ASE.Process;
using System.Configuration;

namespace Phytel.CommService
{

    public static class LogHelper
    {
        public static void LogError(Exception excep)
        {
            string excepMessage = string.Empty;
            Exception exc;

            try
            {
                // Loop inner exceptions to get full description
                exc = excep;
                excepMessage = exc.Message;
                while (exc.InnerException != null)
                {
                    exc = exc.InnerException;
                    excepMessage += Environment.NewLine + "INNER: " + exc.Message;
                }

                LogError(excepMessage);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LogError(string msg)
        {
            try
            {
                QueueMessage queueMsg = new QueueMessage();
                queueMsg.Body = "Comm Service Exception: " + Environment.NewLine + msg;
                queueMsg.Header.Type = Phytel.Framework.ASE.Common.MessageType.LogError;
                queueMsg.Header.SourceProcessID = Convert.ToInt16(ConfigurationManager.AppSettings.Get("ASEPDSProcessID"));
                MessageQueueHelper.SendMessage(ConfigurationManager.AppSettings.Get("ASELogQueueName"), queueMsg, "Comm Service Error");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
