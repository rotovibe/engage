using Phytel.Services.Dates;
using Phytel.Services.ServiceStack.DTO;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;

namespace Phytel.Services.ServiceStack.Filter
{
    public class EffectiveDateTimeRequestFilterAttribute : RequestFilterAttribute
    {
        public IDateTimeProxy DateTimeProxy { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            if(DateTimeProxy == null)
            {
                throw new Exception("IDateTimeProxy was not initialized. Make sure IDateTimeProxy has been registered with the IoC container.");
            }

            if (requestDto is IEffectiveDateRequest)
            {
                IEffectiveDateRequest effectiveDateRequest = requestDto as IEffectiveDateRequest;
                if (effectiveDateRequest != null && effectiveDateRequest.EffectiveDate.HasValue == false)
                {
                    effectiveDateRequest.EffectiveDate = DateTimeProxy.GetCurrentDateTime();
                }
            }
        }
    }
}