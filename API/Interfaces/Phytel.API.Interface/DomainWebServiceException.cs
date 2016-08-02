using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Interface
{
    public class DomainWebServiceException : WebServiceException
    {
        public DomainWebServiceException(WebServiceException wex)
        {
            this.BaseException = wex;
            this.ASEProcessID = 0;
            this.HasBeenLogged = false;
        }

        public WebServiceException BaseException { get; set; }
        public bool HasBeenLogged { get; set; }
        public int ASEProcessID { get; set; }
    }
}
