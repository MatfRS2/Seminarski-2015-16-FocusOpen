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
	public class NullByteArray : ByteArray
	{
		#region Singleton implementation

		private static NullByteArray m_instance;

		private NullByteArray()
		{
		}

		public static NullByteArray Instance
		{
			get
			{
				return m_instance ?? (m_instance = new NullByteArray());
			}
		}

		#endregion

		public override Byte[] ContentBytes
		{
			get
			{
				return null;
			}
		}

		public override bool HasData
		{
			get
			{
				return false;
			}
		}

		public override object DBValue
		{
			get
			{
				return "NULL";
			}
		}

		public override Boolean IsNull
		{
			get
			{
				return true;
			}
		}

		public override void Fill(Stream stream)
		{
		}

		public override void Fill(Byte[] bytes)
		{
		}
	}
}