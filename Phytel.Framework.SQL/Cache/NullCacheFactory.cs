namespace Phytel.Framework.SQL.Cache
{
	/// <summary>
	/// Single implementation of a CacheFactory for NullCache instances.
	/// Useful in tests, or when implementing default behaviors.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class NullCacheFactory : AbstractCacheFactory 
	{
		internal override ICache CreateNewCache(string name) 
		{
			return new NullCache();
		}
	}
}
