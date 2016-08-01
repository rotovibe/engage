using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact
{
    public interface ICommonDataManager
    {
        GetPatientsCareTeamInfoDataResponse GetPatientsCareTeamInfo(GetPatientsCareTeamInfoDataRequest dataRequest);
    }
}
