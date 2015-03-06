using Phytel.API.Common.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubCommonFormatterUtil : ICommonFormatterUtil
    {
        public string FormatDateOfBirth(string val)
        {
            throw new NotImplementedException();
        }

        public void FormatExceptionResponse<T>(T response, ServiceStack.ServiceHost.IHttpResponse httpResponse, Exception ex) where T : Interface.IDomainResponse
        {
            //throw new NotImplementedException();
        }
    }
}
