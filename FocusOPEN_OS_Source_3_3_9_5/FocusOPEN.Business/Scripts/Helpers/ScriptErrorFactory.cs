using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using log4net;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace FocusOPEN.Business.Scripts
{
    public class ScriptErrorFactory
    {
        //expected name for the errors file
        private const string ErrorsFile = "APIErrors.xml";
        private static Dictionary<int, string> m_Errors = new Dictionary<int, string>();

        #region Singleton

		private static ScriptErrorFactory m_Instance;

        private ScriptErrorFactory()
		{
            LoadErrors(ErrorsFile);
		}

        public static ScriptErrorFactory Instance
		{
			get
			{
				if (m_Instance == null)
                    m_Instance = new ScriptErrorFactory();

				return m_Instance;
			}
		}

		#endregion



        #region Protected methods

        /// <summary>
        /// Loads up the recognised API errors from an errors xml file
        /// </summary>
        protected static void LoadErrors(string errorsXmlFileName)
        {
            //file should be at the root of the scripts path
            string errorsXmlFile = Path.Combine(ScriptEngine.ScriptsPath, errorsXmlFileName);

            try
            {
                //check that file exists
                if (File.Exists(errorsXmlFile))
                {
                    XDocument xDoc = XDocument.Load(errorsXmlFile);
                    var errors = from e in xDoc.Root.Elements("Error") select e;

                    if (errors.Count() > 0)
                    {
                        foreach (XElement err in errors)
                        {
                            int code = Int32.Parse(err.Attribute("id").Value);

                            //check that duplicate code does not exist
                            //as all error codes should be unique
                            if (!m_Errors.ContainsKey(code))
                            {
                                m_Errors.Add(code, err.Attribute("description").Value);
                            }
                            else
                            {
                                throw new Exception(String.Format("An error with id {0} already exists in the script engine error list xml file {1}", code, errorsXmlFile));
                            }
                        }
                    }
                }
                else
                {
                    //m_Logger.Warn(String.Format("The script engine error list xml file {0} does not exist.", errorsXmlFile));
                }
            }
            catch (Exception exp)
            {
               // m_Logger.Warn(String.Format("Error loading script engine error list xml file {0}.  Error: {1}", errorsXmlFile, exp.Message));
            }
        }


        #endregion


        #region Public Methods

        public ScriptError LookupError(int errorCode)
        {
            ScriptError err = new ScriptError();

            if (m_Errors.ContainsKey(errorCode))
            {
                //recognised error code so return status
                err.Code = errorCode;
                err.Description =  m_Errors[errorCode];
            }
            else
            {
                //error code not recognised so return generic unknown error
                //m_Logger.WarnFormat("Unrecognised error code called in script engine: {0}", errorCode);
                err.Code = -9999;
                err.Description = String.Format("Unrecognised error code - {0}", errorCode);
            }

            return err;
        }

        #endregion



    }
}
