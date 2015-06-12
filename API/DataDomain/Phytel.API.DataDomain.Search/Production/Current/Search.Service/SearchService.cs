using System;
using System.Linq;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search.Service
{
    public class SearchService : ServiceBase
    {
        protected readonly ISearchDataManager Manager;

        public SearchService(ISearchDataManager mgr)
        {
            Manager = mgr;
        }

        public GetSearchResponse Post(GetSearchRequest request)
        {
            var response = new GetSearchResponse{ Version = request.Version};
            try
            {
                RequireUserId(request);
                //response.MedResults = Manager.GetSearchByID(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetSearchResponse Get(GetSearchRequest request)
        {
            var response = new GetSearchResponse { Version = request.Version };
            try
            {
                RequireUserId(request);

                var type = (SearchEnum)Enum.Parse(typeof(SearchEnum), request.Type);
                switch (type)
                {
                    case SearchEnum.Medication:
                    {
                        response.MedResults = Manager.GetTermSearchResults(request, type).Cast<TextValuePair>().ToList();
                        break;
                    }
                    case SearchEnum.Allergy:
                    {
                        response.AllergyResults = Manager.GetTermSearchResults(request, type).Cast<IdNamePair>().ToList();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.LogMessageToFile(ex.Message + " trace:" + ex.StackTrace);
                RaiseException(response, ex);
            }
            return response;
        }

        public PutMedRegistrationResponse Put(PutMedRegistrationRequest request)
        {
            var response = new PutMedRegistrationResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.Success = Manager.InsertMedDocInIndex(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutDeleteMedsResponse Put(PutDeleteMedsRequest request)
        {
            var response = new PutDeleteMedsResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.Success = Manager.DeleteMedDocs(request.MedDocuments);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutAllergyRegistrationResponse Put(PutAllergyRegistrationRequest request)
        {
            var response = new PutAllergyRegistrationResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.Success = Manager.InsertAllergyDocInIndex(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllSearchsResponse Post(GetAllSearchsRequest request)
        {
            var response = new GetAllSearchsResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Searchs = Manager.GetSearchList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}