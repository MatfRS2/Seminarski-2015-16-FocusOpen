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
using Daydream.Data;

namespace FocusOPEN.Data
{
	public partial class LightboxLinked
	{
		#region Constructor
		
		protected LightboxLinked()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static LightboxLinked New ()
		{
			return new LightboxLinked() ;
		}

		public static LightboxLinked Empty
		{
			get { return NullLightboxLinked.Instance; }
		}

		public static LightboxLinked Get (Nullable <Int32> LightboxLinkedId)
		{
			LightboxLinked LightboxLinked = LightboxLinkedMapper.Instance.Get (LightboxLinkedId);
			return LightboxLinked ?? Empty;
		}

		public static LightboxLinked Update (LightboxLinked lightboxLinked)
		{
			return LightboxLinkedMapper.Instance.Update(lightboxLinked) ;
		}

		public static void Delete (Nullable <Int32> LightboxLinkedId)
		{
			LightboxLinkedMapper.Instance.Delete (LightboxLinkedId);
		}

		public static EntityList <LightboxLinked> FindMany (LightboxLinkedFinder finder, int Page, int PageSize)
		{
			return LightboxLinkedMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <LightboxLinked> FindMany (LightboxLinkedFinder finder)
		{
			return LightboxLinkedMapper.Instance.FindMany (finder);
		}

		public static LightboxLinked FindOne (LightboxLinkedFinder finder)
		{
			LightboxLinked LightboxLinked = LightboxLinkedMapper.Instance.FindOne(finder);
			return LightboxLinked ?? Empty;
		}

		public static int GetCount (LightboxLinkedFinder finder)
		{
			return LightboxLinkedMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
