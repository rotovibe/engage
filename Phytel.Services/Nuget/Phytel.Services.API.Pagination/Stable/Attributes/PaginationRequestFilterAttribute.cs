using System;
using ServiceStack.Common.Extensions;
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

            request.Skip = req.QueryString["skip"].IsEmpty() ? 0 : int.Parse(req.QueryString["skip"]);
            request.Take = req.QueryString["take"].IsEmpty() ? (int?)null : int.Parse(req.QueryString["take"]);
        }
    }
}



