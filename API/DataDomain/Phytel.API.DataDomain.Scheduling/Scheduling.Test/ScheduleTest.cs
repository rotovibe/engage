using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.DataDomain.Scheduling.Test.Stubs;

namespace Phytel.API.DataDomain.Scheduling.Test
{
    [TestClass]
    public class ScheduleTest
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;


        [TestMethod]
        public void GetSchedule_Test()
        {
            GetScheduleDataRequest request = new GetScheduleDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Id = "53ff6b92d4332314bcab46e0"
            };

            ISchedulingDataManager cm = new SchedulingDataManager { Factory = new SchedulingRepositoryFactory() };
            GetScheduleDataResponse response = cm.GetSchedule(request);

            Assert.IsNotNull(response);
        }
    }
}
