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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FocusOPEN.Shared
{
	public static class EnumUtils
	{
		public static IEnumerable<KeyValuePair<int, string>> GetList<T>() where T:struct
		{
			return GetArray<T>().Select(e => new KeyValuePair<int, string>(Convert.ToInt32(e), GetDescription(e)));
		}

		public static T[] GetArray<T>() where T : struct
		{
			return (T[])Enum.GetValues(typeof(T));
		}

		public static T GetEnumFromValue<T>(int val) where T : struct
		{
			string s = val.ToString();
			return GetEnumFromValue<T>(s);
		}

		public static T GetEnumFromValue<T>(string val) where T : struct
		{
			return (T)Enum.Parse(typeof(T), val, true);
		}

		public static string GetDescription<T>(T en) where T:struct 
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());
			
			if (memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				
				if (attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}

			return GeneralUtils.SplitIntoSentence(en.ToString());
		}

        public static string GetDescription<T>(int val) where T : struct
        {
            var enumVal = GetEnumFromValue<T>(val);

            return GetDescription(enumVal);
        }
	}
}