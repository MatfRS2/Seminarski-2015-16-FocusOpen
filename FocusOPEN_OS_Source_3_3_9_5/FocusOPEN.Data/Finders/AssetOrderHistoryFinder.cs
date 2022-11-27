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

namespace FocusOPEN.Data
{
	public partial class AssetOrderHistoryFinder
	{
		#region Constructor

		public AssetOrderHistoryFinder()
		{
			ForAssetId = 0;
            ForUserId = 0;
		}

		#endregion

		#region Accessors

		public int ForAssetId { get; set; }
        public int ForUserId { get; set; }

		#endregion

		protected void SetCustomSearchCriteria (ref SearchBuilder sb)
		{

			if (ForAssetId != 0)
				sb.Criteria.Add(string.Format("AssetId={0}", ForAssetId));

            if (ForUserId != 0)
                sb.Criteria.Add(string.Format("UserId={0}", ForUserId));
            
		}
	}
}

