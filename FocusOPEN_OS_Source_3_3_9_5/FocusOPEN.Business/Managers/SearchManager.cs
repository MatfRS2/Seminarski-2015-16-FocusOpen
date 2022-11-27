/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class SearchManager
	{
		/// <summary>
		/// Gets the base asset finder to be used for searching
		/// </summary>
		public static AssetFinder GetBaseAssetFinder(User user)
		{
			// Initialise asset finder.
			// Only return completely published assets (published and date in range)
			AssetFinder finder = new AssetFinder {IsCompletelyPublished = true};

			// Default to relevance ordering
			finder.OrderBy = OrderBy.Relevance;

			// Apply additional restrictions for non-superadmins
			if (user.UserRole != UserRole.SuperAdministrator)
			{
				// Non-superadmins users cannot see assets outside of their brand assignments
				user.Brands.ForEach(b => finder.BrandIdList.Add(b.BrandId.GetValueOrDefault()));

				if (user.IsEmployee)
				{
					// Employees should not see assets flagged as hidden from internal users
					finder.InternalUsers_HideFromUsers = false;
				}
				else
				{
					// External users should not see assets flagged as hidden from non-employees
					finder.ExternalUsers_HideFromUsers = false;
				}
			}

			// Favour newer assets
			finder.SortExpressions.Add(new DescendingSort(Asset.Columns.PublishDate));
			finder.SortExpressions.Add(new DescendingSort(Asset.Columns.AssetId));

			return finder;
		}
	}
}