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
	public class NullBrandMetadataSetting : BrandMetadataSetting
	{
		#region Singleton implementation

		private NullBrandMetadataSetting()
		{
		}

		private static readonly NullBrandMetadataSetting m_instance = new NullBrandMetadataSetting();

		public static NullBrandMetadataSetting Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the BrandId of the BrandMetadataSetting object.
		/// </summary>
		public override int BrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the FieldId of the BrandMetadataSetting object.
		/// </summary>
		public override string FieldId
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the FieldName of the BrandMetadataSetting object.
		/// </summary>
		public override string FieldName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsRequired of the BrandMetadataSetting object.
		/// </summary>
		public override bool IsRequired
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the OnAssetForm of the BrandMetadataSetting object.
		/// </summary>
		public override bool OnAssetForm
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the OnAssetDetail of the BrandMetadataSetting object.
		/// </summary>
		public override bool OnAssetDetail
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the AdditionalCopy of the BrandMetadataSetting object.
		/// </summary>
		public override string AdditionalCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ToolTip of the BrandMetadataSetting object.
		/// </summary>
		public override string ToolTip
		{
			get { return String.Empty; }
			set { ; }
		}

		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

