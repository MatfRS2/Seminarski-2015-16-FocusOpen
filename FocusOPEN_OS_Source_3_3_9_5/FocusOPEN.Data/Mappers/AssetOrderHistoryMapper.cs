/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object maps data between the database and a AssetOrderHistory object.
	/// </summary>
	internal partial class AssetOrderHistoryMapper
	{
		#region Singleton behaviour

		private AssetOrderHistoryMapper()
		{
		}

		private static readonly AssetOrderHistoryMapper m_instance = new AssetOrderHistoryMapper();

		public static AssetOrderHistoryMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

	}
}