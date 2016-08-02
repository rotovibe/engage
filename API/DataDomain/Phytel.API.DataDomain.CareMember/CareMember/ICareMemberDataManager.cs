using System.Collections.Generic;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.DataDomain.CareMember
{
    public interface ICareMemberDataManager
    {
        string InsertCareMember(PutCareMemberDataRequest request);
        bool UpdateCareMember(PutUpdateCareMemberDataRequest request);
        CareMemberData GetCareMember(GetCareMemberDataRequest request);
        List<CareMemberData> GetAllCareMembers(GetAllCareMembersDataRequest request);
        CareMemberData GetPrimaryCareManager(GetPrimaryCareManagerDataRequest request);
        DeleteCareMemberByPatientIdDataResponse DeleteCareMemberByPatientId(DeleteCareMemberByPatientIdDataRequest request);
        UndoDeleteCareMembersDataResponse UndoDeleteCareMembers(UndoDeleteCareMembersDataRequest request);
    }
}
