/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace Daydream.Data
{
	internal class SingleRowReader : IReader
	{
		private readonly AbstractDataMapper m_DataMapper;

		public SingleRowReader(AbstractDataMapper dataMapper)
		{
			m_DataMapper = dataMapper;
		}

		#region IReader Members

		public object Read(IRowReader reader)
		{
			return m_DataMapper.ReadSingleRow(reader);
		}

		#endregion
	}

	internal class MultiRowReader : IReader
	{
		private readonly AbstractDataMapper m_dataMapper;

		public MultiRowReader(AbstractDataMapper dataMapper)
		{
			m_dataMapper = dataMapper;
		}

		#region IReader Members

		public object Read(IRowReader reader)
		{
			return m_dataMapper.ReadRowSet(reader);
		}

		#endregion
	}
}