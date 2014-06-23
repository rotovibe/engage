using Phytel.API.Common.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contact.Test.Stubs
{
    public class StubCommonFormatter : ICommonFormatterUtil
    {
        public string FormatDateOfBirth(string val)
        {
            throw new NotImplementedException();
        }

        public void FormatExceptionResponse<T>(T response, ServiceStack.ServiceHost.IHttpResponse httpResponse, Exception ex) where T : Interface.IDomainResponse
        {
            
        }
    }
}
