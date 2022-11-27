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
using System.IO;
using System.Reflection;
using log4net.Config;

namespace FocusOPEN.APS
{
	public abstract class BaseService
	{
		#region Constructor

		protected BaseService()
		{
			DBHelper.EnsureDatabaseExists();
			ConfigureLog4net();
		}

		#endregion

		#region Private Helper Methods

		private static void ConfigureLog4net()
		{
			// No point configuring if it's done already
			if (log4net.LogManager.GetRepository().Configured)
				return;

			// Get the current directory where we're running the app from
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			// Ensure we manage to retrieve it
			if (string.IsNullOrEmpty(assemblyPath))
			{
				Console.WriteLine("Assembly path is empty");
				return;
			}

			// Construct path to log4net config
			string configPath = Path.Combine(assemblyPath, @"Config\Log4net.config");

			// Ensure config file exists
			if (!File.Exists(configPath))
			{
				Console.WriteLine("Log4net Config path does not exist: " + configPath);
				return;
			}

			// Configure log4net
			XmlConfigurator.ConfigureAndWatch(new FileInfo(configPath));
		}

		#endregion
	}
}