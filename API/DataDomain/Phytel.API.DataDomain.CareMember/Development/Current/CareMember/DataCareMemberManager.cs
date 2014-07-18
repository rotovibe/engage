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
    public class CareMemberDataManager : ICareMemberDataManager
    {
        #region Endpoint addresses
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        #endregion

        public ICareMemberRepositoryFactory Factory { get; set; }
        
        public string InsertCareMember(PutCareMemberDataRequest request)
        {
            string careMemberId = string.Empty;
            try
            {
                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                careMemberId = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return careMemberId;
        }

        public bool UpdateCareMember(PutUpdateCareMemberDataRequest request)
        {
            bool updated = false;
            try
            {
                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                updated = (bool)repo.Update(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return updated;
        }

        public CareMemberData GetCareMember(GetCareMemberDataRequest request)
        {
            try
            {
                CareMemberData response = null; 
                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                response = repo.FindByID(request.Id) as CareMemberData;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CareMemberData> GetAllCareMembers(GetAllCareMembersDataRequest request)
        {
            try
            {
                List<CareMemberData> response = null;
                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                response = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public CareMemberData GetPrimaryCareManager(GetPrimaryCareManagerDataRequest request)
        {
            try
            {
                CareMemberData response = null;
                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
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

        private string getCareManagerLookupId(GetLookUpsDataRequest request)
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

        public DeleteCareMemberByPatientIdDataResponse DeleteCareMemberByPatientId(DeleteCareMemberByPatientIdDataRequest request)
        {
            DeleteCareMemberByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteCareMemberByPatientIdDataResponse();

                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                List<CareMemberData> careMembers = repo.FindByPatientId(request.PatientId) as List<CareMemberData>;
                List<string> deletedIds = null;
                if (careMembers != null)
                {
                    deletedIds = new List<string>();
                    careMembers.ForEach(u =>
                    {
                        request.Id = u.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeleteCareMembersDataResponse UndoDeleteCareMembers(UndoDeleteCareMembersDataRequest request)
        {
            UndoDeleteCareMembersDataResponse response = null;
            try
            {
                response = new UndoDeleteCareMembersDataResponse();

                ICareMemberRepository repo = Factory.GetRepository(request, RepositoryType.CareMember);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.CareMemberId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
