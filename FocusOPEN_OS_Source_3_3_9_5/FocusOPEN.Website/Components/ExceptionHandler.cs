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
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using Daydream.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public static class ExceptionHandler
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Private variables

		private static string m_FromEmail;
		private static string m_ToEmail;
		private static string m_Subject = "An unhandled exception has occured";

		private const string TEMPLATE_ROW = "<tr><td valign=\"top\"><strong>#name#:</strong></td><td valign=\"top\">#value#</td></tr>";
		private const string TEMPLATE_ROW_EMPTY = "<tr><td valign=\"top\">#value#<td></tr>";
		private const string TEMPLATE_ROW_HEADER = "<tr><td valign=\"top\" colspan=\"2\" bgcolor=\"#000000\"><font size=\"5\" color=\"#ffffff\">#header#</font></td></tr>";

		#endregion

		#region Accessors

		public static string FromEmail
		{
			get
			{
				return m_FromEmail;
			}
			set
			{
				m_FromEmail = value;
			}
		}

		public static string ToEmail
		{
			get
			{
				return m_ToEmail;
			}
			set
			{
				m_ToEmail = value;
			}
		}

		public static string Subject
		{
			get
			{
				return m_Subject;
			}
			set
			{
				m_Subject = value;
			}
		}

		#endregion

		#region Exception Handling Methods

		/// <summary>
		/// Handled the exception
		/// </summary>
		/// <param name="ex">The exception to be handled</param>
		public static void HandleException(Exception ex)
		{
			HandleException(ex, string.Empty);
		}

		/// <summary>
		/// Handle the exception
		/// </summary>
		/// <param name="ex">The exception to be handled</param>
		/// <param name="message">An additional message to be included in the error email body</param>
		public static void HandleException(Exception ex, string message)
		{
			if (!StringUtils.IsBlank(ToEmail))
			{
				try
				{
					Email email = Email.Create();

					email.FromEmail = FromEmail;

					foreach (string recipient in ToEmail.Split(';'))
						if (StringUtils.IsEmail(recipient))
							email.Recipients.Add(recipient);

					email.Subject = Subject;
					email.Body = GetBody(ex, message);
					email.IsHtml = true;
					email.IsDebugMode = false;

					email.Send();
				}
				catch (Exception hex)
				{
					m_Logger.Warn("Error handling error", hex);
				}
			}
		}

		#endregion

		#region Private Methods

		private static string GetBody(Exception ex, string message)
		{
			StringBuilder sb = new StringBuilder();

			string tempRow;

			// Page title
			sb.Append("<h1>Page Error</h1>");

			// Open table
			sb.Append("<table border='0' cellpadding='5' cellspacing='0'>");
			
			// Open General Details section
			sb.Append(TEMPLATE_ROW_HEADER.Replace("#header#", "General Details"));

			// Custom message
			if (!StringUtils.IsBlank(message))
			{
				tempRow = TEMPLATE_ROW;
				tempRow = tempRow.Replace("#name#", "Message");
				tempRow = tempRow.Replace("#value#", message);
				sb.Append(tempRow);
			}

			// IP Address
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "Remote IP Adddress");
			tempRow = tempRow.Replace("#value#", HttpContext.Current.Request.UserHostAddress);
			sb.Append(tempRow);

			// User Agent
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "User Agent");
			tempRow = tempRow.Replace("#value#", HttpContext.Current.Request.UserAgent);
			sb.Append(tempRow);

			// Page
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "Page");
			tempRow = tempRow.Replace("#value#", HttpContext.Current.Request.Url.AbsoluteUri);
			sb.Append(tempRow);

			// Referer
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "Referer");
			tempRow = tempRow.Replace("#value#", GetReferer());
			sb.Append(tempRow);

			// Time
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "Time");
			tempRow = tempRow.Replace("#value#", DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss"));
			sb.Append(tempRow);

			if (ex == null)
			{
				tempRow = TEMPLATE_ROW;
				tempRow = tempRow.Replace("#name#", "Details");
				tempRow = tempRow.Replace("#value#", "(No exception details available)");
				sb.Append(tempRow);
			}
			else
			{
				// Error Details
				tempRow = TEMPLATE_ROW;
				tempRow = tempRow.Replace("#name#", "Details");
				tempRow = tempRow.Replace("#value#", ex.ToString());
				sb.Append(tempRow);

				// Inner exception details
				if (ex.InnerException != null)
				{
					tempRow = TEMPLATE_ROW;
					tempRow = tempRow.Replace("#name#", "Inner Exception Details");
					tempRow = tempRow.Replace("#value#", ex.InnerException.ToString());
					sb.Append(tempRow);

					if (ex.InnerException is AbstractDataMapperException)
					{
						AbstractDataMapperException admex = (AbstractDataMapperException) ex.InnerException;

						sb.Append(TEMPLATE_ROW_HEADER.Replace("#header#", "SQL Exception Details"));

						tempRow = TEMPLATE_ROW;
						tempRow = tempRow.Replace("#name#", "Sql Statement");
						tempRow = tempRow.Replace("#value#", admex.DbCommand.CommandText);
						sb.Append(tempRow);

						StringBuilder paramsSb = new StringBuilder();

						foreach (IDataParameter param in admex.DbCommand.Parameters)
							paramsSb.AppendFormat("{0} = {1}<br />", param.ParameterName, param.Value);

						tempRow = TEMPLATE_ROW;
						tempRow = tempRow.Replace("#name#", "Parameters");
						tempRow = tempRow.Replace("#value#", paramsSb.ToString());
						sb.Append(tempRow);
					}
				}
			}

			// Open Application Details section
			sb.Append(TEMPLATE_ROW_HEADER.Replace("#header#", "Application Details"));

			// User ID
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "User ID");
			tempRow = tempRow.Replace("#value#", SessionInfo.Current.User.UserId.ToString());
			sb.Append(tempRow);

			// User Name
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "User&nbsp;Name");
			tempRow = tempRow.Replace("#value#", SessionInfo.Current.User.FullName);
			sb.Append(tempRow);

			// User Email
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "User&nbsp;Email");
			tempRow = tempRow.Replace("#value#", SessionInfo.Current.User.Email);
			sb.Append(tempRow);

			// User Role
			tempRow = TEMPLATE_ROW;
			tempRow = tempRow.Replace("#name#", "User&nbsp;Role");
			tempRow = tempRow.Replace("#value#", SessionInfo.Current.User.UserRole.ToString());
			sb.Append(tempRow);

			// Request.Form section
			AddNameValueCollection(sb, "Request.Form", HttpContext.Current.Request.Form);

			// Request.Querystring section
			AddNameValueCollection(sb, "Request.QueryString", HttpContext.Current.Request.QueryString);

			// Request.ServerVariables section
			AddNameValueCollection(sb, "Request.ServerVariables", HttpContext.Current.Request.ServerVariables);

			// Cookies header
			AddNameValueCollection(sb, "Cookie Values", CookieManager.GetValues());

			// End
			sb.Append(TEMPLATE_ROW_HEADER.Replace("#header#", "&nbsp;"));

			// Close table
			sb.Append("</table>");

			return sb.ToString();
		}

		private static string GetReferer()
		{
			try
			{
				Uri referer = HttpContext.Current.Request.UrlReferrer;

				if (referer != null && referer.AbsoluteUri != null)
					return referer.AbsoluteUri;

				return "[Unknown - referer is null]";
			}
			catch (Exception ex)
			{
				return "[Unknown - Error occured getting referer (" + ex.Message + ")]";
			}
		}

		private static void AddNameValueCollection(StringBuilder sb, string header, NameValueCollection collection)
		{
			sb.Append(TEMPLATE_ROW_HEADER.Replace("#header#", header));

			if (collection.Count > 0)
			{
				foreach (string key in collection)
				{
					string tempRow = TEMPLATE_ROW;
					tempRow = tempRow.Replace("#name#", key);
					tempRow = tempRow.Replace("#value#", collection[key]);
					sb.Append(tempRow);
				}
			}
			else
			{
				sb.Append(TEMPLATE_ROW_EMPTY.Replace("#value#", "No values"));
			}
		}

		#endregion
	}
}