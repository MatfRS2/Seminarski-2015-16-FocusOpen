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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullPlugin : Plugin
	{
		#region Singleton implementation

		private NullPlugin()
		{
		}

		private static readonly NullPlugin m_instance = new NullPlugin();

		public static NullPlugin Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the RegistrationKey of the Plugin object.
		/// </summary>
		public override Guid RegistrationKey
		{
			get { return Guid.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the RelativePath of the Plugin object.
		/// </summary>
		public override string RelativePath
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Filename of the Plugin object.
		/// </summary>
		public override string Filename
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the Plugin object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Checksum of the Plugin object.
		/// </summary>
		public override Nullable <Int32> Checksum
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the PluginType of the Plugin object.
		/// </summary>
		public override Nullable <Int32> PluginType
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDefault of the Plugin object.
		/// </summary>
		public override Nullable <Boolean> IsDefault
		{
			get { return null; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

