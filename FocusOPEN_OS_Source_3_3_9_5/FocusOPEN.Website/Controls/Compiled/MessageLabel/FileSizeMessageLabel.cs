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
using System.Web.UI;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	public class FileSizeMessageLabel : BaseMessageLabel
	{
		private bool m_ShowIcon = true;
		private string m_TextPrefix = "<span class=\"Bold\">Filesize:</span>";

		#region Accessors

		public bool ShowIcon
		{
			get
			{
				return m_ShowIcon;
			}
			set
			{
				m_ShowIcon = value;
			}
		}

		public string TextPrefix
		{
			get
			{
				return m_TextPrefix;
			}
			set
			{
				m_TextPrefix = value;
			}
		}

		#endregion

		public void SetFileSize(long filesize)
		{
			decimal mb = Decimal.Round(Convert.ToDecimal(filesize)/(1024*1024), 2);

			Text = FileUtils.FriendlyFileSize(filesize);
			MessageType = (mb > 25) ? MessageTypes.Negative : MessageTypes.Positive;
			Visible = true;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string html;

			if (TextPrefix.Length > 0)
				writer.Write(TextPrefix + Environment.NewLine);

			if (Text.Length > 0)
			{
				html = string.Format("<span class=\"{0}\">{1}</span>\n", GetHeaderTextClass(), Text);
				writer.Write(html);
			}

			if (ShowIcon)
			{
				string altText = (MessageType == MessageTypes.Negative) ? "warning: this asset may take some time to download" : "[file size]";
				html = string.Format("<img src=\"{0}\" width=\"14\" height=\"14\" alt=\"{1}\" />\n", GetImagePath(), altText);
				writer.Write(html);
			}
		}
	}
}