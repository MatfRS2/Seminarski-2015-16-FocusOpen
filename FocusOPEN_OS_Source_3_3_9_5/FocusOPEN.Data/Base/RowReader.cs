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
using System.Data;

namespace Daydream.Data
{
	public class RowReader : IRowReader
	{
		#region Private Variables

		private readonly DataRow m_Row;

		#endregion

		#region Constructor

		public RowReader(DataRow dataRow)
		{
			m_Row = dataRow;
		}

		#endregion

		#region IRowReader Members

		public object GetFirstField()
		{
			return m_Row[0];
		}

		public Byte[] GetBytes(String fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value) ? new byte[0] : (Byte[]) m_Row[fieldName];
		}

		/// <summary>
		/// Gets the value from the specified column as a Double
		/// </summary>
		public Double GetDouble(string fieldName)
		{
			return
				(m_Row[fieldName] == DBNull.Value)
					? 0.0
					: Convert.ToDouble(m_Row[fieldName]);
		}

		/// <summary>
		/// Gets the value from the specified column as a Float
		/// </summary>
		public Single GetSingle(string fieldName)
		{
			return
				(m_Row[fieldName] == DBNull.Value)
					? 0.0F
					: Convert.ToSingle(m_Row[fieldName]);
		}

		/// <summary>
		/// Gets the value from the specified column as a Decimal
		/// </summary>
		public Decimal GetDecimal(string fieldName)
		{
			return
				(m_Row[fieldName] == DBNull.Value)
					? 0
					: Convert.ToDecimal(m_Row[fieldName]);
		}

		public Byte GetByte(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? (byte) 0
			       	: Convert.ToByte(m_Row[fieldName]);
		}

		public Int32 GetInt32(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? 0
			       	: Convert.ToInt32(m_Row[fieldName]);
		}

		public Int64 GetInt64(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? 0
			       	: Convert.ToInt64(m_Row[fieldName]);
		}

		public String GetString(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? String.Empty
			       	: Convert.ToString(m_Row[fieldName]);
		}

		public Boolean GetBoolean(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? false
			       	: Convert.ToBoolean(m_Row[fieldName]);
		}

		public DateTime GetDateTime(string fieldName)
		{
			return (m_Row[fieldName] == DBNull.Value)
			       	? DateTime.MinValue
			       	: Convert.ToDateTime(m_Row[fieldName]);
		}

		public int? GetNullableInt32(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToInt32(o);
		}

		public long? GetNullableInt64(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToInt64(o);
		}


		public byte? GetNullableByte(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToByte(o);
		}

		public DateTime? GetNullableDateTime(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToDateTime(o);
		}

		public bool? GetNullableBoolean(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToBoolean(o);
		}

		public decimal? GetNullableDecimal(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToDecimal(o);
		}

		public double? GetNullableDouble(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToDouble(o);
		}

		public float? GetNullableSingle(string fieldName)
		{
			object o = m_Row[fieldName];
			if (o == DBNull.Value)
				return null;
			return Convert.ToSingle(o);
		}

		public Int16 GetInt16(string fieldName)
		{
			short result = 0;
			if (m_Row[fieldName] != DBNull.Value)
				result = Convert.ToInt16(m_Row[fieldName]);
			return result;
		}

		public Guid GetGuid(string fieldName)
		{
			if (m_Row[fieldName] != DBNull.Value)
				return new Guid(m_Row[fieldName].ToString());
			return Guid.Empty;
		}

		public Boolean Read()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}