using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IContactManager
    {
        void LogException(Exception ex);

        #region Contact
        #endregion

        #region CareTeam
        CareTeam GetCareTeam(GetCareTeamRequest request);
        #endregion
    }
}
