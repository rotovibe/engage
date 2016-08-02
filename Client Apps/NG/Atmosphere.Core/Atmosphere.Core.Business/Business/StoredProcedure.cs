
namespace C3.Business
{
    public static class StoredProcedure
    {
        #region ApplicationSetting
        public static string GetAllApplicationSettings = "spPhy_GetApplicationSetting";
        #endregion

        #region Application Messages
        public static string GetAllApplicationMessages = "spPhy_GetApplicationMessage";
        #endregion

        #region Appointments
        public static string GetAppointments = "spPhy_C3GetAppointments";
        public static string GetResponses = "spPhy_GetResponses";
        public static string GetAppointmentTypes = "spPhy_C3GetAppointmentTypes";
        #endregion        

        #region Controls
        public static string GetAllControlPermissions = "spPhy_GetAllControlPermissions";

        public static string GetTabs = "spPhy_GetControlTabs_13";
        public static string GetAllControls = "spPhy_GetAllControls_13";
        public static string GetSubTabs = "spPhy_GetControlSubTabs_13";
        public static string GetTabsByUser = "spPhy_GetControlTabsByUserId_13";
        #endregion

        #region Contracts

        public static string GetAllContracts = "spPhy_GetAllContracts";
        public static string GetUsersByContractId = "spPhy_GetContractUsersToImpersonate";
        public static string GetContractById = "spPhy_GetContractById";

        #endregion

        #region Facilities
        public static string GetAllFacilitiesForContract = "spPhy_C3GetAllFacilities";
        #endregion

        #region Groups
        public static string GetAllGroupsByContract = "spPhy_C3GetAllGroups";
        public static string GetGroupsByUser = "spPhy_C3GetGroupsByUser";
        public static string SaveGroup = "spPhy_C3GroupScheduleSave";
        public static string GetGroupsByUserAndProduct = "spPhy_C3GetGroupsByUserAndProduct";
        public static string GetOutreachSettings = "spPhy_C3GetOutreachSettings";

        #endregion

        #region Groups
        public static string GetReportPopulationPerformance = "spPhy_ReportPopulationPerformance";
        #endregion

        #region Measures

        public static string GetProviderMeasureData = "spPhy_C3GetProviderMeasureData";
        #endregion

        #region OrgFunctions

        public static string GetContractOrgFunctions = "spPhy_GetContractOrgFunctions";
        public static string GetUserContractOrgFunctions = "spPhy_GetUserContractOrgFunctions";
        public static string SaveUserContractOrgFunctions = "spPhy_SaveUserContractOrgFunctions";

        #endregion OrgFunctions

        #region OptOut
        public static string GetOptOutTypes = "spPhy_C3GetOptOutTypes";
        public static string GetOptOutReasons = "spPhy_C3GetOptOutReasons";
        public static string GetOptOutByPatientId = "spPhy_C3GetPatientOptOut";
        public static string DeletePatientOptOut = "spPhy_C3DeleteOptOutById";
        public static string UpdatePatientOptOut = "spPhy_C3SetPatientOptOut";
        #endregion

        #region Outreach

        public static string GetORResponses = "spPhy_GetOutreachResponses";
        public static string GetReasonTypes = "spPhy_C3GetORReasonTypes";
        public static string GetOutreachEvents = "spPhy_C3GetOutreachRecalls";

        #endregion

        #region PasswordHistory
        public static string GetPasswordHistory = "spPhy_GetPasswordHistory";
        public static string SetPasswordHistory = "spPhy_SetPasswordHistory";
        #endregion

        #region Patients
        public static string SearchPatients = "spPhy_C3SearchPatients";
		public static string SearchPatientsDetails = "spPhy_C3SearchPatientsDetails";
        public static string GetPatientByPatientId = "spPhy_C3GetPatientSummary";
        public static string GetPatientWithCareOpportunities = "spPhy_C3GetProviderMeasurePatients";
        public static string GetPatientBySubscribers = "spPhy_C3GetProviderPatients";
        public static string GetPopulationSummaryData = "spPhy_C3GetPopulationSummaryData";

        #endregion

        #region Permission
        public static string SaveUserPermission = "spPhy_InsertUserPermission";
        public static string GetAllPermissions = "spPhy_GetAllPermissions";
        #endregion

        #region Population Benchmark

        public static string PopulationBenchmarkGetData = "spPhy_C3GetPopulationBenchmarkData";
        #endregion

        #region Population Summary

        public static string PopulationSummaryGetFilters = "spPhy_C3GetPopulationSummaryReportFilters";
        #endregion

        #region Programs
        public static string GetProgramAndRelatedReportFilters = "spPhy_C3GetReportFilters";
        #endregion

        # region PQRS

        public static string GetPQRSSubmissionSummary = "spPhy_PQRSGetSubscriberMeasureList";
        public static string GetPQRSFilters = "spPhy_PQRSGetFilters";
        public static string GetPQRSSubmissionSurveyFormDefinitions = "spPhy_DistributedGetPqriDocument";
        public static string GetPQRSPatientList = "spPhy_PQRSGetUserDefinedPatientList";
        public static string GetPQRSSubmissionMeasure = "spPhy_PQRSGetMeasureBySubmissionId";
        public static string GetPQRSSubmissionYear = "spPhy_PQRSGetSubmissionYear";
        public static string ManagePQRSPatientSet = "spPhy_PQRSManagePatientSet";
        public static string SubmitPQRSMeasure = "spPhy_PQRSSubmitMeasure";
        public static string UpdatePQRSGroupSet = "spPhy_PQRSUpdateDetail";
        public static string GetPQRSSubmissionPatientList = "spPhy_PQRSGetSubmissionPatients";
        public static string ValidateMeasureCodes = "spPhy_PQRSValidateMeasureCodes";

        #endregion

        #region ResendNotifications
        public static string GetResendTotals = "spPhy_C3GetActivityResendCounts";
        public static string SetResendTotals = "spPhy_C3SetActivityResendCounts";
        #endregion

        #region Reports

        public static string GetAllReports = "spPhy_C3GetAllReports";
        public static string GetReportByID = "spPhy_C3GetReportByReportID";
        public static string GetAllReportsByUser = "spPhy_C3GetAllReportsByUserId";
        public static string GetDefaultReportPath = "spPhy_GetDefaultReportPath";
        public static string AddEditReport = "spPhy_C3AddEditReport";

        #endregion

        #region Roles
        public static string GetContractRoles = "spPhy_GetAllRolesByContract";
        public static string GetRolePermissions = "spPhy_GetRolePermissions";
        public static string SaveUserRoles = "spPhy_SaveUserRoles";
        public static string GetRoleByName = "spPhy_GetRoleByName";
        #endregion

        #region SecurityQuestion
        public static string GetSecurityQuestion = "spPhy_GetSecurityQuestion";
        public static string SetSecurityQuestionAndAnswer = "spPhy_SetPasswordQuestionAndAnswer";
        #endregion

        #region Statuses
        public static string GetAllStatuses = "spPhy_GetAllStatusTypes";
        #endregion

        #region Schedules

        public static string GetSchedulesByUser = "spPhy_C3GetSchedulesByUser";
        public static string GetSchedulesByFacility = "spPhy_C3GetSchedulesByFacility";
        public static string GetSchedulesByGroup = "spPhy_C3GetSchedulesByGroup";
        public static string GetSchedulesByGroups = "spPhy_C3GetSchedulesByGroups";

        #endregion

        #region Subscribers
        public static string GetSubscribersByUser = "spPhy_C3GetSubscriberByUserAndEntityType";
        public static string GetSubscribersByGroup = "spPhy_C3GetSubscribersByGroup";
        public static string GetPQRSSubscriberBySubmissionId = "spPhy_PQRSGetSubscriberById";
        #endregion

        #region Transition
        public static string TransitionGetHospitals = "spPhy_TransitionGetHospitals";
        /// <summary>
        /// spPhy_TransitionGetFollowupHistory
        /// </summary>
        public static string TransitionGetFollowupHistory = "spPhy_TransitionGetFollowupHistory";
        public static string TransitionGetFollowupRecordsByPatient = "spPhy_TransitionGetFollowupRecordsByPatientID";
        public static string TranstitionGetFollowHistoryByFollowup = "spPhy_TransitionGetNotesforFollowup";
        
        /// <summary>
        /// spPhy_TransitionGetPatients
        /// </summary>
        public static string GetTransitionPatients = "spPhy_TransitionGetPatients";
        //public static string GetTransitionPatients = "spPhy_TransitionGetPatients_Filtered";
        public static string TransitionAddNote = "spPhy_TransitionAddNote";
        public static string TransitionCloseFollowup = "spPhy_TransitionCloseFollowup";
        public static string GetCampaignReportData = "spPhy_TransitionGetCampaignReportData";
        public static string GetClientSettings = "spPhy_TransitionGetClientSettings";
        public static string GetDialerSettings = "spPhy_TransitionGetDialerSettings";
        public static string GetReportSettings = "spPhy_TransitionGetReportSettings";
        public static string GetTransitionSurveys = "spPhy_TransitionGetTransitionSurveys";
        public static string TransitionAddFollowup = "spPhy_TransistionAddFollowup";
        public static string TransitionModifyFollowup = "spPhy_TransitionModifyFollowup";
        public static string GetDischargeConfiguration = "spPhy_TransitionDischargeConfiguration";
        public static string TransitionCancelFollowup = "spPhy_TransitionCancelScheduledFollowup";
        public static string GetFollowupAction = "spPhy_TransitionGetFollowupActionList";
        public static string SavePatientDetails = "spPhy_TransitionSaveFollowupDetails";
        public static string GetPatientHeaderByContact = "spPhy_TransitionGetPatientsFromContactId";
        #endregion

        #region TOS
        public static string GetLatestTOS = "spPhy_GetLatestTOS";
        #endregion

        #region UserContracts
        public static string GetUserContracts = "spPhy_GetUserContracts";
        public static string InsertUserContract = "spPhy_InsertUserContract";
        public static string DeleteUserFromContract = "spPhy_UserContract_Delete";
        public static string DeleteUserContractByUser = "spPhy_UserContractDeleteByUserId";
        #endregion

        #region UserGroups
        public static string SaveUserGroups = "spPhy_C3UserGroupSave";
        #endregion

        #region User
        public static string GenerateUserToken = "spPhy_GenerateUserToken";
        public static string UpdateUser = "spPhy_UpdateUser";
        public static string GetUser = "spPhy_GetUser";
        public static string GetPhytelUsers = "spPhy_GetPhytelUsers";
        public static string CheckUserValid = "spPhy_CheckUserValid";
        public static string SetFirstTimeUser = "spPhy_SetFirstTimeUser";
        public static string GetUsers = "spPhy_GetUsers";
        public static string LockOutUser = "spPhy_SetLockOut";
        public static string GetUserRoles = "spPhy_GetUserRolesByContract";
        public static string GetUserRolesAll = "spPhy_GetUserRolesByContractForAllUsers";
        public static string GetStatuses = "spPhy_GetStatuses";
        public static string SetPasswordExpiration = "spPhy_SetPasswordExpiration";
        public static string SaveUserSession = "spPhy_SaveUserSession";
        public static string SetFailedPasswordAttemptCount = "spPhy_SetFailedPasswordAttemptCount";
        public static string SetFailedPasswordAnswerAttemptCount = "spPhy_SetFailedPasswordAnswerAttemptCount";
        public static string GetAdminUsersByContract = "spPhy_GetAdminUsersByContract";
        public static string GetPrefixForUserName = "spPhy_GetUserNamePrefix";
        public static string GetGroupById = "spPhy_C3GetGroup";
        public static string GetImpersonationUsersByContractId = "spPhy_GetContractUsersToImpersonate";        
        public static string SavePageView = "spPhy_SavePageView";
        public static string DeletePageView = "spPhy_DeletePageView";

        public static string GetUserPermissions = "spPhy_GetUserControlPermissions_13";
        public static string GetPageViews = "spPhy_GetPageViewsByUser_13";

        #endregion

		#region UserContractProperty
		public static string GetUserContractProperty = "spPhy_GetUserContractProperties";
		public static string SetUserContractProperty = "spPhy_SetUserContractProperty";
		#endregion

		#region ContractProperty
		public static string GetContractProperty = "spPhy_GetContractProperties";
		#endregion
	}
}
