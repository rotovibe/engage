using Phytel.API.DataDomain.CareMember.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.CareMember;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.CareMember
{
    public static class CareMemberDataManager
    {
        public static string InsertCareMember(PutCareMemberDataRequest request)
        {
            string careMemberId = string.Empty;
            try
            {
                ICareMemberRepository<PutCareMemberDataResponse> repo = CareMemberRepositoryFactory<PutCareMemberDataResponse>.GetCareMemberRepository(request.ContractNumber, request.Context);
                careMemberId = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return careMemberId;
        }

        public static bool UpdateCareMember(PutUpdateCareMemberDataRequest request)
        {
            bool updated = false;
            try
            {
                ICareMemberRepository<PutUpdateCareMemberDataResponse> repo = CareMemberRepositoryFactory<PutUpdateCareMemberDataResponse>.GetCareMemberRepository(request.ContractNumber, request.Context);
                updated = (bool)repo.Update(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return updated;
        }

        public static CareMemberData GetCareMember(GetCareMemberDataRequest request)
        {
            try
            {
                CareMemberData response = null;
                ICareMemberRepository<CareMemberData> repo = CareMemberRepositoryFactory<CareMemberData>.GetCareMemberRepository(request.ContractNumber, request.Context);
                response = repo.FindByID(request.Id) as CareMemberData;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CareMemberData> GetAllCareMembers(GetAllCareMembersDataRequest request)
        {
            try
            {
                List<CareMemberData> response = null;
                ICareMemberRepository<List<CareMemberData>> repo = CareMemberRepositoryFactory<List<CareMemberData>>.GetCareMemberRepository(request.ContractNumber, request.Context);
                response = repo.FindByPatientId(request) as List<CareMemberData>;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
