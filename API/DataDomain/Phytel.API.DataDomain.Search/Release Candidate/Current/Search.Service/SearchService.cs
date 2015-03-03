using System;
using Phytel.API.DataDomain.Search;
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
                //response.Results = Manager.GetSearchByID(request);
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
                response.Results = Manager.GetTermSearchResults(request);
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