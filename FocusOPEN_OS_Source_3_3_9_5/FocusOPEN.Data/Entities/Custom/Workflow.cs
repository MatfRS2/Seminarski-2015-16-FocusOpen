/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Workflow.
	/// </summary>
	public partial class Workflow
	{
		private List<WorkflowUser> m_WorkflowUserList;

		public List<WorkflowUser> WorkflowUserList
		{
			get
			{
				if (m_WorkflowUserList == null)
				{
					WorkflowUserFinder finder = new WorkflowUserFinder {WorkflowId = WorkflowId.GetValueOrDefault(-1)};
					m_WorkflowUserList = WorkflowUser.FindMany(finder);
				}

				return m_WorkflowUserList;
			}
		}

		private bool IsWorkflowUserListLoaded
		{
			get
			{
				return (m_WorkflowUserList != null);
			}
		}
	}
}
