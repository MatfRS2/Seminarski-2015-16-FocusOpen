/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	/// <summary>
	/// The cache manager handles invalidating local and remote caches.  The ASP.NET application cache is stored per domain
	/// so we need to iterate through each brand and invalidate its cache.  This is done by using a background thread.
	/// This is actually a not a good way of doing this and we should really use a better cache management system
	/// (something like Microsoft Velocity or memcached perhaps) that all domains can access independently.
	/// </summary>
	public static class CacheManager
	{
		// Logger
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Number of seconds to wait between calling the invalidate URL for each brand
		private const int WAIT_BETWEEN_REQUESTS = 10;

		/// <summary>
		/// Invalidate the cache
		/// </summary>
		/// <param name="section">The section to be invalidated</param>
		/// <param name="cacheType">The cache to be invalidated</param>
		public static void InvalidateCache(string section, CacheType cacheType)
		{
			m_Logger.DebugFormat("InvalidateCache {0}, type: {1}", section, cacheType);

			// Invalidate the local cache
			if (cacheType.IsSet(CacheType.Local))
				InvalidateLocalCache(section);

			// Invalidate the remote cache
			// (This just calls a handler on this website on the other domains to invalidate the cache)
			if (cacheType.IsSet(CacheType.Remote))
			{
				RemoteCacheInvalidator remoteCacheInvalidator = new RemoteCacheInvalidator(section);
				Thread t = new Thread(remoteCacheInvalidator.CallInvalidateUrl) { IsBackground = true, Priority = ThreadPriority.Lowest };
				t.Start();
			}
		}

		/// <summary>
		/// Invalidates the local cache
		/// </summary>
		/// <param name="section">The name of the section to be invalidated</param>
		private static void InvalidateLocalCache(string section)
		{
			section = section.ToLower();

			m_Logger.DebugFormat("InvalidateLocalCache : {0}", section);

			if (StringUtils.IsBlank(section) || section == "assetfilepath")
				AssetFilePathCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "assetimagesize")
				AssetImageSizeCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "assettype")
				AssetTypeCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "assettypefileextension")
				AssetTypeFileExtensionCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "brand")
				BrandCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "brandmetadata")
				BrandMetadataSettingCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "metadata")
				MetadataCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "category")
				CategoryCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "country")
				CountryCache.Instance.InvalidateCache();

			if (StringUtils.IsBlank(section) || section == "company")
				CompanyCache.Instance.InvalidateCache();

            if (StringUtils.IsBlank(section) || section == "pluginsunregistered")
                PluginsUnregisteredCache.Instance.InvalidateCache();
            
            if (StringUtils.IsBlank(section) || section == "pluginsregistered")
                PluginsRegisteredCache.Instance.InvalidateCache();

            if (StringUtils.IsBlank(section) || section == "plugins")
            {
                PluginsUnregisteredCache.Instance.InvalidateCache();
                PluginsRegisteredCache.Instance.InvalidateCache();
            }

		}

		#region Nested type: RemoteCacheInvalidator

		private class RemoteCacheInvalidator
		{
			private readonly string m_Section;

			public RemoteCacheInvalidator(string section)
			{
				m_Section = section;
			}

			public void CallInvalidateUrl()
			{
				try
				{
					m_Logger.DebugFormat("Invalidating cache for section: {0}", m_Section);

					// Get list of brands
					BrandFinder finder = new BrandFinder();
					IList<Brand> brandList = Brand.FindMany(finder);

					m_Logger.DebugFormat("Found {0} brands where the cache needs to be invalidated", brandList.Count);

					// List of URL's that have been processed
					// This is in case brand share URL's, in which case
					// we don't want to invalidate the same cache twice.
					List<string> processUrlList = new List<string>();

					foreach (Brand brand in brandList)
					{
						// Get the website URL, ensure ends with /
						string websiteUrl = brand.WebsiteUrl.EnsureEndsWith("/");

						// Skip duplicate URL's
						if (processUrlList.Contains(websiteUrl))
						{
							m_Logger.DebugFormat("Skipping {0}.  Duplicate Url: {1}", brand.Name, websiteUrl);
							continue;
						}

						// Not a duplicate, add it to the list
						processUrlList.Add(websiteUrl);

						// Construct the URL to the invalidate request
						string url = string.Concat(websiteUrl, "InvalidateCache.ashx?section=", m_Section);

						m_Logger.DebugFormat("Sending cache invalidation request to '{0}' (BrandId: {1}) - URL: {2}", brand.Name, brand.BrandId, url);

						try
						{
							// Send the request
							HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
							request.Method = "GET";
							request.KeepAlive = false;
							request.GetResponse().Close();

							m_Logger.DebugFormat("Cache invalidation request done.  Waiting {0}s before sending next request.", WAIT_BETWEEN_REQUESTS);
						}
						catch (Exception e)
						{
							m_Logger.WarnFormat("Error sending invalidation request to: {0}.  Error: {1}", url, e.Message);
						}

						// Wait a few seconds before sending the next request
						Thread.Sleep(WAIT_BETWEEN_REQUESTS*1000);
					}

					m_Logger.Debug("Done sending invalidation requests.");
				}
				catch (Exception e)
				{
					m_Logger.Warn(string.Format("Error invalidating cache: {0}", e.Message), e);
				}
			}
		}

		#endregion
	}

	[Flags]
	public enum CacheType : uint
	{
		Local = 1,
		Remote = 2,
		All = Local | Remote
	}
}