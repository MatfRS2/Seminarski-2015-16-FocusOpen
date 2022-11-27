/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	public class TimeFrameDropDownList : AbstractDictionaryDropDownList
	{
		#region Private Variables

		private string m_FirstTimeframeName = string.Empty;

		#endregion

		#region Accessors

		public string FirstTimeframeName
		{
			get
			{
				return m_FirstTimeframeName;
			}
			set
			{
				m_FirstTimeframeName = value;
			}
		}

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			Dictionary<int, string> list = EnumUtils.GetList<TimeFrame>().ToDictionary(timeframe => timeframe.Key, timeframe => timeframe.Value);

			if (!StringUtils.IsBlank(FirstTimeframeName) && list.Count > 0)
				list[0] = FirstTimeframeName;

			return list;
		}

		#endregion
	}
}