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
	/// This object represents the properties and methods of a OrderItemComment.
	/// </summary>
	public partial class OrderItemComment
	{
		#region Constructor
		
		protected OrderItemComment()
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
		
		public static OrderItemComment New ()
		{
			return new OrderItemComment() ;
		}

		public static OrderItemComment Empty
		{
			get { return NullOrderItemComment.Instance; }
		}

		public static OrderItemComment Get (Nullable <Int32> OrderItemCommentId)
		{
			OrderItemComment OrderItemComment = OrderItemCommentMapper.Instance.Get (OrderItemCommentId);
			return OrderItemComment ?? Empty;
		}

		public static OrderItemComment Update (OrderItemComment orderItemComment)
		{
			return OrderItemCommentMapper.Instance.Update(orderItemComment) ;
		}

		public static void Delete (Nullable <Int32> OrderItemCommentId)
		{
			OrderItemCommentMapper.Instance.Delete (OrderItemCommentId);
		}

		public static EntityList <OrderItemComment> FindMany (OrderItemCommentFinder finder, int Page, int PageSize)
		{
			return OrderItemCommentMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <OrderItemComment> FindMany (OrderItemCommentFinder finder)
		{
			return OrderItemCommentMapper.Instance.FindMany (finder);
		}

		public static OrderItemComment FindOne (OrderItemCommentFinder finder)
		{
			OrderItemComment OrderItemComment = OrderItemCommentMapper.Instance.FindOne(finder);
			return OrderItemComment ?? Empty;
		}

		public static int GetCount (OrderItemCommentFinder finder)
		{
			return OrderItemCommentMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
