using Phytel.Services.API.DTO;
using Phytel.Services.API.Provider;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.API.Filter
{
    public class ContextRequestFilterAttribute : RequestFilterAttribute
    {
        public string[] ValidContextCodes { get; set; }

        public ContextRequestFilterAttribute()
        {
        }

        public ContextRequestFilterAttribute(params string[] validContextCodes)
        {
            ValidContextCodes = validContextCodes;
        }

        public IHostContextProxy HostContextProxy { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            string contextCode = string.Empty;

            if (HostContextProxy == null)
            {
                throw new Exception("IHostContextProxy was not initialized. Make sure IHostContextProxy has been registered with the IoC container.");
            }

            if (requestDto is IContextRequest)
            {
                IContextRequest contextRequest = requestDto as IContextRequest;
                if (contextRequest != null && !string.IsNullOrEmpty(contextRequest.Context))
                {
                    contextCode = contextRequest.Context;                    
                }
            }

            if (ValidContextCodes != null && ValidContextCodes.Any() && (string.IsNullOrEmpty(contextCode) || ValidContextCodes.All(x => x.ToLower() != contextCode)))
            {
                string validCodesAsJoinedString = string.Empty;
                foreach(string validContextCode in ValidContextCodes)
                {
                    if(string.IsNullOrEmpty(validCodesAsJoinedString))
                    {
                        validCodesAsJoinedString += validContextCode;
                    }
                    else
                    {
                        validCodesAsJoinedString = validCodesAsJoinedString + ", " + validContextCode;
                    }
                }
                throw new Exception("Invalid context code provided. Valid codes are " + validCodesAsJoinedString + ".");
            }
            else
            {
                HostContextProxy.SetItem(Provider.Constants.HostContextKeyContext, contextCode);
            }
        }
    }
}
