using System.Linq;

namespace Phytel.Services.Communication
{
    public class ActivityMedia
    {
        public string OwnerCode { get; set; }
        public int OwnerID { get; set; }
        public string CategoryCode { get; set; }
        public string Filename { get; set; }
        public string Narrative { get; set; }
        public string LanguagePreferenceCode { get; set; }
        public int FacilityID { get; set; }
    }
}
