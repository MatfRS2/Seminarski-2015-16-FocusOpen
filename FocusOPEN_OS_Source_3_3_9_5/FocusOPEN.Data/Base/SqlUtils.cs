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
using System.Data.SqlTypes;

namespace Daydream.Data
{
	internal static class SqlUtils
	{
		public static string SafeDate(DateTime date)
		{
			if (date == DateTime.MinValue)
				date = SqlDateTime.MinValue.Value;

			if (date == DateTime.MaxValue)
				date = SqlDateTime.MaxValue.Value;

			return date.ToString("dd MMMM yyyy HH:mm:ss");
		}

		public static string SafeValue(string val)
		{
			if (val == null)
				return string.Empty;

			return val.Replace("'", "''");
		}

		public static int BitValue(bool flag)
		{
			return (flag) ? 1 : 0;
		}
	}
}