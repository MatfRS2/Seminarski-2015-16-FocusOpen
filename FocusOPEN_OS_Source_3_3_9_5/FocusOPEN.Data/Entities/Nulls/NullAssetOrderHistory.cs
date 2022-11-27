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
	public class NullAssetOrderHistory : AssetOrderHistory
	{
		#region Singleton implementation

		private NullAssetOrderHistory()
		{
		}

		private static readonly NullAssetOrderHistory m_instance = new NullAssetOrderHistory();

		public static NullAssetOrderHistory Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors



        /// <summary>
        /// Returns the AssetId of the AssetOrderHistory object.
        /// </summary>
        public override int? AssetId
        {
            get { return 0; }
            set { ; }
        }

        //protected int? m_orderId = null;
        /// <summary>
        /// Returns the OrderId of the AssetOrderHistory object.
        /// </summary>
        public override int? OrderId
        {
            get { return 0; }
            set { ; }
        }

        /// <summary>
        /// Returns the OrderDate of the AssetOrderHistory object.
        /// </summary>
        public override DateTime? OrderDate
        {
            get { return null; }
            set { ; }
        }

        /// <summary>
        /// Returns the DeadlineDate of the AssetOrderHistory object.
        /// </summary>
        public override DateTime? DeadlineDate
        {
            get { return null; }
            set { ; }
        }

        /// <summary>
        /// Returns the UserEmail of the AssetOrderHistory object.
        /// </summary>
        public override string UserEmail
        {
            get { return String.Empty; }
            set { ; }
        }


        /// <summary>
        /// Returns the UserId of the AssetOrderHistory object.
        /// </summary>
        public override int? UserId
        {
            get { return 0; }
            set { ; }
        }

        /// <summary>
        /// Returns the UserName of the AssetOrderHistory object.
        /// </summary>
        public override string UserName
        {
            get { return String.Empty; }
            set { ; }
        }


        /// <summary>
        /// Returns the Notes of the AssetOrderHistory object.
        /// </summary>
        public override string Notes
        {
            get { return String.Empty; }
            set { ; }
        }


        /// <summary>
        /// Returns the OrderItemStatusDate of the AssetOrderHistory object.
        /// </summary>
        public override DateTime? OrderItemStatusDate
        {
            get { return null; }
            set { ; }
        }

        
        /// <summary>
        /// Returns the OrderItemStatus of the AssetOrderHistory object.
        /// </summary>
        public override int? OrderItemStatusId
        {
            get { return null; }
            set { ; }
        }

        
        
        #endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

