using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.AppDomain.NG
{
    public interface IContactEndpointUtil
    {
        #region Contact
        #endregion

        #region CareTeam
        CareTeamData GetCareTeam(GetCareTeamRequest request);
        SaveCareTeamDataResponse SaveCareTeam(SaveCareTeamRequest request);

        #endregion

    }
}
