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
	/// This object maps data between the database and a WorkflowUser object.
	/// </summary>
	internal partial class WorkflowUserMapper
	{
		#region Singleton behaviour

		private WorkflowUserMapper()
		{
		}
		
		private static readonly WorkflowUserMapper m_instance = new WorkflowUserMapper();
		public static WorkflowUserMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		public void DeleteWorkflowUsers(int workflowId)
		{
			IDbCommand command = CreateCommand();
			command.CommandText = "DELETE FROM [WorkflowUser] WHERE (WorkflowId = @workflowId)";
			command.Parameters.Add(CreateParameter("@workflowId", workflowId));
			ExecuteCommand(command);
		}
	}
}

