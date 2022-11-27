/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Business;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class HomepageImageHandler : BaseFileHandler
	{
		public override void ProcessRequest()
		{
			int homepageId = GetIdFromFilename();
			int imageNumber = WebUtils.GetIntRequestParam("image", 0);

			string path = HomepageImageManager.GetHomepageImagePath(homepageId, imageNumber);

			if (path == string.Empty)
				path = Context.Server.MapPath("~/Images/Spacer.gif");

			WriteFileToResponseStream(path);
		}
	}
}