using Phytel.API.DataDomain.CareMember.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.CareMember;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using System.Configuration;
using ServiceStack.Service;
using Phytel.API.Common.CustomObject;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.LookUp.DTO;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.CareMember
{
    public static class CareMemberDataManager
    {
        #region Endpoint addresses
        static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        #endregion
        
        public static string InsertCareMember(PutCareMemberDataRequest request)
        {
            string careMemberId = string.Empty;
            try
            {
                ICareMemberRepository<PutCareMemberDataResponse> repo = CareMemberRepositoryFactory<PutCareMemberDataResponse>.GetCareMemberRepository(request.ContractNumber, request.Context, request.UserId);

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
                ICareMemberRepository<PutUpdateCareMemberDataResponse> repo = CareMemberRepositoryFactory<PutUpdateCareMemberDataResponse>.GetCareMemberRepository(request.ContractNumber, request.Context, request.UserId);

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
                ICareMemberRepository<CareMemberData> repo = CareMemberRepositoryFactory<CareMemberData>.GetCareMemberRepository(request.ContractNumber, request.Context, request.UserId);

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
                ICareMemberRepository<List<CareMemberData>> repo = CareMemberRepositoryFactory<List<CareMemberData>>.GetCareMemberRepository(request.ContractNumber, request.Context, request.UserId);

                response = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static CareMemberData GetPrimaryCareManager(GetPrimaryCareManagerDataRequest request)
        {
            try
            {
                CareMemberData response = null;
                ICareMemberRepository<List<CareMemberData>> repo = CareMemberRepositoryFactory<List<CareMemberData>>.GetCareMemberRepository(request.ContractNumber, request.Context, request.UserId);

                List<CareMemberData> careMembers = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
                if(careMembers != null)
                {
                    GetLookUpsDataRequest lookupDataRequest = new GetLookUpsDataRequest { Context = request.Context, ContractNumber = request.ContractNumber, Name = LookUpType.CareMemberType.ToString(), UserId = request.UserId, Version = request.Version };
                    string careManagerLookUpId = getCareManagerLookupId(lookupDataRequest);
                    response = careMembers.Find(c => c.Primary == true && c.TypeId == careManagerLookUpId);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string getCareManagerLookupId(GetLookUpsDataRequest request)
        {
            string lookupId = string.Empty;
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Type/{4}",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber,
                                                                        request.Name), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Type/{Name}", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetLookUpsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetLookUpsDataResponse>(url);

                List<IdNamePair> dataList = dataDomainResponse.LookUpsData;
                if (dataList != null && dataList.Count > 0)
                {
                    IdNamePair careManagerLookUp = dataList.Find(a => a.Name == Constants.CareManager);
                    if (careManagerLookUp != null)
                        lookupId = careManagerLookUp.Id;
                }
                return lookupId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
