
using System.Web;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.Allergy.Service
{
	public class ServiceProxy : IServiceProxy
	{
	    protected HostContext _hostContext;

		public string ContractNumber 
		{
			get
			{
				return GetContractNumber();
			}
			set
			{
				SetContractNumber(value);
			}
		}

		public ServiceProxy(HostContext hostContext = null)
		{
			if (hostContext == null)
			{
				hostContext = HostContext.Instance;
			}
			_hostContext = hostContext;
		}

		protected virtual void SetContractNumber(string contractNumber)
		{
			if ( _hostContext.Items["ContractNumber"] == null)
			{
				_hostContext.Items.Add("ContractNumber", contractNumber);
				return;
			}
			_hostContext.Items["ContractNumber"] = contractNumber;
		}

		protected virtual string GetContractNumber()
		{
			string result = null;
            //var obj = _hostContext.Items["ContractNumber"];
            //if (obj != null)
            //    result = obj.ToString();
		    result = "InHealth001";
			return result;
		}
	}
}