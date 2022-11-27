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
	public abstract class AbstractEntity : IEntity
	{
		protected bool m_isDirty = false;

		public bool IsDirty
		{
			get
			{
				return m_isDirty;
			}
			set
			{
				m_isDirty = value;
			}
		}

		public abstract Boolean IsNew { get; }

		public abstract Boolean IsNull { get; }
	}
}