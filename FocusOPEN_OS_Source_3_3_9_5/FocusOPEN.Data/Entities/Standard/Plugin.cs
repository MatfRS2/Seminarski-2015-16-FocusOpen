/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using Daydream.Data;

namespace FocusOPEN.Data
{
	public partial class Plugin
	{
		#region Constructor
		
		protected Plugin()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static Plugin New ()
		{
			return new Plugin() ;
		}

		public static Plugin Empty
		{
			get { return NullPlugin.Instance; }
		}

		public static Plugin Get (Nullable <Int32> PluginId)
		{
			Plugin Plugin = PluginMapper.Instance.Get (PluginId);
			return Plugin ?? Empty;
		}

		public static Plugin Update (Plugin plugin)
		{
			return PluginMapper.Instance.Update(plugin) ;
		}

		public static void Delete (Nullable <Int32> PluginId)
		{
			PluginMapper.Instance.Delete (PluginId);
		}

		public static EntityList <Plugin> FindMany (PluginFinder finder, int Page, int PageSize)
		{
			return PluginMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Plugin> FindMany (PluginFinder finder)
		{
			return PluginMapper.Instance.FindMany (finder);
		}

		public static Plugin FindOne (PluginFinder finder)
		{
			Plugin Plugin = PluginMapper.Instance.FindOne(finder);
			return Plugin ?? Empty;
		}

		public static int GetCount (PluginFinder finder)
		{
			return PluginMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
