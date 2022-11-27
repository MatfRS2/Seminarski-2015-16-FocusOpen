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

namespace FocusOPEN.Shared
{
	[Serializable]
	public class ChangedProperty
	{
		#region Private Variables

		private string m_PropertyName;
		private object m_OldValue;
		private object m_NewValue;

		#endregion

		#region Accessors

		public string PropertyName
		{
			get
			{
				return m_PropertyName;
			}
			set
			{
				m_PropertyName = value;
			}
		}

		public object OldValue
		{
			get
			{
				return m_OldValue;
			}
			set
			{
				m_OldValue = value;
			}
		}

		public object NewValue
		{
			get
			{
				return m_NewValue;
			}
			set
			{
				m_NewValue = value;
			}
		}

		#endregion

		public ChangedProperty(string propertyName, object oldValue, object newValue)
		{
			m_PropertyName = propertyName;
			m_OldValue = oldValue;
			m_NewValue = newValue;
		}
	}
}
