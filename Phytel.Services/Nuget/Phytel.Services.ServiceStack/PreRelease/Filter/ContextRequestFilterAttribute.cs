using Phytel.Services.ServiceStack.DTO;
using Phytel.Services.ServiceStack.Proxy;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.ServiceStack.Filter
{
    public class ContextRequestFilterAttribute : RequestFilterAttribute
    {
        public string[] ValidContextCodes { get; set; }

        public ContextRequestFilterAttribute(params string[] validContextCodes)
        {
            ValidContextCodes = validContextCodes;
        }

        public IHostContextProxy HostContextProxy { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            string contextCode = string.Empty;

            if (requestDto is IContextRequest)
            {
                IContextRequest contextRequest = requestDto as IContextRequest;
                if (contextRequest != null && !string.IsNullOrEmpty(contextRequest.ContextCode))
                {
                    contextCode = contextRequest.ContextCode;                    
                }
            }

            if(string.IsNullOrEmpty(contextCode) || ValidContextCodes.All(x => x != contextCode))
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
                throw new Exception("Context code was either not provided or did not match one of the valid context codes for this request. Valid codes are " + validCodesAsJoinedString + ".");
            }
            else
            {
                HostContextProxy.SetItem(Constants.HostContextKeyContextCode, contextCode);
            }
        }
    }
}
