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
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	///	Used to create the SQL statement to find orders in the database
	/// </summary>
	/// <remarks>
	/// If any new properties are added to this class for searching, ensure
	/// you log out of the system and back in to clear the session, or you
	/// might get a NullReferenceException due to the new property not
	/// being intialised (as an instance of this class will already be stored
	/// the session, in SavedOrderSearch).
	/// </remarks>
	public partial class OrderFinder
	{
		#region Private variables

		public OrderFinder()
		{
			ContainingAssetsFromBrandId = 0;
			ContainingAssetsUploadedByUserId = 0;
			PendingOrdersOnly = null;
		}

		#endregion

		#region Accessors

		public int ContainingAssetsFromBrandId { get; set; }

		public int ContainingAssetsUploadedByUserId { get; set; }

		public bool? PendingOrdersOnly { get; set; }

		public string GeneralKeyword { get; set; }

		#endregion

		protected void SetCustomSearchCriteria (ref SearchBuilder sb)
		{
			if (ContainingAssetsFromBrandId != 0)
			{
				string criteria = string.Format("{0}.OrderId IN (SELECT OrderId FROM [v_OrderItem] OI WHERE OI.AssetBrandId={1})", sb.TableAlias, ContainingAssetsFromBrandId);
				sb.Criteria.Add(criteria);
			}

			if (ContainingAssetsUploadedByUserId != 0)
			{
				string criteria = string.Format("{0}.OrderId IN (SELECT OrderId FROM [v_OrderItem] OI WHERE OI.AssetUploadUserId={1})", sb.TableAlias, ContainingAssetsUploadedByUserId);
				sb.Criteria.Add(criteria);
			}

			if (PendingOrdersOnly.HasValue)
			{
				sb.Criteria.Add(PendingOrdersOnly.Value ? "CompletionDate IS NULL" : "CompletionDate IS NOT NULL");
			}

			if (!StringUtils.IsBlank(GeneralKeyword))
			{
				JoinableList jList = new JoinableList(" OR ");

				if (NumericUtils.IsInt32(GeneralKeyword))
				{
					jList.Add(string.Format("{0}={1}", Order.Columns.OrderId, GeneralKeyword));
				}
				else
				{
					foreach (string column in new[] {"UserName", "UserEmail", "UserPrimaryBrandName"})
					{
						jList.Add(string.Format("{0} LIKE '%{1}%'", column, SqlUtils.SafeValue(GeneralKeyword)));
					}
				}

				sb.Criteria.Add(jList);
			}
		}
	}
}

