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
	/// This object represents the properties and methods of a LightboxAsset.
	/// </summary>
	public partial class LightboxAsset
	{
		#region Lazy Loads

		private Asset m_Asset = null;
		private Lightbox m_Lightbox = null;

		public Asset Asset
		{
			get
			{
				if (m_Asset == null)
					m_Asset = Asset.Get(AssetId);

				return m_Asset;
			}
		}

		public Lightbox Lightbox
		{
			get
			{
				if (m_Lightbox == null)
					m_Lightbox = Lightbox.Get(LightboxId);

				return m_Lightbox;
			}
		}

		#endregion
	}
}