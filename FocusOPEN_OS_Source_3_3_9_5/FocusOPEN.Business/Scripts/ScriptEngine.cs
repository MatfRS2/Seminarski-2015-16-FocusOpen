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
using System.Linq;
using System.Reflection;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;
using System.Web;
using System.Xml.Linq;
using Jint;
using Jint.Expressions;
using Jint.Native;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FocusOPEN.Business.Scripts
{
    public enum ScriptAPI
    {
        Unknown,
        REST
    }

    public class ScriptEngine : JintEngine
    {

        #region Singleton

		private static ScriptEngine m_Instance;

        private ScriptEngine(): base(Options.Ecmascript5 | Options.Strict)
		{
            ScriptEngineVisitor extVisitor = new ScriptEngineVisitor(((JsGlobal)visitor.Global).Options);

            JsObject global = extVisitor.Global as JsObject;
            global["ToBoolean"] = extVisitor.Global.FunctionClass.New(new Func<object, Boolean>(Convert.ToBoolean));
            global["ToByte"] = extVisitor.Global.FunctionClass.New(new Func<object, Byte>(Convert.ToByte));
            global["ToChar"] = extVisitor.Global.FunctionClass.New(new Func<object, Char>(Convert.ToChar));
            global["ToDateTime"] = extVisitor.Global.FunctionClass.New(new Func<object, DateTime>(Convert.ToDateTime));
            global["ToDecimal"] = extVisitor.Global.FunctionClass.New(new Func<object, Decimal>(Convert.ToDecimal));
            global["ToDouble"] = extVisitor.Global.FunctionClass.New(new Func<object, Double>(Convert.ToDouble));
            global["ToInt16"] = extVisitor.Global.FunctionClass.New(new Func<object, Int16>(Convert.ToInt16));
            global["ToInt32"] = extVisitor.Global.FunctionClass.New(new Func<object, Int32>(Convert.ToInt32));
            global["ToInt64"] = extVisitor.Global.FunctionClass.New(new Func<object, Int64>(Convert.ToInt64));
            global["ToSByte"] = extVisitor.Global.FunctionClass.New(new Func<object, SByte>(Convert.ToSByte));
            global["ToSingle"] = extVisitor.Global.FunctionClass.New(new Func<object, Single>(Convert.ToSingle));
            global["ToString"] = extVisitor.Global.FunctionClass.New(new Func<object, String>(Convert.ToString));
            global["ToUInt16"] = extVisitor.Global.FunctionClass.New(new Func<object, UInt16>(Convert.ToUInt16));
            global["ToUInt32"] = extVisitor.Global.FunctionClass.New(new Func<object, UInt32>(Convert.ToUInt32));
            global["ToUInt64"] = extVisitor.Global.FunctionClass.New(new Func<object, UInt64>(Convert.ToUInt64));
            extVisitor.AllowClr = true;

            visitor = extVisitor;

		}

        public static ScriptEngine Instance
		{
			get
			{
				if (m_Instance == null)
                    m_Instance = new ScriptEngine();

				return m_Instance;
			}
		}

		#endregion

		#region Fields

        private static string m_scriptsPath = String.Empty;
        private static string m_apiLogFolder = String.Empty;
        private static string m_apiRestrictAccounts = String.Empty;
        private static bool m_apiObeyIpAddressRestrictions = true;
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static List<string> loadedScripts = new List<string>();
        private static string m_Response = String.Empty;

		#endregion


        #region Accessors

        public static string ScriptsPath
        {
            get
            {
                return m_scriptsPath;
            }
            set
            {
                m_scriptsPath = WebUtils.GetSubFolderPath(value);
            }
        }

        public static string APILogFolder
        {
            get
            {
                return m_apiLogFolder;
            }
            set
            {
                m_apiLogFolder = value;
            }
        }


        public static string APIRestrictAccounts
        {
            get
            {
                return m_apiRestrictAccounts;
            }
            set
            {
                m_apiRestrictAccounts = value;
            }
        }

        public static bool APIObeyIpAddressRestrictions
        {
            get
            {
                return m_apiObeyIpAddressRestrictions;
            }
            set
            {
                m_apiObeyIpAddressRestrictions = value;
            }
        }

        #endregion


        #region Protected methods


        /// <summary>
        /// Parses the API type information from the request
        /// </summary>
        protected ScriptAPI GetAPIFromRequest(string request)
        {
            ScriptAPI APICallType = ScriptAPI.Unknown;
            string filename = Path.GetFileNameWithoutExtension(request) ?? string.Empty;
            string[] tokens = filename.Split('.');

            //assume request will take the form Scripts.APIType
            if (tokens.Length > 1)
            {
                //get the requested API
                switch (tokens[1].ToUpper())
                {
                    case "REST":
                        APICallType = ScriptAPI.REST;
                        break;
                }
            }

            return APICallType;
        }


        protected ScriptResult GetEmptyResults()
        {
            ScriptRequest request = new ScriptRequest();
            ScriptResponse response = new ScriptResponse();
            //initialise available data
            request.ResponseId = response.ResponseId;
            response.RequestId = request.ResponseId;
            request.HttpMethod = HttpContext.Current.Request.HttpMethod;
            request.IpAddress = BusinessHelper.GetCurrentIpAddress();
            // return response with generic unknown error
            response.Error = ScriptErrorFactory.Instance.LookupError(-7);

            string responseFormat = HttpContext.Current.Request.QueryString["ResponseFormat"];
            response.Format = ((!String.IsNullOrEmpty(responseFormat) && responseFormat.ToLower() == "xml") ? ResponseFormat.XML : ResponseFormat.JSON); 

            ScriptResult result = new ScriptResult();
            result.Response = response;
            result.Request = request;

            return result;
        }



        #endregion


        #region Public methods

        /// <summary>
        /// Loads the specified API and passes in the request to process
        /// </summary>
        public ScriptResult Process(string request)
        {

            try
            {
                ScriptAPI api = GetAPIFromRequest(request);

                if (api != ScriptAPI.Unknown)
                {
                    loadedScripts = new List<string>(); 
                    StringBuilder compiledScript = new StringBuilder(); 
                    string apiFolder = Path.Combine(ScriptsPath, "API");

                    //load Base API 
                    string baseFile = Path.Combine(apiFolder, "BaseAPI.js");
                    compiledScript.Append(File.ReadAllText(baseFile));

                    string apiPrefix = String.Empty;

                    //set the appropriate api prefix
                    switch (api)
                    {
                        case ScriptAPI.REST:
                            //load main rest api file
                            apiPrefix = "REST";
                            break;
                    }

                    //load up the specified API
                    string apiFile = Path.Combine(apiFolder, String.Format("{0}API.js", apiPrefix));
                    compiledScript.Append(File.ReadAllText(apiFile));

                    //load extended api with any user defined custom methods etc if it exists
                    string extFile = Path.Combine(apiFolder, String.Format("{0}Ext.js", apiPrefix));
                    if (File.Exists(extFile))
                    {
                        compiledScript.Append(File.ReadAllText(extFile));
                    }

                    
                    //instantiate the correct api to use
                    compiledScript.AppendLine(String.Format("var api = new {0}API();", apiPrefix));

                    //initialise the data
                    compiledScript.AppendLine("api.InitialiseData();");

                    //parse the request string
                    compiledScript.AppendLine("api.ParseRequest();");

                    //run generic request validation
                    compiledScript.AppendLine("api.RequestValidation();");

                    //call the process request
                    compiledScript.AppendLine("return api.ProcessRequest();");

                    //assign the Default External Functions
                    Instance.SetFunction("LoadScriptFile", new Jint.Delegates.Func<string, bool>(ScriptEngine.LoadScriptFile));
                    Instance.SetFunction("ConvertDictionaryToList", new Jint.Delegates.Func<Dictionary<string, object>, List<KeyValuePair<string, object>>>(ScriptEngine.ConvertDictionaryToList));
                    Instance.SetFunction("CurrentIPAddress", new Jint.Delegates.Func<string>(FocusOPEN.Business.BusinessHelper.GetCurrentIpAddress));
                    Instance.SetFunction("CurrentHttpRequest", new Jint.Delegates.Func<HttpRequest>(ScriptEngine.CurrentHttpRequest));

                    //disable security
                    Instance.DisableSecurity();

                    //execute the compiled script
                    object returnVal = Instance.Run(compiledScript.ToString());


                    //this should return a ScriptResponse object
                    if (returnVal is ScriptResult)
                    {
                        return (ScriptResult)returnVal;
                    }
                    else
                    {
                        //log problem 
                        m_Logger.WarnFormat("The request {0} for the {1} API did not return a valid response object: {2}", request, api.ToString(), returnVal);
                    }
                }
            }catch(JintException jexp){

                m_Logger.Error(String.Format("Script Error processing request {0}. Jint reported the following exception  - {1}", request, jexp.Message), jexp);
            }
            catch (Exception exp)
            {
                m_Logger.Error(String.Format("Error processing request {0} in the Script Engine", request), exp);
            }
        
            return GetEmptyResults();
        }
        #endregion




        #region External Functions
         
        /// <summary>
        /// Loads a script file to the current execution context
        /// </summary>
        public static bool LoadScriptFile(string scriptFile)
        {
            bool loaded = false;

            try
            {
                //work out the full path of the script file to be loaded
                if (!Path.IsPathRooted(scriptFile))
                {
                    string basePath; 
                    if (scriptFile.StartsWith("~"))
                    {
                        //relative to scripts path
                        basePath = ScriptsPath;
                        scriptFile = scriptFile.Substring(1);
                    }
                    else
                    {
                        //relative to current script folder
                        basePath = Path.GetDirectoryName(scriptFile);
                    }

                    while (scriptFile.StartsWith(@"\")) { scriptFile = scriptFile.Substring(1); }
                    scriptFile = Path.GetFullPath(Path.Combine(basePath, scriptFile));
                }


                //check that script file is not already loaded
                if (!loadedScripts.Contains(scriptFile, StringComparer.OrdinalIgnoreCase))
                {
                    //check that script exists
                    if (File.Exists(scriptFile))
                    {
                        //load file into the current ExecutionVisitor
                        string script = File.ReadAllText(scriptFile);

                        //load into current execution visitor
                        Program program = JintEngine.Compile(script, false);
                        foreach (var statement in program.ReorderStatements())
                        {
                            statement.Accept(m_Instance.visitor);
                        }

                        //add to scripts collection to stop reloading
                        loadedScripts.Add(scriptFile);
                        loaded = true;
                    }
                    else
                    {
                        m_Logger.WarnFormat("The script file to append could not be found at {0}", scriptFile);
                    }
                }
                else
                {
                    //alread loaded
                    loaded = true;
                }
            }
            catch (Exception exp)
            {
                m_Logger.WarnFormat("An error occurred appending the script {0} in the script engine: {1}", scriptFile, exp.Message);
            }

            return loaded;
        }


        public static List<KeyValuePair<string, object>> ConvertDictionaryToList(Dictionary<string, object> dic)
        {
            return dic.ToList();
        }

        public static HttpRequest CurrentHttpRequest()
        {
            //requires wrapper function to access HttpContext.Current
            return HttpContext.Current.Request;
        }


        #endregion

    }


    //extends execution visitor to use ScriptEngineTypeResolver
    public class ScriptEngineVisitor : ExecutionVisitor
    {
        public ScriptEngineVisitor(Options options)
            : base(options)
        {
            typeResolver = new ScriptEngineTypeResolver();
        }
    }

}
