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
	public class NullWorkflow : Workflow
	{
		#region Singleton implementation

		private NullWorkflow()
		{
		}

		private static readonly NullWorkflow m_instance = new NullWorkflow();

		public static NullWorkflow Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Name of the Workflow object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the Workflow object.
		/// </summary>
		public override int BrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the Workflow object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

