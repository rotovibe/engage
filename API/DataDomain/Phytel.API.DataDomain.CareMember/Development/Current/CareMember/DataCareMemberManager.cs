using Phytel.API.DataDomain.CareMember.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.CareMember;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.CareMember
{
    public static class CareMemberDataManager
    {
        public static GetCareMemberResponse GetCareMemberByID(GetCareMemberRequest request)
        {
            try
            {
                GetCareMemberResponse result = new GetCareMemberResponse();

                ICareMemberRepository<GetCareMemberResponse> repo = CareMemberRepositoryFactory<GetCareMemberResponse>.GetCareMemberRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.CareMemberID) as GetCareMemberResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetCareMemberResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllCareMembersResponse GetCareMemberList(GetAllCareMembersRequest request)
        {
            try
            {
                GetAllCareMembersResponse result = new GetAllCareMembersResponse();

                ICareMemberRepository<GetAllCareMembersResponse> repo = CareMemberRepositoryFactory<GetAllCareMembersResponse>.GetCareMemberRepository(request.ContractNumber, request.Context);
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
