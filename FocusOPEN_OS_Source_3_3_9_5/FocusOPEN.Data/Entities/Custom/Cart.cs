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
	/// This object represents the properties and methods of a Cart.
	/// </summary>
	public partial class Cart
	{
		#region Private variables

		private Asset m_Asset = null;

		#endregion

		#region Lazy Loads

		public Asset Asset
		{
			get
			{
				if (m_Asset == null)
					m_Asset = Asset.Get(AssetId);

				return m_Asset;
			}
		}

		#endregion
	}
}