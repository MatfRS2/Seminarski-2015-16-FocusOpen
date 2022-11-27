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

namespace FocusOPEN.Website.Controls
{
	public class YearDropDownList : AbstractDropDownList
	{
		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			IList<Int32> years = new List<Int32>();

			for (int i = 1875; i < DateTime.Now.Year + 1; i++)
			{
				years.Add(i);
			}

			return years;
		}

		#endregion
	}
}