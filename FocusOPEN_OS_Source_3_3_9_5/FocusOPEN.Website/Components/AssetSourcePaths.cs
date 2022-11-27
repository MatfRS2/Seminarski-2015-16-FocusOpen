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
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
    public static class AssetSourcePaths
    {
        #region Private Accessors

        public static List<AssetSourcePath> Paths
        {
            get
            {
                List<AssetSourcePath> paths = HttpRuntime.Cache.Get("AssetSourcePaths") as List<AssetSourcePath>;

                if (paths == null)
                {
                    // Read the paths from the configuration file
                	paths = (from x in XDocument.Load(ConfigurationPath).Elements("Paths").Elements()
                	         let name = x.GetAttribute("Name", string.Empty)
                	         let path = x.GetAttribute("Path", string.Empty)
                	         where (!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(path))
                	         select new AssetSourcePath
                	                	{
                	                		Name = name,
                	                		Path = path
                	                	}).ToList();

                    HttpRuntime.Cache.Insert("AssetSourcePaths", paths, new CacheDependency(ConfigurationPath));
                }

                return paths;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the path to the XML file with asset type info
        /// </summary>
        public static string ConfigurationPath { get; set; }
    }

    public class AssetSourcePath
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
