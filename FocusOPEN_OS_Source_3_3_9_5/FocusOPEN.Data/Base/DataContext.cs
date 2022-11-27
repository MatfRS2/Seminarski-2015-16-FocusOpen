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
	public static class DataContext
	{
		private static IDataFactory m_factory = SqlDataFactory.Instance;
		private static string m_connectionString = String.Empty;
		private static int m_maxOpenConnections = 1000;

		public static int MaxOpenConnections
		{
			get
			{
				return m_maxOpenConnections;
			}
			set
			{
				m_maxOpenConnections = value;
				OnChanged();
			}
		}

		public static IDataFactory Factory
		{
			get
			{
				return m_factory;
			}
			set
			{
				m_factory = value;
				OnChanged();
			}
		}

		public static string ConnectionString
		{
			get
			{
				return m_connectionString;
			}
			set
			{
				m_connectionString = value;
				OnChanged();
			}
		}

		private static void OnChanged()
		{
			if (Changed != null)
				Changed(null, new EventArgs());
		}

		public static event EventHandler Changed = null;
	}
}