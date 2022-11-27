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
	public class InvalidLightboxException : BaseEntityException<Lightbox>
	{
		public InvalidLightboxException(string message) : base(message)
		{
		}

		public InvalidLightboxException(ErrorList errors, Lightbox entity) : base(errors, entity)
		{
		}

		public InvalidLightboxException(ErrorList errors) : base(errors)
		{
		}

		public InvalidLightboxException(string message, Lightbox entity) : base(message, entity)
		{
		}
	}
}