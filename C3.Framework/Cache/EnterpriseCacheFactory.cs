using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace C3.Framework.Cache
{
	/// <summary>
	/// Generator of caches based on the Microsoft Enterprise Application Block.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class EnterpriseCacheFactory : AbstractCacheFactory
	{
		/// <summary>
		/// Pulls a configured cache from the Enterprise Block configuration.
		/// 
		/// TODO: This will throw any number of runtime exceptions that we don't catch
		/// if there is a misconfiguration.
		/// </summary>
		/// <param name="name">The name of the cache as specified in cachingConfiguration.config</param>
		/// <returns></returns>
		internal override ICache CreateNewCache(string name)
		{
			CacheManager mgr = CacheFactory.GetCacheManager(name);
			var cache = new EnterpriseCache( name );
			cache.CacheManager = mgr;

			return cache;
		}
	}
}
