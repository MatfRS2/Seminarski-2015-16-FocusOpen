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
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class FeedbackLabel : BaseMessageLabel
	{
		#region Private variables

		private ErrorList m_Errors = new ErrorList();

		#endregion

		#region Accessors

		public bool LineBreakOnTop { get; set; }

		public bool UseContainer { get; set; }

		/// <summary>
		/// Header text
		/// </summary>
		public string Header
		{
			get
			{
				return GetFromViewState("Header", string.Empty);
			}
			set
			{
				ViewState["Header"] = value;
			}
		}

		/// <summary>
		/// Number of linebreaks to render after the output
		/// </summary>
		public int LineBreaks
		{
			get
			{
				return GetFromViewState("LineBreaks", 0);
			}
			set
			{
				ViewState["LineBreaks"] = value;
			}
		}

		/// <summary>
		/// Boolean value specifying whether this message label is pinned,
		/// in which case it's visibility will not be changed
		/// </summary>
		public bool Pinned
		{
			get
			{
				return GetFromViewState("Pinned", false);
			}
			set
			{
				ViewState["Pinned"] = value;
			}
		}

		public string TextPrefix
		{
			get
			{
				return GetFromViewState("TextPrefix", " - ");
			}
			set
			{
				ViewState["TextPrefix"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!Pinned)
				Visible = false;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Pinned)
				Visible = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (LineBreakOnTop || UseContainer)
				writer.WriteLine("<br class=\"h5\"/>");

			if (UseContainer)
				writer.WriteLine("<div class=\"AppCtrlMrg\">");

			string html = string.Format("<img src=\"{0}\" width=\"14\" height=\"14\" />", GetImagePath());
			writer.WriteLine(html);

			if (Header.Length > 0)
			{
				Header = SiteUtils.ConvertTextToHtml(Header, GetHeaderTextClass());
				html = string.Format("<span class=\"{0}\">{1}<br /></span>", GetHeaderTextClass(), Header);
				writer.WriteLine(html);
			}

			if (m_Errors.Count > 0)
			{
				for (int i = 0; i < m_Errors.Count; i++)
				{
					string error = m_Errors[i].ToString();

					error = SiteUtils.ConvertTextToHtml(error, GetTextClass());
					html = string.Format("<span class=\"{0}\"> - {1}<br /></span>", GetTextClass(), error);
					writer.Write(html);
				}
			}
			else if (Text.Length > 0)
			{
				Text = SiteUtils.ConvertTextToHtml(Text, GetTextClass());
				html = string.Format("<span class=\"{0}\">{1}{2}<br /></span>", GetTextClass(), TextPrefix, Text);
				writer.WriteLine(html);
			}

			for (int i = 0; i < LineBreaks; i++)
				writer.WriteBreak();

			if (UseContainer)
				writer.WriteLine("</div>");
		}

		#endregion

		#region Message methods

		public void SetErrorMessage(string header)
		{
			SetMessage(header, string.Empty, null, MessageTypes.Negative);
		}


		public void SetErrorMessage(string header, string text)
		{
			SetMessage(header, text, null, MessageTypes.Negative);
		}

		public void SetErrorMessage(string header, ErrorList errors)
		{
			TextPrefix = string.Empty;
			SetMessage(header, string.Empty, errors, MessageTypes.Negative);
		}

        /// <summary>
        /// Adds an additional message to the internal list of errors
        /// </summary>
        /// <param name="text"></param>
        public void AddErrorMessage(string text)
        {
            AddErrorMessage(text, null);
        }

        /// <summary>
        /// Appends a list of errors to the existing internal list of errors.
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrorMessage(ErrorList errors)
        {
            AddErrorMessage(String.Empty, errors);
        }

		public void SetSuccessMessage(string header)
		{
			SetSuccessMessage(header, string.Empty);
		}

		public void SetSuccessMessage(string header, string text)
		{
			SetMessage(header, text, null, MessageTypes.Positive);
		}

		public void SetSuccessMessage(string header, ErrorList messageList)
		{
			TextPrefix = string.Empty;
			SetMessage(header, string.Empty, messageList, MessageTypes.Positive);
		}

		public void SetMessage(string header, ErrorList messageList, bool isError)
		{
			if (isError)
			{
				SetErrorMessage(header, messageList);
			}
			else
			{
				SetSuccessMessage(header, messageList);
			}
		}

		#endregion

		private void SetMessage(string header, string text, ErrorList errors, MessageTypes messageType)
		{
			if (errors == null)
				errors = new ErrorList();

			Header = header;
			Text = text;
			MessageType = messageType;
			m_Errors = errors;
			Visible = true;
		}

        private void AddErrorMessage(string text, ErrorList errors)
        {
            if (text != String.Empty)
                m_Errors.Add(text);

            if (errors != null)
                m_Errors.AddRange(errors);

            MessageType = MessageTypes.Negative;
            Visible = true;
        }

    }
}