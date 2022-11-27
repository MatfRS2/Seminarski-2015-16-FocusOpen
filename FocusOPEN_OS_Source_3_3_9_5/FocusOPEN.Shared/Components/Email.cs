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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace FocusOPEN.Shared
{
	public class Email
	{
		#region Email Exceptions

		/// <summary>
		/// This exception is thrown when the specified email template cannot be found
		/// </summary>
		public sealed class EmailTemplateNotFoundException : Exception
		{
			private readonly string m_Filename;

			internal EmailTemplateNotFoundException(string filename)
			{
				m_Filename = filename;
			}

			public override string Message
			{
				get
				{
					string message = string.Format("The email message template '{0}' could not be found.  Please check this file exists and has correct permissions.  No email message will be sent.", m_Filename);
					return (message);
				}
			}
		}

		#endregion

		#region Private Variables

		private bool m_IsDebugMode;

		#endregion

		#region Constructor / Factory

		static Email()
		{
			EngineEnabled = true;
			DefaultBodyIsHtml = true;
		}

		private Email()
		{
			TemplateFilename = string.Empty;
			FromName = DefaultFromName;
			FromEmail = DefaultFromEmail;
			Recipients = new List<string>();
			CC = new List<string>();
			BCC = new List<String>();
			Subject = string.Empty;
			Body = string.Empty;
			BodyXmlDoc = XmlUtils.GetBlankXmlDoc();
			Attachments = new List<Attachment>();
			BodyParameters = new Hashtable();
			IsHtml = DefaultBodyIsHtml;
			m_IsDebugMode = HasDebugMode;
			IncludeStaticBccRecipients = true;
		}

		public static Email CreateFromTemplate(string templateFilename)
		{
			Email email = new Email();

			string filename = templateFilename + ".xsl";
			email.TemplateFilename = filename;

			return (email);
		}

		public static Email Create()
		{
			return new Email();
		}

		#endregion

		#region Static Accessors

		/// <summary>
		/// Boolean value specifying whether the email engine is enabled
		/// </summary>
		public static bool EngineEnabled { get; set; }

		/// <summary>
		/// Boolean value specifying whether debug mode is available
		/// </summary>
		public static bool HasDebugMode
		{
			get
			{
				return (DebugEmail.Length > 0);
			}
		}

		/// <summary>
		/// Gets or sets the default from name to be used for email messages
		/// </summary>
		public static string DefaultFromName { get; set; }

		/// <summary>
		/// Gets or sets the default from email to be used for email messages
		/// </summary>
		public static string DefaultFromEmail { get; set; }

		/// <summary>
		/// Gets or sets the mail server address to be used to send messages
		/// </summary>
		public static string MailServer { get; set; }

		/// <summary>
		/// Gets or sets the mail server username to be used to send messages
		/// if the mail server requires authentication
		/// </summary>
		public static string MailServerUsername { get; set; }

		/// <summary>
		/// Gets or sets the mail server password to be used to send messages
		/// if the mail server requires authentication
		/// </summary>
		public static string MailServerPassword { get; set; }

		/// <summary>
		/// Gets or sets the mail server logon domain that should be used
		/// if a username and password are specified
		/// </summary>
		public static string MailServerLogonDomain { get; set; }

		/// <summary>
		/// Gets or sets the BCC email that all messages should be BCC'ed to
		/// </summary>
		public static string BccEmail { get; set; }

		/// <summary>
		/// Gets or sets the debug email address.  When this is set, all email
		/// messages will be sent to this email address instead of the actual
		/// recipients email address.
		/// </summary>
		public static string DebugEmail { get; set; }

		/// <summary>
		/// Gets or sets the absolute path to the location of the email templates
		/// </summary>
		public static string TemplatePath { get; set; }

		/// <summary>
		/// Boolean value specifying the default setting for email message body types
		/// </summary>
		public static bool DefaultBodyIsHtml { get; set; }

		#endregion

		#region Public Accessors

		/// <summary>
		/// The template filename to be used to create the email
		/// </summary>
		public string TemplateFilename { get; private set; }

		/// <summary>
		/// The name from which the email will be sent
		/// </summary>
		public string FromName { get; set; }

		/// <summary>
		/// The email address from which the email address will be sent
		/// </summary>
		public string FromEmail { get; set; }

		/// <summary>
		/// List of recipient email addresses
		/// </summary>
		public IList<string> Recipients { get; private set; }

		/// <summary>
		/// List of CC email addresses
		/// </summary>
		public IList<string> CC { get; private set; }

		/// <summary>
		/// List of BCC email addresses
		/// </summary>
		public IList<string> BCC { get; private set; }

		/// <summary>
		/// The email address subject
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// The email address body
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// The XML document to be used to transform against the email template
		/// </summary>
		public XmlDocument BodyXmlDoc { get; set; }

		/// <summary>
		/// The list of parameters to be used to perform replacements in the email body
		/// </summary>
		public Hashtable BodyParameters { get; set; }

		/// <summary>
		/// Boolean value specifying whether the email message is HTML
		/// </summary>
		public bool IsHtml { get; set; }

		/// <summary>
		/// List of attachments
		/// </summary>
		public IList<Attachment> Attachments { get; private set; }

		/// <summary>
		/// Boolean value specifying whether fixed list of BCC recipients should
		/// be included in this email message
		/// </summary>
		public bool IncludeStaticBccRecipients { get; set; }

		/// <summary>
		/// Boolean value specifying whether the debug mode should be used is available.
		/// If debug mode is not available, and this value is set to true, an exception will be thrown
		/// </summary>
		public bool IsDebugMode
		{
			get
			{
				return m_IsDebugMode;
			}
			set
			{
				if (value && !HasDebugMode)
					throw new Exception("Debug mode cannot be enabled if no debug email address has been specified");

				m_IsDebugMode = value;
			}
		}

		#endregion

		#region General Methods

		/// <summary>
		/// Adds a parameter to the body parameters list, used for doing text replacements
		/// or logic in the XSL template.  These values are passed in as XSL parameters
		/// </summary>
		/// <param name="paramName">The parameter name</param>
		/// <param name="paramValue">The parameter value</param>
		public void AddBodyParameter(string paramName, object paramValue)
		{
			BodyParameters[paramName] = paramValue.ToString();
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Gets the physical path to the specified template
		/// </summary>
		/// <param name="filename">The filename</param>
		/// <returns>Path to the filename</returns>
		private static string GetPhysicalPath(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentException("Template filename is empty");

			return Path.Combine(TemplatePath, filename);
		}

		/// <summary>
		/// Gets the mail address based on the specified name and email
		/// </summary>
		private static MailAddress GetMailAddress(string email, string name)
		{
			if (string.IsNullOrEmpty(name))
				return new MailAddress(email);

			return new MailAddress(email, name);
		}

		#endregion

		#region Attachments Helper Methods

		/// <summary>
		/// Adds an attachment to the attachments list
		/// </summary>
		/// <param name="data">File text data</param>
		/// <param name="filename">Filename</param>
		public void AddAttachment(string data, string filename)
		{
			MemoryStream stream = new MemoryStream(UTF32Encoding.Default.GetBytes(data)) { Position = 0 };
			Attachment attachment = new Attachment(stream, filename);
			Attachments.Add(attachment);
		}

		/// <summary>
		/// Adds an attachment to the attachments list
		/// </summary>
		/// <param name="filename">The path to the file to be attached</param>
		public void AddAttachment(string filename)
		{
			Attachment attachment = new Attachment(filename);
			Attachments.Add(attachment);
		}

		#endregion

		#region Send Methods

		/// <summary>
		/// Sends the email message.
		/// </summary>
		public void Send()
		{
			// Initialize the body from the email template if we don't have a body set
			// and a template filename has been specified.

			if ((Body.Length == 0) && (TemplateFilename.Length > 0))
			{
				string filename = GetPhysicalPath(TemplateFilename);

				if (!File.Exists(filename))
					throw new EmailTemplateNotFoundException(TemplateFilename);

				Body = XmlUtils.Transform(BodyXmlDoc.InnerXml, filename, BodyParameters);
			}

			// Initialize the email message
			MailMessage message = new MailMessage
			{
				From = GetMailAddress(FromEmail, FromName),
				Subject = Subject,
				Body = Body,
				BodyEncoding = Encoding.ASCII,
				IsBodyHtml = IsHtml
			};

			message.Headers.Add("x-sender-application", "FocusOPEN");

			if (HasDebugMode && IsDebugMode)
			{
				// Add debug email addresses
				foreach (string debugEmailAddress in DebugEmail.Split(';').Where(StringUtils.IsEmail))
					message.To.Add(new MailAddress(debugEmailAddress));

				// List of actual recipients
				JoinableList jList = new JoinableList();

				// To recipients
				foreach (string recipient in Recipients)
					jList.Add(recipient);

				// CC recipients
				foreach (string recipient in CC.Where(recipient => !Recipients.Contains(recipient)))
					jList.Add("CC:" + recipient);

				// BCC recipients
				foreach (string recipient in BCC.Where(recipient => !Recipients.Contains(recipient) && !BCC.Contains(recipient)))
					jList.Add("BCC:" + recipient);

				// Build our debug message
				StringBuilder sb = new StringBuilder();

				if (IsHtml)
				{
					// Wrap our debug message in a DIV for better viewing
					sb.Append("<div style='margin:30px 0px 30px 0px;border:2px solid red;background:#eee;padding:10px;color:#000;'>");
					sb.Append("<p><strong>This email has been sent in debug mode.</strong></p>");
					sb.Append("<p><strong>Actual Recipients:</strong></p>");
					sb.Append(jList);
					sb.Append("</div>");
				}
				else
				{
					// Just add a few line breaks for text email messages
					sb.AppendFormat("\n\n\nThis email has been sent in debug mode.  Actual recipients: {0}", jList);
				}

				// Add debug message to the email message body
				message.Body += sb.ToString();
			}
			else
			{
				//---------------------------------------------------
				// Add all recipients
				//---------------------------------------------------

				foreach (string emailaddress in Recipients)
				{
					try
					{
						message.To.Add(emailaddress);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}

				//---------------------------------------------------
				// Add all CC recipients which haven't
				// already been added into the recipients list
				//---------------------------------------------------

				foreach (string emailaddress in CC.Where(emailaddress => !Recipients.Contains(emailaddress)))
				{
					try
					{
						message.CC.Add(emailaddress);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}

				//---------------------------------------------------
				// Add all BCC recipients which haven't
				// already been added into the CC or recipients list
				//---------------------------------------------------

				foreach (string emailaddress in BCC.Where(emailaddress => !Recipients.Contains(emailaddress) && !CC.Contains(emailaddress)))
				{
					try
					{
						message.Bcc.Add(emailaddress);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}

				//---------------------------------------------------
				// Add all static BCC recipients which haven't
				// already been added into any other lists
				//---------------------------------------------------

				if (IncludeStaticBccRecipients)
				{
					foreach (string bccEmailAddress in BccEmail.Split(';').Where(bccEmailAddress => !Recipients.Contains(bccEmailAddress) && !CC.Contains(bccEmailAddress) && !BCC.Contains(bccEmailAddress)))
					{
						try
						{
							message.Bcc.Add(bccEmailAddress);
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex.Message);
						}
					}
				}
			}

			// Add attachments
			foreach (Attachment attachment in Attachments)
				message.Attachments.Add(attachment);

			// Only send the email message if the email engine is actually enabled
			if (EngineEnabled)
			{
				// Initialize the smtp client to send the email
				SmtpClient client = new SmtpClient(MailServer);

				// Add credentials to authenticate to mail server if required
				if (!StringUtils.IsBlank(MailServerUsername) || !StringUtils.IsBlank(MailServerPassword))
					client.Credentials = new System.Net.NetworkCredential(MailServerUsername, MailServerPassword, MailServerLogonDomain);

				// Send the message
				client.Send(message);
			}
		}

		#endregion
	}
}