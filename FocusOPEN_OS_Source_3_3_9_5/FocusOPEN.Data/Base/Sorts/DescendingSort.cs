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

namespace Daydream.Data
{
    [Serializable]
    public class DescendingSort : ISortExpression
	{
		#region Private variables

		private readonly string m_fieldName = String.Empty;

		#endregion

		#region Constructors

		public DescendingSort (string fieldName)
		{
			m_fieldName = fieldName;
		}

		public DescendingSort(Enum fieldName)
		{
			m_fieldName = fieldName.ToString();
		}

		#endregion

		#region Properties

		public String FieldName
		{
			get { return m_fieldName; }
		}

		public Boolean IsAscending
		{
			get { return false; }
		}

		#endregion

		public override string ToString()
		{
			return m_fieldName + " DESC";
		}
	}
}
