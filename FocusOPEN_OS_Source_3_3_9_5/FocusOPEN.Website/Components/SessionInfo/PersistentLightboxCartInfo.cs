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

namespace FocusOPEN.Website.Components
{
	[Serializable]
	public class PersistentLightboxCartInfo
	{
		#region Constructor

		public PersistentLightboxCartInfo()
		{
			PersistentCartLightboxMode = PersistentCartLightboxMode.Lightbox;
			PersistentCartLightboxState = PersistentCartLightboxState.Open;
			SelectedLightboxId = 0;
			LightboxOffSet = 0;
			CartOffSet = 0;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the persistent cart lightbox mode (cart or lightbox)
		/// </summary>
		public PersistentCartLightboxMode PersistentCartLightboxMode { get; set; }

		/// <summary>
		/// Gets or sets the state of the persistent cart lightbox (open or closed)
		/// </summary>
		public PersistentCartLightboxState PersistentCartLightboxState { get; set; }

		/// <summary>
		/// Gets or sets the selected lightbox id.
		/// </summary>
		/// <value>The selected lightbox id.</value>
		public int SelectedLightboxId { get; set; }

		/// <summary>
		/// Gets or sets the lightbox offset.
		/// </summary>
		public int LightboxOffSet { get; set; }

		/// <summary>
		/// Gets or sets the cart offset.
		/// </summary>
		public int CartOffSet { get; set; }

		#endregion
	}
}