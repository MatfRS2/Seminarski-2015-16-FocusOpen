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
using System.Collections.Generic;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a OrderItemComment.
	/// </summary>
	[Serializable]
	public partial class OrderItemComment : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderItemCommentId = null;
		
		// Table variables
		protected int m_orderItemId = 0;
		protected int m_userId = 0;
		protected string m_commentText = String.Empty;
		protected DateTime m_commentDate = DateTime.MinValue;
		
		// View Variables
		protected string m_userFullName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the OrderItemComment object.
		/// </summary>
		public Nullable <Int32> OrderItemCommentId
		{
			get
			{
				return m_orderItemCommentId;
			}
			set 
			{
				if (value != m_orderItemCommentId)
				{
					m_orderItemCommentId = value;
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_orderItemCommentId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the OrderItemId of the OrderItemComment object.
		/// </summary>
		public virtual int OrderItemId
		{
			get
			{
				return m_orderItemId;
			}
			set 
			{ 
				if ((value != m_orderItemId))
				{
					m_ChangedProperties["OrderItemId"] = new ChangedProperty("OrderItemId", m_orderItemId, value);
					
					m_orderItemId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the OrderItemComment object.
		/// </summary>
		public virtual int UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				if ((value != m_userId))
				{
					m_ChangedProperties["UserId"] = new ChangedProperty("UserId", m_userId, value);
					
					m_userId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CommentText of the OrderItemComment object.
		/// </summary>
		public virtual string CommentText
		{
			get
			{
				return m_commentText;
			}
			set 
			{ 
				if ((value != m_commentText))
				{
					m_ChangedProperties["CommentText"] = new ChangedProperty("CommentText", m_commentText, value);
					
					m_commentText = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CommentDate of the OrderItemComment object.
		/// </summary>
		public virtual DateTime CommentDate
		{
			get
			{
				return m_commentDate;
			}
			set 
			{ 
				if ((value != m_commentDate))
				{
					m_ChangedProperties["CommentDate"] = new ChangedProperty("CommentDate", m_commentDate, value);
					
					m_commentDate = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string UserFullName
		{
			get
			{
				return m_userFullName;
			}
			set 
			{ 
				m_userFullName = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			OrderItemCommentId,
			OrderItemId,
			UserId,
			CommentText,
			CommentDate
		}
	}
}

