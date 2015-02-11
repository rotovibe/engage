using ServiceStack.Common;
using System;

namespace Phytel.Services.API.Provider
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

        public double GetItemAsDouble(string key)
        {
            double rvalue = default(double);

            string rvalueAsString = GetItemAsString(key);
            if(!string.IsNullOrEmpty(rvalueAsString))
            {
                double rvalueAsDouble;
                if(double.TryParse(rvalueAsString, out rvalueAsDouble))
                {
                    rvalue = rvalueAsDouble;
                }
            }

            return rvalue;
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
    }
}