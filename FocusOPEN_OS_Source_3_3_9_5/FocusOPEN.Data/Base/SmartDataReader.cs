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
	/// <summary>
	/// SmartDataReader class, takes an IDataReader parameter and allows us to return type safe data
	/// </summary>
	public class SmartDataReader : IRowReader
	{
		private readonly DateTime m_DefaultDate;
		private readonly IDataReader m_reader;

		public object GetFirstField()
		{
			return m_reader[0];
		}

		/// <summary>
		/// Initialises the smart data reader with the specified SqlDataReader
		/// </summary>
		public SmartDataReader(IDataReader reader)
		{
			m_DefaultDate = DateTime.MinValue;
			m_reader = reader;
		}

		/// <summary>
		/// Gets the value from the specified column as a Double
		/// </summary>
		public Double GetDouble(String column)
		{
			double data = 0.0;
			int index = m_reader.GetOrdinal(column);
			if (! m_reader.IsDBNull(index))
				data = Convert.ToDouble(m_reader[column]);
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as a Double
		/// </summary>
		public Decimal GetDecimal(String column)
		{
			decimal data = 0;
			int index = m_reader.GetOrdinal(column);
			if (! m_reader.IsDBNull(index))
				data = Convert.ToDecimal(m_reader[column]);
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as an Int32
		/// </summary>
		public Int32 GetInt32(String column)
		{
			int data = 0;
			int index = m_reader.GetOrdinal(column);
			if (! m_reader.IsDBNull(index))
				data = Convert.ToInt32(m_reader[column]);
			return data;
		}


		/// <summary>
		/// Gets the value from the specified column as an Int64
		/// </summary>
		public Int64 GetInt64(String column)
		{
			int index = m_reader.GetOrdinal(column);
			if (! m_reader.IsDBNull(index))
				return Convert.ToInt64(m_reader[column]);
			return 0;
		}

		/// <summary>
		/// Gets the value from the specified column as an int16
		/// </summary>
		public Int16 GetInt16(String column)
		{
			short data = (m_reader.IsDBNull(m_reader.GetOrdinal(column))) ? (short) 0 : (short) m_reader[column];
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as a byte
		/// </summary>
		public Byte GetByte(String column)
		{
			Byte data = (m_reader.IsDBNull(m_reader.GetOrdinal(column))) ? (byte) 0 : (byte) m_reader[column];
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as a float
		/// </summary>
		public Single GetSingle(String column)
		{
			float data = (m_reader.IsDBNull(m_reader.GetOrdinal(column))) ? 0 : Single.Parse(m_reader[column].ToString());
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as a boolean
		/// </summary>
		public Boolean GetBoolean(String column)
		{
			bool data = (m_reader.IsDBNull(m_reader.GetOrdinal(column))) ? false : (bool) m_reader[column];
			return data;
		}

		public Byte[] GetBytes(String column)
		{
			int i = m_reader.GetOrdinal(column);
			return (m_reader[i] == DBNull.Value) ? new byte[0] : (Byte[]) m_reader.GetValue(i);
		}

		/// <summary>
		/// Gets the value from the specified column as a string
		/// </summary>
		public String GetString(String column)
		{
			String data = (m_reader.IsDBNull(m_reader.GetOrdinal(column)))
			              	? String.Empty
			              	: m_reader[column].ToString();
			return data;
		}

		/// <summary>
		/// Gets the value from the specified column as a DateTime
		/// </summary>
		public DateTime GetDateTime(String column)
		{
			DateTime data = (m_reader.IsDBNull(m_reader.GetOrdinal(column))) ? m_DefaultDate : (DateTime) m_reader[column];
			return data;
		}

		/// <summary>
		/// Read the next value from the SqlDataReader
		/// </summary>
		public bool Read()
		{
			return (m_reader.Read());
		}

		public DateTime? GetNullableDateTime(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return (DateTime) m_reader[columnName];
		}

		public bool? GetNullableBoolean(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return (Boolean) m_reader[columnName];
		}

		public decimal? GetNullableDecimal(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return (Decimal) m_reader[columnName];
		}

		public double? GetNullableDouble(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return (Double) m_reader[columnName];
		}

		public int? GetNullableInt32(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return Convert.ToInt32(m_reader[columnName]);
		}

		public long? GetNullableInt64(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return Convert.ToInt64(m_reader[columnName]);
		}

		public byte? GetNullableByte(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return Convert.ToByte(m_reader[columnName]);
		}

		public float? GetNullableSingle(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return null;
			return Convert.ToSingle(m_reader[columnName]);
		}

		public Guid GetGuid(string columnName)
		{
			if (m_reader.IsDBNull(m_reader.GetOrdinal(columnName)))
				return Guid.Empty;
			return new Guid(m_reader[columnName].ToString());
		}
	}
}