using System;
namespace Phytel.Services.ServiceStack.Proxy
{
    public interface IHostContextProxy
    {
        [Obsolete("Use GetItem<string>(Constants.HostContextKeyContractNumber) and SetItem(Constants.HostContextKeyContractNumber, object value) instead", true)]
        string ContractNumber { get; set; }

        T GetItem<T>(string key) where T : class;

        object GetItem(string key);

        string GetItemAsString(string key);

        void SetItem(string key, object value);
    }
}