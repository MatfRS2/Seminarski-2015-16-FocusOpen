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
using System.Linq;
using System.ServiceModel;

/* tra la la */

namespace FocusOPEN.APS.ConsoleApp
{
	internal class Program
	{
		private static void Main()
		{
			AssemblyInfo ai = new AssemblyInfo(System.Reflection.Assembly.GetExecutingAssembly());

			Console.WriteLine("****************************************************************************");
			Console.WriteLine(" FocusOPEN Asset Processing Server Command Line Utility");
			Console.WriteLine(" v{0}.  Last Modified: {1}", ai.Version, ai.LastModifiedDate.ToString("dd MMM yyyy HH:mm"));
			Console.WriteLine("****************************************************************************");

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine(" 1. Launch service host");
			Console.WriteLine(" 2. List plugins and settings");
			Console.WriteLine(" 3. List 15 most recent jobs");
			Console.WriteLine(" 4. Delete jobs by asset id");
			Console.WriteLine(" 5. Delete database");
			Console.WriteLine(" 6. Exit");

			Console.WriteLine();
			Console.Write("> ");

			string key = Console.ReadKey().KeyChar.ToString().ToLower();

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();

			if (key == "1")
				SetupServiceHost();

			if (key == "2")
				DisplayPluginInfo();

			if (key == "3")
				ListJobs();

			if (key == "4")
				DeleteJobsByAssetId();

			if (key == "5")
				DeleteDatabase();

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Press <ENTER> to exit");
			Console.ReadLine();
		}

		private static void DeleteJobsByAssetId()
		{
			Console.Write("Please enter the asset id: ");
			string s = Console.ReadLine();

			int assetId;
			
			if (!Int32.TryParse(s, out assetId))
			{
				Console.WriteLine("Invalid asset id");
				return;
			}

			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var jobs = (from job in db.APSQueuedJob
				            where job.AssetId == assetId
				            select job);

				Console.WriteLine(string.Format("Found {0} jobs", jobs.Count()));

				foreach (APSQueuedJob job in jobs)
				{
					db.DeleteObject(job);
					Console.WriteLine("Deleted job with id: " + job.QueuedJobId);
				}

				db.SaveChanges();

				Console.Write("All done");
			}
		}

		private static void DeleteDatabase()
		{
			try
			{
				DBHelper.DeleteDatabase();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error deleting database: " + ex.Message);
			}
		}

		private static void ListJobs()
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var jobs = (from job in db.APSQueuedJob
							orderby job.DateAdded descending
				            select job).Take(15);

				Console.Write("Job ID".PadRight(10));
				Console.Write("Asset ID".PadRight(10));
				Console.Write("Date Added".PadRight(25));
				Console.Write("Status".PadRight(40));
				Console.Write(Environment.NewLine);
				Console.WriteLine("-----------------------------------------------------------------");
				Console.Write(Environment.NewLine);

				foreach (var job in jobs)
				{
					Console.Write(job.QueuedJobId.ToString().PadRight(10));
					Console.Write(job.AssetId.ToString().PadRight(10));
					Console.Write(job.DateAdded.ToString("dd MMM yy HH:mm").PadRight(25));
					//Console.Write(job.Message.PadRight(40));
					Console.Write(job.Status.ToString().PadRight(40));
					Console.Write(Environment.NewLine);
				}
			}
		}

		private static void DisplayPluginInfo()
		{
			Console.WriteLine("Listing all plugins");
			Console.WriteLine();
			Console.WriteLine();

			foreach (var pi in PluginManager.Instance.GetFullPluginList())
			{
				Console.WriteLine(" - Name: " + pi.Name);
				Console.WriteLine(" - Type: " + pi.Type);
				Console.WriteLine(" - Extensions: " + pi.Extensions);
				Console.WriteLine(" - Enabled: " + pi.Enabled);
				Console.WriteLine(" - Settings");

				Console.WriteLine();

				foreach (string kvp in pi.Settings)
					Console.WriteLine("   " + kvp.PadRight(25, ' ') + " : " + pi.Settings.GetValue(kvp));

				Console.WriteLine();
				Console.WriteLine();
			}
		}

		private static void SetupServiceHost()
		{
			ServiceHost serviceHost = null;

			try
			{
				Console.WriteLine("Initializing service host...");
				serviceHost = new ServiceHost(typeof (ProcessingService));
				Console.WriteLine("Done initializing");
				Console.WriteLine();

				Console.Write("Opening...");
				serviceHost.Open();
				Console.Write("Done");

				Console.WriteLine();
				Console.WriteLine();

				Console.Write("Waiting... Press any key to end.");

				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine();

				Console.ReadLine();
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine(ex.ToString());
				Console.WriteLine();
				Console.WriteLine();
			}
			finally
			{
				Console.Write("Closing service host... ");

				try
				{
					if (serviceHost != null && serviceHost.State != CommunicationState.Closed)
						serviceHost.Close();

					Console.Write("Done");
				}
				catch (Exception ex)
				{
					Console.Write(ex.Message);
				}
			}
		}
	}
}