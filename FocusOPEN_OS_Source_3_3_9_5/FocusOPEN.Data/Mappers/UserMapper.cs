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
using System.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object maps data between the database and a User object.
	/// </summary>
	internal partial class UserMapper
	{
		#region Singleton behaviour

		private UserMapper()
		{
		}

		private static readonly UserMapper m_instance = new UserMapper();

		public static UserMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		/// <summary>
		/// Gets the number of bad logins for the specified user, if they have not had
		/// any recent successful logins (After a successful login, this is reset to 0)
		/// </summary>
		public DataRow GetBadLoginInfo(int userId, int minutes)
		{
			DateTime cutoff = DateTime.Now.AddMinutes(minutes - (minutes*2));

			using (IDbCommand command = CreateCommand())
			{
				command.CommandText = "usp_GetBadLoginInfo";
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(CreateParameter("@UserId", userId));
				command.Parameters.Add(CreateParameter("@BadLoginDateCutOff", cutoff));

				return GetDataRow(command);
			}
		}


        public string GenerateUserAPIToken(string email)
        {
            using (IDbCommand command = CreateCommand())
            {
                command.CommandText = "SELECT dbo.GenerateUserAPIToken(@Email)";
                command.Parameters.Add(CreateParameter("@Email", email));
                return (string)ExecScalar(command);
            }
        }


	}
}