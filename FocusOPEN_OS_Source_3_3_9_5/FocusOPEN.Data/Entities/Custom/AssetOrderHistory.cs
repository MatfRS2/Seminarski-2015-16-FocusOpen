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
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AuditAssetHistory.
	/// </summary>
	public partial class AssetOrderHistory
	{
        #region Lazy Loads

        private User m_User;

        public virtual User User
        {
            get
            {
                if (m_User == null && UserId.HasValue)
                    m_User = User.Get(UserId.Value);

                return (m_User);
            }
        }

        private string m_approvedYesNo = String.Empty;
        public virtual string ApprovedYesNo
        {
            get
            {
                string result = "No";
                if (OrderItemStatusId.HasValue)
                {
                    switch (EnumUtils.GetEnumFromValue<OrderItemStatus>(OrderItemStatusId.Value))
                    {
                        case OrderItemStatus.Approved:
                        case OrderItemStatus.Preapproved:
                            result = "Yes";
                            break;

                    }
                }
                return result;
            }
        }

        #endregion
    }
}