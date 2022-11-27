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
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FocusOPEN.Website.Components
{
	public class AssetTypeInfo
	{
		#region Private Accessors

		private static Dictionary<string,AssetTypeInfo> AssetTypeInfoDict
		{
			get
			{
				Dictionary<string, AssetTypeInfo> dict = HttpRuntime.Cache.Get("AssetTypeInfoDict") as Dictionary<string, AssetTypeInfo>;

				if (dict == null)
				{
					dict = new Dictionary<string, AssetTypeInfo>();

					var list = from fe in XDocument.Load(ConfigurationPath).XPathSelectElements("//FileExtension")
					           let g = fe.Parent.Parent
					           select new
					                  	{
					                  		Extension = fe.Value,
					                  		HasOrientation = Boolean.Parse(g.Attribute("HasOrientation").Value),
					                  		HasDimensions = Boolean.Parse(g.Attribute("HasDimensions").Value),
					                  		HasDuration = Boolean.Parse(g.Attribute("HasDuration").Value)
					                  	};

					foreach (var n in list)
						dict[n.Extension] = new AssetTypeInfo(n.HasOrientation, n.HasDuration, n.HasDimensions);

					HttpRuntime.Cache.Insert("AssetTypeInfoDict", dict, new CacheDependency(ConfigurationPath));
				}

				return dict;
			}
		}

		#endregion

		#region Public Accessors

		/// <summary>
		/// Gets or sets the path to the XML file with asset type info
		/// </summary>
		public static string ConfigurationPath { get; set; }

		// Standard accessors
		public bool HasOrientation { get; private set; }
		public bool HasDuration { get; private set; }
		public bool HasDimensions { get; private set; }

		#endregion

		#region Constructors

		private AssetTypeInfo()
		{
		}

		private AssetTypeInfo(bool hasOrientation, bool hasDuration, bool hasDimensions)
		{
			HasOrientation = hasOrientation;
			HasDuration = hasDuration;
			HasDimensions = hasDimensions;
		}

		#endregion

		public static AssetTypeInfo Get(string fileExtension)
		{
			string extension = fileExtension.ToLower();

			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			if (AssetTypeInfoDict.ContainsKey(extension))
				return AssetTypeInfoDict[fileExtension];

			return new AssetTypeInfo();
		}
	}
}