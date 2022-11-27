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
	public class NullBrandMetadataSelectableSetting : BrandMetadataSelectableSetting
	{
		#region Singleton implementation

		private NullBrandMetadataSelectableSetting()
		{
		}

		private static readonly NullBrandMetadataSelectableSetting m_instance = new NullBrandMetadataSelectableSetting();

		public static NullBrandMetadataSelectableSetting Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		public override int BrandMetadataSettingId { get { return 0; } set { ; } }
		public override int SelectableType		 { get { return 0; } set { ; } }
		public override int Depth				 { get { return 0; } set { ; } }
		public override bool IsLinear			 { get { return false; } set { ; } }
		public override int SortType			 { get { return 0; } set { ; } }
        public override bool AllowMultiple { get { return false; } set { ; } }
		public override int OrderType			 { get { return 0; } set { ; } }
		public override int ColumnCount				 { get { return 0; } set { ; } }

		public override bool IsNull
		{
			get { return true; }
        }

        #endregion
    }
}

