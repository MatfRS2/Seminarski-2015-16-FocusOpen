/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Data
{
	internal partial class PluginMapper
	{
		#region Singleton behaviour

		private PluginMapper()
		{
		}
		
		private static PluginMapper m_instance = new PluginMapper();
		public static PluginMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion
	}
}

