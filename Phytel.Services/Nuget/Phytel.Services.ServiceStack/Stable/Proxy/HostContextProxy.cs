using Phytel.Services.ServiceStack.DTO;
using ServiceStack.Common;

namespace Phytel.Services.ServiceStack.Proxy
{
    public class HostContextProxy : IHostContextProxy
    {
        protected HostContext _hostContext;

        public HostContextProxy(HostContext hostContext = null)
        {
            if (hostContext == null)
            {
                hostContext = HostContext.Instance;
            }

            _hostContext = hostContext;
        }

        public string ContractNumber
        {
            get
            {
                return OnGetContractNumber();
            }
            set
            {
                OnSetContractNumber(value);
            }
        }

        public T GetItem<T>(string key)
            where T : class
        {
            T rvalue = null;

            object rvalueAsObject = GetItem(key);
            if (rvalueAsObject != null)
            {
                rvalue = rvalueAsObject as T;
            }

            return rvalue;
        }

        public object GetItem(string key)
        {
            return _hostContext.Items[key];
        }

        public string GetItemAsString(string key)
        {
            string rvalue = null;

            object rvalueAsObject = GetItem(key);

            if (rvalueAsObject != null)
            {
                rvalue = rvalueAsObject.ToString();
            }

            return rvalue;
        }

        public void SetItem(string key, object value)
        {
            if (_hostContext.Items[key] == null)
            {
                _hostContext.Items.Add(key, value);
            }
            else
            {
                _hostContext.Items[key] = value;
            }
        }

        protected virtual string OnGetContractNumber()
        {
            return GetItemAsString(ContractRequest.HostContextKeyContractNumber);
        }

        protected virtual void OnSetContractNumber(string contractNumber)
        {
            SetItem(ContractRequest.HostContextKeyContractNumber, contractNumber);
        }
    }
}