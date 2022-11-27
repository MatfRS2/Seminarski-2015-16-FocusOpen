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
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace FocusOPEN.Shared
{
	public static class Settings
	{
		#region Fields

		private static string m_TempFolder;
		private static string m_CachedAssetFilesFolder = string.Empty;
		private static string m_ImageFolder = string.Empty;

		#endregion

		#region Accessors

		/// <summary>
		/// Absolute path to temp folder
		/// </summary>
		public static string TempFolder
		{
			get
			{
				return m_TempFolder;
			}
			set
			{
			  if( value.EndsWith("\\") )	
				  m_TempFolder = value;
				else
					m_TempFolder = value + "\\";
				if (!Directory.Exists(m_TempFolder))
					Directory.CreateDirectory(m_TempFolder);
			}
		}

		/// <summary>
		/// Absolute path to cache folder
		/// </summary>
		public static string CachedAssetFilesFolder
		{
			get
			{
				return m_CachedAssetFilesFolder;
			}
			set
			{
				m_CachedAssetFilesFolder = VirtualPathUtility.AppendTrailingSlash(value);
			}
		}

		/// <summary>
		/// Absolute path to image folder
		/// </summary>
		public static string ImageFolder
		{
			get
			{
				return m_ImageFolder;
			}
			set
			{
				m_ImageFolder = VirtualPathUtility.AppendTrailingSlash(value);
			}
		}


        public static int SessionTimeout
        {
            get
            {
                SessionStateSection state = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                return Convert.ToInt32(state.Timeout.TotalMinutes);               
            }
        }



		#endregion

		#region Simple Accessors

		public static int MaxBulkDeleteAllowed { get; set; }

		public static int NumberOfMetadataFields { get; set; }

        public static int MaxSelectableMetadataDepth { get; set; }

		/// <summary>
		/// Boolean value specifying whether IP address restrictions are enabled
		/// </summary>
		public static bool IpAddressRestrictionEnabled { get; set; }

		/// <summary>
		/// Minimum SQL date time that can be stored in a SQL Server SmallDateTime field
		/// </summary>
		public static readonly DateTime MIN_DATETIME = new DateTime(1900, 1, 1);

		/// <summary>
		/// Maximum SQL date time that can be stored in a SQL Server SmallDateTime field
		/// </summary>
		public static readonly DateTime MAX_DATETIME = new DateTime(2079, 6, 1);

		#endregion
	}
}
