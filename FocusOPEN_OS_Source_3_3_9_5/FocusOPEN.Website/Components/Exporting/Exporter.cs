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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FocusOPEN.Website.Components.Exporting
{
	/// <summary>
	/// Base class for exporting data to CSV files
	/// </summary>
	public class Exporter
	{
		#region Private Variables

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the filename for the exported file
		/// </summary>
		public string Filename { get; set; }

        private IFileGenerator FileGenerator { get; set; }

		#endregion

        #region Constructor

        public Exporter(string fileName, IFileGenerator generator)
        {
            FileGenerator = generator;
            Filename = fileName;
        }

        #endregion
        
        #region Public Methods
		/// <summary>
		/// Converts the data to CSV and exports it (user is prompted to download)
		/// </summary>
		public void Export()
		{
			if (Filename == null) throw (new NullReferenceException("Filename"));

			string HttpHeader = string.Format("attachment;filename={0}", Filename);
			byte[] Bytes = FileGenerator.GetFileBytes();

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ContentType = "application/octet-stream";

			HttpContext.Current.Response.AddHeader("Content-Disposition", HttpHeader);
			HttpContext.Current.Response.AddHeader("Content-Length", Bytes.Length.ToString());

			HttpContext.Current.Response.BinaryWrite(Bytes);
			HttpContext.Current.Response.End();
		}

		#endregion

	}

}