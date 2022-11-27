using System;
using System.Reflection;
using System.Xml;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components.Handlers
{
	public abstract class AbstractProcessingCallbackHandler : BaseHandler
	{
		protected static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected int AssetId { get; private set; }
		protected Asset Asset { get; private set; }
		protected string PreviewPath { get; private set; }
		protected string ThumbnailPath { get; private set; }
		protected string MetadataXml { get; private set; }
		protected AdditionalDataInfo AdditionalData { get; private set; }

		protected abstract void ProcessFiles();

		protected void WriteLine(string message)
		{
			Context.Response.Write(message + Environment.NewLine);
		}

		protected void WriteLine(string message, params object[] args)
		{
			WriteLine(string.Format(message, args));
		}

		public override void ProcessRequest()
		{
			m_Logger.Debug("ProcessingCallbackHandler called");

			if (Context.Request.QueryString["testcallback"] == "1")
			{
				Context.Response.Write("Test: " + DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss") + ", " + Guid.NewGuid().ToString().ToLower());
				return;
			}

#if DEBUG

			foreach (string key in Context.Request.Form.Keys)
			{
				string val = Context.Request.Form[key];

				if (val.Length > 150)
					val = val.Substring(0, 150) + "...";

				m_Logger.DebugFormat("  - {0} - {1}", key, val);
			}

#endif

			AssetId = WebUtils.GetIntRequestParam("AssetId", 0);
			PreviewPath = WebUtils.GetRequestParam("PreviewPath", string.Empty);
			ThumbnailPath = WebUtils.GetRequestParam("ThumbnailPath", string.Empty);
			MetadataXml = WebUtils.GetRequestParam("MetadataXml", string.Empty);
			AdditionalData = AdditionalDataInfo.Get(WebUtils.GetRequestParam("AdditionalData", string.Empty));

			if (AssetId == 0)
			{
				Context.Response.Write("ERROR - Missing asset id");
				return;
			}

			Asset = Asset.Get(AssetId);

			if (Asset.IsNull)
			{
				Context.Response.Write("ERROR - Invalid asset id");
				return;
			}

			m_Logger.DebugFormat("Asset ID: {0}", AssetId);

			ProcessFiles();
		}

		protected class AdditionalDataInfo
		{
			public bool Notify { get; set; }
			public string AssetBitmapReference { get; set; }

			public static AdditionalDataInfo Get(string xmldata)
			{
				AdditionalDataInfo adi = new AdditionalDataInfo();
				adi.Notify = false;
				adi.AssetBitmapReference = string.Empty;

				if (!StringUtils.IsBlank(xmldata))
				{
					try
					{
						var doc = new XmlDocument();
						doc.LoadXml(xmldata);

						adi.Notify = (doc.SelectSingleNode("//Notify") != null && Boolean.Parse(doc.SelectSingleNode("//Notify").InnerText));
						adi.AssetBitmapReference = (doc.SelectSingleNode("//AssetBitmapReference") != null) ? doc.SelectSingleNode("//AssetBitmapReference").InnerText : string.Empty;
					}
					catch (Exception ex)
					{
						m_Logger.WarnFormat("Unable to get additional data: {0}", ex.Message);
					}
				}

				return adi;
			}
		}
	}
}