using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using log4net;
using Phytel.Framework.SQL.Configuration;

namespace Phytel.Framework.SQL.Cache
{
	public class CacheFactoryCreator
	{
		private static readonly ILog LOG = LogManager.GetLogger( typeof( CacheFactoryCreator ) );
		private static readonly Dictionary<string, string> _mappings = new Dictionary<string, string>();

		public static void AddMapping( string name, string type )
		{
			_mappings[name] = type;	
		}

		public static ICacheFactory Create( string name )
		{
			string factoryType = _mappings.ContainsKey( name ) ?
				_mappings[name] : ConfigurationUtils.GetConfigString( name + ".cacheFactory" );

			ICacheFactory factory = null;

			if( ! string.IsNullOrEmpty( factoryType ) )
			{
				try 
				{
					string[] typeComponents = factoryType.Split(',');
					
					if( typeComponents.Length < 2 )
					{
						throw new ValidationException( "Invalid cache type declaration: " + factoryType );
					}

					ObjectHandle handle = Activator.CreateInstance( typeComponents[1].Trim(), typeComponents[0].Trim() );

					if( handle == null )
					{
						throw new ValidationException( "Unable to instantiate cache of type: " + factoryType );
					}
					
					object target = handle.Unwrap();
					
					LOG.InfoFormat( "Created new instance of {0} for cache {1}", typeComponents[0], name );

					factory = target as ICacheFactory;
				}
				catch( Exception e )
				{
					LOG.Warn( "Could not create factory of configured type: " + factoryType, e);
				}
			}

			return factory;
		}
	}
}
