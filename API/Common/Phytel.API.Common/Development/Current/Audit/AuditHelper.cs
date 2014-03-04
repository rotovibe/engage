using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Interface;
using Phytel.Framework.ASE.Data.Common;
using Phytel.Services;
using ServiceStack.ServiceHost;
using ASE = Phytel.Framework.ASE.Process;

namespace Phytel.API.DataAudit
{
    public class AuditHelper
    {
        static string _requesttype;
        static string _entitytype;

        /// <summary>
        /// Gets an auditdata record for the current context
        /// </summary>
        /// <returns></returns>
        public static AuditData GetAuditLog(int auditTypeId, IAppDomainRequest request, List<string> patientids, HttpRequest webrequest, string methodCalledFrom, bool isError = false)
        {
            ///{Version}/{ContractNumber}/patient/{PatientID}
            AuditData auditLog = new AuditData()
            {
                Type = isError ? "ErrorMessage" : string.Format("NG_{0}", methodCalledFrom),
                AuditTypeId = auditTypeId, //derive this from the type passed in (lookup on PNG.AuditType.Name column)
                UserId = new Guid(request.UserId), //derive this from Mongo.APISessions.uid...lookup on Mongo.APISessions._id)
                SourcePage = methodCalledFrom, //derive this from querystring...utility function
                Browser = webrequest.Browser.Type,
                SessionId = request.Token,
                ContractID = GetContractID(request.ContractNumber), //request.ContractNumber
                Patients = patientids
            };

            //this assignment is randomly throwing an exception, so it needs it's own method
            try
            {
                auditLog.SourceIP = webrequest.UserHostAddress;
            }
            catch (Exception)
            {
                auditLog.SourceIP = "Unknown";
            }

            return auditLog;
        }

        private static DataAudit GetAuditLog(string userId, string collectionName, string entityId, DataAuditType auditType, string contractNumber)
        {
           
            DataAudit auditLog = new DataAudit()
            {
                EntityID = entityId,
                UserId = userId,
                Contract = contractNumber,
                EntityType = collectionName,
                Type = auditType.ToString(),
                Entity = GetMongoEntity(collectionName, entityId)
            };

            return auditLog;
        }

        private static object GetMongoEntity(string collectionName, string entityId)
        {
            return null; ///returning null until I can get the process to run end to end, then I'll build the getter for this
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

                case "GetAllPatientProblems":
                    audittypeid = (int)AuditType.GetAllPatientProblems;
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

        public static void LogAuditData(string userId, string collectionName, string entityId, DataAuditType auditType, string contractNumber)
        {
            //hand to a new thread here, and immediately return this thread to caller
            try
            {
                //throw new SystemException("test before new thread starts");
                new Thread(() =>
                {
                    try
                    {
                        AuditAsynch(userId, collectionName, entityId, auditType, contractNumber);
                    }
                    catch (Exception newthreadex)
                    {
                        //if there's an error from the new thread, handle it here, so we don't black the main thread

                        int procID = int.Parse(ConfigurationManager.AppSettings.Get("ASEProcessID"));
                        ASE.Log.LogError(procID, newthreadex, LogErrorCode.Error, LogErrorSeverity.Medium);

                    }

                }).Start();

            }
            catch (Exception ex)
            {
                //handle the exception here, to make sure we don't block the main thread?

            }

            return;


        }

        public static void LogAuditData(IAppDomainRequest request, List<string> patientids, HttpRequest webreq, string returnTypeName)
        {
            //hand to a new thread here, and immediately return this thread to caller
            try
            {
                //throw new SystemException("test before new thread starts");
                new Thread(() =>
                {
                    try
                    {
                        AuditAsynch(request, patientids, webreq, returnTypeName);
                    }
                    catch (Exception newthreadex)
                    {
                        //if there's an error from the new thread, handle it here, so we don't black the main thread

                        int procID = int.Parse(ConfigurationManager.AppSettings.Get("ASEProcessID"));
                        ASE.Log.LogError(procID, newthreadex, LogErrorCode.Error, LogErrorSeverity.Medium);

                    }

                }).Start();

            }
            catch (Exception ex)
            {
                //handle the exception here, to make sure we don't block the main thread?

            }

            return;


        }

        private static void AuditAsynch(IAppDomainRequest request, List<string> patientids, HttpRequest webreq, string returnTypeName)
        {
            //throw new SystemException("test error in new thread starts");

            string callingMethod = FindMethodType(returnTypeName);
            int auditTypeId = GetAuditTypeID(callingMethod);
            AuditData data = GetAuditLog(auditTypeId, request, patientids, webreq, callingMethod);
            AuditDispatcher.WriteAudit(data);
        }

        private static void AuditAsynch(string userId, string collectionName, string entityId, DataAuditType auditType, string contractNumber)
        {
            //throw new SystemException("test error in new thread starts");

            DataAudit data = GetAuditLog(userId, collectionName, entityId, auditType, contractNumber);
            AuditDispatcher.WriteAudit(data);
        }


    }
}
