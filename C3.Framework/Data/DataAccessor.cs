using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace C3.Framework.Data
{
    /// <summary>
    /// A data accessor provides plumbing to retrieve information in a consistent
    /// way from a data source.
    /// </summary>
    public class DataAccessor : IDataAccessor
    {
        private IDataContext _context;
    	private int _timeout;

        protected DataAccessor()
        {
        }

        public DataAccessor( IDataContext context )
        {
            _context = context;
        }

        public IDataContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

		/// <summary>
		/// The data access timeout to use, in seconds
		/// </summary>
    	public int Timeout
    	{
    		get { return _timeout; }
    		set { _timeout = value; }
    	}

    	public T Find<T>( IDbCommand finder, Build<T> builder )
        {
    		return Find( finder, builder, null );
        }

		public ITypeReader FindReader(IDbCommand finder)
		{
			ITypeReader reader = new TypeConvertingReader(_context.ExecuteReader(finder), Converter.ConverterInstance);

			return reader;
		}

        public DataSet FindDataSet(IDbCommand finder)
        {
            DataSet results = new DataSet();

            results = _context.ExecuteDataSet(finder);
            return results;
        }

        public DataTable Find(IDbCommand finder)
        {
            DataTable results = new DataTable();

            using (ITypeReader reader = new TypeConvertingReader(
                _context.ExecuteReader(finder), Converter.ConverterInstance))
            {
                results.Load(reader);
                return results;
            }     
        }

        public DataTable Find(IDbCommand finder, CacheAccessor cacher)
        {
            using (ITypeReader reader = new TypeConvertingReader(_context.ExecuteReader(finder), Converter.ConverterInstance))
            {
                DataTable item = new DataTable();

                item.Load(reader);
                if (cacher != null)
                {
                    cacher.PutCacheResult(item);
                }
                return item;
            }
        }
		/// <summary>
		/// Queries for and returns a single item of the given type, or the default
		/// for the type if the command has no result.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="finder"></param>
		/// <param name="builder"></param>
		/// <param name="cacher"></param>
		/// <returns></returns>
    	public T Find<T>( IDbCommand finder, Build<T> builder, CacheAccessor cacher )
        {
            using( ITypeReader reader = new TypeConvertingReader( _context.ExecuteReader( finder ), Converter.ConverterInstance ) )
            {
            	T item = default( T );

				if( reader.Read() )
				{
					item = builder( reader );

					if( cacher != null )
					{
						cacher.PutCacheResult( item );
					}
				}

            	return item;
            }
        }

		public List<T> FindAll<T>( IDbCommand finder, Build<T> builder )
		{
			return FindAll( finder, builder, null );
		}

		/// <summary>
		/// Queries for and returns a list of the given type, or an empty list
		/// if the command has no result.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="finder"></param>
		/// <param name="builder"></param>
		/// <param name="cacher"></param>
		/// <returns></returns>
        public List<T> FindAll<T>( IDbCommand finder, Build<T> builder, CacheAccessor cacher )
        {
            using( ITypeReader reader = new TypeConvertingReader(
				_context.ExecuteReader(finder), Converter.ConverterInstance ) )
            {
                var results = new List<T>();

                while( reader.Read() )
                {
                	T item = builder( reader );

					if( cacher != null && ! cacher.IsGroupCache )
					{
						cacher.PutCacheResult( reader.GetString( cacher.Key ), item );
					}

                    results.Add( item );
                }

				if( cacher != null && cacher.IsGroupCache )
				{
					cacher.PutCacheListResult( results );
				}

                return results;
            }     
        }

		/// <summary>
		/// Finds all records from a given multi-set query and applies a
		/// build transformer to each one.
		/// </summary>
		/// <param name="finder"></param>
		/// <param name="builders"></param>
		/// <returns></returns>
		public IList FindAll( IDbCommand finder, List<Build> builders )
		{
			return FindAll( finder, builders, null );
		}

        public IList FindAll( IDbCommand finder, List<Build> builders, CacheAccessor cacher )
        {
            using( ITypeReader reader = new TypeConvertingReader(
				_context.ExecuteReader(finder), Converter.ConverterInstance ) )
            {
                IList results = new ArrayList();

				for( int i = 0; i < builders.Count; i++ )
				{
					var rowList = new ArrayList();

					while( reader.Read() )
					{
						rowList.Add( builders[i](reader) );
					}
					
					results.Add( rowList );
					
					if( i < ( builders.Count - 1 ) )
					{
						reader.NextResult();
					}
				}

				if( cacher != null && cacher.IsGroupCache )
				{
					cacher.PutCacheResult( results );
				}

                return results;
            }     
        }

		/// <summary>
		/// Creates a dictionary of given key and value type from a command, using 
		/// builders for both.  Will return an empty dictionary if there are no results.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="finder"></param>
		/// <param name="builder"></param>
		/// <param name="keyBuilder"></param>
		/// <returns></returns>
		public Dictionary<K, V> FindAll<K, V>( IDbCommand finder, Build<V> builder, Build<K> keyBuilder )
		{
			return FindAll( finder, builder, keyBuilder, null );
		}

		public Dictionary<K, V> FindAll<K, V>( IDbCommand finder, Build<V> builder, Build<K> keyBuilder, CacheAccessor cacher )
		{
            using( ITypeReader reader = new TypeConvertingReader(
				_context.ExecuteReader(finder), Converter.ConverterInstance ) )
            {
                var results = new Dictionary<K, V>();

                while( reader.Read() )
                {
                	V item = builder( reader );
                	K key = keyBuilder( reader );

					if( cacher != null && ! cacher.IsGroupCache )
					{
						cacher.PutCacheResult( reader.GetString( cacher.Key ), item );
					}

                    results.Add( key, item );
                }

				if( cacher != null && cacher.IsGroupCache )
				{
					cacher.PutCacheResult( results );
				}

                return results;
            }			
		}

		/// <summary>
		/// Pulls the first value from the reader as an integer.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected static int FirstInt( ITypeReader reader )
		{
			return Converter.ConverterInstance.ToInt( reader[0] );
		}

		/// <summary>
		/// Pulls the first value from the read as a string.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected static string FirstString( ITypeReader reader )
		{
			return Converter.ConverterInstance.ToString( reader[0] );			
		}
    }
}
