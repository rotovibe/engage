using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.CareMember.Test
{
    class StubCareMemberDataManager : ICareMemberDataManager
    {
        public ICareMemberRepositoryFactory Factory { get; set; }
        
        public string InsertCareMember(DTO.PutCareMemberDataRequest request)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCareMember(DTO.PutUpdateCareMemberDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.CareMemberData GetCareMember(DTO.GetCareMemberDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.CareMemberData> GetAllCareMembers(DTO.GetAllCareMembersDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.CareMemberData GetPrimaryCareManager(DTO.GetPrimaryCareManagerDataRequest request)
        {
            DTO.CareMemberData pcm = new DTO.CareMemberData();

            return pcm;
        }
    }
}
