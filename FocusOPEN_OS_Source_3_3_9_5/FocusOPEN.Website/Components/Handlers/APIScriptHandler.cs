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
using System.Reflection;
using System.Text;
using System.Xml;
using FocusOPEN.Business;
using FocusOPEN.Business.Scripts;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.SessionState;

namespace FocusOPEN.Website.Components.Handlers
{
    public class APIScriptHandler : BaseHandler, IRequiresSessionState
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override void ProcessRequest()
		{			
            try{
                //log the request in the general log file
                m_Logger.InfoFormat("API Call to {0}.", Context.Request.Path);

                //pass the request to the script engine for processing
                ScriptResult result = ScriptEngine.Instance.Process(Context.Request.Path);

                User user = User.Get(result.Response.UserId);
                //if response was successful then update the user's last operation date
                if (result.Response.Error.Code == 0)
                {
                    UserManager.UpdateLastAPIOperation(user);

                    //update sessioninfo user to synchonise the SessionAPITokens etc.
                    if (!SessionInfo.Current.User.IsNull && !user.IsNull && SessionInfo.Current.User.UserId == user.UserId)
                    {
                        SessionInfo.Current.User = user;
                    }

                    m_Logger.InfoFormat("Successful call to {0}. Request: {1}, Response: {2}", Context.Request.Path, result.Request.RequestId, result.Response.ResponseId);
                }
                else
                {
                    m_Logger.InfoFormat("Unsuccessful call to {0}. Request: {1}, Response: {2}", Context.Request.Path,result.Request.RequestId,result.Response.ResponseId);
                }
             
                if (!user.IsNull)
                {
                    //log the request made in the audit trail
                    AuditLogManager.LogUserAction(user, AuditUserAction.APICall, String.Format("API Call to {0}", Context.Request.Path));
                }

                //save the response and request to the file system if API log folder
                //setting has been configured correctly
                if (!String.IsNullOrEmpty(ScriptEngine.APILogFolder) && Directory.Exists(ScriptEngine.APILogFolder))
                {
                    string requestFile = Path.Combine(ScriptEngine.APILogFolder,String.Format("{0}_request_{1}.xml", result.Request.CallName, result.Request.RequestId));
                    FileUtils.SaveToXmlFile(requestFile, result.Request);
                    string responseFile = Path.Combine(ScriptEngine.APILogFolder, String.Format("{0}_response_{1}.xml", result.Response.CallName, result.Response.ResponseId));
                    FileUtils.SaveToXmlFile(responseFile, result.Response);
                }
                else
                {
                    m_Logger.Warn("The API Log Folder has not been configured correctly. API Request-Response could not be saved.");
                }
                
			    //Write the response according to the response format
                if (result.Response.Format == ResponseFormat.XML )
                {
                    //Xml response
                    m_Logger.Debug("Xml Response");
                    Context.Response.ContentType = "application/xml";
                    Context.Response.ContentEncoding = Encoding.UTF8;           
                    XmlSerializer xserialize = new XmlSerializer(typeof(ScriptResponse));
                    xserialize.Serialize(Context.Response.OutputStream, result.Response);                 
                }
                else
                {
                    //Json response (Default)
                    m_Logger.Debug("Json Response");
                    Context.Response.ContentType = "application/json";
                    Context.Response.ContentEncoding = Encoding.UTF8;
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ScriptResponse));
                    json.WriteObject(Context.Response.OutputStream, result.Response);
                    

                }
            }
            catch (Exception exp)
            {
                m_Logger.ErrorFormat("An exception occurred processing a script request {0} - {1}", Context.Request.Path, exp.Message);
                throw new HttpException(500, "An unexpected error occurred");
            }
		}


	}
}