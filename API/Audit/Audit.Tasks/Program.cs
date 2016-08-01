using System;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

namespace Audit.Tasks
{
    class Program
    {
        static string _token = "531796e0d6a4850c20c9c97d";
        static string _version = "1.0";
        static string _contractnumber = "inhealth001";
        static string _patientid = "52f55859072ef709f84e5e20";
        static string _cohortid = "528ed977072ef70e10099685";
        static string _patientprogramid = "52f56d9fd6a4850fd025fb67";
        static string _typename = "CommMode";
        static string _flagged = "true";
        static string _patientgoalid = "0";
        static string _id = "0";

        static void Main(string[] args)
        {
            EndpointTest test = new EndpointTest(_token, _version, _contractnumber, _patientid, _cohortid,
                                                                        _patientprogramid, _typename, _flagged, _patientgoalid, _id);

            test.HitEndpoints_GET(true);

            //TestAuditFailure(3);

            Console.ReadLine();
        }

        private static void TestAuditFailure(int numberOfMessagesToCreate)
        {
            while (numberOfMessagesToCreate > 0)
            {
                QueueMessage msg = new QueueMessage();

                DataAudit da = new DataAudit();
                da.Contract = "inhealth001";
                da.EntityID = "531f2dcc072ef727c4d29e22";
                da.EntityType = "testentitytype";
                da.UserId = "531f2dcc072ef727c4d29yyy";
                da.TimeStamp = DateTime.Now;

                string xmlBody = AuditDispatcher.ToXML(da);
                string messageQueue = @".\private$\failure";


                QueueMessage newMessage = new QueueMessage(ASEMessageType.Process, messageQueue);
                newMessage.Body = xmlBody;

                MessageQueueHelper.SendMessage(@messageQueue, newMessage, "TestFailureType");

                --numberOfMessagesToCreate;
            }


        }
    }
}
