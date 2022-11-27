/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object maps data between the database and a HomepageCategory object.
	/// </summary>
	internal partial class HomepageCategoryMapper
	{
		#region Singleton behaviour

		private HomepageCategoryMapper()
		{
		}
		
		private static readonly HomepageCategoryMapper m_instance = new HomepageCategoryMapper();
		public static HomepageCategoryMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		public void DeleteHomepageCategories(int homepageId)
		{
			IDbCommand command = CreateCommand();
			command.CommandText = "DELETE FROM [HomepageCategory] WHERE (HomepageId = @homepageId)";
			command.Parameters.Add(CreateParameter("@homepageId", homepageId));
			ExecuteCommand(command);
		}
	}
}

