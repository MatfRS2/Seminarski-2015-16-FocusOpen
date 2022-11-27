/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data.Xml;
using FocusOPEN.Shared;

namespace Daydream.Data
{
	public interface IPluginContext
	{
		ContextType[] UseForType { get; set; }
		PluginContextMarkup Markup { get; set; }
		string[] Resources { get; set; }
		string Name { get; set; }
	}
}