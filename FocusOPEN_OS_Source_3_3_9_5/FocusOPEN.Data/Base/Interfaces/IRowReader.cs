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
    public interface IRowReader
    {
        object GetFirstField ();
        Int16 GetInt16 (string columnName);
        Int32 GetInt32 (string columnName);
        Int64 GetInt64 (string columnName);
        Byte GetByte (string columnName);
        Double GetDouble (string columnName);
        Single GetSingle(string columnName);
        String GetString (string columnName);
        Boolean GetBoolean (string columnName);
        DateTime GetDateTime (string columnName);
        Decimal GetDecimal (string columnName);
        Byte[] GetBytes (String column);
        
        DateTime? GetNullableDateTime (string columnName);
        int? GetNullableInt32(string columnName);
        long? GetNullableInt64(string columnName);
        byte? GetNullableByte(string columnName);
        bool? GetNullableBoolean (string columnName);
        decimal? GetNullableDecimal (string columnName);
        double? GetNullableDouble (string columnName);
        float? GetNullableSingle(string columnName);

        Guid GetGuid (string columnName);
        Boolean Read ();
    }
}