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
using System.Security;
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class ZipDownloadHandler : BaseFileHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			// Ensure we have a valid user first
			if (SessionInfo.Current.User.IsNull)
				throw new SecurityException("Access denied");

			// Get querystring values
			int orderId = WebUtils.GetIntRequestParam("orderId", 0);
			string dateString = WebUtils.GetRequestParam("d", string.Empty);
			string guid = WebUtils.GetRequestParam("guid", string.Empty);

			// Ensure we have an order id
			if (orderId == 0)
				throw new HttpException(404, "Missing order id");

			// Ensure we have other unique download identifiers
			if (StringUtils.IsBlank(dateString) || StringUtils.IsBlank(guid))
				throw new SystemException("Required parameters are missing");

			// Get the order
			Order order = Order.Get(orderId);

			// Ensure that the order was placed by the current user
			if (order.UserId != SessionInfo.Current.User.UserId.GetValueOrDefault())
				throw new SecurityException("Access denied");

			// Set the download filename
			string downloadFilename = string.Format("assets_from_order_{0}.zip", orderId);

			// Get the session folder where this order is stored
			string sessionFolder = SessionHelper.GetForCurrentSession().CreateSessionTempFolder();

			// Get the output filename for this order
			string outputFilename = string.Format("assets_from_order_{0}_{1}_{2}.zip", orderId, dateString, guid);

			// Construct the path to the zip file
			string outputPath = Path.Combine(sessionFolder, outputFilename);

			// Ensure it exists
			if (!File.Exists(outputPath))
				throw new FileNotFoundException();

			// Send it to the browser
			WriteFileToResponseStream(outputPath, downloadFilename);
		}
	}
}