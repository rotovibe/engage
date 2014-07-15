using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;

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

        public DeleteCareMemberByPatientIdDataResponse DeleteCareMemberByPatientId(DeleteCareMemberByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.CareMemberData GetCareMember(DTO.GetCareMemberDataRequest request)
        {
            ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
            CareMemberData careMember = repo.FindByID(request.Id) as CareMemberData;
            return careMember;
        }

        public List<DTO.CareMemberData> GetAllCareMembers(DTO.GetAllCareMembersDataRequest request)
        {
            ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
            List<CareMemberData> careMembers = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
            return careMembers;
        }

        public DTO.CareMemberData GetPrimaryCareManager(DTO.GetPrimaryCareManagerDataRequest request)
        {
            CareMemberData response = null;
            ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
            List<CareMemberData> careMembers = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
            if (careMembers != null)
            {
                response = careMembers.Find(c => c.Primary == true && c.TypeId == "530cd571d433231ed4ba969b");
            }
            return response;
        }


        public UndoDeleteCareMembersDataResponse UndoDeleteCareMembers(UndoDeleteCareMembersDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
