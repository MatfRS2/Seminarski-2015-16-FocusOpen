/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
    public class AssetSourcePathManager
    {
        public static List<string> GetAssetFiles(string path)
        {
            ValidateSourcePath(path);
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList<string>();
        }


        #region Validation

        private static void ValidateSourcePathWithException(string sourcePath)
        {
            ErrorList errors = ValidateSourcePath(sourcePath);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }

        private static ErrorList ValidateSourcePath(string sourcePath)
        {
            ErrorList errors = new ErrorList();

            if (sourcePath.Trim() == string.Empty)
            {
                errors.Add("Path cannot be empty");
            }
            else if (!Directory.Exists(sourcePath))
            {
                errors.Add(string.Format("Path '{0}' does not exist", sourcePath));
            }
            return errors;
        }

        #endregion

    }
}
