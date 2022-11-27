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

namespace FocusOPEN.Website.Components
{
    public static class DateHelper
    {
        public static string GetDateString(DateTime? dateValue, string valueWhenNull)
        {
            return (dateValue.HasValue && dateValue!=DateTime.MinValue ? dateValue.Value.ToString(Global.DateFormat) : valueWhenNull);
        }

        public static string GetDateTimeString(DateTime? dateValue, string valueWhenNull)
        {
            return (dateValue.HasValue && dateValue != DateTime.MinValue ? dateValue.Value.ToString(Global.DateTimeFormat) : valueWhenNull);
        }
    }
}