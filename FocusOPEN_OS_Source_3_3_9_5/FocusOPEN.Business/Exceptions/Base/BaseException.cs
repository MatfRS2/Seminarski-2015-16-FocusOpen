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
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public abstract class BaseException : Exception
	{
		private readonly ErrorList m_Errors = new ErrorList();

		public ErrorList Errors
		{
			get
			{
				return m_Errors;
			}
		}

		public BaseException(ErrorList errors)
		{
			m_Errors = errors;
		}

		public BaseException(string message) : base(message)
		{
		}

		public override string Message
		{
			get
			{
				if (m_Errors.Count > 0)
					return m_Errors.ToString();

				return base.Message;
			}
		}
	}
}