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

namespace Daydream.Data
{
    [Serializable]
    public class NullDateRange : DateRange
	{
	    #region Singleton implementation

	    private static readonly DateRange m_instance = new NullDateRange ();

	    public static DateRange Instance
	    {
	        get { return m_instance; }
	    }

	    private NullDateRange () {}

	    #endregion
	    
		#region INullable Members

		public override Boolean IsNull
		{
			get { return true; }
		}

		#endregion
	}
}
