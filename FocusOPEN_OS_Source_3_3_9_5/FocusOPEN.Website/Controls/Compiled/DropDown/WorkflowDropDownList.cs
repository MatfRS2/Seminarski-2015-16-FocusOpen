/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class WorkflowDropDownList : AbstractDropDownList
	{
		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			// Initialize finder
			WorkflowFinder finder = new WorkflowFinder();

			// Non super-admins can only see workflows in their brand
			if (SessionInfo.Current.User.UserRole != UserRole.SuperAdministrator)
				finder.BrandId = SessionInfo.Current.User.PrimaryBrandId;

			// Sort and return
    		finder.SortExpressions.Add(new AscendingSort(Workflow.Columns.Name));
			return Workflow.FindMany(finder);
		}

		public override string GetDataTextField()
		{
			return Workflow.Columns.Name.ToString();
		}

		public override string GetDataValueField()
		{
			return Workflow.Columns.WorkflowId.ToString();
		}

		#endregion
	}
}