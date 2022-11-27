/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Daydream.Data;
using log4net;

namespace FocusOPEN.Data
{
	public abstract class BaseEntityCache<T> where T:AbstractEntity
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected abstract List<T> GetData();
		protected abstract bool Compare(T entity, int id);
		protected abstract T Empty { get; }

		#region Protected Virtual Methods

		protected virtual void BeforeGetData()
		{
		}

		protected virtual void AfterGetData()
		{
		}

		protected virtual void BeforeRefreshCache(List<T> list)
		{
		}

		protected virtual void AfterRefreshCache(List<T> list)
		{
		}

        protected virtual string CacheKey
        {
            get
            {
                return "DataListCache_" + typeof(T);
            }
        }



		#endregion



		/// <summary>
		/// Gets a list of all items from the cache.  If not cached, items
		/// will be retrieved from the database and then added to the cache.
		/// </summary>
		public List<T> GetList()
		{
			List<T> list = HttpRuntime.Cache.Get(CacheKey) as List<T>;

			if (list == null)
			{
				m_Logger.DebugFormat("{0} : cache is empty", CacheKey);

				BeforeGetData();
				list = GetData();
				m_Logger.DebugFormat("{0} : Got data, {1} rows", CacheKey, list.Count);
				AfterGetData();

				BeforeRefreshCache(list);
				AddToCache(list);
				m_Logger.DebugFormat("{0} : Added rows to cache", CacheKey);
				AfterRefreshCache(list);
			}

			return list;
		}

		public T GetById(int id)
		{
			foreach (T entity in GetList().Where(entity => Compare(entity, id)))
				return entity;

			return Empty;
		}

		public virtual void InvalidateCache()
		{
			HttpRuntime.Cache.Remove(CacheKey);
			m_Logger.DebugFormat("{0} : Invalidated cache", CacheKey);
		}

		protected virtual void AddToCache(List<T> list)
		{
			HttpRuntime.Cache[CacheKey] = list;
		}
	}
}
