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
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using FocusOPEN.Business.Scripts;

namespace FocusOPEN.Website
{
	public class Global : HttpApplication
	{
		#region Fields

		private static readonly ILog m_Logger = LogManager.GetLogger(typeof(Global));

		public const string DateTimeFormat = "dd MMMM yyyy HH:mm";
		public const string DateFormat = "dd MMMM yyyy";
        #endregion

		#region Properties

		/// <summary>
		/// Boolean value specifying whether public registration is allowed
		/// </summary>
		public static bool AllowPublicRegistration { get; private set; }

		/// <summary>
		/// Boolean value specifying whether raw errors should be displayed to users
		/// </summary>
		private static bool ShowRawErrors { get; set; }

		#endregion

		#region Standard application events

		protected void Session_Start(object sender, EventArgs e)
		{
			//m_Logger.DebugFormat("Session_Start : SessionId: {0}", Session.SessionID);
		}

		protected void Session_End(object sender, EventArgs e)
		{
			// The Session_End event is only fired when using the InProc state management,
			// not with the StateServer or SqlStateServer.  However, we have this code here
			// for consistency sake (in case we need to change back to using inproc)

			m_Logger.DebugFormat("Session_End : SessionId: {0}", Session.SessionID);
			SessionHelper.GetForSession(Session).DeleteSessionTempFolder();
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			m_Logger.Debug("Application started");

			// Wire up the SessionEnd module
			SessionEndModule.SessionObjectKey = SessionInfo.SessionKey;
			SessionEndModule.SessionEnd += new SessionEndEventHandler(SessionEndModule_SessionEnd);
			m_Logger.Debug("Wired up session end module");

			// Load the config settings now
			LoadConfigSettings();

			// Initialise the managers
			IntialiseManagers();

			// Check Paths
			CheckFilePaths();
		}

		protected void Application_End(object sender, EventArgs e)
		{
			m_Logger.Debug("Application ended");

			HttpRuntime runtime = (HttpRuntime) typeof (HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);

			if (runtime != null)
			{
				string shutDownMessage = (string) runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
				m_Logger.WarnFormat("Application shutdown - message: {0}", shutDownMessage);
			}

			// Shutdown the log
			LogManager.Shutdown();
		}

		protected void Application_OnError(object sender, EventArgs e)
		{
			// Get the last error
			Exception exception = Server.GetLastError();

			// Deal with 404 errors. These don't need to be logged
			// as they're a common problem.
			if (exception is HttpException)
			{
				HttpException hex = (HttpException) exception;

				if (hex.GetHttpCode() == 404)
				{
					Response.Redirect("~/Errors/PageNotFound.htm", false);
					Server.ClearError();
					return;
				}
			}

			// Drop out for exceptions that should be ignored
			if (IgnoreException(exception))
			{
				m_Logger.Warn(string.Format("Ignored exception: {0}", exception.Message), exception);
				Server.ClearError();
				return;
			}

			// Log the exception to the log file
			m_Logger.Fatal("Unhandled application error", exception);


#if DEBUG
			return;
#else

			// Log in exception handler for non-local errors (emails error details to admins)
			if (!Request.IsLocal)
				ExceptionHandler.HandleException(exception, "Unhandled application error");

#endif


            // For local requests and engineers, don't bother with the handled error page
			if (Request.IsLocal || SessionInfo.Current.User.IsEngineer)
				return;

			// Redirect to friendly error page if required
			if (!ShowRawErrors)
			{
				Response.Redirect("~/Errors/HandledError.htm", false);
				Server.ClearError();
			}
		}

		/// <summary>
		/// Checks if an exception should be ignored
		/// </summary>
		/// <param name="exception">The exception to be checked</param>
		/// <returns>[True] if exception should be ignored, otherwise [False]</returns>
		private static bool IgnoreException(Exception exception)
		{
			if (exception.Message.ToLower().Contains("client disconnected after receiving"))
				return true;

			return false;
		}

		#endregion

		#region Additional events

		private static void SessionEndModule_SessionEnd(object sender, SessionEndedEventArgs e)
		{
			m_Logger.Debug("SessionEndModule_SessionEnd : " + e.SessionId);

			SessionInfo sessionInfo = e.SessionObject as SessionInfo;
			SessionHelper.GetForSessionInfo(sessionInfo).DeleteSessionTempFolder();
		}

		#endregion

		#region Private helper methods

		private static void CheckFolderExists(string folder)
		{
			if (!Directory.Exists(folder))
			{
				m_Logger.ErrorFormat("The folder does not exist: {0}", folder);
				throw new SystemException(string.Format("Folder '{0}' does not exist", folder));
			}
		}

		private static void CheckFileExists(string path)
		{
			if (!File.Exists(path))
			{
				m_Logger.ErrorFormat("The file does not exist: {0}", path);
				throw new SystemException(string.Format("File '{0}' does not exist", path));
			}
		}

		private static void CheckFilePaths()
		{
			CheckFolderExists(AssetFilePathManager.GetDefault().Path);
			CheckFolderExists(Settings.TempFolder);
			CheckFolderExists(Settings.CachedAssetFilesFolder);
			CheckFolderExists(HomepageImageManager.HomepageImagesFolder);
			CheckFolderExists(Settings.ImageFolder);
			CheckFileExists(ImageUtils.ImageMagickConvert);
			CheckFileExists(ImageUtils.ImageMagickComposite);
			CheckFileExists(ImageUtils.ImageMagickIdentify);
			CheckFileExists(ImageUtils.ColorspaceProfileCMYK);
			CheckFileExists(ImageUtils.ColorspaceProfileRGB);
		}

		/// <summary>
		/// Initialise managers (in business layer) and hook up events
		/// </summary>
		private void IntialiseManagers()
		{
			//-----------------------------------------------------------------
			// Initialise asset manager
			//-----------------------------------------------------------------
			AssetManager.AssetDelegated += new AssetDelegatedEventHandler(NotifyEngine.AssetDelegated);

			//-----------------------------------------------------------------
			// Initialise user manager
			//-----------------------------------------------------------------
			UserManager.UserCreatedByAdmin += new UserEventHandler(NotifyEngine.UserCreatedByAdmin);
			UserManager.UnapprovedUserCreated += new UserEventHandler(NotifyEngine.UnapprovedUserCreated);
			UserManager.PreApprovedUserCreated += new UserEventHandler(NotifyEngine.PreApprovedUserCreated);
			UserManager.PasswordResetRequested += new UserEventHandler(NotifyEngine.PasswordResetRequested);
			UserManager.NonEmployeeProcessed += new UserEventHandler(NotifyEngine.NonEmployeeProcessed);

			//-----------------------------------------------------------------
			// Initialise the lightbox manager
			//-----------------------------------------------------------------
			LightboxManager.LightboxSentToUser += new LightboxSentEventHandler(NotifyEngine.LightboxSentToUser);

			//-----------------------------------------------------------------
			// Initialise the order manager
			//-----------------------------------------------------------------
			OrderManager.OrderCreated += new OrderEventHandler(NotifyEngine.OrderCreated);
			OrderManager.OrderCompleted += new OrderEventHandler(NotifyEngine.OrderCompleted);
			OrderManager.OrderItemsProcessed += new OrderItemsEventHandler(NotifyEngine.OrderItemsProcessed);
			OrderManager.NewAdminOrderItemComment += new OrderItemCommentEventHandler(NotifyEngine.NewAdminOrderItemComment);
			OrderManager.NewUserOrderItemComment += new OrderItemCommentEventHandler(NotifyEngine.NewUserOrderItemComment);

			//-----------------------------------------------------------------
			// Initialise the workflow manager
			//-----------------------------------------------------------------
			WorkflowManager.AssetSubmittedToWorkflow += new AssetEventHandler(NotifyEngine.AssetSubmittedToWorkflow);
			WorkflowManager.AssetWorkflowUserSelected += new AssetWorkflowUserHandler(NotifyEngine.AssetWorkflowUserSelected);
			WorkflowManager.AssetWorkflowCommenterInvited += new AssetWorkflowCommenterHandler(NotifyEngine.AssetWorkflowCommenterInvited);
			WorkflowManager.AssetWorkflowCommenterUpdated += new AssetWorkflowCommenterHandler(NotifyEngine.AssetWorkflowCommenterUpdated);
			WorkflowManager.AssetWorkflowRejected += new AssetWorkflowRejectedEventHandler(NotifyEngine.AssetWorkflowRejected);
			WorkflowManager.AssetWorkflowComplete += new AssetWorkflowEventHandler(NotifyEngine.AssetWorkflowComplete);
			WorkflowManager.AssetWorkflowCancelled += new AssetWorkflowEventHandler(NotifyEngine.AssetWorkflowCancelled);

			//-----------------------------------------------------------------
			// Configure the mimetype manager
			//-----------------------------------------------------------------
			MimeTypeManager.ConfigurationPath = Server.MapPath("~/App_Data/MimeTypes.Config");

			//-----------------------------------------------------------------
			// Configure the asset type information
			//-----------------------------------------------------------------
			AssetTypeInfo.ConfigurationPath = Server.MapPath("~/Config/AssetPropertyGroups.Config");

            //-----------------------------------------------------------------
            // Configure the asset file source paths
            //-----------------------------------------------------------------
            AssetSourcePaths.ConfigurationPath = Server.MapPath("~/Config/AssetSourcePaths.Config");

			//-----------------------------------------------------------------
			// Configure the asset bitmap groups
			//-----------------------------------------------------------------
			AssetBitmapGroupManager.ConfigurationPath = Server.MapPath("~/Config/AssetBitmapGroups.Config");

			//-----------------------------------------------------------------
			// Configure cookie manager
			//-----------------------------------------------------------------
			CookieManager.CookieName = "FocusOPEN";
			CookieManager.ExpirationHours = 14*24;

			//-----------------------------------------------------------------
			// Configure APS Gateway
			//-----------------------------------------------------------------
			APSGateway.Instance.BrandsPath = Server.MapPath("~/Brands/");

			//-----------------------------------------------------------------
			// Configure UserQueryParser Noise Words
			//-----------------------------------------------------------------
			ConfigureNoiseWords();

			m_Logger.Info("Configured managers");
		}

		/// <summary>
		/// Configures the user query parser noise words
		/// </summary>
		private static void ConfigureNoiseWords()
		{
			string path = HttpContext.Current.Server.MapPath("~/Config/NoiseWords.Config");
			using (StreamReader sr = File.OpenText(path))
			{
				while (!sr.EndOfStream)
				{
					UserQueryParser.NoiseWords.Add(sr.ReadLine());
				}
				sr.Close();
			}

			m_Logger.Info("Loaded noise words");
		}

		/// <summary>
		/// Reloads all settings from config file into relevant objects
		/// </summary>
		internal static void LoadConfigSettings()
		{
			//-----------------------------------------------------------------
			// Initialise data context
			//-----------------------------------------------------------------
			DataContext.ConnectionString = ConfigurationManager.AppSettings.GetValue("ConnectionString");

			if (StringUtils.IsBlank(DataContext.ConnectionString))
				throw new SystemException("ConnectionString cannot be blank");

			//-----------------------------------------------------------------
			// Initialise email
			//-----------------------------------------------------------------
			Email.EngineEnabled = ConfigurationManager.AppSettings.GetBoolValue("Email.EngineEnabled");
			Email.BccEmail = ConfigurationManager.AppSettings.GetValue("Email.BccEmail");
			Email.DebugEmail = ConfigurationManager.AppSettings.GetValue("Email.DebugEmail");
			Email.MailServer = ConfigurationManager.AppSettings.GetValue("Email.MailServer");
			Email.TemplatePath = HttpContext.Current.Server.MapPath("~/Templates/Email/");
			Email.DefaultBodyIsHtml = true;

			// Write a warning to the log if email engine is disabled
			if (!Email.EngineEnabled)
				m_Logger.Warn("Email engine is disabled.  No emails will be sent");


			//-----------------------------------------------------------------
			// Initialise error
			//-----------------------------------------------------------------
			ExceptionHandler.FromEmail = ConfigurationManager.AppSettings.GetValue("Error.FromEmail");
			ExceptionHandler.ToEmail = ConfigurationManager.AppSettings.GetValue("Error.ToEmail");
			ExceptionHandler.Subject = ConfigurationManager.AppSettings.GetValue("Error.Subject");


			//-----------------------------------------------------------------
			// Initialise the file manager
			//-----------------------------------------------------------------
			HomepageImageManager.HomepageImagesFolder = Path.Combine(ConfigurationManager.AppSettings.GetValue("DataFolder"), "Homepage");
			AssetFileManager.ZipAssetFiles = ConfigurationManager.AppSettings.GetBoolValue("ZipAssetFiles");


			//-----------------------------------------------------------------
			// Set up folders
			//-----------------------------------------------------------------
			Settings.TempFolder = Path.Combine(Path.GetTempPath(), "FocusOPEN");
			Settings.CachedAssetFilesFolder = Path.Combine(ConfigurationManager.AppSettings.GetValue("DataFolder"), "CachedAssetFiles");
			Settings.ImageFolder = HttpContext.Current.Server.MapPath("~/Images/");


			//-----------------------------------------------------------------
			// Initialise the login manager
			//-----------------------------------------------------------------
			LoginManager.BadLoginCountThreshold = ConfigurationManager.AppSettings.GetIntValue("LoginManager.BadLoginCountThreshold", 5);
			LoginManager.BadLoginLockoutMinutes = ConfigurationManager.AppSettings.GetIntValue("LoginManager.BadLoginLockoutMinutes", 5);


			//-----------------------------------------------------------------
			// Initialise the user manager
			//-----------------------------------------------------------------
			UserManager.AccountExpiryDays = ConfigurationManager.AppSettings.GetIntValue("UserManager.AccountExpiryDays", 90);
			UserManager.PasswordExpiryDays = ConfigurationManager.AppSettings.GetIntValue("UserManager.PasswordExpiryDays", 30);

            //Default value.
            UserManager.RegistrationEmailFormat = RegistrationEmailFormatType.InternalUsers;

            if (ConfigurationManager.AppSettings["RegisterUserRequiresKnownEmailFormat"] != null)
            {
                if (ConfigurationManager.AppSettings.GetValue("RegisterUserRequiresKnownEmailFormat").Trim() == string.Empty)
                {
                    UserManager.RegistrationEmailFormat = RegistrationEmailFormatType.Empty;
                }
                else
                { 
                    try
                    {
                        UserManager.RegistrationEmailFormat = (RegistrationEmailFormatType)Enum.Parse(
                            typeof(RegistrationEmailFormatType),
                            ConfigurationManager.AppSettings.GetValue("RegisterUserRequiresKnownEmailFormat"),
                            true);
                    }
                    catch(Exception ex){}                
                }
            }

			//-----------------------------------------------------------------
            // Initialise the lightbox manager
            //-----------------------------------------------------------------
            LightboxManager.MaxNumberCCEmails = ConfigurationManager.AppSettings.GetIntValue("LightboxManager.MaxNumberCCEmails", 20);
        
			//-----------------------------------------------------------------
            // Initialise the plugin manager
            //-----------------------------------------------------------------
            PluginManager.PluginsPath =  ConfigurationManager.AppSettings.GetValue("PluginsPath");
          
            //-----------------------------------------------------------------
            // Initialise the script engine 
            //-----------------------------------------------------------------
            ScriptEngine.ScriptsPath = ConfigurationManager.AppSettings.GetValue("ScriptsPath");
            ScriptEngine.APILogFolder = ConfigurationManager.AppSettings.GetValue("APILogFolder");
            ScriptEngine.APIRestrictAccounts = ConfigurationManager.AppSettings.GetValue("APIRestrictAccounts");
            if(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["APIObeyIPAddressRestrictions"]))
            {
                ScriptEngine.APIObeyIpAddressRestrictions = ConfigurationManager.AppSettings.GetBoolValue("APIObeyIPAddressRestrictions");
            }


			//-----------------------------------------------------------------
			// Configure Image Utils
			//-----------------------------------------------------------------
			ImageUtils.DEFAULT_WATERMARK = HttpContext.Current.Server.MapPath("~/Images/Logo.png");
			ImageUtils.ImageMagickConvert = ConfigurationManager.AppSettings.GetValue("ImageUtils.ImageMagickConvert");
			ImageUtils.ImageMagickComposite = ConfigurationManager.AppSettings.GetValue("ImageUtils.ImageMagickComposite");
			ImageUtils.ImageMagickIdentify = ConfigurationManager.AppSettings.GetValue("ImageUtils.ImageMagickIdentify");
			ImageUtils.ColorspaceProfileCMYK = ConfigurationManager.AppSettings.GetValue("ImageUtils.ColorspaceProfileCMYK");
			ImageUtils.ColorspaceProfileRGB = ConfigurationManager.AppSettings.GetValue("ImageUtils.ColorspaceProfileRGB");

			
			//-----------------------------------------------------------------
			// Configure asset finder
			//-----------------------------------------------------------------
			AssetFinder.FullTextSearchedEnabled = ConfigurationManager.AppSettings.GetBoolValue("AssetFinder.FullTextSearchedEnabled");
			AssetFinder.FileIndexingEnabled = ConfigurationManager.AppSettings.GetBoolValue("AssetFinder.FileIndexingEnabled");
			AssetFinder.FileContentSearchingEnabled = ConfigurationManager.AppSettings.GetBoolValue("AssetFinder.FileContentSearchingEnabled");


			//-----------------------------------------------------------------
			// Configure External Publisher
			//-----------------------------------------------------------------
			ExternalPublisher.Instance.Folder = ConfigurationManager.AppSettings.GetValue("ExternalStoragePath");
			ExternalPublisher.Instance.Prefix = "FocusOPEN";


			//-----------------------------------------------------------------
			// Other
			//-----------------------------------------------------------------
			APSGateway.Instance.OverrideCallbackUrl = ConfigurationManager.AppSettings.Get("OverrideCallbackUrl");
			m_Logger.DebugFormat("APSGateway OverrideCallbackUrl: {0}", APSGateway.Instance.OverrideCallbackUrl);

			SSLModule.UseSSL = ConfigurationManager.AppSettings.GetBoolValue("UseSSL");
			m_Logger.DebugFormat("SSL Enabled: {0}", SSLModule.UseSSL);

			SearchResults.ShowFiltersCount = ConfigurationManager.AppSettings.GetIntValue("SearchResults.ShowFiltersCount", 0);
			m_Logger.DebugFormat("ShowFiltersCount: {0}", SearchResults.ShowFiltersCount);

			AllowPublicRegistration = ConfigurationManager.AppSettings.GetBoolValue("AllowPublicRegistration");
			m_Logger.DebugFormat("AllowPublicRegistration: {0}", AllowPublicRegistration);

			Settings.IpAddressRestrictionEnabled = ConfigurationManager.AppSettings.GetBoolValue("IpAddressRestrictionEnabled");
			m_Logger.DebugFormat("IpAddressRestrictionEnabled : {0}", Settings.IpAddressRestrictionEnabled);

            Settings.NumberOfMetadataFields = ConfigurationManager.AppSettings.GetIntValue("NumberOfMetadataFields", 50);
			m_Logger.DebugFormat("NumberOfMetadataFields : {0}", Settings.NumberOfMetadataFields);
            
			Settings.MaxBulkDeleteAllowed = ConfigurationManager.AppSettings.GetIntValue("MaxBulkDeleteAllowed", 50);
			m_Logger.DebugFormat("MaxBulkDeleteAllowed : {0}", Settings.MaxBulkDeleteAllowed);

            Settings.MaxSelectableMetadataDepth = ConfigurationManager.AppSettings.GetIntValue("MaxSelectableMetadataDepth", 10);
            m_Logger.DebugFormat("MaxSelectableMetadataDepth : {0}", Settings.MaxSelectableMetadataDepth);


			ShowRawErrors = ConfigurationManager.AppSettings.GetBoolValue("Error.ShowRawErrors");
			m_Logger.DebugFormat("ShowRawErrors: {0}", ShowRawErrors);

			UpdateLog4netConfig();

			m_Logger.Info("Loaded config settings");
		}

		private static void UpdateLog4netConfig()
		{
			Hierarchy root = LogManager.GetRepository() as Hierarchy;

			if (root == null)
				return;

			SmtpAppender sa = root.Root.GetAppender("SmtpAppender") as SmtpAppender;

			if (sa == null)
				return;

			sa.To = ExceptionHandler.ToEmail;
			sa.From = ExceptionHandler.FromEmail;
			sa.ActivateOptions();

			m_Logger.DebugFormat("Updated log4net SmtpAppender to address: {0}", sa.To);
		}

		#endregion
	}
}