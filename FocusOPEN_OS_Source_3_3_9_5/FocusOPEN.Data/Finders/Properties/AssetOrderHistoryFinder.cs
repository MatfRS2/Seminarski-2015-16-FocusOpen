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
	[Serializable]
	public partial class AssetOrderHistoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetOrderHistoryId = null;

        // View Variables
        protected int? m_assetId = null;
        protected int? m_orderId = null;
        protected DateTime? m_orderDate = null;
        protected DateTime? m_deadlineDate = null;
        protected string m_userEmail = String.Empty;
        protected int? m_userId = null;
        protected string m_userName = String.Empty;
        protected string m_notes = String.Empty;
        protected DateTime? m_orderItemStatusDate = null;
        protected int? m_orderItemStatusId = null;
		
		#endregion
		
		#region Other Private Variables
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors

        public int FindCriteriaCount
        {
            get
            {
                return m_FindCriteriaCount;
            }
        }

		#endregion
		
		#region View Accessors

        public virtual int? AssetOrderHistoryId
        {
            get
            {
                return m_assetOrderHistoryId;
            }
            set
            {
                 m_assetOrderHistoryId = value;
            }
        }


        public virtual int? AssetId
        {
            get
            {
                return m_assetId;
            }
            set
            {
                m_assetId = value;
            }
        }


        public virtual int? OrderId
        {
            get
            {
                return m_orderId;
            }
            set
            {
                m_FindCriteriaCount++;
            }
        }

        public virtual DateTime? OrderDate
        {
            get
            {
                return m_orderDate;
            }
            set
            {
                m_orderDate = value;
            }
        }


        public virtual DateTime? DeadlineDate
        {
            get
            {
                return m_deadlineDate;
            }
            set
            {
                m_deadlineDate = value;
            }
        }

        public virtual int? UserId
        {
            get
            {
                return m_userId;
            }
            set
            {
               m_userId = value;
            }
        }

        public virtual string UserName
        {
            get
            {
                return m_userName;
            }
            set
            {
                m_userName = value;
            }
        }
        public virtual string UserEmail
        {
            get
            {
                return m_userEmail;
            }
            set
            {
               m_userEmail = value;
            }
        }

        public virtual string Notes
        {
            get
            {
                return m_notes;
            }
            set
            {
                m_notes = value;
            }
        }

        public virtual DateTime? OrderItemStatusDate
        {
            get
            {
                return m_orderItemStatusDate;
            }
            set
            {
                m_orderItemStatusDate = value;
            }
        }

        public virtual int? OrderItemStatusId
        {
            get
            {
                return m_orderItemStatusId;
            }
            set
            {
                m_orderItemStatusId = value;
            }
        }


		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetOrderHistory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetOrderHistoryId.HasValue)
			{
                sb.Criteria.Add("AssetOrderHistoryId=@auditAssetHistoryId");
				sb.AddDataParameter("@auditAssetHistoryId", AssetOrderHistoryId.Value);
			}

            if (AssetId.HasValue)
            {
                sb.Criteria.Add("AssetId=@assetId");
                sb.AddDataParameter("@assetId", AssetId.Value);
            }
            
            if (OrderId.HasValue)
            {
                sb.Criteria.Add("OrderId=@orderId");
                sb.AddDataParameter("@orderId", OrderId.Value);
            }

            if (OrderDate.HasValue)
            {
                sb.Criteria.Add("OrderDate=@orderDate");
                sb.AddDataParameter("@orderDate", OrderDate.Value);
            }

            if (DeadlineDate.HasValue)
            {
                sb.Criteria.Add("DeadlineDate=@deadlineDate");
                sb.AddDataParameter("@deadlineDate", DeadlineDate.Value);
            }

            if (UserEmail != String.Empty)
            {
                sb.Criteria.Add("UserEmail=@userEmail");
                sb.AddDataParameter("@userEmail", UserEmail);
            }						

            if (UserId.HasValue)
            {
                sb.Criteria.Add("UserId=@userId");
                sb.AddDataParameter("@userId", UserId.Value);
            }

            if (UserName != String.Empty)
            {
                sb.Criteria.Add("UserName=@userName");
                sb.AddDataParameter("@userName", UserName);
            }						

            if (Notes != String.Empty)
            {
                sb.Criteria.Add("Notes=@notes");
                sb.AddDataParameter("@notes", Notes);
            }
            
            if (OrderItemStatusDate.HasValue)
            {
                sb.Criteria.Add("OrderItemStatusDate=@orderItemStatusDate");
                sb.AddDataParameter("@orderItemStatusDate", OrderItemStatusDate.Value);
            }

            if (OrderItemStatusId.HasValue)
            {
                sb.Criteria.Add("OrderItemStatusId=@orderItemStatusId");
                sb.AddDataParameter("@orderItemStatusId", OrderItemStatusId);
            }						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}