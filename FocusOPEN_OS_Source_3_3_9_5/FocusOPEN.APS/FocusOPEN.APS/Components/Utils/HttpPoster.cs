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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;

namespace FocusOPEN.APS
{
	public class HttpPoster
	{
		#region Events

		public delegate void BeforePostEventHandler(object sender, BeforePostEventArgs e);
		public delegate void AfterPostEventHandler(object sender, AfterPostEventArgs e);

		/// <summary>
		/// Fired before the data is posted to the url
		/// </summary>
		public event BeforePostEventHandler BeforePost;

		/// <summary>
		/// Fired after the advert is posted to the url
		/// </summary>
		public event AfterPostEventHandler AfterPost;

		#endregion

		#region Private Variables

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly Dictionary<string, Header> m_HeaderList = new Dictionary<string, Header>();
		private readonly Dictionary<string, Parameter> m_ParameterList = new Dictionary<string, Parameter>();

		#endregion

		#region Constructor

		public HttpPoster()
		{
		}

		#endregion

		#region Properties

		public string Url { get; set; }

		#endregion

		#region Public Helper Methods

		public void AddHeader(string name, object value)
		{
			Header header = new Header { Name = name, Value = value.ToString() };
			m_HeaderList[name] = header;
		}

		public void AddParameter(string name, object value)
		{
			string val = string.Empty;

			if (value != null)
				val = value.ToString();

			Parameter parameter = new Parameter { Name = name, Value = val };
			m_ParameterList[name] = parameter;
		}

		#endregion

		/// <summary>
		/// Performs the post
		/// </summary>
		public string Post()
		{
			// Get the parameters string ready for posting
			string postData = GetParametersForPosting();

			// Fire before post event
			if (BeforePost != null)
			{
				BeforePostEventArgs args = new BeforePostEventArgs {PostData = postData};
				BeforePost(this, args);
			}

			if (m_Logger.IsDebugEnabled)
			{
				string _postData = (postData.Length <= 50) ? postData : postData.Substring(0, 50) + "...(" + postData.Length + " chars)";

				m_Logger.DebugFormat("URL: {0}", Url);
				m_Logger.DebugFormat("PostData: {0}", _postData);
			}

			// Construct the web request
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = postData.Length;

			// Add the headers
			foreach (Header header in m_HeaderList.Values)
				webRequest.Headers.Add(header.Name, header.Value);

			// Post the data
			using (StreamWriter streamOut = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII))
			{
				streamOut.Write(postData);
				streamOut.Close();
			}

			string responseText;

			// Get the response
			using (StreamReader streamIn = new StreamReader(webRequest.GetResponse().GetResponseStream() ?? Stream.Null))
			{
				responseText = streamIn.ReadToEnd();
				streamIn.Close();
			}

			// Fire the after post event
			if (AfterPost != null)
			{
				AfterPostEventArgs args = new AfterPostEventArgs {RawResponse = responseText};
				AfterPost(this, args);
			}

			return responseText;
		}

		#region Helper Methods

		private string GetParametersForPosting()
		{
			StringBuilder postData = new StringBuilder();

			foreach (Parameter parameter in m_ParameterList.Values)
			{
				postData.AppendFormat("{0}={1}&", parameter.Name, parameter.Value);
			}

			string returnVal = postData.ToString();

			if (returnVal.EndsWith("&"))
				returnVal = returnVal.Substring(0, returnVal.Length - 1);

			return returnVal;
		}

		#endregion

		#region Helper Classes

		public class BeforePostEventArgs : EventArgs
		{
			public string PostData { internal set; get; }
		}

		public class AfterPostEventArgs : EventArgs
		{
			public string RawResponse { internal set; get; }
		}

		private struct Header
		{
			/// <summary>
			/// Header Name
			/// </summary>
			public string Name { internal set; get; }

			/// <summary>
			/// Header Value
			/// </summary>
			public string Value { internal set; get; }
		}

		private struct Parameter
		{
			/// <summary>
			/// Parameter Name
			/// </summary>
			public string Name { internal set; get; }

			/// <summary>
			/// Parameter Value
			/// </summary>
			public string Value { internal set; get; }
		}

		#endregion
	}
}
