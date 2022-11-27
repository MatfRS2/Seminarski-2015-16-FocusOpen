/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections;

namespace FocusOPEN.Shared
{
	public class ErrorList : ArrayList
	{
		#region Private

		private const string m_Separator = "<br />";
		private const string m_Prefix = "*&nbsp;";

		#endregion

		#region Properties

		public bool HasErrors
		{
			get
			{
				return (Count > 0);
			}
		}

		#endregion

		public override string ToString()
		{
			string buf = string.Empty;
			int errorIndex = 1;

			foreach (object nextError in this)
			{
				buf += m_Prefix + nextError;

				if (errorIndex++ < Count)
					buf += m_Separator;
			}

			return buf;
		}

		public static ErrorList operator +(ErrorList list1, ErrorList list2)
		{
			list1.AddRange(list2);
			return list1;
		}
	}
}