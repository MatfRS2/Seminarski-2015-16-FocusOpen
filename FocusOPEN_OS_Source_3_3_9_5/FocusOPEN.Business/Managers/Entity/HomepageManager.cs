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
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class HomepageManager
	{
		#region Private Variables

		private static readonly List<String> m_ThumbnailImageExtensions = new List<string>();
		private static readonly List<String> m_FeatureImageExtensions = new List<string>();

		#endregion

		#region Constructor

		static HomepageManager()
		{
			m_ThumbnailImageExtensions.Add("gif");
			m_ThumbnailImageExtensions.Add("jpg");
			m_ThumbnailImageExtensions.Add("png");

			m_FeatureImageExtensions.AddRange(m_ThumbnailImageExtensions);
			m_FeatureImageExtensions.Add("swf");
			m_FeatureImageExtensions.Add("flv");
		}

		#endregion

		#region Public Methods

		public static Homepage GetCurrentHomepage(int brandId)
		{
			HomepageFinder finder = new HomepageFinder { BrandId = brandId, IsPublished = true };
			finder.SortExpressions.Add(new DescendingSort(Homepage.Columns.LastModifiedDate));
			finder.SortExpressions.Add(new DescendingSort(Homepage.Columns.HomepageId));
			Homepage homepage = Homepage.FindOne(finder);

			if (!homepage.IsNull)
				return homepage;

			int masterBrandId = BrandCache.Instance.GetMasterBrandId();

			if (masterBrandId == brandId)
				throw new SystemException("No published homepage for master brand");

			return GetCurrentHomepage(masterBrandId);
		}

		/// <summary>
		/// Publish homepage
		/// </summary>
		/// <param name="user">The user publishing the homepage</param>
		/// <param name="homepage">The homepage to be published</param>
		/// <param name="imageList">The image files for the homepage</param>
		public static void PublishHomepage(User user, Homepage homepage, IList<BinaryFile> imageList)
		{
			SaveHomepage(user, homepage, imageList, true);
		}

		/// <summary>
		/// Preview homepage
		/// </summary>
		/// <param name="user">The user publishing the homepage</param>
		/// <param name="homepage">The homepage to be published</param>
		/// <param name="imageList">The image files for the homepage</param>
		public static void PreviewHomepage(User user, Homepage homepage, IList<BinaryFile> imageList)
		{
			SaveHomepage(user, homepage, imageList, false);
		}

		/// <summary>
		/// Creates an unpublished copy of the homepage and returns it
		/// </summary>
		/// <param name="homepage">The homepage to be copied</param>
		/// <returns>Copy of homepage</returns>
		public static Homepage GetCopy(Homepage homepage)
		{
			// List of images
			IList<BinaryFile> imageList = new List<BinaryFile>();

			// Get the ID of the homepage being copied
			int homepageId = homepage.HomepageId.GetValueOrDefault();

			// Set the homepage ID to null so we create a new record on save
			homepage.HomepageId = null;

			// The copied homepage is not published
			homepage.IsPublished = false;

			// Save the homepage
			Homepage.Update(homepage);

			// Get all of the images
			for (int i = 1; i <= 4; i++)
			{
				BinaryFile file = BinaryFile.Empty;

				string imagePath = HomepageImageManager.GetHomepageImagePath(homepageId, i);

				if (!StringUtils.IsBlank(imagePath) && File.Exists(imagePath))
					file = new BinaryFile(imagePath, BinaryFile.SaveMode.Copy);
				
				imageList.Add(file);
			}

			// Save the homepage images
			SaveHomepageImages(homepage, imageList);

			// Return the copy
			return homepage;
		}

		#endregion

		#region Private Helper Methods

		private static void SaveHomepage(User user, Homepage homepage, IList<BinaryFile> imageList, bool publish)
		{
			ErrorList errors = ValidateHomepage(homepage, imageList);

			if (errors.Count > 0)
				throw new InvalidHomepageException(errors, homepage);

			homepage.IsPublished = publish;
			Homepage.Update(homepage);

			if (homepage.CategoriesLoaded)
			{
				HomepageCategory.DeleteHomepageCategories(homepage.HomepageId.GetValueOrDefault());

				foreach (HomepageCategory hc in homepage.HomepageCategoryList)
				{
					hc.HomepageId = homepage.HomepageId.GetValueOrDefault();
					HomepageCategory.Update(hc);
				}
			}

			SaveHomepageImages(homepage, imageList);

			if (publish)
			{
				AuditLogManager.LogUserAction(user, AuditUserAction.PublishHomepage, string.Format("Published Homepage: {0}", homepage.HomepageId));
			}
			else
			{
				AuditLogManager.LogUserAction(user, AuditUserAction.PreviewHomepage, string.Format("Preview Homepage created: {0}", homepage.HomepageId));
			}
		}

		private static void SaveHomepageImages(Homepage homepage, IEnumerable<BinaryFile> imageList)
		{
			var count = 0;
			var size = new Size(168, 124);

			foreach (BinaryFile image in imageList)
			{
				count++;
				HomepageImageManager.SaveHomepageImage(homepage, count, image, size);
			}
		}

		private static ErrorList ValidateHomepage(Homepage homepage, IEnumerable<BinaryFile> imageList)
		{
			ErrorList errors = new ErrorList();

			if (homepage.HomepageTypeId == 1 && StringUtils.IsBlank(homepage.IntroText))
				errors.Add("Intro text is required");

			if (homepage.HomepageTypeId > 1 && StringUtils.IsBlank(homepage.CustomHtml))
				errors.Add("HTML content is required for the selected homepage type");

			var count = 0;

			foreach (BinaryFile image in imageList)
			{
				count++;

				if (!image.IsEmpty && !m_ThumbnailImageExtensions.Contains(image.FileExtension))
					errors.Add(string.Format("Image {0} is not a valid image.", count));
			}

			return errors;
		}

		#endregion
	}
}