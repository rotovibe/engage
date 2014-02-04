using System;
using System.Configuration;
using System.Data;
using System.Web;
using Phytel.API.Interface;
using Phytel.Services;
using ServiceStack.ServiceHost;

namespace Phytel.API.Common.Audit
{
    public class AuditHelper
    {
        /// <summary>
        /// Gets an auditdata record for the current context
        /// </summary>
        /// <returns></returns>
        public static AuditData GetAuditLog(int auditTypeId, IAppDomainRequest request, HttpRequest webrequest, string methodCalledFrom, bool isError = false)
        {
            ///{Version}/{ContractNumber}/patient/{PatientID}
            AuditData auditLog = new AuditData() 
            {
                Type = isError ? "ErrorMessage": string.Format("NG_{0}",methodCalledFrom) ,  
                AuditTypeId = auditTypeId, //derive this from the type passed in (lookup on PNG.AuditType.Name column)
                UserId = new Guid(request.UserId), //derive this from Mongo.APISessions.uid...lookup on Mongo.APISessions._id)
                SourcePage = methodCalledFrom, //derive this from querystring...utility function
                SourceIP = webrequest.UserHostAddress,
                Browser = webrequest.Browser.Type,
                SessionId = request.Token,
                ContractID = GetContractID(request.ContractNumber) //request.ContractNumber
            };

            return auditLog;
        }

        private static int GetContractID(string contractNumber)
        {
            int id = 0;
             
            string dbname = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
            Parameter parm = new Parameter("@ContractNumber", contractNumber, SqlDbType.VarChar, ParameterDirection.Input, 30);
            ParameterCollection parms = new ParameterCollection();
            parms.Add(parm);

            DataSet ds = SQLDataService.Instance.GetDataSet(dbname, false, "spPhy_GetContract", parms);
            if (ds.Tables[0].Rows.Count > 0)
            {
                id = int.Parse(ds.Tables[0].Rows[0]["ContractId"].ToString());
            }

            return id;
        }

        public static int GetAuditTypeID(string callingMethod)
        {
            int audittypeid = 0;

            switch (callingMethod)
            {
                case "GetPatient":
                    audittypeid = (int)AuditType.GetPatient;
                    break;

                case "GetActionsInfo":
                    audittypeid = (int)AuditType.GetActionsInfo;
                    break;

                case "GetActivePrograms":
                    audittypeid = (int)AuditType.GetActivePrograms;
                    break;

                case "GetAllCommModes":
                    audittypeid = (int)AuditType.GetAllCommModes;
                    break;

                case "GetAllCommTypes":
                    audittypeid = (int)AuditType.GetAllCommTypes;
                    break;

                case "GetAllLanguages":
                    audittypeid = (int)AuditType.GetAllLanguages;
                    break;

                case "GetAllSettings":
                    audittypeid = (int)AuditType.GetAllSettings;
                    break;

                case "GetAllStates":
                    audittypeid = (int)AuditType.GetAllStates;
                    break;

                case "GetAllTimesOfDays":
                    audittypeid = (int)AuditType.GetAllTimesOfDays;
                    break;

                case "GetAllTimeZones":
                    audittypeid = (int)AuditType.GetAllTimeZones;
                    break;

                case "GetCohortPatients":
                    audittypeid = (int)AuditType.GetCohortPatients;
                    break;

                case "GetCohorts":
                    audittypeid = (int)AuditType.GetCohorts;
                    break;

                case "GetContact":
                    audittypeid = (int)AuditType.GetContact;
                    break;

                case "GetModuleInfo":
                    audittypeid = (int)AuditType.GetModuleInfo;
                    break;

                case "GetPatientProblems":
                    audittypeid = (int)AuditType.GetPatientProblems;
                    break;

                case "GetPatientProgramDetailsSummary":
                    audittypeid = (int)AuditType.GetPatientProgramDetailsSummary;
                    break;

                case "GetProblems":
                    audittypeid = (int)AuditType.GetProblems;
                    break;

                case "GetResponses":
                    audittypeid = (int)AuditType.GetResponses;
                    break;

                case "GetSpamElement":
                    audittypeid = (int)AuditType.GetSpamElement;
                    break;

                case "GetStepsInfo":
                    audittypeid = (int)AuditType.GetStepsInfo;
                    break;

                case "PostPatientToProgram":
                    audittypeid = (int)AuditType.PostPatientToProgram;
                    break;

                case "PutPatientDetailsUpdate":
                    audittypeid = (int)AuditType.PutPatientDetailsUpdate;
                    break;

                case "PutPatientFlaggedUpdate":
                    audittypeid = (int)AuditType.PutPatientFlaggedUpdate;
                    break;

             }

            return audittypeid;
        }

        public static string FindMethodType(string returnTypeName)
        {
            return returnTypeName.ToString().Replace("Request", "").Replace("Response", "");
        }

    }
}
