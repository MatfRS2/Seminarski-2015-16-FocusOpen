using System;
using System.Configuration;
using System.Reflection;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using log4net;

namespace FocusOPEN.CategoryCountUpdater
{
	internal class Program
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static void Main()
		{
			Console.WriteLine("***************************************************************");
			Console.WriteLine("FocusOPEN Category Count Updater");
			Console.WriteLine("***************************************************************");

			Console.WriteLine();
			Console.WriteLine();

			try
			{
				m_Logger.Debug("Updating categories...");
				UpdateCategories();
				m_Logger.Debug("Categories updated successfully");
			}
			catch (Exception ex)
			{
				m_Logger.Error(string.Format("Error updating categories: {0}", ex.Message), ex);
			}

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("All done");
		}

		private static void UpdateCategories()
		{
			DataContext.ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

			var finder = new CategoryFinder();
			var list = Category.FindMany(finder);

			foreach (var category in list)
			{
				var categoryId = category.CategoryId.GetValueOrDefault();

				var publishedCount = CategoryManager.GetPublishedAssetCount(categoryId);
				var fullCount = CategoryManager.GetFullAssetCount(categoryId);

				category.AvailableAssetCount = publishedCount;
				category.FullAssetCount = fullCount;

				Category.Update(category);

				m_Logger.DebugFormat("Updated category: {0} - Published count: {1}, Full Count: {2}", category.Name, publishedCount, fullCount);
			}

			CacheManager.InvalidateCache("Category", CacheType.All);
		}
	}
}