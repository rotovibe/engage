using ServiceStack.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phytel.API.DataDomain.ToDo.Service.ContextProxy
{
    public class HostContextProxy : IHostContextProxy
    {
        protected HostContext _hostContext;
        public HostContextProxy(HostContext hostContext = null){}

        public string ContractNumber { get; set; }
        protected virtual string OnGetContractNumber(){ return string.Empty;}
        protected virtual void OnSetContractNumber(string contractNumber){}
    }
}