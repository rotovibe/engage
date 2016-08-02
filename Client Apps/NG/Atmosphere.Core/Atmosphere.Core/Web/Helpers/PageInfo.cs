using System;

namespace C3.Web.Helpers
{
    [Serializable]
    public class PageInfo
    {
        public bool UserIsValid { get; set; }
        public bool ClearUserSession { get; set; }
        public string RedirectURL { get; set; }
        public string RedirectPageName { get; set; }
        public string InformationMessageCode { get; set; }
        public string ErrorMessageCode { get; set; }
        public Exception LastSystemErrorMessage { get; set; }
        public string LastPage { get; set; }
        public string LastPageName { get; set; }
        public bool UserTimedOut { get; set; }
        public string Action { get; set; }
        public Guid EditUserId { get; set; }
        public int PatientId { get; set; }
        public string SessionId { get; set; }
        public bool IsFromPatientSearch { get; set; }
        public bool ClosePatientSummary { get; set; }

    }
}
