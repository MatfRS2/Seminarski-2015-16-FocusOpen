using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Exporting
{
    public interface IFileGenerator
    {
        byte[] GetFileBytes();
    }

    /// <summary>
    /// Exporter class for generating CSV files from DataTables
    /// </summary>
    public sealed class DataTableCsvExporter : IFileGenerator
    {
        #region Private Variables

        private readonly DataTable m_DataTable;

        #endregion

        #region Constructor

        public DataTableCsvExporter(DataTable dataTable)
        {
            FieldMap = new Dictionary<String, String>();
            m_DataTable = dataTable;
        }

        #endregion

        #region Accessors

        private Dictionary<string, string> FieldMap { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a mapped field (to be included in generated the CSV file)
        /// </summary>
        /// <param name="fieldname">The name of the property of the class</param>
        /// <param name="newname">The name to give this property in the generated CSV file</param>
        public void AddFieldMapping(string fieldname, string newname)
        {
            FieldMap.Add(fieldname.ToLower(), newname);
        }

        #endregion

        #region Private Helper Methods

        private string GetColumName(string columnName)
        {
            string name = columnName;

            if (FieldMap.ContainsKey(columnName.ToLower()))
                name = FieldMap[columnName.ToLower()];

            return (name);
        }

        #endregion

        #region IFileGenerator Implementation

        public byte[] GetFileBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    if (FieldMap.Count == 0)
                    {
                        foreach (DataColumn column in m_DataTable.Columns)
                        {
                            sw.Write(column.ColumnName + ",");
                        }
                        sw.Write("\n");

                        foreach (DataRow row in m_DataTable.Rows)
                        {
                            foreach (DataColumn column in m_DataTable.Columns)
                            {
                                sw.Write("\"" + row[column] + "\",");
                            }
                            sw.Write("\n");
                        }
                        sw.Write("\n");
                    }
                    else
                    {
                        foreach (string field in FieldMap.Keys)
                        {
                            foreach (DataColumn column in m_DataTable.Columns)
                            {
                                if (field.ToLower() == column.ColumnName.ToLower())
                                {
                                    sw.Write(GetColumName(column.ColumnName) + ",");
                                }
                            }
                        }
                        sw.Write("\n");

                        foreach (DataRow row in m_DataTable.Rows)
                        {
                            foreach (string field in FieldMap.Keys)
                            {
                                foreach (DataColumn column in m_DataTable.Columns)
                                {
                                    if (field.ToLower() == column.ColumnName.ToLower())
                                    {
                                        sw.Write("\"" + row[column] + "\",");
                                    }
                                }
                            }
                            sw.Write("\n");
                        }
                        sw.Write("\n");
                    }

                    sw.Flush();
                    sw.Close();
                }

                byte[] Data = stream.ToArray();

                stream.Flush();
                stream.Close();

                return (Data);
            }
        }

        #endregion
    }

    /// <summary>
    /// Exporter class for generating CSV files from custom entities
    /// </summary>
    public sealed class EntityListCsvExporter : IFileGenerator
    {
        #region Private Variables

        private readonly IList m_Source = null;

        #endregion

        #region Constructor

        public EntityListCsvExporter(IList source)
        {
            FieldMap = new Dictionary<String, String>();
            m_Source = source;
        }

        #endregion

        #region Accessors

        private Dictionary<string, string> FieldMap { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a mapped field (to be included in generated the CSV file)
        /// </summary>
        /// <param name="fieldname">The name of the property of the class</param>
        /// <param name="newname">The name to give this property in the generated CSV file</param>
        public void AddFieldMapping(string fieldname, string newname)
        {
            FieldMap.Add(fieldname.ToLower(), newname);
        }

        #endregion

        #region Private Helper Methods

        private string GetColumName(PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;

            if (FieldMap.ContainsKey(propertyInfo.Name.ToLower()))
                name = FieldMap[propertyInfo.Name.ToLower()];

            return (name);
        }

        #endregion

        #region IFileGenerator Implementation

        public byte[] GetFileBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    Type t = m_Source[0].GetType();
                    PropertyInfo[] properties = t.GetProperties();

                    List<PropertyInfo> propertiesToDisplay = new List<PropertyInfo>();

                    if (FieldMap.Count == 0)
                    {
                        propertiesToDisplay.AddRange(properties);
                    }
                    else
                    {
                        propertiesToDisplay.AddRange(from field in FieldMap.Keys
                                                     from propertyInfo in properties
                                                     where propertyInfo.Name.ToLower() == field && propertyInfo.CanRead
                                                     select propertyInfo);
                    }

                    for (int i = 0; i < propertiesToDisplay.Count; i++)
                    {
                        PropertyInfo propertyInfo = propertiesToDisplay[i];

                        string column = GetColumName(propertyInfo);
                        sw.Write(column);

                        if (i < propertiesToDisplay.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }

                    sw.Write("\n");

                    foreach (object obj in m_Source)
                    {
                        for (int i = 0; i < propertiesToDisplay.Count; i++)
                        {
                            PropertyInfo propertyInfo = propertiesToDisplay[i];

                            object o = propertyInfo.GetValue(obj, null);
                            string val = (o == null) ? "NULL" : o.ToString();
                            sw.Write("\"" + val + "\"");

                            if (i < propertiesToDisplay.Count - 1)
                            {
                                sw.Write(",");
                            }
                        }

                        sw.Write("\n");
                    }
                    sw.Write("\n");

                    sw.Flush();
                    sw.Close();
                }

                byte[] Data = stream.ToArray();

                stream.Flush();
                stream.Close();

                return (Data);
            }
        }

        #endregion
    }

    public sealed class EntityListXmlExporter : IFileGenerator
    {
        #region Private Variables

        private readonly XmlTree m_RawDocument;

        #endregion

        #region Constructor

        public EntityListXmlExporter(XmlTree source)
        {
            m_RawDocument = source;
        }

        #endregion

        public byte[] GetFileBytes()
        {
            //retrieve a valid XML document from the parameter data structure supplied
            var doc = m_RawDocument.ConvertToXmlDocument();

			//encode data into utf bytes and return
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(doc.OuterXml);
        }
    }
}