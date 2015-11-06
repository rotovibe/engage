
namespace Phytel.Services.Communication
{
    public class ActivityDetail
    {
        public int SendID { get; set; }
        public int ContractID { get; set; }
        public string ContractNumber { get; set; }
        public string TaskTypeCode { get; set; }
        public string TaskTypeCategory { get; set; }
        public int FacilityID { get; set; }
        public int RecipientSchedID { get; set; }
        public int ProviderID { get; set; }
        public int PatientID { get; set; }
        public string ScheduleDateTime { get; set; }
        public int ScheduleDuration { get; set; }
        public string ProviderACDNumber { get; set; }
        public int ActivityID { get; set; }
        public string DeliveryMethod { get; set; }
        public string ToEmailAddress { get; set; }
        public int TemplateID { get; set; }
        public int CampaignID { get; set; }
        public int ContactRoleID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientNameLF { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderNameLF { get; set; }
        public string FacilityAddrLine1 { get; set; }
        public string FacilityAddrLine2 { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityState { get; set; }
        public string FacilityZipCode { get; set; }
    }
}
