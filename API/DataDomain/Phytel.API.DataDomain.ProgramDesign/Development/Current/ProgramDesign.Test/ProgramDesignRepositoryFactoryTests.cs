using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.ProgramDesign;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ProgramDesign.DTO;

namespace Phytel.API.DataDomain.ProgramDesign.Test
{
    [TestClass()]
    public class ProgramDesignRepositoryFactoryTests
    {
        [TestClass()]
        public class GetRepository_Method
        {
            [TestMethod()]
            public void Get_Program()
            {
                MongoProgramRepository<GetProgramRequest> cRepo = new MongoProgramRepository<GetProgramRequest>("InHealth001");
                IProgramDesignRepository<GetProgramRequest> repo = ProgramDesignRepositoryFactory<GetProgramRequest>.GetProgramRepository("InHealth001", "NG", "531f2df9072ef727c4d2a3df");
                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_Module()
            {
                MongoModuleRepository<GetModuleRequest> cRepo = new MongoModuleRepository<GetModuleRequest>("InHealth001");
                IProgramDesignRepository<GetModuleRequest> repo = ProgramDesignRepositoryFactory<GetModuleRequest>.GetModuleRepository("InHealth001", "NG", "531f2df9072ef727c4d2a3df");
                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_Action()
            {
                MongoActionRepository<GetActionDataRequest> cRepo = new MongoActionRepository<GetActionDataRequest>("InHealth001");
                IProgramDesignRepository<GetActionDataRequest> repo = ProgramDesignRepositoryFactory<GetActionDataRequest>.GetActionRepository("InHealth001", "NG", "531f2df9072ef727c4d2a3df");
                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_Step()
            {
                MongoStepRepository<GetStepDataRequest> cRepo = new MongoStepRepository<GetStepDataRequest>("InHealth001");
                IProgramDesignRepository<GetStepDataRequest> repo = ProgramDesignRepositoryFactory<GetStepDataRequest>.GetStepRepository("InHealth001", "NG", "531f2df9072ef727c4d2a3df");
                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }
        }
    }
}
