/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public abstract class BaseEntityException<T> : BaseException where T : AbstractEntity
	{
		#region Private Variables

		private T m_Entity;

		#endregion

		#region Constructor

		public BaseEntityException(string message) : base(message)
		{
		}

		public BaseEntityException(string message, T entity) : this(message)
		{
			m_Entity = entity;
		}

		public BaseEntityException(ErrorList errors) : base(errors)
		{
		}

		public BaseEntityException(ErrorList errors, T entity) : this(errors)
		{
			m_Entity = entity;
		}

		#endregion

		#region Accessors

		public T Entity
		{
			get
			{
				return m_Entity;
			}
		}

		#endregion
	}
}