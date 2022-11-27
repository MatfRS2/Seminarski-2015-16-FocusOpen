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
	/// <summary>
	/// This object represents the properties and methods of a LightboxSent.
	/// </summary>
	public partial class LightboxSent
	{
		#region Constructor
		
		protected LightboxSent()
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
		
		public static LightboxSent New ()
		{
			return new LightboxSent() ;
		}

		public static LightboxSent Empty
		{
			get { return NullLightboxSent.Instance; }
		}

		public static LightboxSent Get (Nullable <Int32> LightboxSentId)
		{
			LightboxSent LightboxSent = LightboxSentMapper.Instance.Get (LightboxSentId);
			return LightboxSent ?? Empty;
		}

		public static LightboxSent Update (LightboxSent lightboxSent)
		{
			return LightboxSentMapper.Instance.Update(lightboxSent) ;
		}

		public static void Delete (Nullable <Int32> LightboxSentId)
		{
			LightboxSentMapper.Instance.Delete (LightboxSentId);
		}

		public static EntityList <LightboxSent> FindMany (LightboxSentFinder finder, int Page, int PageSize)
		{
			return LightboxSentMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <LightboxSent> FindMany (LightboxSentFinder finder)
		{
			return LightboxSentMapper.Instance.FindMany (finder);
		}

		public static LightboxSent FindOne (LightboxSentFinder finder)
		{
			LightboxSent LightboxSent = LightboxSentMapper.Instance.FindOne(finder);
			return LightboxSent ?? Empty;
		}

		public static int GetCount (LightboxSentFinder finder)
		{
			return LightboxSentMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
