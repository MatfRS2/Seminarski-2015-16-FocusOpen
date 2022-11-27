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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class HomepageImageManager
	{
		#region Private Variables

		private static string m_HomepageImagesFolder;

		#endregion

		#region Accessors

		/// <summary>
		/// Absolute path to homepage images
		/// </summary>
		public static string HomepageImagesFolder
		{
			get
			{
				
				return m_HomepageImagesFolder;
			}
			set
			{
				m_HomepageImagesFolder = VirtualPathUtility.AppendTrailingSlash(value);
			}
		}

		#endregion

		/// <summary>
		/// Gets the homepage image filename prefix
		/// </summary>
		/// <param name="homepageId">Homepage ID</param>
		/// <param name="number">Image Number</param>
		/// <returns></returns>
		private static string GetHomepageImagePrefix(int homepageId, int number)
		{
			return string.Format("Image_{0}_{1}", homepageId, number);
		}

		/// <summary>
		/// Gets list of files for the specified homepage and image number
		/// </summary>
		/// <param name="homepageId">Homepage ID</param>
		/// <param name="number">Image Number</param>
		/// <returns>List of files matching the specified homepage and image number</returns>
		private static IEnumerable<string> GetHomepageImageFiles(int homepageId, int number)
		{
			string prefix = GetHomepageImagePrefix(homepageId, number).ToLower();

			return (from path in Directory.GetFiles(HomepageImagesFolder)
			        where (Path.GetFileName(path) ?? string.Empty).ToLower().StartsWith(prefix)
			        select path).ToList();
		}

		/// <summary>
		/// Gets the first file matching the specified homepage ID and image number
		/// If there are no files, an empty string is returned instead.
		/// </summary>
		/// <param name="homepageId">Homepage ID</param>
		/// <param name="number">Image Number</param>
		/// <returns>First file matching specified homepage id and image number.  Empty string if no file.</returns>
		public static string GetHomepageImagePath(int homepageId, int number)
		{
			return GetHomepageImageFiles(homepageId, number).FirstOrDefault() ?? string.Empty;
		}

		public static void SaveHomepageImage(Homepage homepage, int number, BinaryFile file, Size maxDimensions)
		{
			if (homepage.IsNew)
				throw new SystemException("Homepage must be saved before image can be saved");

			if (!file.IsEmpty)
			{
				// The ID of the homepage being saved
				int homepageId = homepage.HomepageId.GetValueOrDefault();

				// Delete the old files
				foreach (string f in GetHomepageImageFiles(homepageId, number))
					File.Delete(f);

				// Construct path to the new homepage image
				string prefix = GetHomepageImagePrefix(homepageId, number);
				string filename = string.Format("{0}.{1}", prefix, file.FileExtension);
				string path = Path.Combine(HomepageImagesFolder, filename);
				
				// Save the file
				file.SaveAs(path);

				// Resize the file if required
				if (!maxDimensions.IsEmpty)
					ImageUtils.ResizeImageDown(path, maxDimensions.Height, maxDimensions.Width);
			}
		}
	}
}