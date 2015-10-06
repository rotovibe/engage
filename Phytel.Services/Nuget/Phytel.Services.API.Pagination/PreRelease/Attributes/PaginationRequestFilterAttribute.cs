using System;
using ServiceStack.ServiceHost;

namespace Phytel.Services.API.Pagination.Attributes
{
    public class PaginationRequestFilterAttribute : Attribute, IHasRequestFilter
    {
        public IHasRequestFilter Copy()
        {
            return this;
        }

        public int Priority
        {
            get { return -100; }
        }

        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            var request = requestDto as IPaginationRequest;
            if (request == null)
                return;

            var paginationManager = new PaginationManager();

            request.Take = paginationManager.GetNormalizeTake(requestDto);
            request.Skip = paginationManager.GetNormalizeSkip(requestDto);

        }
    }
}



