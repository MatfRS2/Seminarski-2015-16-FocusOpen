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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using FocusOPEN.Data.Xml;
using System.IO;
using Daydream.Data;
using FocusOPEN.Shared;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Reflection;




namespace FocusOPEN.Business
{
    /// <summary>
    /// Class to help manage xml plugins on the file system
    /// </summary>
    internal static class PluginHelper
    {

        #region Public methods
        

        /// <summary>
        /// Finds the plugin's schema version from the file
        /// </summary>
        public static decimal PluginSchemaVersion(string path){

            //work out which version of the plugin it is
            XDocument xdoc = XDocument.Load(path);
            XAttribute version = (from attrib in xdoc.Elements("Plugin").Attributes("schemaVersion") select attrib).SingleOrDefault();

            if (version != null)
            {
                return Decimal.Parse(version.Value);
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Gets the appropriate plugin class for the plugin schema version
        /// </summary>
        public static Type GetPluginSchemaType(decimal version)
        {
            switch (Convert.ToInt32(version))
            {
                case 1:
                    return typeof(Plugin_V1);
                default:
                    return typeof(Plugin_V1);
            }
        }


        public static XmlSchema LoadSchema(Type schemaType)
        {

            string resource = String.Empty;

            if (schemaType == typeof(Plugin_V1))
            {
                resource = "FocusOPEN.Data.Schemas.FocusOPEN_Plugin_V1.xsd";
            }

            if (!String.IsNullOrEmpty(resource))
            {
                using (Stream stream = Assembly.GetAssembly(typeof(FocusOPEN.Data.Plugin)).GetManifestResourceStream(resource))
                {
                    return XmlSchema.Read(stream, null);
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Reads the plugin from the specified path and validates against schema
        /// </summary>
        public static IPlugin ReadPlugin(string path)
        {
            return ReadPlugin(path, true);
        }


        /// <summary>
        /// Reads the plugin from the specified path and validates against schema where required
        /// </summary>
        public static IPlugin ReadPlugin(string path, bool validate)
        {
            decimal version = PluginSchemaVersion(path);
            Type pluginType = GetPluginSchemaType(version);

            //settings to validate plugin
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(LoadSchema(pluginType));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemaSet;

            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                XmlSerializer xserializer = new XmlSerializer(pluginType);
                return (IPlugin)xserializer.Deserialize(reader);               
            }

        }

        /// <summary>
        /// Do all the plugin's resources exist 
        /// </summary>
        public static bool ResourcesExist(string pluginFolder, string[] resources)
        {
            if (resources != null)
            {
                foreach (string res in resources)
                {
                    string filePath = Path.Combine(pluginFolder, res);

                    if (!File.Exists(filePath))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Compares the folder's checksum with the checksum value passed in
        /// </summary>
        public static bool ChecksumsMatch(string pluginFolder, int checksum)
        {

            return (FileUtils.GetFolderHash(pluginFolder, true) == checksum);
        }



        #endregion
    }
}
