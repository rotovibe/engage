using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.AppDomain.NG
{
    public class ContactManager : ManagerBase, IContactManager
    {
        public IContactEndpointUtil EndpointUtil { get; set; }

        #region Contact
        #endregion

        #region CareTeam
        public CareTeam GetCareTeam(GetCareTeamRequest request)
        {
            CareTeam careTeam = null;
            try
            {
                CareTeamData careTeamData = EndpointUtil.GetCareTeam(request);
                if (careTeamData != null)
                {
                    careTeam = Mapper.Map<CareTeam>(careTeamData);
                }
                return careTeam;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
