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
using System.IO;

namespace Daydream.Data
{
	public class ByteArray : INullable
	{
		private byte[] m_contentBytes = null;
		private int m_contentLength = 0;

		public virtual Byte[] ContentBytes
		{
			get { return m_contentBytes; }
		}

		public virtual bool HasData
		{
			get { return m_contentLength > 0; }
		}

		public virtual void Fill (Stream stream)
		{
			long originalPosition = stream.Position;

			m_contentBytes = new byte[stream.Length];

			int offset = 0;
			int remaining = m_contentBytes.Length;

			while (remaining > 0)
			{
				int read = stream.Read(m_contentBytes, offset, remaining);
				
				if (read <= 0)
					throw new EndOfStreamException(String.Format("End of stream reached with {0} bytes left to read", remaining));

				remaining -= read;
				offset += read;
			}

			m_contentLength = m_contentBytes.Length;
			
			stream.Seek(originalPosition, SeekOrigin.Begin); 
		}

		public virtual void Fill (Byte[] bytes)
		{
			m_contentBytes = bytes; 
			m_contentLength = bytes.Length;
		}

		private ByteArray(Stream inputStream)
		{
			Fill (inputStream);
		}

		protected ByteArray() {}

		public virtual object DBValue
		{
			get
			{
				if (m_contentLength == 0)
					return "NULL";

				return m_contentBytes;
			}
		}

		#region Static operations

		public static ByteArray Empty
		{
			get { return NullByteArray.Instance; }
		}

		public static ByteArray New ()
		{
			return new ByteArray() ; 
		}

		public static ByteArray New (Stream inputStream)
		{
			return new ByteArray(inputStream); 
		}

		#endregion

		#region INullable implementation

		public virtual Boolean IsNull
		{
			get { return false; }
		}

		#endregion
	}
}
