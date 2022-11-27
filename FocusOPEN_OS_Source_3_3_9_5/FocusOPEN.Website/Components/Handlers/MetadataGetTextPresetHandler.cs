/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Text;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class MetadataGetTextPresetHandler : BaseHandler
	{
		public override void ProcessRequest()
		{
			var metadataId = WebUtils.GetIntRequestParam("metadataId", -1);

			if (metadataId > 0)
			{
				var m = Metadata.Get(metadataId);

				Context.Response.ContentEncoding = Encoding.UTF8;
				Context.Response.ContentType = "plain/text";
				Context.Response.Write(m.Name);
			}
		}
	}
}