﻿using System;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG
{

    public class UnAssignedPCMRule : ICareMemberCohortRule, ICohortCommand
    {
        public void Run(CareTeam careTeam)
        {
            if(careTeam == null)
                throw new ArgumentNullException("careTeam");

            if (NGUtils.CheckIfCareTeamHasActiveCorePCM(careTeam))
            {
                //Remove from UnAssigned PCM
            }
            else
            {
               //Add to UnAssigned PCM.
            }

        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
        
    }
}
