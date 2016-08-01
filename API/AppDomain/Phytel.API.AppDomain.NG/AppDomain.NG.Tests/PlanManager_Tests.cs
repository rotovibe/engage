using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanManager_Tests
    {
        [TestMethod()]
        public void SaveActionResults_Test()
        {
            PlanManager pm = new PlanManager();
            PostSaveActionRequest request = new PostSaveActionRequest();
            request.Action = new Actions { Id = "534b652dd6a48504b03565f0", ModuleId = "534b652dd6a48504b03565ef" };
            request.PatientId = "5325da1cd6a4850adcbba542";
            request.Token = "534b523dd6a48504b03b0564";
            request.ProgramId = "534b652dd6a48504b035649c";
            request.UserId = "testing";
            request.Version = 1.0;
            request.ContractNumber = "InHealth001";
            pm.SaveActionResults(request);
        }
    }
}
