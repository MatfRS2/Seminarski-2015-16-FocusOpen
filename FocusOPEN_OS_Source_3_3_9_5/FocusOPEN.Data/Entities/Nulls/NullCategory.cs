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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullCategory : Category
	{
		#region Singleton implementation

		private NullCategory()
		{
		}

		private static readonly NullCategory m_instance = new NullCategory();

		public static NullCategory Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the ParentCategoryId of the Category object.
		/// </summary>
		public override Nullable <Int32> ParentCategoryId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the Category object.
		/// </summary>
		public override int BrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the Category object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExternalRef of the Category object.
		/// </summary>
		public override string ExternalRef
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Message of the Category object.
		/// </summary>
		public override string Message
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Synonyms of the Category object.
		/// </summary>
		public override string Synonyms
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the OwnerUserId of the Category object.
		/// </summary>
		public override int OwnerUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CategoryOrder of the Category object.
		/// </summary>
		public override int CategoryOrder
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the FullAssetCount of the Category object.
		/// </summary>
		public override int FullAssetCount
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the AvailableAssetCount of the Category object.
		/// </summary>
		public override int AvailableAssetCount
		{
			get { return 0; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

