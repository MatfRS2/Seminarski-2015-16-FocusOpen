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
	/// <summary>
	/// This object represents a null OrderItemComment.
	/// </summary>
	[Serializable]
	public class NullOrderItemComment : OrderItemComment
	{
		#region Singleton implementation

		private NullOrderItemComment()
		{
		}

		private static readonly NullOrderItemComment m_instance = new NullOrderItemComment();

		public static NullOrderItemComment Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the OrderItemId of the OrderItemComment object.
		/// </summary>
		public override int OrderItemId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserId of the OrderItemComment object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CommentText of the OrderItemComment object.
		/// </summary>
		public override string CommentText
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the CommentDate of the OrderItemComment object.
		/// </summary>
		public override DateTime CommentDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

