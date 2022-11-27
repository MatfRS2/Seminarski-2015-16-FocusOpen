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
using System.Web;
using System.Web.UI;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public abstract class BaseMessageLabel : BaseWebControl, ITextControl, INamingContainer
	{
		public enum MessageTypes
		{
			Positive,
			Negative,
			Pending,
			Withdrawn
		}

		#region ITextControl Implementation

		public string Text
		{
			get
			{
				return GetFromViewState("Text", string.Empty);
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		#endregion

		#region Accessors

		public MessageTypes MessageType
		{
			get
			{
				return GetFromViewState("MessageType", MessageTypes.Positive);
			}
			set
			{
				ViewState["MessageType"] = value;
			}
		}

		#endregion

		#region Protected Helper Methods

		protected string GetImagePath()
		{
			string filename = string.Format("fbk{0}.gif", MessageType);
			string iconPath = SiteUtils.GetIconPath(filename);
			return ResolveUrl(iconPath);
		}

		protected virtual string GetHeaderTextClass()
		{
			return GetTextClass("FbkPosTxt", "FbkNegTxt", "FbkNullTxt", "FbkNullTxt");
		}

        protected virtual string GetTextClass()
		{
			return GetTextClass("PosTxt", "NegTxt", "NullTxt", "NullTxt");
		}

		protected string GetTextClass(string positiveClass, string negativeClass, string pendingClass, string withdrawnClass)
		{
			switch (MessageType)
			{
				case MessageTypes.Positive:
					return positiveClass;

				case MessageTypes.Negative:
					return negativeClass;

				case MessageTypes.Pending:
					return pendingClass;

				case MessageTypes.Withdrawn:
					return withdrawnClass;

				default:
					throw new Exception("Unknown message type: " + MessageType);
			}
		}

		#endregion
	}
}