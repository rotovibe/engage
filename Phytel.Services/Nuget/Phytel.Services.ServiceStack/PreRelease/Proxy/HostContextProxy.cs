using Phytel.Services.ServiceStack.DTO;
using ServiceStack.Common;
using System;

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

        [Obsolete("Use GetItem<string>(Constants.HostContextKeyContractNumber) and SetItem(Constants.HostContextKeyContractNumber, object value) instead", true)]
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
            return GetItemAsString(Constants.HostContextKeyContractNumber);
        }

        protected virtual void OnSetContractNumber(string contractNumber)
        {
            SetItem(Constants.HostContextKeyContractNumber, contractNumber);
        }
    }
}