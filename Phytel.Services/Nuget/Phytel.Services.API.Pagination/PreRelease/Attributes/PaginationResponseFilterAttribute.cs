using System;
using ServiceStack.ServiceHost;

namespace Phytel.Services.Pagination.Attributes
{
    public class PaginationResponseFilterAttribute : Attribute, IHasResponseFilter
    {
        public IHasResponseFilter Copy()
        {
            return this;
        }

        public int Priority
        {
            get { return -101; }
        }

        public void ResponseFilter(IHttpRequest req, IHttpResponse res, object response)
        {
            //var castedResponse = response as IPaginationResponse;
            //if (castedResponse == null)
            //    return;

            //int resolvedTake = 0;

            //if (castedResponse.Take != null)
            //{
            //     resolvedTake = castedResponse.Take.Value;
            //}

            //castedResponse.TotalPages = (castedResponse.TotalCount - 1) / resolvedTake + 1;
        }
    }
}
