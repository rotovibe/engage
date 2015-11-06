
namespace Phytel.Services.Communication
{
    public class EmailActivityDetail : ActivityDetail
    {        
        public string ToEmailAddress { get; set; }        
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
