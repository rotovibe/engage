﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public class GoalsManager : ManagerBase
    {

        public GetInitializeTaskResponse GetInitialTask(GetInitializeTaskRequest request)
        {
            try
            {
                GetInitializeTaskResponse itr = new GetInitializeTaskResponse();
                string id = GoalsEndpointUtil.GetInitialTaskRequest(request);
                itr.Id = id;
                return itr;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
