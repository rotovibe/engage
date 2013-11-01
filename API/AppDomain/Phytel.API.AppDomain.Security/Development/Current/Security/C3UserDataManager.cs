using Phytel.API.DataDomain.C3User.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.C3User
{
    public static class C3UserDataManager
    {
        public static C3UserDataResponse LoginUser(C3UserDataRequest request)
        {
            C3UserDataResponse result = null;

            IC3UserRepository<C3UserDataResponse> repo = Phytel.API.DataDomain.C3User.C3UserRepositoryFactory<C3UserDataResponse>.GetC3UserRepository(string.Empty);
            result = repo.ProcessUserToken(request.UserToken);
            if (result == null)
                result = new C3UserDataResponse();

            return result;
        }
    }
}   
