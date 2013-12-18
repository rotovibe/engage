using System.Collections;

namespace Phytel.Framework.SQL.Cache
{
	/// <summary>
	/// A cache that is nothing more than a bounded map.  It is threadsafe
	/// and implements what amounts to a random eviction policy when the 
	/// maximum size is reached.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class SimpleCache : ICache 
	{
		private const int MAX_DEFAULT_ENTRIES = 10000;

		private ICacheConfiguration _config;
		private IDictionary _data;
		private int _maxEntries;

		/// <summary>
		/// Creates a new cache with the default maximum number of entries.
		/// </summary>
		public SimpleCache() : this(MAX_DEFAULT_ENTRIES)
		{
		}

		/// <summary>
		/// Creates a new cache with the specified maximum number of entries.
		/// </summary>
		/// <param name="maxEntries">The maximum elements this cache may hold.</param>
		public SimpleCache( int maxEntries ) 
		{
			_maxEntries = maxEntries;
			_data = new Hashtable();
		}
	
		public IDictionary Entries() 
		{
			// TODO: Need some way to return a defensive copy
			return _data;
		}

		/// <summary>
		/// The maximum allowable entries in this cache.  If negative, will
		/// be effectively unbounded.
		/// </summary>
		public int MaxEntries
		{
			get { return _maxEntries; }
		}

		public int Size() 
		{
			return _data.Count;
		}
	
		public object Get(object key)
		{
			lock(this)
			{
				return _data[key];
			}
		}

		public void Invalidate() 
		{
			lock(this)
			{
				_data = new Hashtable();
			}
		}

		public object Peek(object key) 
		{
			lock(this)
			{
				return Get(key);
			}
		}

		public bool Put(object key, object value)
		{
			lock(this)
			{
				if( _data.Count == _maxEntries ) 
				{
					IDictionaryEnumerator enumerator = _data.GetEnumerator();					
					enumerator.MoveNext();

					object firstKey = enumerator.Key;
					_data.Remove(firstKey);
				}

				_data[key] = value;
		
				return true;				
			}
		}

		public object Remove(object key) 
		{
			lock(this)
			{
				object victim = _data[key];

				_data.Remove(key);

				return victim;
			}
		}

		public ICacheConfiguration Configuration
		{
			get { return _config; }
			set { _config = value; }
		}
	}
}
