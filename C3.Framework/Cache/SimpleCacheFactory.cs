namespace C3.Framework.Cache
{
	/// <summary>
	/// An implementation of a factory for simple data caches.  This factory
	/// is not pre-configured; the caches it produces are created on demand.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class SimpleCacheFactory : AbstractCacheFactory 
	{
		public static readonly int FACTORY_DEFAULT_CACHE_SIZE = 1000;

		internal override ICache CreateNewCache(string name) 
		{
			var config = new CacheConfiguration(name);
			var cache = new SimpleCache( config.GetInt( "maxSize", FACTORY_DEFAULT_CACHE_SIZE ) );

			cache.Configuration = config;

			return cache;
		}
	}
}
