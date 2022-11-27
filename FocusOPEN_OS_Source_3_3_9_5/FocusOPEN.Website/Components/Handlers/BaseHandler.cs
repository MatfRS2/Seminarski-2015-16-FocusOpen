/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web;

namespace FocusOPEN.Website.Components.Handlers
{
	public abstract class BaseHandler : IHttpHandler
	{
		#region Private variables

		private HttpContext m_Context;

		#endregion

		#region Accessors

		protected HttpContext Context
		{
			get
			{
				return m_Context;
			}
		}

		#endregion

		public abstract void ProcessRequest();

		#region IHttpHandler Implementation

		public virtual void ProcessRequest(HttpContext context)
		{
			m_Context = context;
			ProcessRequest();
		}

		public virtual bool IsReusable
		{
			get
			{
				return true;
			}
		}

		#endregion
	}
}