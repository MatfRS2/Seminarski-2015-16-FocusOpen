/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public class InvalidOrderException : BaseEntityException<Order>
	{
		public InvalidOrderException(string message, Order entity) : base(message, entity)
		{
		}

		public InvalidOrderException(string message) : base(message)
		{
		}

		public InvalidOrderException(ErrorList errors, Order entity) : base(errors, entity)
		{
		}

		public InvalidOrderException(ErrorList errors) : base(errors)
		{
		}
	}
}